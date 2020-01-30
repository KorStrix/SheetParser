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
using System.Threading.Tasks;
using SpreadSheetParser;
using System.Threading;

/// <summary>
/// Editor 폴더 안에 위치해야 합니다.
/// </summary>
public class UnitySO_Generator : EditorWindow
{
    /* const & readonly declaration             */

    /* enum & struct declaration                */

    public class SOInstance
    {
        public TypeData pTypeData;
        public ScriptableObject pSO_Container;
        public List<ScriptableObject> listSO = new List<ScriptableObject>();

        public SOInstance(TypeData pTypeData, ScriptableObject pSO_Container)
        {
            this.pTypeData = pTypeData; this.pSO_Container = pSO_Container;
        }
    }

    public class Refrence_OtherSO_Data
    {
        public ScriptableObject pSOInstance;
        public FieldInfo pFieldInfo;

        public System.Type pType_OtherSO;
        public string strValue;

        public Refrence_OtherSO_Data(ScriptableObject pObjectInstance, FieldInfo pFieldInfo, System.Type pType_OtherSO, string strValue)
        {
            this.pSOInstance = pObjectInstance; this.pFieldInfo = pFieldInfo; this.pType_OtherSO = pType_OtherSO; this.strValue = strValue;
        }
    }

    #region Container_Caching
    public abstract class Container_CachingLogicBase
    {
        protected ScriptableObject _pContainerInstance { get; private set; }
        protected FieldInfo _pFieldInfo { get; private set; }
        protected Dictionary<string, FieldInfo> mapFieldInfo_SO { get; private set; }

        public Container_CachingLogicBase(ScriptableObject pContainerInstance, FieldInfo pFieldInfo_CachedContainer, Dictionary<string, FieldInfo> mapFieldInfo_SO)
        {
            this._pContainerInstance = pContainerInstance;
            this._pFieldInfo = pFieldInfo_CachedContainer;
        }

        abstract public void Process_CachingLogic(object pObject, List<FieldTypeData> listFieldData);
    }

    public class Container_CachingLogic_List : Container_CachingLogicBase
    {
        MethodInfo _pMethod_Add;
        object _pInstance;

        public Container_CachingLogic_List(ScriptableObject pContainerInstance, FieldInfo pFieldInfo_CachedContainer, Dictionary<string, FieldInfo> mapFieldInfo_SO) : base(pContainerInstance, pFieldInfo_CachedContainer, mapFieldInfo_SO)
        {
            _pMethod_Add = pFieldInfo_CachedContainer.FieldType.GetMethod("Add");
            _pInstance = System.Activator.CreateInstance(pFieldInfo_CachedContainer.FieldType);
            pFieldInfo_CachedContainer.SetValue(pContainerInstance, _pInstance);
        }

        public override void Process_CachingLogic(object pObject, List<FieldTypeData> listFieldData)
        {
            _pMethod_Add.Invoke(_pInstance, new object[] { pObject });
        }
    }

    public class Container_CachingLogic_Dictionary_Single : Container_CachingLogicBase
    {
        MethodInfo _pMethod_Add;
        FieldInfo _pKeyFieldInfo_Instance;
        object _pInstance;

        public Container_CachingLogic_Dictionary_Single(ScriptableObject pContainerInstance, FieldInfo pFieldInfo_CachedContainer, Dictionary<string, FieldInfo> mapFieldInfo_SO) : base(pContainerInstance, pFieldInfo_CachedContainer, mapFieldInfo_SO)
        {
            System.Type[] arrGenericArguments = pFieldInfo_CachedContainer.FieldType.GetGenericArguments();

            _pMethod_Add = pFieldInfo_CachedContainer.FieldType.GetMethod("Add", new[] {
                                arrGenericArguments[0], arrGenericArguments[1]});
            _pInstance = System.Activator.CreateInstance(pFieldInfo_CachedContainer.FieldType);
            pFieldInfo_CachedContainer.SetValue(pContainerInstance, _pInstance);

            string strContainTargetName = pFieldInfo_CachedContainer.Name.Replace("mapData_Key_Is_", "");
            _pKeyFieldInfo_Instance = mapFieldInfo_SO.Where(p => p.Key == strContainTargetName).FirstOrDefault().Value;
            if (_pKeyFieldInfo_Instance == null)
            {
                Debug.LogError($"Not Found Field Instance Type : {pContainerInstance.GetType()} - {strContainTargetName}");
            }
        }

