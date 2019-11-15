using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace SpreadSheetParser
{
    public partial class SpreadSheetParser : Form
    {
        public enum ECommandLine
        {
            Error = -1,

            type,
            comment,
            typename,
        }

        public enum EState
        {
            None,
            IsConnected,
            IsConnected_And_SelectTable,
        }

        static public SpreadSheetParser isntance => _instance;
        static private SpreadSheetParser _instance;

        delegate void SafeCallDelegate(string text);
        delegate void delOnParsingText(IList<object> listRow, string strText, int iRowIndex, int iColumnIndex);

        SpreadSheetConnector _pSheetConnector = new SpreadSheetConnector();
        SaveData_SpreadSheet _pSpreadSheet_CurrentConnected;
        SaveData_Sheet _pSheet_CurrentConnected;
        CodeFileBuilder _pCodeFileBuilder = new CodeFileBuilder();
        Config _pConfig;

        Dictionary<string, SaveData_SpreadSheet> _mapSaveData = new Dictionary<string, SaveData_SpreadSheet>();

        EState _eState;

        public SpreadSheetParser()
        {
            InitializeComponent();

            _instance = this;
        }

        static public void WriteConsole(string strText)
        {
            // Winform 컨트롤을 스레드로부터 안전하게 호출하는 법
            // https://docs.microsoft.com/ko-kr/dotnet/framework/winforms/controls/how-to-make-thread-safe-calls-to-windows-forms-controls
            if (_instance.textBox_Console.InvokeRequired)
            {
                var pDelegate = new SafeCallDelegate(WriteConsole);
                _instance.textBox_Console.Invoke(pDelegate, new object[] { strText });
            }
            else
            {
                _instance.textBox_Console.AppendText(strText);
                _instance.textBox_Console.AppendText(Environment.NewLine);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetState(EState.None);

            _pConfig = SaveDataManager.LoadConfig();
            _mapSaveData = SaveDataManager.LoadSheet();

            comboBox_SaveSheet.Items.Clear();
            foreach (var pData in _mapSaveData.Values)
                comboBox_SaveSheet.Items.Add(pData);

            SaveData_SpreadSheet pSheet_LastEdit = GetSheet_LastEdit(_mapSaveData);
            if (pSheet_LastEdit != null)
            {
                textBox_SheetID.Text = pSheet_LastEdit.strSheetID;
                if (_pConfig.bAutoConnect)
                {
                    WriteConsole("Config - 자동연결로 인해 연결을 시작합니다..");
                    button_Connect_Click(null, null);
                }
            }

            checkBox_AutoConnect.Checked = _pConfig.bAutoConnect;

            checkedListBox_SheetList.ItemCheck += CheckedListBox_TableList_ItemCheck;

        }

        private void CheckedListBox_TableList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            _pSpreadSheet_CurrentConnected.listTable[e.Index].bEnable = e.NewValue == CheckState.Checked;
            AutoSaveAsync_CurrentSheet();

            WriteConsole("여기 작업 해야함");
        }

        private void button_Connect_Click(object sender, EventArgs e)
        {
            WriteConsole("연결 시작");

            string strSheetID = textBox_SheetID.Text;

            //// 테스트 시트
            //// https://docs.google.com/spreadsheets/d/1_s89xLPwidVwRsmGS4bp3Y6huaLWoBDq7SUW7lYyxl4/edit#gid=0
            //strSheetID = "1_s89xLPwidVwRsmGS4bp3Y6huaLWoBDq7SUW7lYyxl4";

            checkedListBox_SheetList.Items.Clear();
            List<SheetWrapper> listSheet;
            System.Exception pException;
            if (_pSheetConnector.DoConnect(strSheetID, out listSheet, out pException) == false)
            {
                WriteConsole("연결 실패 " + pException);
                return;
            }

            if (_mapSaveData.ContainsKey(strSheetID))
            {
                _pSpreadSheet_CurrentConnected = _mapSaveData[strSheetID];
                List<SaveData_Sheet> listSavedTable = _pSpreadSheet_CurrentConnected.listTable;

                for (int i = 0; i < listSheet.Count; i++)
                {
                    string strSheetName = listSheet[i].ToString();
                    if (listSavedTable.Where(x => (x.strSheetName == strSheetName)).Count() == 0)
                        listSavedTable.Add(new SaveData_Sheet(strSheetName));
                }
            }
            else
            {
                _pSpreadSheet_CurrentConnected = new SaveData_SpreadSheet(strSheetID);
                _mapSaveData[_pSpreadSheet_CurrentConnected.strSheetID] = _pSpreadSheet_CurrentConnected;

                _pSpreadSheet_CurrentConnected.listTable.Clear();
                for (int i = 0; i < listSheet.Count; i++)
                    _pSpreadSheet_CurrentConnected.listTable.Add(new SaveData_Sheet(listSheet[i].ToString()));

                SaveDataManager.SaveSheet(_pSpreadSheet_CurrentConnected);

                WriteConsole("새 파일을 만들었습니다.");
            }

            checkedListBox_SheetList.Items.Clear();
            List<SaveData_Sheet> listSheetSaved = _pSpreadSheet_CurrentConnected.listTable;
            for (int i = 0; i < listSheetSaved.Count; i++)
                checkedListBox_SheetList.Items.Add(listSheetSaved[i], listSheetSaved[i].bEnable);

            UpdateUI();
            SetState(EState.IsConnected);
            WriteConsole("연결 성공");
        }

        private void button_StartParsing_Click(object sender, EventArgs e)
        {
            _pCodeFileBuilder = new CodeFileBuilder();

            foreach (var pItem in checkedListBox_SheetList.CheckedItems)
            {
                SheetWrapper pWrapper = (SheetWrapper)pItem;
                
                var pCodeType = _pCodeFileBuilder.AddCodeType(pWrapper.ToString());
                pCodeType.IsClass = true;

                ParsingSheet(pWrapper.ToString(),
                    (listRow, strText, iRow, iColumn) =>
                    {
                        if (strText.StartsWith("-"))
                        {
                            if (listRow.Count < iColumn + 1)
                                Execute_CommandLine(pCodeType, strText, (string)listRow[iColumn + 1]);
                            else
                                Execute_CommandLine(pCodeType, strText, "");

                            return;
                        }

                        if (strText.Contains(":"))
                        {
                            string[] arrText = strText.Split(':');
                            if (CheckIsEnum(arrText[1]))
                                pCodeType.AddEnumField(new EnumFieldData(arrText[0]));
                            else
                                pCodeType.AddField(new FieldData(arrText[0], arrText[1]));
                        }
                    });
            }

            _pCodeFileBuilder.GenerateCSharpCode("test2");
        }

        private static bool CheckIsEnum(string strText)
        {
            return strText.ToLower().Equals("enum");
        }

        private void button_CheckTable_Click(object sender, EventArgs e)
        {
            SaveData_Sheet pSheetData = GetCurrentSelectedTable_OrNull();
            WriteConsole("테이블 유효성 체크중.." + pSheetData.ToString());
            try
            {
                ParsingSheet(pSheetData.ToString(), 
                (listRow, strText, iRow, iColumn) =>
                {
                    if (strText.StartsWith("-"))
                    {
                        ECommandLine eCommandLine = Parsing_CommandLine(strText);
                        switch (eCommandLine)
                        {
                            case ECommandLine.type:
                            case ECommandLine.comment:
                            case ECommandLine.typename:
                                if(string.IsNullOrEmpty((string)listRow[iColumn + 1]))
                                    throw new System.Exception("Command Parsing Error - Value Is Null");
                                break;

                            default: throw new System.Exception("Command Parsing Error - Parsing Error");
                        }
                        
                        return;
                    }

                    if (strText.Contains(":"))
                    {
                    }
                });
            }
            catch (Exception pException)
            {
                WriteConsole("테이블 유효성 에러 " + pException);
                return;
            }

            WriteConsole("테이블 유효성 체크 - 이상없음");
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
            ECommandLine eCommandLine = Parsing_CommandLine(strText);
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

        private static ECommandLine Parsing_CommandLine(string strText)
        {
            ECommandLine eCommandLine;
            strText = strText.Remove(0, 1);

            if (System.Enum.TryParse(strText, out eCommandLine) == false)
                eCommandLine = ECommandLine.Error;

            return eCommandLine;
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
                _pSpreadSheet_CurrentConnected.strOutputPath_Csharp = textBox_Csharp_Path.Text;
                AutoSaveAsync_CurrentSheet();
            }
        }

        private void Button_CSV_PathSetting_Click(object sender, EventArgs e)
        {
            if(SettingPath(ref textBox_CSV_Path))
            {
                _pSpreadSheet_CurrentConnected.strOutputPath_CSV = textBox_CSV_Path.Text;
                AutoSaveAsync_CurrentSheet();
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
                    groupBox2_TableSetting.Enabled = false;
                    groupBox3_OutputSetting.Enabled = false;
                    groupBox_SelectedTable.Enabled = false;
                    break;

                case EState.IsConnected:
                    groupBox2_TableSetting.Enabled = true;
                    groupBox3_OutputSetting.Enabled = true;
                    groupBox_SelectedTable.Enabled = false;

                    if (GetCurrentSelectedTable_OrNull() != null)
                        SetState(EState.IsConnected_And_SelectTable);

                    break;

                case EState.IsConnected_And_SelectTable:

                    groupBox2_TableSetting.Enabled = true;
                    groupBox3_OutputSetting.Enabled = true;
                    groupBox_SelectedTable.Enabled = true;

                    var pWrapper = GetCurrentSelectedTable_OrNull();
                    if (pWrapper != null)
                        Update_Step_2_TableSetting(pWrapper);
                    else
                        SetState(EState.IsConnected);

                    break;

                default:
                    break;
            }
        }


        private void AutoSaveDone()
        {
            WriteConsole("자동 저장 완료..");
        }

        void ParsingSheet(string strSheetName, delOnParsingText OnParsingText)
        {
            IList<IList<Object>> pData = _pSheetConnector.GetExcelData(strSheetName);
            if (pData == null)
                return;

            if (OnParsingText == null)
                OnParsingText = (a, b, c, d) => { };

            for (int i = 0; i < pData.Count; i++)
            {
                IList<object> listRow = pData[i];
                for (int j = 0; j < listRow.Count; j++)
                {
                    string strText = (string)listRow[j];
                    if (string.IsNullOrEmpty(strText))
                        continue;

                    OnParsingText(listRow, strText, i, j);
                }
            }
        }

        private void UpdateUI()
        {
            textBox_Csharp_Path.Text = _pSpreadSheet_CurrentConnected.strOutputPath_Csharp;
            textBox_CSV_Path.Text = _pSpreadSheet_CurrentConnected.strOutputPath_CSV;
        }

        private void checkBox_AutoConnect_CheckedChanged(object sender, EventArgs e)
        {
            _pConfig.bAutoConnect = checkBox_AutoConnect.Checked;
            AutoSaveAsync_Config();
        }


        private void AutoSaveAsync_CurrentSheet()
        {
            WriteConsole("자동 저장 중.." + _pSpreadSheet_CurrentConnected.strSheetID);
            SaveDataManager.SaveSheet_Async(_pSpreadSheet_CurrentConnected, AutoSaveDone);
        }

        private void AutoSaveAsync_Config()
        {
            WriteConsole("자동 저장 중.. Config");
            SaveDataManager.SaveConfig_Async(_pConfig, AutoSaveDone);
        }

        private void checkedListBox_TableList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetState(EState.IsConnected_And_SelectTable);
        }

        private SaveData_Sheet GetCurrentSelectedTable_OrNull()
        {
            return (SaveData_Sheet)checkedListBox_SheetList.SelectedItem;
        }

        private void Update_Step_2_TableSetting(SaveData_Sheet pSheetData)
        {
            _pSheet_CurrentConnected = pSheetData;
            if (pSheetData == null)
                return;

            groupBox_SelectedTable.Text = pSheetData.ToString();
            textBox_CommandLine.Text = pSheetData.strCommandLine;
        }

        private void button_TableSave_Click(object sender, EventArgs e)
        {
            _pSheet_CurrentConnected.strCommandLine = textBox_CommandLine.Text;
            AutoSaveAsync_CurrentSheet();
        }
    }
}
