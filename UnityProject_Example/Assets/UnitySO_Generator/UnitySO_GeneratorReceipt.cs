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
    public string strJsonRootPath;
    public UnitySO_GeneratorReceipt_Type[] arrReciptType;

    public static UnitySO_GeneratorReceipt Dummy
    {
        get
        {
            UnitySO_GeneratorReceipt pDummy = new UnitySO_GeneratorReceipt();
            pDummy.strJsonRootPath = "ExportData/Json/";

            pDummy.arrReciptType = new UnitySO_GeneratorReceipt_Type[1];
            pDummy.arrReciptType[0] = new UnitySO_GeneratorReceipt_Type();
            pDummy.arrReciptType[0].strType = "UnitySO";
            pDummy.arrReciptType[0].arrMemberTypeInfo = new MemberTypeInfo[1];

            pDummy.arrReciptType[0].arrMemberTypeInfo[0] = new MemberTypeInfo();
            pDummy.arrReciptType[0].arrMemberTypeInfo[0].strMemberName = "pMaterial";
            pDummy.arrReciptType[0].arrMemberTypeInfo[0].strDependencyMemberName = "MaterialPath";
            pDummy.arrReciptType[0].arrMemberTypeInfo[0].strConvertType = "Material";

            return pDummy;
        }
    }
}

[System.Serializable]
public class UnitySO_GeneratorReceipt_Type
{
    public string strType;
    public MemberTypeInfo[] arrMemberTypeInfo;
}

[System.Serializable]
public class MemberTypeInfo
{
    public string strMemberName;
    public string strConvertType;

    public string strDependencyMemberName;
}
