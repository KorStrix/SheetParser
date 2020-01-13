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

    public string strJsonRootFolderPath;


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
    public static object CreateSOFile(System.Type pType)
    {
        ScriptableObject pAsset = ScriptableObject.CreateInstance(pType);
        string strPath = "Assets";

        if (Path.GetExtension(strPath) != "")
        {
            strPath = strPath.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string strAssetPathAndName = AssetDatabase.GenerateUniqueAssetPath(strPath + "/New " + pType.Name + ".asset");

        AssetDatabase.CreateAsset(pAsset, strAssetPathAndName);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = pAsset;

        return pAsset;
    }

    #endregion
}