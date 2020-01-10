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
        UnitySO_GeneratorReceipt pReciept = null;


    }

    // ========================================================================== //

    /* protected - Override & Unity API         */

    private void OnGUI()
    {
        UnitySO_GeneratorConfig pConfig = UnitySO_GeneratorConfig.instance;
        
        GUILayout.BeginHorizontal();
        AutoSizeLabel("Root Folder Path : ");
        GUILayout.Label(pConfig.strLastRootFolderPath);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Root Folder Setting"))
        {
            pConfig.strLastRootFolderPath = EditorUtility.OpenFolderPanel("Root Folder", "", "");
        }

        bool bEditorEnable = string.IsNullOrEmpty(pConfig.strLastRootFolderPath) == false;
        EditorGUI.BeginDisabledGroup(bEditorEnable == false);

        if (GUILayout.Button("Test"))
        {
            Debug.Log("test");
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

    #endregion Private
}