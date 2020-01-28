using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace SpreadSheetParser
{
    public partial class SpreadSheetParser_MainForm : Form
    {
        public enum EState
        {
            None,
            IsConnected,
            IsConnected_And_SelectTable,
        }

        static public SpreadSheetParser_MainForm isntance => _instance;
        static private SpreadSheetParser_MainForm _instance;

        static public SaveData_SpreadSheet pSpreadSheet_CurrentConnected { get; private set; }
        static public SpreadSheetConnector pSheetConnector { get; private set; } = new SpreadSheetConnector();

        delegate void SafeCallDelegate(string text);

        TypeData _pSheet_CurrentConnected;
        CodeFileBuilder _pCodeFileBuilder = new CodeFileBuilder();
        Config _pConfig;

        Dictionary<string, SaveData_SpreadSheet> _mapSaveData = new Dictionary<string, SaveData_SpreadSheet>();

        EState _eState;
        bool _bIsConnecting;
        bool _bIsUpdating_TableUI;
        bool _bIsLoading_CreateForm = false;

        public SpreadSheetParser_MainForm()
        {
            InitializeComponent();

            _instance = this;
        }

        static public void WriteConsole(string strText)
        {
            // Winform 컨트롤을 스레드로부터 안전하게 호출하는 법
            // https://docs.microsoft.com/ko-kr/dotnet/framework/winforms/controls/how-to-make-thread-safe-calls-to-windows-forms-controls
            if (_instance.textBox_Log.InvokeRequired)
            {
                var pDelegate = new SafeCallDelegate(WriteConsole);
                _instance.textBox_Log.Invoke(pDelegate, new object[] { strText });
            }
            else
            {
                _instance.textBox_Log.AppendText(strText);
                _instance.textBox_Log.AppendText(Environment.NewLine);
            }
        }

        static public void DoOpenFolder(string strPath)
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

        public delegate void delOnCheck_IsCorrectPath(string strPath, ref string strErrorMessage);

        static public bool DoShowFileBrowser_And_SavePath(bool bIsAbsolutePath, ref TextBox pTextBox_Path, delOnCheck_IsCorrectPath OnCheck_IsCorrect)
        {
            if (OnCheck_IsCorrect == null)
                OnCheck_IsCorrect = (string strPath, ref string strErrorMessage) => strErrorMessage = null;

            using (OpenFileDialog pDialog = new OpenFileDialog())
            {
                if (pDialog.ShowDialog() == DialogResult.OK)
                {
                    string strErrorMessage = null;
                    OnCheck_IsCorrect(pDialog.FileName, ref strErrorMessage);
                    if(string.IsNullOrEmpty(strErrorMessage))
                    {
                        if (bIsAbsolutePath)
                            pTextBox_Path.Text = pDialog.FileName;
                        else
                            pTextBox_Path.Text = DoMake_RelativePath(pDialog.FileName);
                        return true;
                    }
                    else
                    {
                        MessageBox.Show(strErrorMessage, null, MessageBoxButtons.OK);
                    }
                }
            }

            return false;
        }

        static public bool DoShowFolderBrowser_And_SavePath(bool bIsAbsolutePath, ref TextBox pTextBox_Path)
        {
            using (FolderBrowserDialog pDialog = new FolderBrowserDialog())
            {
                pDialog.SelectedPath = Directory.GetCurrentDirectory();
                if (pDialog.ShowDialog() == DialogResult.OK)
                {
                    if (bIsAbsolutePath)
                        pTextBox_Path.Text = pDialog.SelectedPath;
                    else
                        pTextBox_Path.Text = DoMake_RelativePath(pDialog.SelectedPath);

                    return true;
                }
            }

            return false;
        }

        // https://stackoverflow.com/questions/13266756/absolute-to-relative-path
        public static string DoMake_RelativePath(string strPath)
        {
            var pFileURI = new Uri(strPath);
            var pCurrentURI = new Uri(Directory.GetCurrentDirectory());

            return pCurrentURI.MakeRelativeUri(pFileURI).ToString();
        }

        public static string DoMake_AbsolutePath(string strPath)
        {
            if (Path.IsPathRooted(strPath))
                return strPath;

            var pCurrentURI = new Uri(Directory.GetCurrentDirectory());
            return $"{pCurrentURI.AbsolutePath}/../{strPath}";
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetState(EState.None);
            _bIsLoading_CreateForm = true;

            _pConfig = SaveDataManager.LoadConfig();
            _mapSaveData = SaveDataManager.LoadSheet(WriteConsole);

            comboBox_DependencyField.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_DependencyField_Sub.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_SaveSheet.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_SaveSheet.Items.Clear();
            comboBox_SaveSheet.Items.AddRange(_mapSaveData.Values.Where(p => p.eType == ESpreadSheetType.GoogleSpreadSheet).Cast<object>().ToArray());

            SaveData_SpreadSheet pSheet_LastEdit = GetSheet_LastEdit(_mapSaveData);
            if (pSheet_LastEdit != null)
            {
                switch (pSheet_LastEdit.eType)
                {
                    case ESpreadSheetType.MSExcel:
                        textBox_ExcelPath_ForConnect.Text = pSheet_LastEdit.strSheetID;
                        if (_pConfig.bAutoConnect)
                        {
                            WriteConsole("Config - 자동연결로 인해 연결을 시작합니다..");
                            button_Connect_Excel_Click(null, null);
                        }
                        break;

                    case ESpreadSheetType.GoogleSpreadSheet:
                        textBox_SheetID.Text = pSheet_LastEdit.strSheetID;
                        if (_pConfig.bAutoConnect)
                        {
                            WriteConsole("Config - 자동연결로 인해 연결을 시작합니다..");
                            button_Connect_Click(null, null);
                        }
                        break;
                }
            }

            comboBox_ExcelPath_Saved.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_ExcelPath_Saved.Items.Clear();
            comboBox_ExcelPath_Saved.Items.AddRange(_mapSaveData.Values.Where(p => p.eType == ESpreadSheetType.MSExcel).Cast<object>().ToArray());

            checkBox_AutoConnect.Checked = _pConfig.bAutoConnect;
            checkedListBox_SheetList.ItemCheck += CheckedListBox_TableList_ItemCheck;
            checkedListBox_SheetList.SelectedIndexChanged += CheckedListBox_SheetList_SelectedIndexChanged;
            checkedListBox_WorkList.ItemCheck += CheckedListBox_WorkList_ItemCheck;
            checkedListBox_WorkList.SelectedIndexChanged += CheckedListBox_WorkList_SelectedIndexChanged;
            CheckedListBox_WorkList_SelectedIndexChanged(null, null);

            comboBox_WorkList.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_WorkList.Items.Clear();
            var listWork = GetEnumerableOfType<WorkBase>();
            foreach(var pWork in listWork)
                comboBox_WorkList.Items.Add(pWork);

            listView_Field.SelectedIndexChanged += ListView_Field_SelectedIndexChanged;

            _bIsLoading_CreateForm = false;
        }

        private void SetState(EState eState)
        {
            _eState = eState;

            switch (_eState)
            {
                case EState.None:
                    groupBox_2_1_TableSetting.Enabled = false;
                    groupBox3_WorkSetting.Enabled = false;
                    groupBox_SelectedTable.Enabled = false;
                    break;

                case EState.IsConnected:
                    groupBox_2_1_TableSetting.Enabled = true;
                    groupBox3_WorkSetting.Enabled = true;
                    groupBox_SelectedTable.Enabled = false;

                    if (GetCurrentSelectedTable_OrNull() != null)
                        SetState(EState.IsConnected_And_SelectTable);

                    break;

                case EState.IsConnected_And_SelectTable:
                    groupBox_2_1_TableSetting.Enabled = true;
                    groupBox3_WorkSetting.Enabled = true;
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

        private void AutoSaveAsync_CurrentSheet()
        {
            if (_bIsUpdating_TableUI)
                return;

            pSpreadSheet_CurrentConnected.UpdateDate();
            WriteConsole("자동 저장 중.." + pSpreadSheet_CurrentConnected.GetFileName());
            SaveDataManager.SaveSheet_Async(pSpreadSheet_CurrentConnected, AutoSaveDone);
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

        private TypeData GetCurrentSelectedTable_OrNull()
        {
            return (TypeData)checkedListBox_SheetList.SelectedItem;
        }

        public static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorArgs)
            where T : class
        {
            List<T> objects = new List<T>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            {
                objects.Add((T)Activator.CreateInstance(type, constructorArgs));
            }
            return objects;
        }

        private void button_LogClear_Click(object sender, EventArgs e)
        {
            textBox_Log.Text = "";
        }
    }
}
