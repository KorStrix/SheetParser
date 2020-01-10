#region Header
/*	============================================
 *	작성자 : Strix
 *	작성일 : 2020-01-08 오후 4:38:04
 *	개요 : 
   ============================================ */
#endregion Header

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
[System.Serializable]
public class UnitySO_GeneratorReceipt
{
    public string strType;
    public MemberTypeInfo[] arrMemberTypeInfo;
}

public class MemberTypeInfo
{
    public string strMemberName;
    public string strConvertType;
    public bool bConvertFail_IsError;
}
