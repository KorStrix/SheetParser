using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SpreadSheetParser
{
    public class Config
    {
        public bool bAutoConnect = true;
        public bool bOpenPath_AfterBuild_Csharp;
        public bool bOpenPath_AfterBuild_CSV;
    }

    public class SaveData_SpreadSheet
    {
        public string strSheetID;
        public DateTime date_LastEdit;
        public List<TypeData> listTable = new List<TypeData>();
        public List<WorkBase> listSaveWork = new List<WorkBase>();

        public SaveData_SpreadSheet(string strSheetID)
        {
            this.strSheetID = strSheetID;
            UpdateDate();
        }

        public void UpdateDate()
        {
            date_LastEdit = System.DateTime.Now;
        }

        public override string ToString()
        {
            return strSheetID;
        }
    }

    static public class FieldDataHelper
    {
        static public ListViewItem ConvertListViewItem(this FieldTypeData pFieldData)
        {
            ListViewItem pViewItem = new ListViewItem();
            pFieldData.Reset_ListViewItem(pViewItem);
            pViewItem.Tag = pFieldData;

            return pViewItem;

        }

        static public void Reset_ListViewItem(this FieldTypeData pFieldData, ListViewItem pViewItem)
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
