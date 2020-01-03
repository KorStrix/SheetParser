using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SpreadSheetParser
{
    public partial class SpreadSheetParser_MainForm : Form
    {
        public enum ECommandLine
        {
            Error = -1,

            comment,
            typename,
        }

        public enum EState
        {
            None,
            IsConnected,
            IsConnected_And_SelectTable,
        }

        static public SpreadSheetParser_MainForm isntance => _instance;
        static private SpreadSheetParser_MainForm _instance;

        delegate void SafeCallDelegate(string text);
        delegate void delOnParsingText(IList<object> listRow, string strText, int iRowIndex, int iColumnIndex);

        SpreadSheetConnector _pSheetConnector = new SpreadSheetConnector();
        SaveData_SpreadSheet _pSpreadSheet_CurrentConnected;
        SaveData_Sheet _pSheet_CurrentConnected;
        CodeFileBuilder _pCodeFileBuilder = new CodeFileBuilder();
        Config _pConfig;

        Dictionary<string, SaveData_SpreadSheet> _mapSaveData = new Dictionary<string, SaveData_SpreadSheet>();

        EState _eState;
        bool _bIsConnecting;

        public SpreadSheetParser_MainForm()
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

        bool _bIsLoading_CreateForm = false;
        private void MainForm_Load(object sender, EventArgs e)
        {
            SetState(EState.None);
            _bIsLoading_CreateForm = true;

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
            checkBox_OpenFolder_AfterBuild_Csharp.Checked = _pConfig.bOpenPath_AfterBuild_Csharp;
            checkBox_OpenFolder_AfterBuild_CSV.Checked = _pConfig.bOpenPath_AfterBuild_CSV;

            checkedListBox_SheetList.ItemCheck += CheckedListBox_TableList_ItemCheck;

            _bIsLoading_CreateForm = false;
        }

        private void CheckedListBox_TableList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_bIsConnecting)
                return;

            _pSpreadSheet_CurrentConnected.listTable[e.Index].bEnable = e.NewValue == CheckState.Checked;
            AutoSaveAsync_CurrentSheet();
        }

        private async void button_Connect_Click(object sender, EventArgs e)
        {
            _bIsConnecting = true;

            string strSheetID = textBox_SheetID.Text;
            WriteConsole($"연결 시작 Sheet ID : {strSheetID}");

            //// 테스트 시트
            //// https://docs.google.com/spreadsheets/d/1_s89xLPwidVwRsmGS4bp3Y6huaLWoBDq7SUW7lYyxl4/edit#gid=0
            //strSheetID = "1_s89xLPwidVwRsmGS4bp3Y6huaLWoBDq7SUW7lYyxl4";

            checkedListBox_SheetList.Items.Clear();
            await _pSheetConnector.DoConnect(strSheetID, OnFinishConnect);
            _bIsConnecting = false;
        }

        private void OnFinishConnect(string strSheetID, List<SheetWrapper> listSheet, Exception pException_OnError)
        {
            if(pException_OnError != null)
            {
                WriteConsole("연결 실패 " + pException_OnError);
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

            UpdateUI_Sheet();
            SetState(EState.IsConnected);
            WriteConsole("연결 성공");
        }

        private void button_StartParsing_Click(object sender, EventArgs e)
        {
            WriteConsole("코드 파일 생성중..");
            _pCodeFileBuilder = new CodeFileBuilder();

            try
            {
                foreach (var pItem in checkedListBox_SheetList.CheckedItems)
                {
                    SaveData_Sheet pSheetData = (SaveData_Sheet)pItem;
                    List<CommandLineArg> listCommandLine = Parsing_CommandLine(pSheetData.strCommandLine);

                    bool bIsEnum = pSheetData.eType == SaveData_Sheet.EType.Enum;
                    if (bIsEnum)
                    {
                        ParsingSheet(pSheetData.ToString(),
                            (listRow, strText, iRow, iColumn) =>
                            {
                                if (iRow == 0)
                                {

                                }

                                // pCodeType.AddEnumField(new EnumFieldData(strText));
                            });
                    }
                    else
                    {
                        var pCodeType = _pCodeFileBuilder.AddCodeType(pSheetData.ToString());

                        switch (pSheetData.eType)
                        {
                            case SaveData_Sheet.EType.Class: pCodeType.IsClass = true; break;
                            case SaveData_Sheet.EType.Struct: pCodeType.IsStruct = true; break;

                            default:
                                break;
                        }

                        ParsingSheet(pSheetData.ToString(),
                            (listRow, strText, iRow, iColumn) =>
                            {
                                if (strText.Contains(":"))
                                {
                                    string[] arrText = strText.Split(':');
                                    if (CheckIsEnum(arrText[1]))
                                        pCodeType.AddEnumField(new EnumFieldData(arrText[0]));
                                    else
                                        pCodeType.AddField(new FieldData(arrText[0], arrText[1]));
                                }
                            });

                        Execute_CommandLine(pCodeType, listCommandLine);
                    }
                }
            }
            catch (Exception pException)
            {
                WriteConsole("코드 파일 생성 실패.." + pException);
                return;
            }

            _pCodeFileBuilder.GenerateCSharpCode(_pSpreadSheet_CurrentConnected.strOutputPath_Csharp + "/" + _pSpreadSheet_CurrentConnected.strFileName_Csharp);

            if (_pConfig.bOpenPath_AfterBuild_Csharp)
                Button_OpenPath_Csharp_Click(null, null);

            if (_pConfig.bOpenPath_AfterBuild_CSV)
                Button_OpenPath_CSV_Click(null, null);

            WriteConsole("코드 파일 생성 완료");
        }

        private static bool CheckIsEnum(string strText)
        {
            return strText.ToLower().Equals("enum");
        }

        private void button_CheckTable_Click(object sender, EventArgs e)
        {
            SaveData_Sheet pSheetData = GetCurrentSelectedTable_OrNull();
            WriteConsole("테이블 유효성 체크중.." + pSheetData.ToString());
            int iErrorCount = 0;

            try
            {
                string strCommandLine = textBox_CommandLine.Text;
                if (string.IsNullOrEmpty(strCommandLine) == false)
                {
                    Parsing_CommandLine(strCommandLine);

                    // Parsing_CommandLine(strCommandLine);
                    //if (eCommandLineText == ECommandLine.Error)
                    //{
                    //    WriteConsole($"커맨드라인 파싱 에러 - {strCommandLine}");
                    //    return;
                    //}
                }

                ParsingSheet(pSheetData.ToString(), null);
            }
            catch (Exception pException)
            {
                WriteConsole("테이블 유효성 - 치명적인 에러 " + pException);
                return;
            }

            if (iErrorCount > 0)
                WriteConsole($"테이블 유효성 체크 - 에러, 개수 : {iErrorCount}");
            else
                WriteConsole("테이블 유효성 체크 - 이상없음");
        }

        private static List<CommandLineArg> Parsing_CommandLine(string strCommandLine)
        {
            return CommandLineParser.Parsing_CommandLine(strCommandLine,
                (string strCommandLineText, out bool bHasValue) =>
                {
                    ECommandLine eCommandLine;
                    bool bIsValid = Enum.TryParse(strCommandLineText, out eCommandLine);
                    switch (eCommandLine)
                    {
                        case ECommandLine.comment:
                        case ECommandLine.typename:
                            bHasValue = true;
                            break;

                        default:
                            bHasValue = false;
                            break;
                    }

                    return bIsValid;
                },
                (string strCommandLineText, CommandLineParser.Error eError) =>
                {
                    WriteConsole($"테이블 유효성 에러 Text : {strCommandLineText} Error : {eError}");
                    // iErrorCount++;
                });
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

            if (pSheet_LastEdit != null)
                WriteConsole(string.Format("마지막에 수정한 시트를 찾았다. 수정한 시간 {0}, SheetID {1}", pSheet_LastEdit.date_LastEdit, pSheet_LastEdit.strSheetID));
            else
                WriteConsole(string.Format("마지막에 수정한 시트를 못찾았다.."));

            return pSheet_LastEdit;
        }

        private void Execute_CommandLine(System.CodeDom.CodeTypeDeclaration pCodeType, List<CommandLineArg> listCommandLine)
        {
            for(int i = 0; i < listCommandLine.Count; i++)
            {
                ECommandLine eCommandLine = (ECommandLine)Enum.Parse(typeof(ECommandLine), listCommandLine[i].strArgName);
                switch (eCommandLine)
                {
                    case ECommandLine.comment:
                        pCodeType.AddComment(listCommandLine[i].strArgValue);
                        break;

                    case ECommandLine.typename:
                        pCodeType.Name = listCommandLine[i].strArgValue;
                        break;

                    default:
                        break;
                }
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
            if (SettingPath(ref textBox_Csharp_Path))
            {
                _pSpreadSheet_CurrentConnected.strOutputPath_Csharp = textBox_Csharp_Path.Text;
                AutoSaveAsync_CurrentSheet();
            }
        }

        private void Button_CSV_PathSetting_Click(object sender, EventArgs e)
        {
            if (SettingPath(ref textBox_CSV_Path))
            {
                _pSpreadSheet_CurrentConnected.strOutputPath_CSV = textBox_CSV_Path.Text;
                AutoSaveAsync_CurrentSheet();
            }
        }


        private void button_TableSave_Click(object sender, EventArgs e)
        {
            _pSheet_CurrentConnected.strCommandLine = textBox_CommandLine.Text;
            AutoSaveAsync_CurrentSheet();
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


        void ParsingSheet(string strSheetName, delOnParsingText OnParsingText)
        {
            const string const_strIgnoreString = "#";

            IList<IList<Object>> pData = _pSheetConnector.GetExcelData(strSheetName);
            if (pData == null)
                return;

            if (OnParsingText == null) // For Loop에서 Null Check 방지
                OnParsingText = (a, b, c, d) => { };

            HashSet<int> setIgnoreColumnIndex = new HashSet<int>();
            for (int i = 1; i < pData.Count; i++)
            {
                IList<object> listRow = pData[i];
                for (int j = 0; j < listRow.Count; j++)
                {
                    if (setIgnoreColumnIndex.Contains(j))
                        continue;

                    string strText = (string)listRow[j];
                    if (listRow[j].Equals(const_strIgnoreString))
                    {
                        setIgnoreColumnIndex.Add(j);
                        continue;
                    }

                    if (string.IsNullOrEmpty(strText))
                        continue;

                    OnParsingText(listRow, strText, i, j);
                }
            }
        }

        private void UpdateUI_Sheet()
        {
            textBox_Csharp_Path.Text = _pSpreadSheet_CurrentConnected.strOutputPath_Csharp;
            textBox_CSV_Path.Text = _pSpreadSheet_CurrentConnected.strOutputPath_CSV;

            textBox_FileName_Csharp.Text = _pSpreadSheet_CurrentConnected.strFileName_Csharp;
        }

        private void checkBox_AutoConnect_CheckedChanged(object sender, EventArgs e)
        {
            if (_bIsLoading_CreateForm)
                return;

            _pConfig.bAutoConnect = checkBox_AutoConnect.Checked;
            AutoSaveAsync_Config();
        }

        private void checkBox_OpenFolder_AfterBuild_Csharp_CheckedChanged(object sender, EventArgs e)
        {
            if (_bIsLoading_CreateForm)
                return;

            _pConfig.bOpenPath_AfterBuild_Csharp = checkBox_OpenFolder_AfterBuild_Csharp.Checked;
            AutoSaveAsync_Config();
        }

        private void checkBox_OpenFolder_AfterBuild_CSV_CheckedChanged(object sender, EventArgs e)
        {
            if (_bIsLoading_CreateForm)
                return;

            _pConfig.bOpenPath_AfterBuild_CSV = checkBox_OpenFolder_AfterBuild_CSV.Checked;
            AutoSaveAsync_Config();
        }

        private void AutoSaveAsync_CurrentSheet()
        {
            if (_bIsUpdating_TableUI)
                return;

            WriteConsole("자동 저장 중.." + _pSpreadSheet_CurrentConnected.strSheetID);
            SaveDataManager.SaveSheet_Async(_pSpreadSheet_CurrentConnected, AutoSaveDone);
        }

        private void AutoSaveAsync_Config()
        {
            WriteConsole("자동 저장 중.. Config");
            SaveDataManager.SaveConfig_Async(_pConfig, AutoSaveDone);
        }

        private void AutoSaveDone(bool bIsSuccess)
        {
            if (bIsSuccess)
                WriteConsole("자동 저장 완료..");
            else
                WriteConsole("자동 저장 실패!");
        }

        private void checkedListBox_TableList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetState(EState.IsConnected_And_SelectTable);
        }

        private SaveData_Sheet GetCurrentSelectedTable_OrNull()
        {
            return (SaveData_Sheet)checkedListBox_SheetList.SelectedItem;
        }

        bool _bIsUpdating_TableUI;
        private void Update_Step_2_TableSetting(SaveData_Sheet pSheetData)
        {
            _bIsUpdating_TableUI = true;

            _pSheet_CurrentConnected = pSheetData;
            if (pSheetData == null)
                return;

            groupBox_SelectedTable.Text = pSheetData.ToString();
            textBox_CommandLine.Text = pSheetData.strCommandLine;

            switch (pSheetData.eType)
            {
                case SaveData_Sheet.EType.Class: radioButton_Class.Checked = true; break;
                case SaveData_Sheet.EType.Struct: radioButton_Struct.Checked = true; break;
                case SaveData_Sheet.EType.Enum: radioButton_Enum.Checked = true; break;
            }

            _bIsUpdating_TableUI = false;
        }

        private void button_Save_FileName_Csharp_Click(object sender, EventArgs e)
        {
            _pSpreadSheet_CurrentConnected.strFileName_Csharp = textBox_FileName_Csharp.Text;
            AutoSaveAsync_CurrentSheet();
        }

        const string const_SheetURL = "https://docs.google.com/spreadsheets/d/";

        private void button_OpenLink_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start($"{const_SheetURL}/{textBox_SheetID.Text}");
        }

        private void button_Cancel_TableCommandLine_Click(object sender, EventArgs e)
        {
            textBox_CommandLine.Text = _pSheet_CurrentConnected.strCommandLine;
        }

        private void radioButton_class_CheckedChanged(object sender, EventArgs e) { _pSheet_CurrentConnected.eType = SaveData_Sheet.EType.Class; AutoSaveAsync_CurrentSheet(); }
        private void radioButton_Struct_CheckedChanged(object sender, EventArgs e) { _pSheet_CurrentConnected.eType = SaveData_Sheet.EType.Struct; AutoSaveAsync_CurrentSheet(); }
        private void radioButton_Enum_CheckedChanged(object sender, EventArgs e) { _pSheet_CurrentConnected.eType = SaveData_Sheet.EType.Enum; AutoSaveAsync_CurrentSheet(); }

        private void button_Info_Click(object sender, EventArgs e)
        {
            CommandLine_InfoForm secondForm = new CommandLine_InfoForm();
            secondForm.Show();
        }
    }
}
