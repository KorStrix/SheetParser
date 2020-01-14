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

/// <summary>
/// Editor 폴더 안에 위치해야 합니다.
/// </summary>
public class UnitySO_Generator : EditorWindow
{
    /* const & readonly declaration             */

    /* enum & struct declaration                */

    /* public - Field declaration            */


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

    static public void Test()  
    {
        ScriptableObject.CreateInstance("");
    }

    static public void DoBuild()
    {
        Debug.Log("Build");

        UnitySO_GeneratorConfig pConfig = UnitySO_GeneratorConfig.instance;

        TypeDataList pTypeDataList = new TypeDataList();
        if(GetData_FromJson(nameof(TypeDataList), ref pTypeDataList) == false)
        {
            Debug.LogError("Error");
            return;
        }

        foreach (var pFileName in pTypeDataList.listFileName)
        {
            TypeData pTypeData = new TypeData();
            if (GetData_FromJson(pFileName, ref pTypeData) == false)
            {
                Debug.LogError("Error");
                continue;
            }

            System.Type pType_SO = System.Type.GetType(pTypeData.strType);
            if (pType_SO.BaseType != typeof(UnityEngine.ScriptableObject))
                continue;

            System.Type pType_Container = System.Type.GetType(pTypeData.strType + "_Container");
            ScriptableObject pContainerInstance = (ScriptableObject)UnitySO_GeneratorConfig.CreateSOFile(pType_Container, pTypeData.strType + "_Container");

            Dictionary<string, System.Reflection.FieldInfo> mapFieldInfo_SO = pType_SO.GetFields().ToDictionary((pFieldInfo) => pFieldInfo.Name);
            Dictionary<string, System.Reflection.FieldInfo> mapFieldInfo_Container = pType_Container.GetFields().ToDictionary((pFieldInfo) => pFieldInfo.Name);


            System.Reflection.FieldInfo pField_ListData = mapFieldInfo_Container["listData"];
            var Method_ListAdd = pField_ListData.FieldType.GetMethod("Add");
            var pInstanceList = System.Activator.CreateInstance(pField_ListData.FieldType);

            pField_ListData.SetValue(pContainerInstance, pInstanceList);

            string strHeaderField = pTypeData.strHeaderFieldName;
            string strFileName = pType_SO.Name;
            int iLoopIndex = 0;
            foreach (var pInstance in pTypeData.listInstance)
            {
                ScriptableObject pSO = (ScriptableObject)UnitySO_GeneratorConfig.CreateInstance(pType_SO);

                FieldData pFieldData_Header = pInstance.listField.Where((pFieldData) => pFieldData.strFieldName == strHeaderField).FirstOrDefault();
                if (pFieldData_Header != null)
                    pSO.name = pFieldData_Header.strValue;
                else
                    pSO.name = $"{strFileName}_{iLoopIndex}";

                AssetDatabase.AddObjectToAsset(pSO, pContainerInstance);
                AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(pContainerInstance));

                Method_ListAdd.Invoke(pInstanceList, new object[] { pSO });

                // 실제 멤버
                var listRealField = pInstance.listField.Where((pFieldData) => pFieldData.bIsVirtualField == false);
                foreach(var pMember in listRealField)
                {
                    if (pMember.bDeleteThisField_InCode || pMember.bConvertStringToEnum)
                        continue;

                    System.Reflection.FieldInfo pFieldInfo = mapFieldInfo_SO[pMember.strFieldName];

                    try
                    {
                        switch (pMember.strFieldType)
                        {
                            case "int": pFieldInfo.SetValue(pSO, int.Parse(pMember.strValue)); break;
                            case "float": pFieldInfo.SetValue(pSO, float.Parse(pMember.strValue)); break;
                            case "string": pFieldInfo.SetValue(pSO, pMember.strValue); break;

                            default:
                                System.Type pType_Field = System.Type.GetType(pMember.strFieldType);
                                if(pType_Field.IsEnum)
                                    pFieldInfo.SetValue(pSO, System.Enum.Parse(pType_Field, pMember.strValue));

                                if(pType_Field == null)
                                    Debug.LogWarning($"아직 지원되지 않은 형식.. {pMember.strFieldType}");
                                break;
                        }
                    }
                    catch(System.Exception e)
                    {
                        Debug.LogError($"Parsing  Fail - {pTypeData.strType}/{pMember.strFieldType} {pMember.strFieldName} : {pMember.strValue}\n" +
                            $"Exception : {e}");
                    }
                }

                // 가상 멤버
                var listVirtualField = pInstance.listField.Where((pFieldData) => pFieldData.bIsVirtualField);
                foreach (var pMember in listVirtualField)
                {
                    System.Reflection.FieldInfo pFieldInfo = mapFieldInfo_SO[pMember.strFieldName];
                    System.Type pType_Field = GetTypeFromAssemblies(pMember.strFieldType);

                    FieldData pFieldData_Dependency = pInstance.listField.Where(pFieldData => pFieldData.strFieldName == pMember.strDependencyFieldName).FirstOrDefault();
                    if(pFieldData_Dependency == null)
                    {
                        Debug.LogError($"Error Not Found Field {pMember.strDependencyFieldName}");
                        continue;
                    }

                    Object pObject = AssetDatabase.LoadAssetAtPath(pFieldData_Dependency.strValue, pType_Field);
                    if(pObject == null)
                    {
                        Debug.LogError($"Value Is Null Or Empty - Type : {pMember.strFieldType} {pFieldData_Dependency.strValue}");
                    }

                    pFieldInfo.SetValue(pSO, pObject);
                }

                iLoopIndex++;
            }

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }
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

