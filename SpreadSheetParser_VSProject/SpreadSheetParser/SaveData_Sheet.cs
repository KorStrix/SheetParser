using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SpreadSheetParser
{
    public class Config
    {
        public bool bAutoConnect = true;
    }

    public class SaveData_SheetSource
    {
        public string strSheetSourceID;
        public DateTime date_LastEdit;
        public ESheetSourceType eSourceType;
        public List<TypeData> listTable = new List<TypeData>();
        public List<WorkBase> listSaveWork = new List<WorkBase>();

        public SaveData_SheetSource(string strSheetSourceID, ESheetSourceType eSourceType)
        {
            this.strSheetSourceID = strSheetSourceID;
            this.eSourceType = eSourceType;

            UpdateDate();
        }

        public void UpdateDate()
        {
            date_LastEdit = DateTime.Now;
        }

        public string GetFileName()
        {
            string strID = strSheetSourceID;
            switch (eSourceType)
            {
                case ESheetSourceType.MSExcel: strID = Path.GetFileNameWithoutExtension(strSheetSourceID); break;
            }

            return $"{eSourceType}_{strID}";
        }

        public override string ToString()
        {
            return strSheetSourceID;
        }
    }

    public static class FieldDataHelper
    {
        public static ListViewItem ConvertListViewItem(this FieldTypeData pFieldData)
        {
            ListViewItem pViewItem = new ListViewItem();
            pFieldData.Reset_ListViewItem(pViewItem);
            pViewItem.Tag = pFieldData;

            return pViewItem;
        }

        public static void Reset_ListViewItem(this FieldTypeData pFieldData, ListViewItem pViewItem)
        {
            pViewItem.SubItems.Clear();
            pViewItem.SubItems.Add(pFieldData.strFieldType);
            if (pFieldData.bIsVirtualField)
                pViewItem.SubItems.Add(pFieldData.strDependencyFieldName);
            else
                pViewItem.SubItems.Add("X");

            pViewItem.Text = pFieldData.strFieldName;
        }
    }
}
