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
public class ScriptableObjectGenerator : EditorWindow
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
        ScriptableObjectGenerator window = (ScriptableObjectGenerator)GetWindow(typeof(ScriptableObjectGenerator), false);

        window.minSize = new Vector2(800, 600);
        window.Show();
    }

    static public void Test()  
    {
        ScriptableObject.CreateInstance("");
    }

    // ========================================================================== //

    /* protected - Override & Unity API         */


    /* protected - [abstract & virtual]         */


    // ========================================================================== //

    #region Private

    #endregion Private
}