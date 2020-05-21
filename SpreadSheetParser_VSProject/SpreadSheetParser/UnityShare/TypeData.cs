#region Header
/*	============================================
 *	작성자 : Strix
 *	작성일 : 2020-01-12 오후 10:11:50
 *	개요 : 
   ============================================ */
#endregion Header

using System.Collections.Generic;

namespace SpreadSheetParser
{
    [System.Serializable]
    public class TypeDataList
    {
        public string strFileName;
        public List<TypeData> listTypeData = new List<TypeData>();

        public TypeDataList(string strFileName)
        {
            this.strFileName = strFileName;
        }
    }


    [System.Serializable]
    public class TypeData
    {
        public SheetSourceConnector pSheetSourceConnector { get; private set; }

        public bool bEnable = true;
        public string strFileName;
        public string strHeaderFieldName;

        public int iOrder;

        public string strSheetID;
        public string strSheetName;
        public ESheetType eType;
        public string strCommandLine;
        public List<FieldTypeData> listFieldData = new List<FieldTypeData>();
        public List<string> listEnumName = new List<string>();

        public TypeData(SheetSourceConnector pSheetSourceConnector, string strSheetID, string strSheetName, int iOrder)
        {
            this.pSheetSourceConnector = pSheetSourceConnector;
            this.strSheetID = strSheetID;
            this.strSheetName = strSheetName;
            this.strFileName = strSheetName;
            this.iOrder = iOrder;
        }

        public void DoSetSheetSourceConnector(SheetSourceConnector pSheetSourceConnector)
        {
            this.pSheetSourceConnector = pSheetSourceConnector;
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