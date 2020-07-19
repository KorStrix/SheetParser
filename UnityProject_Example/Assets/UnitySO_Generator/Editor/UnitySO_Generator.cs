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
public partial class UnitySO_Generator : EditorWindow
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

        public Container_CachingLogicBase(ScriptableObject pContainerInstance, FieldInfo pFieldInfo_CachedContainer)
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

        public Container_CachingLogic_List(ScriptableObject pContainerInstance, FieldInfo pFieldInfo_CachedContainer) : base(pContainerInstance, pFieldInfo_CachedContainer)
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
    static TypeDataList _pTypeDataList;

    static string _strSheetID_Connected;

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

        await _pConnector.DoConnect(pConfig.strSheetID, 
            (strSheetID, strFileName, eSheetType, listSheet, pException_OnError) => 
            {
                _strSheetID_Connected = strSheetID;
                if (pException_OnError != null)
                {
                    Debug.LogError(pException_OnError);
                    return;
                }
                Debug.Log("Success Connect - " + strSheetID);
            },
            pConfig.strCredential_FilePath
            );
    }

    public static void DoDownload_And_Update()
    {
        Debug.Log($"{nameof(DoDownload_And_Update)} Start");

        Download(_pTypeDataList.listTypeData.Where(p => p.bEnable).ToArray());
        DoUpdate_FromLocalFile();
        Debug.Log($"{nameof(DoDownload_And_Update)} Finish");
    }

    public static void DoUpdate_FromLocalFile()
    {
        Stopwatch pStopWatch = new Stopwatch();
        pStopWatch.Start();

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
        Debug.Log($"{nameof(DoUpdate_FromLocalFile)} Finish Elapse : " + pStopWatch.Elapsed);
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

        if (_pTypeDataList == null)
        {
            ParsingConfig(pConfig);
        }

        GUILayout.BeginHorizontal();
        {
            EditorGUI.BeginChangeCheck();
            pConfig.pTypeDataFile = (TextAsset)EditorGUILayout.ObjectField("TypeData File : ", pConfig.pTypeDataFile, typeof(TextAsset), false, GUILayout.Width(700f));
            if (EditorGUI.EndChangeCheck())
                pConfig.DoSave();

            if (GUILayout.Button("Parsing File"))
            {
                if (ParsingConfig(pConfig) == false)
                {
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

        bool bIsPossibleUpdate = Check_IsPossible_Update(pConfig);
        GUI.enabled = bIsPossibleUpdate;

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
#pragma warning disable 4014
                DoConnect();
#pragma warning restore 4014
            }
        }
        GUILayout.EndHorizontal();

        if(_pConnector.bIsConnected)
            GUILayout.Label($"Excel is Connected : {_strSheetID_Connected} - Sheet List");
        else
            GUILayout.Label($"Excel is Not Connected", GetRedGUIStyle());


        GUILayout.Space(30f);

        if (_pTypeDataList == null)
            return;

        bool bIsMatchSheetID = Check_IsMatch_SheetID();
        bool bIsPossibleDownload = Check_IsPossibleDownload(bIsMatchSheetID);

        DrawSheetsScroll(pConfig, bIsPossibleUpdate, bIsPossibleDownload);

        GUILayout.Space(30f);

        if(bIsMatchSheetID == false)
            GUILayout.Label($"SheetID Is Not Match - Local : {_pTypeDataList.strFileName}, Current Connected : {_strSheetID_Connected}", GetRedGUIStyle());

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label($"CommandLine : ", GUILayout.Width(100f));

            EditorGUI.BeginChangeCheck();
            pConfig.strSOCommandLine = GUILayout.TextField(pConfig.strSOCommandLine);
            if (EditorGUI.EndChangeCheck())
                pConfig.DoSave();
        }
        GUILayout.EndHorizontal();

        GUI.enabled = bIsPossibleDownload;
        if (GUILayout.Button("Download And Update", GUILayout.Width(200f)))
        {
            DoDownload_And_Update();
        }
    }

    /* protected - [abstract & virtual]         */


    // ========================================================================== //

    #region Private

    private static bool ParsingConfig(UnitySO_GeneratorConfig pConfig)
    {
        _pTypeDataList = new TypeDataList("");
        if (GetData_FromJson(pConfig.pTypeDataFile, ref _pTypeDataList) == false)
        {
            _pTypeDataList = null;
            return false;
        }

        return true;
    }

    private static void Download(params TypeData[] arrTypeData)
    {
        SettingWork_FromConfig();
        CodeFileBuilder pCodeFileBuilder = new CodeFileBuilder();

        foreach (var pTypeData in arrTypeData)
            pTypeData.DoWork(_pConnector, pCodeFileBuilder, null);

        _pWork_Json.DoWork(pCodeFileBuilder, _pConnector, arrTypeData, null);
        _pWork_SO.DoWork(pCodeFileBuilder, _pConnector, arrTypeData, null);
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        Debug.Log(" EditorApplication.isCompiling : " + EditorApplication.isCompiling);
    }

    private static void SettingWork_FromConfig()
    {
        UnitySO_GeneratorConfig pConfig = UnitySO_GeneratorConfig.instance;
        _pWork_Json.strExportPath = pConfig.strJsonData_FolderPath;
        _pWork_SO.strExportPath = pConfig.strSOScript_FolderPath;
        _pWork_SO.strCommandLine = pConfig.strSOCommandLine;
    }

    private Vector3 _vecScrollVector;

    private void DrawSheetsScroll(UnitySO_GeneratorConfig pConfig, bool bIsPossibleUpdate, bool bIsPossibleDownload)
    {
        GUILayout.BeginHorizontal(GUI.skin.box);
        _vecScrollVector = EditorGUILayout.BeginScrollView(_vecScrollVector);
        for (int i = 0; i < _pTypeDataList.listTypeData.Count; i++)
        {
            GUILayout.BeginHorizontal();

            TypeData pTypeData = _pTypeDataList.listTypeData[i];
            pTypeData.bEnable = EditorGUILayout.Toggle(pTypeData.strFileName, pTypeData.bEnable);

            GUI.enabled = bIsPossibleDownload;
            if (GUILayout.Button("Download And Update"))
            {
                SettingWork_FromConfig();
                Download(pTypeData);
                ClearData();
                Generate_TypeToSO(pTypeData, pConfig);
            }

            GUI.enabled = bIsPossibleUpdate;
            if (GUILayout.Button("Update From Local"))
            {
                ClearData();
                Generate_TypeToSO(pTypeData, pConfig);
            }

            GUILayout.EndHorizontal();
        }

        GUI.enabled = true;
        EditorGUILayout.EndScrollView();
        GUILayout.EndHorizontal();
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

    private static GUIStyle GetRedGUIStyle()
    {
        GUIStyle pStyle = new GUIStyle();
        pStyle.normal.textColor = Color.red;

        return pStyle;
    }

    private static bool Check_IsPossible_Update(UnitySO_GeneratorConfig pConfig)
    {
        return _pTypeDataList != null && string.IsNullOrEmpty(pConfig.strJsonData_FolderPath) == false;
    }

    private static bool Check_IsMatch_SheetID()
    {
        return _pTypeDataList.strFileName == _strSheetID_Connected;
    }

    private static bool Check_IsPossibleDownload(bool bIsMatchSheetID)
    {
        return _pConnector.bIsConnected && _pTypeDataList.listTypeData.Count > 0 && bIsMatchSheetID;
    }

    #endregion Private
}
