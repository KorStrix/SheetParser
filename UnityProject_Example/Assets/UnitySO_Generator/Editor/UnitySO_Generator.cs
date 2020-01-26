#region Header
/*	============================================
 *	작성자 : Strix
 *	작성일 : 2020-01-07 오후 2:13:23
 *	개요 : 
   ============================================ */
#endregion Header

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using System.IO;
using System.Reflection;
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

/// <summary>
/// Editor 폴더 안에 위치해야 합니다.
/// </summary>
public class UnitySO_Generator : EditorWindow
{
    /* const & readonly declaration             */

    /* enum & struct declaration                */

    /* public - Field declaration            */

    #region Container
    public abstract class Container_CashingLogicBase
    {
        protected ScriptableObject _pContainerInstance { get; private set; }
        protected FieldInfo _pFieldInfo { get; private set; }
        protected Dictionary<string, FieldInfo> mapFieldInfo_SO { get; private set; }

        public Container_CashingLogicBase(ScriptableObject pContainerInstance, FieldInfo pFieldInfo_CashedContainer, Dictionary<string, FieldInfo> mapFieldInfo_SO)
        {
            this._pContainerInstance = pContainerInstance;
            this._pFieldInfo = pFieldInfo_CashedContainer;
        }

        abstract public void Process_CashingLogic(object pObject, List<FieldTypeData> listFieldData);
    }

    public class Container_CashingLogic_List : Container_CashingLogicBase
    {
        MethodInfo _pMethod_Add;
        object _pInstance;

        public Container_CashingLogic_List(ScriptableObject pContainerInstance, FieldInfo pFieldInfo_CashedContainer, Dictionary<string, FieldInfo> mapFieldInfo_SO) : base(pContainerInstance, pFieldInfo_CashedContainer, mapFieldInfo_SO)
        {
            _pMethod_Add = pFieldInfo_CashedContainer.FieldType.GetMethod("Add");
            _pInstance = System.Activator.CreateInstance(pFieldInfo_CashedContainer.FieldType);
            pFieldInfo_CashedContainer.SetValue(pContainerInstance, _pInstance);
        }

        public override void Process_CashingLogic(object pObject, List<FieldTypeData> listFieldData)
        {
            _pMethod_Add.Invoke(_pInstance, new object[] { pObject });
        }
    }

    public class Container_CashingLogic_Dictionary_Single : Container_CashingLogicBase
    {
        MethodInfo _pMethod_Add;
        FieldInfo _pKeyFieldInfo_Instance;
        object _pInstance;

        public Container_CashingLogic_Dictionary_Single(ScriptableObject pContainerInstance, FieldInfo pFieldInfo_CashedContainer, Dictionary<string, FieldInfo> mapFieldInfo_SO) : base(pContainerInstance, pFieldInfo_CashedContainer, mapFieldInfo_SO)
        {
            System.Type[] arrGenericArguments = pFieldInfo_CashedContainer.FieldType.GetGenericArguments();

            _pMethod_Add = pFieldInfo_CashedContainer.FieldType.GetMethod("Add", new[] {
                                arrGenericArguments[0], arrGenericArguments[1]});
            _pInstance = System.Activator.CreateInstance(pFieldInfo_CashedContainer.FieldType);
            pFieldInfo_CashedContainer.SetValue(pContainerInstance, _pInstance);

            string strContainTargetName = pFieldInfo_CashedContainer.Name.Replace("mapData_Key_Is_", "");
            _pKeyFieldInfo_Instance = mapFieldInfo_SO.Where(p => p.Key == strContainTargetName).FirstOrDefault().Value;
            if (_pKeyFieldInfo_Instance == null)
            {
                Debug.LogError($"Not Found Field Instance Type : {pContainerInstance.GetType()} - {strContainTargetName}");
            }
        }

        public override void Process_CashingLogic(object pObject, List<FieldTypeData> listFieldData)
        {
            if (_pKeyFieldInfo_Instance == null)
                return;

            object pKeyValue = _pKeyFieldInfo_Instance.GetValue(pObject);
            if(pKeyValue == null)
            {
                Debug.LogError($"{pObject.GetType()}-{_pKeyFieldInfo_Instance.Name} - value == null");
                return;
            }

            try
            {
                _pMethod_Add.Invoke(_pInstance, new object[] { pKeyValue, pObject });
            }
            catch (System.Exception e)
            {
                Debug.LogError($"{pObject.GetType()}-{_pKeyFieldInfo_Instance.Name} - Key : {pKeyValue}" +
                    $"\nError : {e}");
            }
        }
    }

