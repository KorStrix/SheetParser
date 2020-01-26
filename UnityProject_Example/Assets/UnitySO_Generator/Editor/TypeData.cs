#region Header
/*	============================================
 *	작성자 : Strix
 *	작성일 : 2020-01-12 오후 10:11:50
 *	개요 : 
   ============================================ */
#endregion Header

using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class TypeDataList
{
    public List<TypeData> listTypeData = new List<TypeData>();

    [System.NonSerialized]
    public Dictionary<string, TypeData> mapType = new Dictionary<string, TypeData>();
    public void DoInit()
    {
        mapType = listTypeData.ToDictionary(p => p.strType);
    }
}


[System.Serializable]
public class TypeData
{
    public string strType;
    public string strHeaderFieldName;
    public List<FieldTypeData> listField = new List<FieldTypeData>();
}


[System.Serializable]
public class FieldTypeData
{
    public string strFieldName;
    public string strFieldType;

    public string strComment;
    public string strDependencyFieldName;
    public string strDependencyFieldName_Sub;
    public string strEnumName;

    public bool bIsVirtualField;
    public bool bDeleteThisField_InCode = false;
    public bool bIsKeyField = false;
    public bool bIsOverlapKey = false;

    public bool bConvertStringToEnum = false;

    public FieldTypeData()
    {
    }

    public FieldTypeData(string strMemberName, string strMemberType)
    {
        this.strFieldName = strMemberName; this.strFieldType = strMemberType;
    }
}