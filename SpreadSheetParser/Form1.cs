using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SpreadSheetParser
{
    public partial class SpreadSheetParser : Form
    {
        public enum ECommandLine
        {
            type,
            comment,
            typename,
        }

        SpreadSheetConnector _pSheetConnector = new SpreadSheetConnector();
        CodeFileBuilder _pCodeFileBuilder = new CodeFileBuilder();

        public SpreadSheetParser()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button_Connect_Click(object sender, EventArgs e)
        {
            // 테스트 시트
            // https://docs.google.com/spreadsheets/d/1_s89xLPwidVwRsmGS4bp3Y6huaLWoBDq7SUW7lYyxl4/edit#gid=0
            string strSheetID = "1_s89xLPwidVwRsmGS4bp3Y6huaLWoBDq7SUW7lYyxl4";

            checkedListBox_TableList.Items.Clear();
            List<SheetWrapper> listSheet = _pSheetConnector.DoConnect(strSheetID);
            for (int i = 0; i < listSheet.Count; i++)
                checkedListBox_TableList.Items.Add(listSheet[i], true);

            _pCodeFileBuilder = new CodeFileBuilder();

            foreach (var pItem in checkedListBox_TableList.CheckedItems)
            {
                SheetWrapper pWrapper = (SheetWrapper)pItem;
                string strSheetName = pWrapper.ToString();

                var pCodeType = _pCodeFileBuilder.AddCodeType(strSheetName);
                pCodeType.IsClass = true;

                IList<IList<Object>> pData = _pSheetConnector.GetExcelData(strSheetName);
                if (pData == null)
                    continue;

                for (int i = 0; i < pData.Count; i++)
                {
                    IList<object> pRow = pData[i];
                    for (int k = 0; k < pRow.Count; k++)
                    {
                        string strText = (string)pRow[k];
                        if (string.IsNullOrEmpty(strText))
                            continue;

                        if (strText.StartsWith("-"))
                        {
                            if(pRow.Count < k + 1)
                                Execute_CommandLine(pCodeType, strText, (string)pRow[k + 1]);
                            else
                                Execute_CommandLine(pCodeType, strText, "");

                            continue;
                        }

                        if (strText.Contains(":"))
                        {
                            string[] arrText = strText.Split(':');
                            if(arrText[1].ToLower().Equals("enum"))
                                pCodeType.AddEnumField(new EnumFieldData(arrText[0]));
                            else
                                pCodeType.AddField(new FieldData(arrText[0], arrText[1]));
                        }
                    }
                }
            }
             
            _pCodeFileBuilder.GenerateCSharpCode("test2");
        }

        private void Execute_CommandLine(System.CodeDom.CodeTypeDeclaration pCodeType, string strText, string strCommandLineValue)
        {
            strText = strText.Remove(0, 1);
            ECommandLine eCommandLine = (ECommandLine)System.Enum.Parse(typeof(ECommandLine), strText);
            switch (eCommandLine)
            {
                case ECommandLine.type:

                    pCodeType.IsClass = false;
                    pCodeType.IsStruct = false;
                    pCodeType.IsEnum = false;

                    switch (strCommandLineValue.ToLower())
                    {
                        case "class": pCodeType.IsClass = true; break;
                        case "struct": pCodeType.IsStruct = true; break;
                        case "enum": pCodeType.IsEnum = true; break;
                    }
                    break;

                case ECommandLine.comment:
                    pCodeType.AddComment(strCommandLineValue);
                    break;

                case ECommandLine.typename:
                    pCodeType.Name = strCommandLineValue;
                    break;

                default:
                    break;
            }
        }

        private void button_StartParsing_Click(object sender, EventArgs e)
        {

        }

    }

}