    public class Container_CashingLogic_Dictionary_List : Container_CashingLogicBase
    {
        MethodInfo _pMethod_Dictionary_Add;
        MethodInfo _pMethod_Dictionary_ContainKey;
        MethodInfo _pMethod_Dictionary_GetItem;

        MethodInfo _pMethod_List_Add;


        FieldInfo _pKeyFieldInfo_Instance;

        System.Type _pTypeList;
        object _pInstance;

        public Container_CashingLogic_Dictionary_List(ScriptableObject pContainerInstance, FieldInfo pFieldInfo_CashedContainer, Dictionary<string, FieldInfo> mapFieldInfo_SO) : base(pContainerInstance, pFieldInfo_CashedContainer, mapFieldInfo_SO)
        {
            System.Type pDictionaryType = pFieldInfo_CashedContainer.FieldType;

            System.Type[] arrGenericArguments = pDictionaryType.GetGenericArguments();
            _pTypeList = arrGenericArguments[1];

            _pMethod_Dictionary_Add = pDictionaryType.GetMethod("Add");
            _pMethod_Dictionary_ContainKey = pDictionaryType.GetMethod("ContainsKey");
            _pMethod_Dictionary_GetItem = pDictionaryType.GetMethod("get_Item");
            _pMethod_List_Add = _pTypeList.GetMethod("Add");

            _pInstance = System.Activator.CreateInstance(pFieldInfo_CashedContainer.FieldType);

            pFieldInfo_CashedContainer.SetValue(pContainerInstance, _pInstance);

            string strContainTargetName = pFieldInfo_CashedContainer.Name.Replace("mapData_Key_Is_", "");
            _pKeyFieldInfo_Instance = mapFieldInfo_SO.Where(p => p.Key == strContainTargetName).FirstOrDefault().Value;
            if(_pKeyFieldInfo_Instance == null)
            {
                Debug.LogError($"Not Found Field Instance {strContainTargetName}");
            }
        }

        public override void Process_CashingLogic(object pObject, List<FieldTypeData> listFieldData)
        {
            if (_pKeyFieldInfo_Instance == null)
                return;

            object pKeyValue = _pKeyFieldInfo_Instance.GetValue(pObject);

            object[] pKey = new object[] { pKeyValue };
            bool bIsContainKey = (bool)_pMethod_Dictionary_ContainKey.Invoke(_pInstance, pKey);
            if(bIsContainKey == false)
            {
                var pListInstanceNew = System.Activator.CreateInstance(_pTypeList);
                _pMethod_Dictionary_Add.Invoke(_pInstance, new object[] { pKeyValue, pListInstanceNew });
            }

            object pListInstance = _pMethod_Dictionary_GetItem.Invoke(_pInstance, pKey);
            _pMethod_List_Add.Invoke(pListInstance, new object[] { pObject });
        }
    }
    #endregion Container

    /* protected & private - Field declaration         */


    // ========================================================================== //

    /* public - [Do] Function
     * 외부 객체가 호출(For External class call)*/

    [MenuItem("Tools/Scriptable Object Generator")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        UnitySO_Generator window = (UnitySO_Generator)GetWindow(typeof(UnitySO_Generator), false);

        window.minSize = new Vector2(800, 600);
        window.Show();
    }

