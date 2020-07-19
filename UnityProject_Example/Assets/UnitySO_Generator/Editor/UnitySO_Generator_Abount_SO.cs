#region Header
/*	============================================
 *	작성자 : Strix
 *	작성일 : 2020-01-07 오후 2:13:23
 *	개요 : 
   ============================================ */
#endregion Header

using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using System.IO;
using System.Reflection;
using System;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using SpreadSheetParser;
using Debug = UnityEngine.Debug;

/// <summary>
/// Editor 폴더 안에 위치해야 합니다.
/// </summary>
public partial class UnitySO_Generator
{

    private static void Generate_TypeToSO(TypeData pTypeData, UnitySO_GeneratorConfig pConfig)
    {
        if (pTypeData.bEnable == false)
            return;

        Debug.Log($"{nameof(Generate_TypeToSO)} {pTypeData.strFileName} - Start");

        Stopwatch pStopWatch = new Stopwatch();
        pStopWatch.Start();

        string strTypeName = pTypeData.strFileName;
        Type pType_SO = GetTypeFromAssemblies(strTypeName, typeof(ScriptableObject));
        if (pType_SO == null)
        {
            Debug.LogError($"pType_SO == null - {strTypeName} - {strTypeName}");
            return;
        }

        Type pType_Container = GetTypeFromAssemblies(strTypeName + "_Container");
        if (pType_Container == null)
        {
            Debug.LogError($"pType_Container == null - {strTypeName} - {strTypeName}");
            return;
        }

        JArray pJArray_Instance = GetDataArray_FromJson(strTypeName);
        if (pJArray_Instance.Count == 0)
        {
            Debug.LogError($"{strTypeName} - pJArray_Instance.Count == 0");
            return;
        }

        GenerateSO(pConfig, pType_Container, pType_SO, pTypeData, pJArray_Instance);
        Debug.Log($"{nameof(Generate_TypeToSO)} {pTypeData.strFileName} - Finish Elapse : " + pStopWatch.Elapsed);
    }

    private static void GenerateSO(UnitySO_GeneratorConfig pConfig, Type pType_Container, Type pType_SO, TypeData pTypeData,
        JArray pJArray_Instance)
    {
        ScriptableObject pContainerInstance = (ScriptableObject)UnitySO_GeneratorConfig.CreateSOFile(pType_Container,
            pConfig.strDataExport_FolderPath + "/" + pTypeData.strFileName + "_Container", true);

        _mapSOInstance.Add(pType_SO, new SOInstance(pTypeData, pContainerInstance));

        List<Container_CachingLogicBase> listCachingLogic = new List<Container_CachingLogicBase>();
        Dictionary<string, FieldInfo> mapFieldInfo_SO = pType_SO.GetFields().ToDictionary((pFieldInfo) => pFieldInfo.Name);
        IEnumerable<FieldInfo> arrFieldInfo_Container = pType_Container.GetFields();

        foreach (var pFieldInfo in arrFieldInfo_Container)
        {
            Type pTypeField = pFieldInfo.FieldType;
            Type pTypeField_Generic = pTypeField.GetGenericTypeDefinition();
            if (pTypeField_Generic == typeof(List<>))
                listCachingLogic.Add(new Container_CachingLogic_List(pContainerInstance, pFieldInfo));
        }

        Generate_SOInstance(pTypeData, pType_SO, pJArray_Instance, pContainerInstance, mapFieldInfo_SO, listCachingLogic,
            pTypeData.listFieldData);
    }

