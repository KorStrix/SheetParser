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

        public enum EState
        {
            None,
            IsConnected,
        }

        static public SpreadSheetParser isntance => _instance;
        static private SpreadSheetParser _instance;

        SpreadSheetConnector _pSheetConnector = new SpreadSheetConnector();
        CodeFileBuilder _pCodeFileBuilder = new CodeFileBuilder();

        Dictionary<string, SaveData_SpreadSheet> _mapSaveData = new Dictionary<string, SaveData_SpreadSheet>();

        EState _eState;

        public SpreadSheetParser()
        {
            InitializeComponent();

            _instance = this;
        }

        static public void WriteConsole(string strText)
        {
            _instance.textBox_Console.AppendText(strText);
            _instance.textBox_Console.AppendText(Environment.NewLine);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetState(EState.None);

            _mapSaveData = SaveDataManager.LoadSheet();

            SaveData_SpreadSheet pSheet_LastEdit = GetSheet_LastEdit(_mapSaveData);
            if (pSheet_LastEdit != null)
                textBox_SheetID.Text = pSheet_LastEdit.strSheetID;

            comboBox_SaveSheet.Items.Clear();
            foreach (var pData in _mapSaveData.Values)
                comboBox_SaveSheet.Items.Add(pData);
        }

        private void button_Connect_Click(object sender, EventArgs e)
        {
            WriteConsole("연결 시작");

            string strSheetID = textBox_SheetID.Text;

            //// 테스트 시트
            //// https://docs.google.com/spreadsheets/d/1_s89xLPwidVwRsmGS4bp3Y6huaLWoBDq7SUW7lYyxl4/edit#gid=0
            //strSheetID = "1_s89xLPwidVwRsmGS4bp3Y6huaLWoBDq7SUW7lYyxl4";

            checkedListBox_TableList.Items.Clear();
            List<SheetWrapper> listSheet;
            System.Exception pException;
            if(_pSheetConnector.DoConnect(strSheetID, out listSheet, out pException) == false)
            {
                WriteConsole("연결 실패 " + pException);
                return;
            }

            for (int i = 0; i < listSheet.Count; i++)
                checkedListBox_TableList.Items.Add(listSheet[i], true);

            SaveData_SpreadSheet pSheet = new SaveData_SpreadSheet(strSheetID);
            _mapSaveData[pSheet.strSheetID] = pSheet;
            SaveDataManager.SaveSheet(_mapSaveData[pSheet.strSheetID]);

            textBox_Csharp_Path.Text = pSheet.strOutputPath_Csharp;
            textBox_CSV_Path.Text = pSheet.strOutputPath_CSV;

            SetState(EState.IsConnected);
            WriteConsole("연결 성공");
        }

        private void button_StartParsing_Click(object sender, EventArgs e)
        {
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
                            if (pRow.Count < k + 1)
                                Execute_CommandLine(pCodeType, strText, (string)pRow[k + 1]);
                            else
                                Execute_CommandLine(pCodeType, strText, "");

                            continue;
                        }

                        if (strText.Contains(":"))
                        {
                            string[] arrText = strText.Split(':');
                            if (arrText[1].ToLower().Equals("enum"))
                                pCodeType.AddEnumField(new EnumFieldData(arrText[0]));
                            else
                                pCodeType.AddField(new FieldData(arrText[0], arrText[1]));
                        }
                    }
                }
            }

            _pCodeFileBuilder.GenerateCSharpCode("test2");
        }



        private SaveData_SpreadSheet GetSheet_LastEdit(Dictionary<string, SaveData_SpreadSheet> mapSaveData)
        {
            WriteConsole("마지막에 수정한 Sheet 찾는 중..");

            DateTime date_LastEdit = DateTime.MinValue;
            SaveData_SpreadSheet pSheet_LastEdit = null;
            foreach (var pSheet in mapSaveData.Values)
            {
                if (pSheet.date_LastEdit > date_LastEdit)
                    pSheet_LastEdit = pSheet;
            }

            if(pSheet_LastEdit != null)
                WriteConsole(string.Format("마지막에 수정한 시트를 찾았다. 수정한 시간 {0}, SheetID {1}", pSheet_LastEdit.date_LastEdit, pSheet_LastEdit.strSheetID));
            else
                WriteConsole(string.Format("마지막에 수정한 시트를 못찾았다.."));

            return pSheet_LastEdit;
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

        private void Button_OpenPath_SaveSheet_Click(object sender, EventArgs e)
        {
            string strSaveFolderPath = SaveDataManager.const_strSaveFolderPath;
            OpenPath(strSaveFolderPath.Remove(strSaveFolderPath.Length - 1, 1));
        }

        private void Button_OpenPath_Csharp_Click(object sender, EventArgs e)
        {
            OpenPath(textBox_Csharp_Path.Text);
        }

        private void Button_OpenPath_CSV_Click(object sender, EventArgs e)
        {
            OpenPath(textBox_CSV_Path.Text);
        }

        private void Button_Csharp_PathSetting_Click(object sender, EventArgs e)
        {
            if(SettingPath(ref textBox_Csharp_Path))
            {
                
            }
        }

        private void Button_CSV_PathSetting_Click(object sender, EventArgs e)
        {
            if(SettingPath(ref textBox_CSV_Path))
            {

            }
        }


        private void OpenPath(string strPath)
        {
            WriteConsole($"폴더 열기 시도.. 경로{strPath}");

            try
            {
                System.Diagnostics.Process.Start(strPath);
                WriteConsole($"폴더 열기 성공.. 경로{strPath}");
            }
            catch (System.Exception pException)
            {
                WriteConsole($"폴더 열기 실패.. 경로{strPath}");
                WriteConsole($"에러:{ pException}");
            }
        }

        private bool SettingPath(ref TextBox pTextBox_Path)
        {
            using (FolderBrowserDialog pDialog = new FolderBrowserDialog())
            {
                if (pDialog.ShowDialog() == DialogResult.OK)
                {
                    pTextBox_Path.Text = pDialog.SelectedPath;
                    return true;
                }
            }

            return false;
        }

        private void SetState(EState eState)
        {
            _eState = eState;

            switch (_eState)
            {
                case EState.None:
                    groupBox2.Enabled = false;
                    groupBox3.Enabled = false;
                    break;

                case EState.IsConnected:
                    groupBox2.Enabled = true;
                    groupBox3.Enabled = true;
                    break;

                default:
                    break;
            }
        }
    }
}