    static public void DoBuild()
    {
        Debug.Log("Build Start");

        UnitySO_GeneratorConfig pConfig = UnitySO_GeneratorConfig.instance;

        TypeDataList pTypeDataList = new TypeDataList();
        if(GetData_FromJson(nameof(TypeDataList), ref pTypeDataList) == false)
        {
            Debug.LogError("TypeDataList JsonParsing Fail");
            return;
        }

        foreach (TypeData pTypeData in pTypeDataList.listTypeData)
        {
            string strTypeName = pTypeData.strType;
            System.Type pType_SO = GetTypeFromAssemblies(pTypeData.strType);
            if (pType_SO == null)
            {
                Debug.LogError($"pType_SO == null - {strTypeName} - {pTypeData.strType}");
                continue;
            }

            JArray pJArray_Instance = GetDataArray_FromJson(strTypeName);
            if (pJArray_Instance.Count == 0)
            {
                Debug.LogError("Error");
                continue;
            }

            if (pType_SO.BaseType != typeof(UnityEngine.ScriptableObject))
                continue;

            System.Type pType_Container = GetTypeFromAssemblies(pTypeData.strType + "_Container");
            ScriptableObject pContainerInstance = (ScriptableObject)UnitySO_GeneratorConfig.CreateSOFile(pType_Container, pConfig.strExportFolderPath + "/" + pTypeData.strType + "_Container", true);

            Dictionary<string, System.Reflection.FieldInfo> mapFieldInfo_SO = pType_SO.GetFields().ToDictionary((pFieldInfo) => pFieldInfo.Name);

            List<Container_CashingLogicBase> listCashingLogic = new List<Container_CashingLogicBase>();
            IEnumerable<System.Reflection.FieldInfo> arrFieldInfo_Container = pType_Container.GetFields();
            foreach (var pFieldInfo in arrFieldInfo_Container)
            {
                System.Type pTypeField = pFieldInfo.FieldType;
                System.Type pTypeField_Generic = pTypeField.GetGenericTypeDefinition();
                if (pTypeField_Generic == typeof(List<>))
                {
                    listCashingLogic.Add(new Container_CashingLogic_List(pContainerInstance, pFieldInfo, mapFieldInfo_SO));
                }
                //else if (pTypeField_Generic == typeof(Dictionary<,>))
                //{
                //    System.Type[] arrGenericArguments = pTypeField.GetGenericArguments();

                //    if(arrGenericArguments[1].IsGenericType)
                //        listCashingLogic.Add(new Container_CashingLogic_Dictionary_List(pContainerInstance, pFieldInfo, mapFieldInfo_SO));
                //    else
                //        listCashingLogic.Add(new Container_CashingLogic_Dictionary_Single(pContainerInstance, pFieldInfo, mapFieldInfo_SO));
                //}
            }

            Process_Field(pTypeData, pType_SO, pJArray_Instance, pContainerInstance, mapFieldInfo_SO, listCashingLogic, pTypeData.listField);

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }

        Debug.Log("Build Finish");
    }