    static void Reference_OtherSO()
    {
        foreach (Refrence_OtherSO_Data pReference_OtherSOData in _setReference_OtherSO)
        {
            if (_mapSOInstance.TryGetValue(pReference_OtherSOData.pType_OtherSO, out var pOtherSO_Instance) == false)
            {
                Debug.LogError($"{nameof(Reference_OtherSO)} - Not Found SO {pReference_OtherSOData.pType_OtherSO}");
                continue;
            }

            var pObject = pOtherSO_Instance.listSO.FirstOrDefault(p => p.name == pReference_OtherSOData.strValue);
            if (pObject == null)
            {
                Debug.LogError($"{nameof(Reference_OtherSO)} - Not Found Other SO Name : {pReference_OtherSOData.strValue}");
                continue;
            }

            pReference_OtherSOData.pFieldInfo.SetValue(pReference_OtherSOData.pSOInstance, pObject);
            EditorUtility.SetDirty(pReference_OtherSOData.pSOInstance);
        }
    }


    private static void Generate_SOInstance(TypeData pTypeData, Type pType_SO, JArray pJArray_Instance, ScriptableObject pContainerInstance, Dictionary<string, FieldInfo> mapFieldInfo_SO, List<Container_CachingLogicBase> listCachingLogic, List<FieldTypeData> listFieldData)
    {
        string strHeaderField = pTypeData.strHeaderFieldName;
        string strFileName = pType_SO.Name;
        int iLoopIndex = 0;

        FieldTypeData pFieldData_Header = listFieldData.FirstOrDefault(pFieldData => pFieldData.strFieldName == strHeaderField);
        FieldTypeData[] arrRealField = listFieldData.Where((pFieldData) => pFieldData.bIsVirtualField == false && pFieldData.bDeleteThisField_InCode == false).ToArray();
        FieldTypeData[] arrVirtualField = listFieldData.Where((pFieldData) => pFieldData.bIsVirtualField && pFieldData.bDeleteThisField_InCode == false).ToArray();


        foreach (JObject pInstanceData in pJArray_Instance)
        {
            ScriptableObject pSO = GenerateSO(pType_SO, pContainerInstance);
            Process_RealField(pTypeData, mapFieldInfo_SO, pInstanceData, pSO, arrRealField);
            Process_VirtualField(pTypeData, mapFieldInfo_SO, pInstanceData, pSO, listFieldData, arrVirtualField);
            Process_SOName(strFileName, iLoopIndex, pSO, pFieldData_Header, pInstanceData);
            EditorUtility.SetDirty(pSO);

            for (int i = 0; i < listCachingLogic.Count; i++)
                listCachingLogic[i].Process_CachingLogic(pSO, listFieldData);

            iLoopIndex++;
        }

        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(pContainerInstance));
        AssetDatabase.SaveAssets();
    }

    private static ScriptableObject GenerateSO(Type pType_SO, ScriptableObject pContainerInstance)
    {
        ScriptableObject pSO = CreateInstance(pType_SO);
        _mapSOInstance[pType_SO].listSO.Add(pSO);

        AssetDatabase.AddObjectToAsset(pSO, pContainerInstance);

        return pSO;
    }

    private static void Process_SOName(string strFileName, int iLoopIndex, ScriptableObject pSO, FieldTypeData pFieldHeader, JObject pInstanceData)
    {
        pSO.name = pFieldHeader != null ? (string)pInstanceData[pFieldHeader.strFieldName] : $"{strFileName}_{iLoopIndex}";
        if (string.IsNullOrEmpty(pSO.name))
            Debug.LogError($"{strFileName} - {nameof(Process_SOName)} -  {string.IsNullOrEmpty(pSO.name)}, pFieldHeader : {pFieldHeader?.strFieldName}");
    }

    private static void Process_RealField(TypeData pTypeData, Dictionary<string, FieldInfo> mapFieldInfo_SO, JObject pInstanceData, ScriptableObject pSO, IEnumerable<FieldTypeData> listRealField)
    {
        foreach (var pMember in listRealField)
        {
            if (mapFieldInfo_SO.TryGetValue(pMember.strFieldName, out var pFieldInfo) == false)
            {
                Debug.LogError($"Not Found Real Field {pMember.strFieldType} {pMember.strFieldName}");
                continue;
            }

            string strValue = (string)pInstanceData[pMember.strFieldName];
            try
            {
                switch (pMember.strFieldType)
                {
                    case "int": pFieldInfo.SetValue(pSO, int.Parse(strValue)); break;
                    case "float": pFieldInfo.SetValue(pSO, float.Parse(strValue)); break;
                    case "string": pFieldInfo.SetValue(pSO, strValue); break;

                    case "System.DateTime": pFieldInfo.SetValue(pSO, DateTime.Parse(strValue)); break;

                    default:
                        Type pType_Field = GetTypeFromAssemblies(pMember.strFieldType);
                        if (pType_Field == null)
                        {
                            Debug.LogError($"아직 지원되지 않은 형식.. {pMember.strFieldType}");
                            continue;
                        }

                        if (pType_Field.IsEnum)
                        {
                            bool bIsNumber = int.TryParse(strValue, out var iEnumNumberValue);
                            if (bIsNumber)
                                pFieldInfo.SetValue(pSO, iEnumNumberValue);
                            else
                                pFieldInfo.SetValue(pSO, Enum.Parse(pType_Field, strValue));
                        }
                        else
                            _setReference_OtherSO.Add(new Refrence_OtherSO_Data(pSO, pFieldInfo, pType_Field, strValue));

                        break;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Parsing  Fail - {pTypeData.strFileName}/{pMember.strFieldType} {pMember.strFieldName} : {strValue}\n" +
                    $"Exception : {e}");
            }
        }
    }

    private static void Process_VirtualField(TypeData pTypeData, Dictionary<string, FieldInfo> mapFieldInfo_SO, JObject pInstanceData, ScriptableObject pSO, IEnumerable<FieldTypeData> listField, IEnumerable<FieldTypeData> listVirtualField)
    {
        foreach (var pMember in listVirtualField)
        {
            FieldInfo pFieldInfo = mapFieldInfo_SO[pMember.strFieldName];
            Type pType_Field = GetTypeFromAssemblies(pMember.strFieldType);
            if (pType_Field == null)
            {
                Debug.LogError($"{pTypeData.strFileName}-{pMember.strFieldName} - Field Type Not Found - {pMember.strFieldType}");
                continue;
            }
            FieldTypeData pFieldData_Dependency = listField.FirstOrDefault(pFieldData => pFieldData.strFieldName == pMember.strDependencyFieldName);
            if (pFieldData_Dependency == null)
            {
                Debug.LogError($"{pTypeData.strFileName}-{pMember.strFieldName} - Dependency Not Found - Name : {pMember.strDependencyFieldName}");
                continue;
            }

            string strDependencyValue = (string)pInstanceData[pFieldData_Dependency.strFieldName];
            if (strDependencyValue == "None")
                continue;

            if (pType_Field.IsEnum)
            {
                if (string.IsNullOrEmpty(strDependencyValue))
                {
                    Debug.LogWarning("TODO string null check");
                    continue;
                }

                try
                {
                    pFieldInfo.SetValue(pSO, Enum.Parse(pType_Field, strDependencyValue));
                }
                catch (Exception e)
                {
                    if (pTypeData.eType != ESheetType.Global)
                        Debug.LogError($"{pTypeData.strFileName} Enum Parse Error - ({pType_Field.MemberType})\"{strDependencyValue}\"" + e);
                }
            }
            else
            {
                FieldTypeData pFieldData_Dependency_Sub = listField.FirstOrDefault(pFieldData => pFieldData.strFieldName == pMember.strDependencyFieldName_Sub);
                if (pFieldData_Dependency_Sub != null)
                {
                    string strDependencyValue_Sub = (string)pInstanceData[pFieldData_Dependency_Sub.strFieldName];

                    UnityEngine.Object[] arrObject = AssetDatabase.LoadAllAssetsAtPath(strDependencyValue);
                    if (arrObject == null || arrObject.Length == 0)
                    {
                        Debug.LogError($"Value Is Null Or Empty - Type : {pMember.strFieldType} {strDependencyValue}");
                        continue;
                    }

                    var pObject = arrObject.FirstOrDefault(p => p.name == strDependencyValue_Sub && p.GetType() == pType_Field);
                    if (pObject == null)
                    {
                        Debug.LogError($"{pTypeData.strFileName} - DependencyValue Sub {strDependencyValue_Sub} Is Null");
                    }

                    pFieldInfo.SetValue(pSO, pObject);
                }
                else
                {
                    switch (pMember.strFieldType)
                    {
                        case "System.DateTime": pFieldInfo.SetValue(pSO, DateTime.Parse(strDependencyValue)); break;

                        default:
                            UnityEngine.Object pObject = AssetDatabase.LoadAssetAtPath(strDependencyValue, pType_Field);
                            if (pObject == null)
                                Debug.LogError($"{pTypeData.strFileName}-{pMember.strFieldName}({pMember.strFieldType})  - Value Is Null Or Empty - Value : {strDependencyValue}");
                            pFieldInfo.SetValue(pSO, pObject);
                            break;
                    }
                }
            }
        }
    }

    private static JArray GetDataArray_FromJson(string strFileName)
    {
        JArray arrObject = new JArray();
        try
        {
            if (strFileName.Contains(".json") == false)
                strFileName += ".json";

            TextAsset pJsonFile = AssetDatabase.LoadAssetAtPath<TextAsset>($"{UnitySO_GeneratorConfig.instance.strJsonData_FolderPath}/{strFileName}");
            JObject pObject = (JObject)JsonConvert.DeserializeObject(pJsonFile.text);
            arrObject = (JArray)pObject?.GetValue("array");
        }
        catch
        {
            // ignored
        }

        return arrObject;
    }


    private static bool GetData_FromJson<T>(string strFileName, ref T pData)
    {
        try
        {
            if (strFileName.Contains(".json") == false)
                strFileName += ".json";

            return GetData_FromJson(AssetDatabase.LoadAssetAtPath<TextAsset>($"{UnitySO_GeneratorConfig.instance.strJsonData_FolderPath}/{strFileName}"), ref pData);
        }
        catch
        {
            return false;
        }
    }

    private static bool GetData_FromJson<T>(TextAsset pJsonFile, ref T pData)
    {
        try
        {
            EditorJsonUtility.FromJsonOverwrite(pJsonFile.text, pData);
        }
        catch
        {
            return false;
        }

        return true;
    }

    void AutoSizeLabel(string strText)
    {
        GUILayout.Label(strText, GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent(strText)).x));
    }

    // https://answers.unity.com/questions/1374822/how-to-convert-a-string-to-type.html
    public static Type GetTypeFromAssemblies(string strTypeName)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        for (int i = 0; i < assemblies.Length; i++)
        {
            var arrType = assemblies[i].GetTypes();
            if (arrType.Length == 0)
                continue;

            var pFindType = arrType.FirstOrDefault(pType => strTypeName.Equals(pType.FullName));
            if (pFindType == null)
                pFindType = arrType.FirstOrDefault(pType => strTypeName.Equals(pType.Name));

            if (pFindType != null)
                return pFindType;
        }

        for (int i = 0; i < assemblies.Length; i++)
        {
            var arrType = assemblies[i].GetTypes();
            if (arrType.Length == 0)
                continue;

            var pFindType = arrType.FirstOrDefault(pType => pType.Name.Equals(strTypeName));

            if (pFindType != null)
                return pFindType;
        }

        return null;
    }

    public static Type GetTypeFromAssemblies(string strTypeName, Type pBaseType)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        for (int i = 0; i < assemblies.Length; i++)
        {
            var arrType = assemblies[i].GetTypes().Where(pType => pType.BaseType == pBaseType).ToArray();
            if (arrType.Length == 0)
                continue;

            var pFindType = arrType.FirstOrDefault(pType => strTypeName.Equals(pType.FullName));
            if (pFindType == null)
                pFindType = arrType.FirstOrDefault(pType => strTypeName.Equals(pType.Name));

            if (pFindType != null)
                return pFindType;
        }

        return null;
    }
}