    // ========================================================================== //

    /* protected - Override & Unity API         */

    private void OnGUI()
    {
        UnitySO_GeneratorConfig pConfig = UnitySO_GeneratorConfig.instance;
        
        GUILayout.BeginHorizontal();
        AutoSizeLabel("Root Folder Path : ");
        GUILayout.Label(pConfig.strJsonRootFolderPath);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Root Folder Setting"))
        {
            string strPath = EditorUtility.OpenFolderPanel("Root Folder", "", "");
            System.Uri pCurrentURI = new System.Uri(Application.dataPath);
            pConfig.strJsonRootFolderPath = pCurrentURI.MakeRelativeUri(new System.Uri(strPath)).ToString();
        }

        bool bEditorEnable = string.IsNullOrEmpty(pConfig.strJsonRootFolderPath) == false;
        EditorGUI.BeginDisabledGroup(bEditorEnable == false);

        if (GUILayout.Button("Build!"))
        {
            DoBuild();
        }

        EditorGUI.EndDisabledGroup();
    }

    /* protected - [abstract & virtual]         */


    // ========================================================================== //

    #region Private

    void AutoSizeLabel(string strText)
    {
        GUILayout.Label(strText, GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent(strText)).x));
    }

    // http://eastfever.blogspot.com/2017/01/unity3d-string-systemtype.html
    // 어셈블리로부터 클래스 이름 문자열을 보내 System.Type을 얻는다.
    public static System.Type GetTypeFromAssemblies(string TypeName)
    {
        // null 반환 없이 Type이 얻어진다면 얻어진 그대로 반환.
        var type = System.Type.GetType(TypeName);
        if (type != null)
            return type;

        // 프로젝트에 분명히 포함된 클래스임에도 불구하고 Type이 찾아지지 않는다면,
        // 실행중인 어셈블리를 모두 탐색 하면서 그 안에 찾고자 하는 Type이 있는지 검사.
        var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();
        var referencedAssemblies = currentAssembly.GetReferencedAssemblies();
        foreach (var assemblyName in referencedAssemblies)
        {
            var assembly = System.Reflection.Assembly.Load(assemblyName);
            if (assembly != null)
            {
                // 찾았다 요놈!!!
                type = assembly.GetType(TypeName);
                if (type != null)
                    return type;
            }
        }

        // 못 찾았음;;; 클래스 이름이 틀렸던가, 아니면 알 수 없는 문제 때문이겠지...
        return null;
    }

    #endregion Private
}