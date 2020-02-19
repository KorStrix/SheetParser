#region Header
/*	============================================
 *	작성자 : Strix
 *	작성일 : 2020-01-09 오후 6:43:47
 *	개요 : 
   ============================================ */
#endregion Header

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// 
/// </summary>
public class UnitySO_GeneratorConfig : ScriptableObject
{
    static public UnitySO_GeneratorConfig instance 
    {
        get 
        {
            if(_instance == null)
            {
                UnitySO_GeneratorConfig[] arrConfig = GetAllInstances<UnitySO_GeneratorConfig>();
                if(arrConfig.Length == 0)
                {
                    arrConfig = new UnitySO_GeneratorConfig[1];
                    arrConfig[0] = CreateSOFile<UnitySO_GeneratorConfig>();
                }

                _instance = arrConfig[0];
            }

            return _instance;
        } 
    }
    static UnitySO_GeneratorConfig _instance = null;

    public string strCredential_FilePath;

    public string strSOScript_FolderPath;
    public string strJsonData_FolderPath;

    public string strDataExport_FolderPath;

    public string strSheetID;
    public string strSOCommandLine;

    public TextAsset pTypeDataFile;

    #region Helper
    public static T[] GetAllInstances<T>() where T : ScriptableObject
    {
        string[] arrGUID = AssetDatabase.FindAssets("t:" + typeof(T).Name);  //FindAssets uses tags check documentation for more info
        T[] arrTemp = new T[arrGUID.Length];
        for (int i = 0; i < arrGUID.Length; i++)         //probably could get optimized 
        {
            string strPath = AssetDatabase.GUIDToAssetPath(arrGUID[i]);
            arrTemp[i] = AssetDatabase.LoadAssetAtPath<T>(strPath);
        }

        return arrTemp;
    }

    public void DoSave()
    {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }


    /// <summary>
    ///	This makes it easy to create, name and place unique new ScriptableObject asset files.
    // https://wiki.unity3d.com/index.php/CreateScriptableObjectAsset
    /// </summary>
    public static T CreateSOFile<T>() where T : ScriptableObject
    {
        T pAsset = ScriptableObject.CreateInstance<T>();
        string strPath = "Assets";

        if (Path.GetExtension(strPath) != "")
        {
            strPath = strPath.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string strAssetPathAndName = AssetDatabase.GenerateUniqueAssetPath(strPath + "/New " + typeof(T).Name + ".asset");

        AssetDatabase.CreateAsset(pAsset, strAssetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = pAsset;

        return pAsset;
    }

    /// <summary>
    ///	This makes it easy to create, name and place unique new ScriptableObject asset files.
    // https://wiki.unity3d.com/index.php/CreateScriptableObjectAsset
    /// </summary>
    public static object CreateSOFile(System.Type pType, string strFileName, bool bOverride_IfAlreadyExists)
    {
        ScriptableObject pAsset = null;

        string strPrefixPath = "";
        if (strFileName.Contains("Assets/") == false)
            strPrefixPath = strPrefixPath.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "") + "/";

        string strFullPath = strPrefixPath + strFileName + ".asset";
        if (bOverride_IfAlreadyExists)
        {
            pAsset = (ScriptableObject)AssetDatabase.LoadAssetAtPath(strFullPath, pType);
            if (pAsset != null)
            {
                // 메인 에셋을 삭제하면 기존의 참조가 다 풀리므로 서브에셋만 삭제합니다.

                // 서브 에셋을 삭제하는 방법
                // https://www.reddit.com/r/Unity3D/comments/8krcrq/is_there_a_way_to_delete_subasset_without/dza0pj0/?context=8&depth=9
                Object[] arrObject = AssetDatabase.LoadAllAssetRepresentationsAtPath(strFullPath);
                for (int i = 0; i < arrObject.Length; i++)
                    DestroyImmediate(arrObject[i], true);
            }
        }

        if(pAsset == null)
        {
            pAsset = CreateInstance(pType);

            string strAssetPathAndName = AssetDatabase.GenerateUniqueAssetPath(strFullPath);
            AssetDatabase.CreateAsset(pAsset, strAssetPathAndName);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = pAsset;

        return pAsset;
    }

    #endregion
}