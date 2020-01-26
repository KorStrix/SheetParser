#region Header
/*	============================================
 *	작성자 : Strix
 *	작성일 : 2020-01-12 오후 10:11:50
 *	개요 : 
   ============================================ */
#endregion Header

using System.Collections.Generic;

[System.Serializable]
public class TypeDataList
{
    public List<string> listFileName = new List<string>();
}


[System.Serializable]
public class TypeData
{
    public string strType;
    public string strHeaderFieldName;
    public List<InstanceData> listInstance = new List<InstanceData>();
}

[System.Serializable]
public class InstanceData
{
    public List<FieldData> listField = new List<FieldData>();
}


[System.Serializable]
public class FieldData
{
    public string strFieldName;
    public string strFieldType;
    public string strValue;

    public string strComment;
    public string strDependencyFieldName;
    public string strDependencyFieldName_Sub;
    public string strEnumName;

    public bool bIsVirtualField;
    public bool bDeleteThisField_InCode = false;
    public bool bIsKeyField = false;
    public bool bIsOverlapKey = false;

    public bool bConvertStringToEnum = false;

    public FieldData()
    {
    }

    public FieldData(string strMemberName, string strMemberType)
    {
        this.strFieldName = strMemberName; this.strFieldType = strMemberType;
    }

    public FieldData(string strMemberName, string strMemberType, string strValue)
    {
        this.strFieldName = strMemberName; this.strFieldType = strMemberType; this.strValue = strValue;
    }

    static public FieldData Clone(FieldData pCopy, object pValue)
    {
        FieldData pNewFieldData = (FieldData)pCopy.MemberwiseClone();
        pNewFieldData.strValue = (string)pValue;

        return pNewFieldData;
    }
}
