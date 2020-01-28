using System;
using System.Reflection;
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.Office.Interop.Excel;

namespace SpreadSheetParser
{
    public class Program_Example_MSExcel
    {
        public static void Main_Example(string[] args)
        {
            Application pExcelApp = new Application();
            Workbook pWorkBook = pExcelApp.Workbooks.Open(Filename: "D:/SpreadSheetParser/SpreadSheetParser_VSProject/SpreadSheetParser/Test.xlsx");

            foreach(_Worksheet pSheet in pWorkBook.Sheets)
            {
                Console.WriteLine(pSheet.Name);

                Range pUsedRange = pSheet.UsedRange;
                int iRowCount = pUsedRange.Rows.Count;
                int iColumnCount = pUsedRange.Columns.Count;
                for (int i = 1; i <= iRowCount; i++)
                {
                    for (int j = 1; j <= iColumnCount; j++)
                    {
                        if (j == 1)
                            Console.Write("\r\n");

                        dynamic Check_HasCell = pUsedRange.Cells[i, j] != null && pUsedRange.Cells[i, j].Value2 != null;
                        if (Check_HasCell)
                            Console.Write(pUsedRange.Cells[i, j].Value2.ToString() + "\t");
                    }
                }

            }
        }
    }
}
