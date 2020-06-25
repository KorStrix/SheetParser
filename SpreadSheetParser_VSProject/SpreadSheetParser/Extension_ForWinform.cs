using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadSheetParser
{
    static class Extension_ForWinform
    {
        public static ListViewItem ConvertListViewItem(this SheetData pSheetData)
        {
            ListViewItem pViewItem = new ListViewItem(pSheetData.strSheetName);
            pViewItem.SubItems.Add(pSheetData.bEnable ? "O" : "X");
            pViewItem.SubItems.Add(pSheetData?.eType.ToString());
            pViewItem.SubItems.Add(pSheetData.pSheetSourceConnector.strSheetSourceID);
            pViewItem.Tag = pSheetData;

            return pViewItem;
        }

        public static ListViewItem ConvertListViewItem(this SheetSourceConnector pSheetConnector)
        {
            ListViewItem pViewItem = new ListViewItem(pSheetConnector.strSheetSourceID);
            pViewItem.SubItems.Add(pSheetConnector.bEnable ? "O" : "X");
            pViewItem.SubItems.Add(pSheetConnector?.eSheetSourceType.ToString());
            pViewItem.Tag = pSheetConnector;

            return pViewItem;
        }

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
