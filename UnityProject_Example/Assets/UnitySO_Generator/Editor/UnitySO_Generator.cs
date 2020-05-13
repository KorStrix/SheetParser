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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using SpreadSheetParser;

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

        public Type pType_OtherSO;
        public string strValue;

        public Refrence_OtherSO_Data(ScriptableObject pObjectInstance, FieldInfo pFieldInfo, Type pType_OtherSO, string strValue)
        {
            pSOInstance = pObjectInstance; this.pFieldInfo = pFieldInfo; this.pType_OtherSO = pType_OtherSO; this.strValue = strValue;
        }
    }

    #region Container_Caching
    public abstract class Container_CachingLogicBase
    {
        protected ScriptableObject _pContainerInstance { get; private set; }
        protected FieldInfo _pFieldInfo { get; private set; }
        protected Dictionary<string, FieldInfo> mapFieldInfo_SO { get; private set; } = new Dictionary<string, FieldInfo>();

        public Container_CachingLogicBase(ScriptableObject pContainerInstance, FieldInfo pFieldInfo_CachedContainer, Dictionary<string, FieldInfo> mapFieldInfo_SO)
        {
            _pContainerInstance = pContainerInstance;
            _pFieldInfo = pFieldInfo_CachedContainer;
        }

        public abstract void Process_CachingLogic(object pObject, List<FieldTypeData> listFieldData);
    }

    public class Container_CachingLogic_List : Container_CachingLogicBase
    {
        readonly MethodInfo _pMethod_Add;
        readonly object _pInstance;

        public Container_CachingLogic_List(ScriptableObject pContainerInstance, FieldInfo pFieldInfo_CachedContainer, Dictionary<string, FieldInfo> mapFieldInfo_SO) : base(pContainerInstance, pFieldInfo_CachedContainer, mapFieldInfo_SO)
        {
            _pMethod_Add = pFieldInfo_CachedContainer.FieldType.GetMethod("Add");
            _pInstance = Activator.CreateInstance(pFieldInfo_CachedContainer.FieldType);
            pFieldInfo_CachedContainer.SetValue(pContainerInstance, _pInstance);
        }

        public override void Process_CachingLogic(object pObject, List<FieldTypeData> listFieldData)
        {
            _pMethod_Add.Invoke(_pInstance, new[] { pObject });
        }
    }


    #endregion Container

    /* public - Field declaration            */

    /* protected & private - Field declaration         */


    static Dictionary<Type, SOInstance> _mapSOInstance = new Dictionary<Type, SOInstance>();
    static HashSet<Refrence_OtherSO_Data> _setReference_OtherSO = new HashSet<Refrence_OtherSO_Data>();

    static SpreadSheetConnector _pConnector = new SpreadSheetConnector();
    static Work_Generate_Json _pWork_Json = new Work_Generate_Json();
    static Work_Generate_Unity_ScriptableObject _pWork_SO = new Work_Generate_Unity_ScriptableObject();

    static List<SheetWrapper> _listSheet = new List<SheetWrapper>();
    static TypeDataList _pTypeDataList;

    static string _strFileName;

    // ========================================================================== //

    /* public - [Do] Function
     * 외부 객체가 호출(For External class call)*/

    [MenuItem("Tools/Scriptable Object Generator")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        UnitySO_Generator window = (UnitySO_Generator)GetWindow(typeof(UnitySO_Generator), false);

        window.minSize = new Vector2(900, 300);
        window.Show();

        UnitySO_GeneratorConfig pConfig = UnitySO_GeneratorConfig.instance;
        if (string.IsNullOrEmpty(pConfig.strSheetID) == false)
        {
#pragma warning disable CS4014 // 이 호출을 대기하지 않으므로 호출이 완료되기 전에 현재 메서드가 계속 실행됩니다.
            DoConnect();
#pragma warning restore CS4014 // 이 호출을 대기하지 않으므로 호출이 완료되기 전에 현재 메서드가 계속 실행됩니다.
        }
    }

    public static async Task DoConnect()
    {
        UnitySO_GeneratorConfig pConfig = UnitySO_GeneratorConfig.instance;
        _listSheet.Clear();

        await _pConnector.DoConnect(pConfig.strSheetID, 
            (strSheetID, strFileName, eSheetType, listSheet, pException_OnError) => 
            {
                _strFileName = strFileName;
                if (pException_OnError != null)
                {
                    Debug.LogError(pException_OnError);
                    return;
                }

                _listSheet.AddRange(listSheet);
                Debug.Log("Success Connect - " + strSheetID);
            },
            pConfig.strCredential_FilePath
            );
    }

    public static void DoDownload_And_Update()
    {
        Debug.Log($"{nameof(DoDownload_And_Update)} Start");

        UnitySO_GeneratorConfig pConfig = UnitySO_GeneratorConfig.instance;
        _pWork_Json.strExportPath = pConfig.strJsonData_FolderPath;
        _pWork_SO.strExportPath = pConfig.strSOScript_FolderPath;
        _pWork_SO.strCommandLine = pConfig.strSOCommandLine;

        
        CodeFileBuilder pCodeFileBuilder = new CodeFileBuilder();
        IEnumerable<TypeData> arrTypeData = _pTypeDataList.listTypeData.Where(p => p.bEnable);
        foreach(var pTypeDatta in arrTypeData)
            pTypeDatta.DoWork(_pConnector, pCodeFileBuilder, null);

        _pWork_Json.DoWork(pCodeFileBuilder, _pConnector, arrTypeData, null);
        _pWork_SO.DoWork(pCodeFileBuilder, _pConnector, arrTypeData, null);
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        Debug.Log(" EditorApplication.isCompiling : " + EditorApplication.isCompiling);

        DoUpdate_FromLocalFile();
        Debug.Log($"{nameof(DoDownload_And_Update)} Finish");
    }

    public static void DoUpdate_FromLocalFile()
    {
        Debug.Log($"{nameof(DoUpdate_FromLocalFile)} Start");

        if(_pTypeDataList == null || _pTypeDataList.listTypeData.Count == 0)
        {
            Debug.LogError("Error");
            return;
        }

        UnitySO_GeneratorConfig pConfig = UnitySO_GeneratorConfig.instance;

        ClearData();
        foreach (TypeData pTypeData in _pTypeDataList.listTypeData)
            Generate_TypeToSO(pTypeData, pConfig);

        Reference_OtherSO();

        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        Debug.Log($"{nameof(DoUpdate_FromLocalFile)} Finish");
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

        GUILayout.Space(10f);
        DrawPath_File(pConfig, "Credential Path", ref pConfig.strCredential_FilePath);
        GUILayout.Space(10f);


        DrawPath_Folder(pConfig, "SOScript Folder Path", ref pConfig.strSOScript_FolderPath);
        DrawPath_Folder(pConfig,  "Json Data Folder Path", ref pConfig.strJsonData_FolderPath);
        GUILayout.Space(10f);

        DrawPath_Folder(pConfig, "SO Export Path", ref pConfig.strDataExport_FolderPath);

        GUILayout.BeginHorizontal();
        {
            EditorGUI.BeginChangeCheck();
            pConfig.pTypeDataFile = (TextAsset)EditorGUILayout.ObjectField("TypeData File : ", pConfig.pTypeDataFile, typeof(TextAsset), false, GUILayout.Width(700f));
            if (EditorGUI.EndChangeCheck())
                pConfig.DoSave();

            if (GUILayout.Button("Parsing File"))
            {
                _pTypeDataList =  new TypeDataList("");
                if (GetData_FromJson(pConfig.pTypeDataFile, ref _pTypeDataList) == false)
                {
                    _pTypeDataList = null;
                    Debug.LogError("TypeDataList JsonParsing Fail");
                    return;
                }
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(30f);

        if (_pTypeDataList != null)
            GUILayout.Label($"TypeData File is Valid");
        else
            GUILayout.Label($"TypeData File is InValid");

        GUI.enabled = _pTypeDataList !=  null && string.IsNullOrEmpty(pConfig.strJsonData_FolderPath) == false;

        if (GUILayout.Button("Update Form Local", GUILayout.Width(200f)))
        {
            DoUpdate_FromLocalFile();
        }
        GUILayout.Space(30f);

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label($"SheetID : ", GUILayout.Width(100f));

            EditorGUI.BeginChangeCheck();
            pConfig.strSheetID = GUILayout.TextField(pConfig.strSheetID);
            if (EditorGUI.EndChangeCheck())
                pConfig.DoSave();

            if (GUILayout.Button("Connect!", GUILayout.Width(100f)))
            {
                DoConnect();
            }
        }
        GUILayout.EndHorizontal();

        if(_pConnector.bIsConnected)
            GUILayout.Label($"Excel is Connected : {_strFileName} - Sheet List");
        else
            GUILayout.Label($"Excel is Not Connected");


        GUILayout.Space(30f);

        if (_pTypeDataList == null)
            return;

        DrawSheetsScroll(pConfig);

        GUILayout.Space(30f);

        bool bIsMatchFileName = _pTypeDataList.strFileName == _strFileName;
        if(bIsMatchFileName == false)
        {
            GUIStyle pStyle = new GUIStyle();
            pStyle.normal.textColor = Color.red;
            GUILayout.Label($"File Is Not Match - TypeData File : {_pTypeDataList.strFileName}, Current Connected : {_strFileName}", pStyle);
        }

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label($"CommandLine : ", GUILayout.Width(100f));

            EditorGUI.BeginChangeCheck();
            pConfig.strSOCommandLine = GUILayout.TextField(pConfig.strSOCommandLine);
            if (EditorGUI.EndChangeCheck())
                pConfig.DoSave();
        }
        GUILayout.EndHorizontal();

        GUI.enabled = _pConnector.bIsConnected && _pTypeDataList.listTypeData.Count > 0 && bIsMatchFileName;
        if (GUILayout.Button("Download And Update", GUILayout.Width(200f)))
        {
            DoDownload_And_Update();
        }
    }


    /* protected - [abstract & virtual]         */


    // ========================================================================== //

    #region Private

    private Vector3 _vecScrollVector;

    private void DrawSheetsScroll(UnitySO_GeneratorConfig pConfig)
    {
        GUILayout.BeginHorizontal(GUI.skin.box);
        _vecScrollVector = EditorGUILayout.BeginScrollView(_vecScrollVector);
        for (int i = 0; i < _pTypeDataList.listTypeData.Count; i++)
        {
            GUILayout.BeginHorizontal();

            TypeData pTypeData = _pTypeDataList.listTypeData[i];
            pTypeData.bEnable = EditorGUILayout.Toggle(pTypeData.strFileName, pTypeData.bEnable);
            if (GUILayout.Button("Build Only This Item"))
            {
                ClearData();
                Generate_TypeToSO(pTypeData, pConfig);
            }

            GUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
        GUILayout.EndHorizontal();
    }

    private static void Generate_TypeToSO(TypeData pTypeData, UnitySO_GeneratorConfig pConfig)
    {
        string strTypeName = pTypeData.strFileName;
        Type pType_SO = GetTypeFromAssemblies(strTypeName, typeof(ScriptableObject));
        if (pType_SO == null)
        {
            Debug.LogError($"pType_SO == null - {strTypeName} - {strTypeName}");
            return;
        }

        JArray pJArray_Instance = GetDataArray_FromJson(strTypeName);
        if (pJArray_Instance.Count == 0)
        {
            Debug.LogError($"{strTypeName} - pJArray_Instance.Count == 0");
            return;
        }

        GenerateSO(strTypeName, pConfig, pType_SO, pTypeData, pJArray_Instance);
    }

    private static void GenerateSO(string strTypeName, UnitySO_GeneratorConfig pConfig, Type pType_SO, TypeData pTypeData,
        JArray pJArray_Instance)
    {
        Type pType_Container = GetTypeFromAssemblies(strTypeName + "_Container");
        ScriptableObject pContainerInstance = (ScriptableObject) UnitySO_GeneratorConfig.CreateSOFile(pType_Container,
            pConfig.strDataExport_FolderPath + "/" + strTypeName + "_Container", true);

        Dictionary<string, FieldInfo> mapFieldInfo_SO = pType_SO.GetFields().ToDictionary((pFieldInfo) => pFieldInfo.Name);

        _mapSOInstance.Add(pType_SO, new SOInstance(pTypeData, pContainerInstance));

        List<Container_CachingLogicBase> listCachingLogic = new List<Container_CachingLogicBase>();
        IEnumerable<FieldInfo> arrFieldInfo_Container = pType_Container.GetFields();
        foreach (var pFieldInfo in arrFieldInfo_Container)
        {
            Type pTypeField = pFieldInfo.FieldType;
            Type pTypeField_Generic = pTypeField.GetGenericTypeDefinition();
            if (pTypeField_Generic == typeof(List<>))
                listCachingLogic.Add(new Container_CachingLogic_List(pContainerInstance, pFieldInfo, mapFieldInfo_SO));
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
                Debug.LogError($"Not Found {pReference_OtherSOData.pType_OtherSO}");
                continue;
            }

            var pObject = pOtherSO_Instance.listSO.FirstOrDefault(p => p.name == pReference_OtherSOData.strValue);
            if (pObject == null)
            {
                Debug.LogError($"Not Found {pReference_OtherSOData.strValue}");
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
        IEnumerable<FieldTypeData> listRealField = listFieldData.Where((pFieldData) => pFieldData.bIsVirtualField == false && pFieldData.bDeleteThisField_InCode == false);
        IEnumerable<FieldTypeData> listVirtualField = listFieldData.Where((pFieldData) => pFieldData.bIsVirtualField && pFieldData.bDeleteThisField_InCode == false);


        foreach (JObject pInstanceData in pJArray_Instance)
        {
            ScriptableObject pSO = GenerateSO(pType_SO, pContainerInstance);
            Process_RealField(pTypeData, mapFieldInfo_SO, pInstanceData, pSO, listRealField);
            Process_VirtualField(pTypeData, mapFieldInfo_SO, pInstanceData, pSO, listFieldData, listVirtualField);
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
        // AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(pContainerInstance));

        return pSO;
    }

    private static void Process_SOName(string strFileName, int iLoopIndex, ScriptableObject pSO, FieldTypeData pFieldHeader, JObject pInstanceData)
    {
        if(pFieldHeader != null)
            pSO.name = (string)pInstanceData[pFieldHeader.strFieldName];
        else
            pSO.name = $"{strFileName}_{iLoopIndex}";

        if (string.IsNullOrEmpty(pSO.name))
        {
            Debug.LogError($"{strFileName} - Process_SOName -  {string.IsNullOrEmpty(pSO.name)}, pFieldHeader : {pFieldHeader.strFieldName}");
        }
    }

    private static void Process_RealField(TypeData pTypeData, Dictionary<string, FieldInfo> mapFieldInfo_SO, JObject pInstanceData, ScriptableObject pSO, IEnumerable<FieldTypeData> listRealField)
    {
        foreach (var pMember in listRealField)
        {
            FieldInfo pFieldInfo;
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
                            if(bIsNumber)
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
                    if(pTypeData.eType != ESheetType.Global)
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

            var pFindType = arrType.FirstOrDefault(pType => pType.FullName.Equals(strTypeName));
            if(pFindType == null)
                pFindType = arrType.FirstOrDefault(pType => pType.Name.Equals(strTypeName));

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
            var arrType = assemblies[i].GetTypes();
            if (arrType.Length == 0)
                continue;

            var pFindType = arrType.FirstOrDefault(pType => pType.FullName.Equals(strTypeName) && pType.BaseType == pBaseType);
            if (pFindType == null)
                pFindType = arrType.FirstOrDefault(pType => pType.Name.Equals(strTypeName) && pType.BaseType == pBaseType);

            if (pFindType != null)
                return pFindType;
        }

        return null;
    }

    private void DrawPath_Folder(UnitySO_GeneratorConfig pConfig, string strExplainName, ref string strFolderPath, bool bIsRelative =  true)
    {
        DrawPath(pConfig, strExplainName, ref strFolderPath, true, bIsRelative);
    }

    private void DrawPath_File(UnitySO_GeneratorConfig pConfig, string strExplainName, ref string strFilePath, bool bIsRelative = true)
    {
        DrawPath(pConfig, strExplainName, ref strFilePath, false, bIsRelative);
    }

    private void  DrawPath(UnitySO_GeneratorConfig pConfig, string strExplainName, ref string strEditPath, bool bIsFolder, bool bIsRelative)
    {
        GUILayout.BeginHorizontal();

        if(bIsFolder)
            GUILayout.Label($"{strExplainName} Folder Path : ", GUILayout.Width(300f));
        else
            GUILayout.Label($"{strExplainName} File Path : ", GUILayout.Width(300f));

        GUILayout.Label(strEditPath, GUILayout.Width(400f));

        if (GUILayout.Button($"Edit {strExplainName}"))
        {
            string strPath = bIsFolder ? EditorUtility.OpenFolderPanel("Root Folder", "", "") : EditorUtility.OpenFilePanel("File Path", "", "");
            if(bIsRelative)
            {
                Uri pCurrentURI = new Uri(Application.dataPath);
                strEditPath = pCurrentURI.MakeRelativeUri(new Uri(strPath)).ToString();
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

        string strRelativePath = $"{new Uri(Application.dataPath).AbsolutePath}/../";
        return strRelativePath + strPath;
    }

    private static void ClearData()
    {
        _mapSOInstance.Clear();
        _setReference_OtherSO.Clear();
    }

    #endregion Private
}