using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadSheetParser
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new SpreadSheetParser());

            SaveData_SpreadSheet pSheet = new SaveData_SpreadSheet();
            pSheet.strSheetID = "TestID";

            pSheet.listTable.Add(new SaveData_Table() { strSheetName = "Test1" });
            pSheet.listTable.Add(new SaveData_Table() { strSheetName = "Test2" });

            SaveDataManager.SaveSheet(pSheet);

            var listSheet = SaveDataManager.LoadSheet();
        }
    }
}