    private static void Process_Field(TypeData pTypeData, Type pType_SO, JArray pJArray_Instance, ScriptableObject pContainerInstance, Dictionary<string, FieldInfo> mapFieldInfo_SO, List<Container_CashingLogicBase> listCashingLogic, List<FieldTypeData> listFieldData)
    {
        string strHeaderField = pTypeData.strHeaderFieldName;
        string strFileName = pType_SO.Name;
        int iLoopIndex = 0;

        FieldTypeData pFieldData_Header = listFieldData.Where((pFieldData) => pFieldData.strFieldName == strHeaderField).FirstOrDefault();
        IEnumerable<FieldTypeData> listRealField = listFieldData.Where((pFieldData) => pFieldData.bIsVirtualField == false && pFieldData.bDeleteThisField_InCode == false);
        IEnumerable<FieldTypeData> listVirtualField = listFieldData.Where((pFieldData) => pFieldData.bIsVirtualField && pFieldData.bDeleteThisField_InCode == false);


        foreach (JObject pInstanceData in pJArray_Instance)
        {
            ScriptableObject pSO = GenerateSO(pType_SO, pContainerInstance);
            Process_RealField(pTypeData, mapFieldInfo_SO, pInstanceData, pSO, listRealField);
            Process_VirtualField(pTypeData, mapFieldInfo_SO, pInstanceData, pSO, listFieldData, listVirtualField);
            Process_SOName(strHeaderField, strFileName, iLoopIndex, listFieldData, pSO, pFieldData_Header, pInstanceData);

            for (int i = 0; i < listCashingLogic.Count; i++)
            {
                try
                {
                    listCashingLogic[i].Process_CashingLogic(pSO, listFieldData);
                }
                catch (System.Exception e)
                {
                    listCashingLogic[i].Process_CashingLogic(pSO, listFieldData);
                }
            }

            iLoopIndex++;
        }

        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(pContainerInstance));
    }

    // ========================================================================== //

    /* protected - Override & Unity API         */

    private void OnGUI()
    {
        UnitySO_GeneratorConfig pConfig = UnitySO_GeneratorConfig.instance;

        if (pConfig == null)
        {
            Debug.LogError("pConfig == null");
            return;
        }

        DrawRootPath(pConfig);
        DrawExportPath(pConfig);

        bool bEditorEnable = string.IsNullOrEmpty(pConfig.strJsonRootFolderPath) == false;
        EditorGUI.BeginDisabledGroup(bEditorEnable == false);

        if (GUILayout.Button("Build!"))
        {
            DoBuild();
        }

        EditorGUI.EndDisabledGroup();
    }

    private void DrawRootPath(UnitySO_GeneratorConfig pConfig)
    {
        GUILayout.BeginHorizontal();

        GUILayout.Label("Root Folder Path : ", GUILayout.Width(200f));
        GUILayout.Label(pConfig.strJsonRootFolderPath, GUILayout.Width(200f));

        if (GUILayout.Button("Root Folder Setting"))
        {
            string strPath = EditorUtility.OpenFolderPanel("Root Folder", "", "");
            System.Uri pCurrentURI = new System.Uri(Application.dataPath);
            pConfig.strJsonRootFolderPath = pCurrentURI.MakeRelativeUri(new System.Uri(strPath)).ToString();
            pConfig.DoSave();
        }

        GUILayout.EndHorizontal();
    }
    private void DrawExportPath(UnitySO_GeneratorConfig pConfig)
    {
        GUILayout.BeginHorizontal();

        GUILayout.Label("Export Folder Path : ", GUILayout.Width(200f));
        GUILayout.Label(pConfig.strExportFolderPath, GUILayout.Width(200f));

        if (GUILayout.Button("Export Folder Setting"))
        {
            string strPath = EditorUtility.OpenFolderPanel("Root Folder", "", "");
            System.Uri pCurrentURI = new System.Uri(Application.dataPath);
            pConfig.strExportFolderPath = pCurrentURI.MakeRelativeUri(new System.Uri(strPath)).ToString();
            pConfig.DoSave();
        }

        GUILayout.EndHorizontal();
    }

    /* protected - [abstract & virtual]         */


    // ========================================================================== //

    #region Private


    private static ScriptableObject GenerateSO(System.Type pType_SO, ScriptableObject pContainerInstance)
    {
        ScriptableObject pSO = (ScriptableObject)UnitySO_GeneratorConfig.CreateInstance(pType_SO);

        AssetDatabase.AddObjectToAsset(pSO, pContainerInstance);
        // AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(pContainerInstance));

        return pSO;
    }

    private static void Process_SOName(string strHeaderField, string strFileName, int iLoopIndex, List<FieldTypeData> listFieldData, ScriptableObject pSO, FieldTypeData pFieldHeader, JObject pInstanceData)
    {
        if(pFieldHeader != null)
            pSO.name = (string)pInstanceData[pFieldHeader.strFieldName];
        else
            pSO.name = $"{strFileName}_{iLoopIndex}";

        if (string.IsNullOrEmpty(pSO.name))
        {
            Debug.LogError("Error");
        }

        EditorUtility.SetDirty(pSO);
    }

    private static void Process_RealField(TypeData pTypeData, Dictionary<string, System.Reflection.FieldInfo> mapFieldInfo_SO, JObject pInstanceData, ScriptableObject pSO, IEnumerable<FieldTypeData> listRealField)
    {
        foreach (var pMember in listRealField)
        {
            System.Reflection.FieldInfo pFieldInfo;
            if (mapFieldInfo_SO.TryGetValue(pMember.strFieldName, out pFieldInfo) == false)
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

                    default:
                        System.Type pType_Field = GetTypeFromAssemblies(pMember.strFieldType);
                        if (pType_Field.IsEnum)
                            pFieldInfo.SetValue(pSO, System.Enum.Parse(pType_Field, strValue));

                        if (pType_Field == null)
                            Debug.LogWarning($"아직 지원되지 않은 형식.. {pMember.strFieldType}");
                        break;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Parsing  Fail - {pTypeData.strType}/{pMember.strFieldType} {pMember.strFieldName} : {strValue}\n" +
                    $"Exception : {e}");
            }
        }
    }

    private static void Process_VirtualField(TypeData pTypeData, Dictionary<string, System.Reflection.FieldInfo> mapFieldInfo_SO, JObject pInstanceData, ScriptableObject pSO, IEnumerable<FieldTypeData> listField, IEnumerable<FieldTypeData> listVirtualField)
    {
        foreach (var pMember in listVirtualField)
        {
            System.Reflection.FieldInfo pFieldInfo = mapFieldInfo_SO[pMember.strFieldName];
            System.Type pType_Field = GetTypeFromAssemblies(pMember.strFieldType);
            if (pType_Field == null)
            {
                Debug.LogError($"Error {pMember.strFieldName} - Field Type Not Found - {pMember.strFieldType}");
                continue;
            }

            FieldTypeData pFieldData_Dependency = listField.Where(pFieldData => pFieldData.strFieldName == pMember.strDependencyFieldName).FirstOrDefault();
            if (pFieldData_Dependency == null)
            {
                Debug.LogError($"Error {pMember.strFieldName} - Dependency Not Found - Name : {pMember.strDependencyFieldName}");
                continue;
            }

            string strDependencyValue = (string)pInstanceData[pFieldData_Dependency.strFieldName];

            if (pType_Field.IsEnum)
            {
                if (string.IsNullOrEmpty(strDependencyValue))
                {
                    Debug.LogWarning("TODO string null check");
                    continue;
                }

                pFieldInfo.SetValue(pSO, System.Enum.Parse(pType_Field, strDependencyValue));
            }
            else
            {
                FieldTypeData pFieldData_Dependency_Sub = listField.Where(pFieldData => pFieldData.strFieldName == pMember.strDependencyFieldName_Sub).FirstOrDefault();
                if (pFieldData_Dependency_Sub != null)
                {
                    string strDependencyValue_Sub = (string)pInstanceData[pFieldData_Dependency.strFieldName];

                    UnityEngine.Object[] arrObject = AssetDatabase.LoadAllAssetsAtPath(strDependencyValue);
                    if (arrObject == null || arrObject.Length == 0)
                        Debug.LogError($"Value Is Null Or Empty - Type : {pMember.strFieldType} {strDependencyValue}");


                    var pObject = arrObject.Where(p => p.name == strDependencyValue_Sub).FirstOrDefault();
                    if (pObject == null)
                    {
                        Debug.LogError($"Value Is Null Or Empty - Type : {pMember.strFieldType} {strDependencyValue}");
                    }

                    pFieldInfo.SetValue(pSO, pObject);
                }
                else
                {
                    UnityEngine.Object pObject = AssetDatabase.LoadAssetAtPath(strDependencyValue, pType_Field);
                    if (pObject == null)
                        Debug.LogError($"Value Is Null Or Empty - Type : {pMember.strFieldType} {strDependencyValue}");
                    pFieldInfo.SetValue(pSO, pObject);
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

            TextAsset pJsonFile = AssetDatabase.LoadAssetAtPath<TextAsset>($"{UnitySO_GeneratorConfig.instance.strJsonRootFolderPath}/{strFileName}");
            JObject pObject = (JObject)JsonConvert.DeserializeObject(pJsonFile.text);
            arrObject = (JArray)pObject.GetValue("array");
        }
        catch
        {
        }

        return arrObject;
    }


    private static bool GetData_FromJson<T>(string strFileName, ref T pData)
    {
        try
        {
            if (strFileName.Contains(".json") == false)
                strFileName += ".json";

            TextAsset pJsonFile = AssetDatabase.LoadAssetAtPath<TextAsset>($"{UnitySO_GeneratorConfig.instance.strJsonRootFolderPath}/{strFileName}");
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
    public static System.Type GetTypeFromAssemblies(string strTypeName)
    {
        var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
        for (int i = 0; i < assemblies.Length; i++)
        {
            var arrType = assemblies[i].GetTypes();
            if (arrType.Length == 0)
                continue;

            var pFindType = arrType.Where(pType => pType.FullName.Equals(strTypeName)).FirstOrDefault();
            if (pFindType != null)
                return pFindType;
        }

        return null;
    }

    #endregion Private
}