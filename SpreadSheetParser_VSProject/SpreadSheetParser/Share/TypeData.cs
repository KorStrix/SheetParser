#region Header
/*	============================================
 *	작성자 : Strix
 *	작성일 : 2020-01-12 오후 10:11:50
 *	개요 : 
   ============================================ */
#endregion Header

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace SpreadSheetParser
{
    [System.Serializable]
    public class TypeDataList
    {
        public List<TypeData> listTypeData = new List<TypeData>();

        [System.NonSerialized]
        public Dictionary<string, TypeData> mapType = new Dictionary<string, TypeData>();
        public void DoInit()
        {
            mapType = listTypeData.ToDictionary(p => p.strFileName);
        }
    }


    [System.Serializable]
    public class TypeData
    {
        public bool bEnable = true;

        public string strFileName;
        public string strSheetName;
        public string strHeaderFieldName;
        public ESheetType eType;
        public string strCommandLine;
        public List<FieldTypeData> listFieldData = new List<FieldTypeData>();
        public TypeData(string strSheetName)
        {
            this.strSheetName = strSheetName;
        }

        public override string ToString()
        {
            return strSheetName;
        }
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
        public bool bIsTemp = false;

        public bool bConvertStringToEnum = false;

        public FieldTypeData(string strFieldName, string strFieldType)
        {
            this.strFieldName = strFieldName; this.strFieldType = strFieldType;
        }
    }
}