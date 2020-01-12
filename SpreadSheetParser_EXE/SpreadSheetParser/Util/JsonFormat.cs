#region Header
/*	============================================
 *	작성자 : Strix
 *	작성일 : 2020-01-12 오후 10:11:50
 *	개요 : 
   ============================================ */
#endregion Header

using System.Collections.Generic;


[System.Serializable]
public class JsonFormat
{
    public List<JsonInstance> listInstance = new List<JsonInstance>();
}

[System.Serializable]
public class JsonInstance
{
    public List<JsonMember> listMember = new List<JsonMember>();
}


[System.Serializable]
public class JsonMember
{
    public string strMemberName;
    public string strMemberType;
    public string strValue;

    public JsonMember(string strMemberName, string strMemberType, string strValue)
    {
        this.strMemberName = strMemberName; this.strMemberType = strMemberType; this.strValue = strValue;
    }
}
