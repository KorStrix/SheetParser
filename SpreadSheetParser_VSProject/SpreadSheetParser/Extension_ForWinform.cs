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
        public static ListViewItem ConvertListViewItem(this TypeData pTypeData)
        {
            ListViewItem pViewItem = new ListViewItem(pTypeData.strSheetName);
            pViewItem.SubItems.Add("O");
            pViewItem.SubItems.Add(pTypeData?.eType.ToString());
            pViewItem.Tag = pTypeData;

            return pViewItem;
        }

        public static ListViewItem ConvertListViewItem(this SheetSourceConnector pSheetConnector)
        {
            ListViewItem pViewItem = new ListViewItem(pSheetConnector.strSheetSourceID);
            pViewItem.SubItems.Add("O");
            pViewItem.SubItems.Add(pSheetConnector?.eSheetSourceType.ToString());
            pViewItem.Tag = pSheetConnector;

            return pViewItem;
        }
    }
}