        public override void Process_CachingLogic(object pObject, List<FieldTypeData> listFieldData)
        {
            if (_pKeyFieldInfo_Instance == null)
                return;

            object pKeyValue = _pKeyFieldInfo_Instance.GetValue(pObject);
            if (pKeyValue == null)
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

    public class Container_CachingLogic_Dictionary_List : Container_CachingLogicBase
    {
        MethodInfo _pMethod_Dictionary_Add;
        MethodInfo _pMethod_Dictionary_ContainKey;
        MethodInfo _pMethod_Dictionary_GetItem;

        MethodInfo _pMethod_List_Add;


        FieldInfo _pKeyFieldInfo_Instance;

        System.Type _pTypeList;
        object _pInstance;

        public Container_CachingLogic_Dictionary_List(ScriptableObject pContainerInstance, FieldInfo pFieldInfo_CachedContainer, Dictionary<string, FieldInfo> mapFieldInfo_SO) : base(pContainerInstance, pFieldInfo_CachedContainer, mapFieldInfo_SO)
        {
            System.Type pDictionaryType = pFieldInfo_CachedContainer.FieldType;

            System.Type[] arrGenericArguments = pDictionaryType.GetGenericArguments();
            _pTypeList = arrGenericArguments[1];

            _pMethod_Dictionary_Add = pDictionaryType.GetMethod("Add");
            _pMethod_Dictionary_ContainKey = pDictionaryType.GetMethod("ContainsKey");
            _pMethod_Dictionary_GetItem = pDictionaryType.GetMethod("get_Item");
            _pMethod_List_Add = _pTypeList.GetMethod("Add");

            _pInstance = System.Activator.CreateInstance(pFieldInfo_CachedContainer.FieldType);

            pFieldInfo_CachedContainer.SetValue(pContainerInstance, _pInstance);

            string strContainTargetName = pFieldInfo_CachedContainer.Name.Replace("mapData_Key_Is_", "");
            _pKeyFieldInfo_Instance = mapFieldInfo_SO.Where(p => p.Key == strContainTargetName).FirstOrDefault().Value;
            if (_pKeyFieldInfo_Instance == null)
            {
                Debug.LogError($"Not Found Field Instance {strContainTargetName}");
            }
        }

        public override void Process_CachingLogic(object pObject, List<FieldTypeData> listFieldData)
        {
            if (_pKeyFieldInfo_Instance == null)
                return;

            object pKeyValue = _pKeyFieldInfo_Instance.GetValue(pObject);

            object[] pKey = new object[] { pKeyValue };
            bool bIsContainKey = (bool)_pMethod_Dictionary_ContainKey.Invoke(_pInstance, pKey);
            if (bIsContainKey == false)
            {
                var pListInstanceNew = System.Activator.CreateInstance(_pTypeList);
                _pMethod_Dictionary_Add.Invoke(_pInstance, new object[] { pKeyValue, pListInstanceNew });
            }

            object pListInstance = _pMethod_Dictionary_GetItem.Invoke(_pInstance, pKey);
            _pMethod_List_Add.Invoke(pListInstance, new object[] { pObject });
        }
    }
    #endregion Container

    /* public - Field declaration            */

    /* protected & private - Field declaration         */


    static Dictionary<System.Type, SOInstance> _mapSOInstance = new Dictionary<System.Type, SOInstance>();
    static HashSet<Refrence_OtherSO_Data> _setReference_OtherSO = new HashSet<Refrence_OtherSO_Data>();

    static SpreadSheetParser.SpreadSheetConnector _pConnector = new SpreadSheetParser.SpreadSheetConnector();
    static Work_Generate_Json _pWork_Json = new Work_Generate_Json();
    static Work_Generate_Unity_ScriptableObject _pWork_SO = new Work_Generate_Unity_ScriptableObject();

    static List<SheetWrapper> _listSheet = new List<SheetWrapper>();
    static TypeDataList _pTypeDataList = new TypeDataList();

    // ========================================================================== //

    /* public - [Do] Function
     * 외부 객체가 호출(For External class call)*/

    [MenuItem("Tools/Scriptable Object Generator")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        UnitySO_Generator window = (UnitySO_Generator)GetWindow(typeof(UnitySO_Generator), false);

        window.minSize = new Vector2(800, 300);
        window.Show();

        UnitySO_GeneratorConfig pConfig = UnitySO_GeneratorConfig.instance;
        if (string.IsNullOrEmpty(pConfig.strSheetID) == false)
        {
            DoConnect();
        }
    }

    static public async Task DoConnect()
    {
        UnitySO_GeneratorConfig pConfig = UnitySO_GeneratorConfig.instance;
        _listSheet.Clear();

        await _pConnector.DoConnect(pConfig.strSheetID, 
            (string strSheetID, ESpreadSheetType eSheetType, List < SheetWrapper> listSheet, Exception pException_OnError) => 
            {
                if (pException_OnError != null)
                {
                    Debug.LogError(pException_OnError);
                    return;
                }

                Debug.Log("Success Connect - " + strSheetID);

                _pTypeDataList = new TypeDataList();
                if (GetData_FromJson(nameof(TypeDataList), ref _pTypeDataList) == false)
                {
                    Debug.LogError("TypeDataList JsonParsing Fail");
                    return;
                }

                _listSheet.AddRange(listSheet);
            },
            pConfig.strCredentialFilePath
            );
    }

    static public void DoUpdate_And_Build()
    {
        Debug.Log("DoUpdate Start");

        UnitySO_GeneratorConfig pConfig = UnitySO_GeneratorConfig.instance;
        _pWork_Json.strExportPath = pConfig.strJsonRootFolderPath;
        _pWork_SO.strExportPath = pConfig.strJsonRootFolderPath;

        CodeFileBuilder pCodeFileBuilder = new CodeFileBuilder();
        IEnumerable<TypeData> arrTypeData = _pTypeDataList.listTypeData.Where(p => p.bEnable);
        foreach(var pTypeDatta in arrTypeData)
            pTypeDatta.DoWork(_pConnector, pCodeFileBuilder, null);

        _pWork_Json.DoWork(pCodeFileBuilder, _pConnector, arrTypeData, null);
        _pWork_SO.DoWork(pCodeFileBuilder, _pConnector, arrTypeData, null);
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        Debug.Log(" EditorApplication.isCompiling : " + EditorApplication.isCompiling);

        DoBuild();
        Debug.Log("DoUpdate Finish");
    }

    static public void DoBuild()
    {
        Debug.Log("Build Start");

        UnitySO_GeneratorConfig pConfig = UnitySO_GeneratorConfig.instance;

        _pTypeDataList = new TypeDataList();
        if(GetData_FromJson(nameof(TypeDataList), ref _pTypeDataList) == false)
        {
            Debug.LogError("TypeDataList JsonParsing Fail");
            return;
        }

        _mapSOInstance.Clear();
        _setReference_OtherSO.Clear();
        foreach (TypeData pTypeData in _pTypeDataList.listTypeData)
        {
            string strTypeName = pTypeData.strFileName;
            System.Type pType_SO = GetTypeFromAssemblies(strTypeName, typeof(UnityEngine.ScriptableObject));
            if (pType_SO == null)
            {
                Debug.LogError($"pType_SO == null - {strTypeName} - {strTypeName}");
                continue;
            }

            JArray pJArray_Instance = GetDataArray_FromJson(strTypeName);
            if (pJArray_Instance.Count == 0)
            {
                Debug.LogError("Error");
                continue;
            }

            System.Type pType_Container = GetTypeFromAssemblies(strTypeName + "_Container");
            ScriptableObject pContainerInstance = (ScriptableObject)UnitySO_GeneratorConfig.CreateSOFile(pType_Container, pConfig.strExportFolderPath + "/" + strTypeName + "_Container", true);

            Dictionary<string, System.Reflection.FieldInfo> mapFieldInfo_SO = pType_SO.GetFields().ToDictionary((pFieldInfo) => pFieldInfo.Name);

            _mapSOInstance.Add(pType_SO, new SOInstance(pTypeData, pContainerInstance));

            List<Container_CachingLogicBase> listCachingLogic = new List<Container_CachingLogicBase>();
            IEnumerable<System.Reflection.FieldInfo> arrFieldInfo_Container = pType_Container.GetFields();
            foreach (var pFieldInfo in arrFieldInfo_Container)
            {
                System.Type pTypeField = pFieldInfo.FieldType;
                System.Type pTypeField_Generic = pTypeField.GetGenericTypeDefinition();
                if (pTypeField_Generic == typeof(List<>))
                {
                    listCachingLogic.Add(new Container_CachingLogic_List(pContainerInstance, pFieldInfo, mapFieldInfo_SO));
                }
                //else if (pTypeField_Generic == typeof(Dictionary<,>))
                //{
                //    System.Type[] arrGenericArguments = pTypeField.GetGenericArguments();

                //    if(arrGenericArguments[1].IsGenericType)
                //        listCachingLogic.Add(new Container_CachingLogic_Dictionary_List(pContainerInstance, pFieldInfo, mapFieldInfo_SO));
                //    else
                //        listCachingLogic.Add(new Container_CachingLogic_Dictionary_Single(pContainerInstance, pFieldInfo, mapFieldInfo_SO));
                 
                //}
            }

            Generate_SOInstance(pTypeData, pType_SO, pJArray_Instance, pContainerInstance, mapFieldInfo_SO, listCachingLogic, pTypeData.listFieldData);
        }

        foreach(Refrence_OtherSO_Data pReference_OtherSOData in _setReference_OtherSO)
        {
            SOInstance pOtherSO_Instance;
            if (_mapSOInstance.TryGetValue(pReference_OtherSOData.pType_OtherSO, out pOtherSO_Instance) == false)
                continue;

            var pObject = pOtherSO_Instance.listSO.Where(p => p.name == pReference_OtherSOData.strValue).FirstOrDefault();
            if(pObject == null)
            {
                Debug.LogError($"Not Found {pReference_OtherSOData.strValue}");
                continue;
            }

            pReference_OtherSOData.pFieldInfo.SetValue(pReference_OtherSOData.pSOInstance, pObject);
            EditorUtility.SetDirty(pReference_OtherSOData.pSOInstance);
        }

        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        Debug.Log("Build Finish");
    }

    private static void Generate_SOInstance(TypeData pTypeData, Type pType_SO, JArray pJArray_Instance, ScriptableObject pContainerInstance, Dictionary<string, FieldInfo> mapFieldInfo_SO, List<Container_CachingLogicBase> listCachingLogic, List<FieldTypeData> listFieldData)
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
            EditorUtility.SetDirty(pSO);

            for (int i = 0; i < listCachingLogic.Count; i++)
            {
                try
                {
                    listCachingLogic[i].Process_CachingLogic(pSO, listFieldData);
                }
                catch (System.Exception e)
                {
                    listCachingLogic[i].Process_CachingLogic(pSO, listFieldData);
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

        DrawPath_File(pConfig, "Cre", ref pConfig.strCredentialFilePath);
        DrawPath_Folder(pConfig,  "JsonFolder Path", ref pConfig.strJsonRootFolderPath);
        DrawPath_Folder(pConfig, "SO Export Path", ref pConfig.strExportFolderPath);

        bool bEditorEnable = string.IsNullOrEmpty(pConfig.strJsonRootFolderPath) == false;
        EditorGUI.BeginDisabledGroup(bEditorEnable == false);

        if (GUILayout.Button("Build!"))
        {
            DoBuild();
        }

        GUILayout.Space(30f);

        GUILayout.BeginHorizontal();
        GUILayout.Label($"SheetID : ", GUILayout.Width(100f));

        EditorGUI.BeginChangeCheck();
        pConfig.strSheetID = GUILayout.TextField(pConfig.strSheetID);
        if(EditorGUI.EndChangeCheck())
            pConfig.DoSave();

        if (GUILayout.Button("Connect!", GUILayout.Width(100f)))
        {
            DoConnect();
        }
        GUILayout.EndHorizontal();

        if(_pConnector.pService != null && _pTypeDataList.listTypeData.Count > 0)
        {
            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < _pTypeDataList.listTypeData.Count; i++)
            {
                TypeData pTypeData = _pTypeDataList.listTypeData[i];
                pTypeData.bEnable = EditorGUILayout.Toggle(pTypeData.strFileName, pTypeData.bEnable);
            }

            if (EditorGUI.EndChangeCheck())
            {
                // JsonSaveManager.SaveData(_pTypeDataList, $"{UnitySO_GeneratorConfig.instance.strJsonRootFolderPath}/{nameof(TypeDataList)}.json");
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Update!", GUILayout.Width(100f)))
            {
                DoUpdate_And_Build();
            }
        }
    }

    /* protected - [abstract & virtual]         */


    // ========================================================================== //

    #region Private


    private static ScriptableObject GenerateSO(System.Type pType_SO, ScriptableObject pContainerInstance)
    {
        ScriptableObject pSO = (ScriptableObject)UnitySO_GeneratorConfig.CreateInstance(pType_SO);
        _mapSOInstance[pType_SO].listSO.Add(pSO);

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
                        else
                            _setReference_OtherSO.Add(new Refrence_OtherSO_Data(pSO, pFieldInfo, pType_Field, strValue));

                        if (pType_Field == null)
                            Debug.LogWarning($"아직 지원되지 않은 형식.. {pMember.strFieldType}");
                        break;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Parsing  Fail - {pTypeData.strFileName}/{pMember.strFieldType} {pMember.strFieldName} : {strValue}\n" +
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
                Debug.LogError($"{pMember.strFieldName} - Field Type Not Found - {pMember.strFieldType}");
                continue;
            }

            FieldTypeData pFieldData_Dependency = listField.Where(pFieldData => pFieldData.strFieldName == pMember.strDependencyFieldName).FirstOrDefault();
            if (pFieldData_Dependency == null)
            {
                Debug.LogError($"{pMember.strFieldName} - Dependency Not Found - Name : {pMember.strDependencyFieldName}");
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
                    string strDependencyValue_Sub = (string)pInstanceData[pFieldData_Dependency_Sub.strFieldName];

                    UnityEngine.Object[] arrObject = AssetDatabase.LoadAllAssetsAtPath(strDependencyValue);
                    if (arrObject == null || arrObject.Length == 0)
                        Debug.LogError($"Value Is Null Or Empty - Type : {pMember.strFieldType} {strDependencyValue}");


                    var pObject = arrObject.Where(p => p.name == strDependencyValue_Sub).FirstOrDefault();
                    if (pObject == null)
                    {
                        Debug.LogError($"{pTypeData.strFileName} - DependencyValue Sub {strDependencyValue_Sub} Is Null");
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
            if(pFindType == null)
                pFindType = arrType.Where(pType => pType.Name.Equals(strTypeName)).FirstOrDefault();

            if (pFindType != null)
                return pFindType;
        }

        for (int i = 0; i < assemblies.Length; i++)
        {
            var arrType = assemblies[i].GetTypes();
            if (arrType.Length == 0)
                continue;

            var pFindType = arrType.Where(pType => pType.Name.Equals(strTypeName)).FirstOrDefault();

            if (pFindType != null)
                return pFindType;
        }

        return null;
    }

    public static System.Type GetTypeFromAssemblies(string strTypeName, System.Type pBaseType)
    {
        var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
        for (int i = 0; i < assemblies.Length; i++)
        {
            var arrType = assemblies[i].GetTypes();
            if (arrType.Length == 0)
                continue;

            var pFindType = arrType.Where(pType => pType.FullName.Equals(strTypeName) && pType.BaseType == pBaseType).FirstOrDefault();
            if (pFindType == null)
                pFindType = arrType.Where(pType => pType.Name.Equals(strTypeName) && pType.BaseType == pBaseType).FirstOrDefault();

            if (pFindType != null)
                return pFindType;
        }

        return null;
    }

    private void DrawPath_Folder(UnitySO_GeneratorConfig pConfig, string strExplaneName, ref string strFolderPath, bool bIsRelative =  true)
    {
        DrawPath(pConfig, strExplaneName, ref strFolderPath, true, bIsRelative);
    }

    private void DrawPath_File(UnitySO_GeneratorConfig pConfig, string strExplaneName, ref string strFilePath, bool bIsRelative = true)
    {
        DrawPath(pConfig, strExplaneName, ref strFilePath, false, bIsRelative);
    }

    private void  DrawPath(UnitySO_GeneratorConfig pConfig, string strExplaneName, ref string strEditPath, bool bIsFolder, bool bIsRealtive)
    {
        GUILayout.BeginHorizontal();

        if(bIsFolder)
            GUILayout.Label($"{strExplaneName} Folder Path : ", GUILayout.Width(200f));
        else
            GUILayout.Label($"{strExplaneName} File Path : ", GUILayout.Width(200f));

        GUILayout.Label(strEditPath, GUILayout.Width(500f));

        if (GUILayout.Button($"Edit {strExplaneName}"))
        {
            string strPath = "";
            if(bIsFolder)
                strPath = EditorUtility.OpenFolderPanel("Root Folder", "", "");
            else
                strPath = EditorUtility.OpenFilePanel("File Path", "", "");

            if(bIsRealtive)
            {
                System.Uri pCurrentURI = new System.Uri(Application.dataPath);
                strEditPath = pCurrentURI.MakeRelativeUri(new System.Uri(strPath)).ToString();
            }
            else
            {
                strEditPath = strPath;
            }

            pConfig.DoSave();
        }

        GUILayout.EndHorizontal();
    }
    static string GetRelative_To_AbsolutePath(string strPath)
    {
        if (Path.IsPathRooted(strPath))
            return strPath;

        string strRelativePath = $"{new Uri(Application.dataPath).AbsolutePath}{"/../"}";
        return strRelativePath + strPath;
    }

    #endregion Private
}