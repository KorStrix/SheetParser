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

    public bool bIsVirtualField;
    public string strComment;
    public string strDependencyFieldName;

    public FieldData(string strMemberName, string strMemberType)
    {
        this.strFieldName = strMemberName; this.strFieldType = strMemberType;
    }

    public FieldData(string strMemberName, string strMemberType, string strValue)
    {
        this.strFieldName = strMemberName; this.strFieldType = strMemberType; this.strValue = strValue;
    }
}
