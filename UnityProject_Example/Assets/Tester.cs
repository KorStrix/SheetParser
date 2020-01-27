#region Header
/*	============================================
 *	작성자 : Strix
 *	작성일 : 2020-01-20 오후 12:26:24
 *	개요 : 
   ============================================ */
#endregion Header

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
public class Tester : MonoBehaviour
{
    /* const & readonly declaration             */

    /* enum & struct declaration                */

    /* public - Field declaration            */

    // public global_Container pGlobalContainer;
    public UnitySO_Container pContainer;

    /* protected & private - Field declaration         */


    // ========================================================================== //

    /* public - [Do] Function
     * 외부 객체가 호출(For External class call)*/


    // ========================================================================== //

    /* protected - Override & Unity API         */

    private void OnEnable()
    {
        pContainer.DoInit();
        Debug.Log($"mapData_Key_Is_AutoEnumValue Count : { pContainer.mapData_Key_Is_AutoEnumValue.Count}");
        Debug.Log($"mapData_Key_Is_strAutoEnum Count : { pContainer.mapData_Key_Is_strAutoEnum.Count}");

        //pGlobalContainer.DoInit();
        //Debug.Log($"pGlobalContainer.mapData_Type_Is_float Count : { pGlobalContainer.mapData_Type_Is_float.Count}");
        //Debug.Log($"pGlobalContainer.mapData_Type_Is_int Count : { pGlobalContainer.mapData_Type_Is_int.Count}");
        //Debug.Log($"pGlobalContainer.mapData_Type_Is_string Count : { pGlobalContainer.mapData_Type_Is_string.Count}");
    }

    /* protected - [abstract & virtual]         */


    // ========================================================================== //

    #region Private

    #endregion Private
}