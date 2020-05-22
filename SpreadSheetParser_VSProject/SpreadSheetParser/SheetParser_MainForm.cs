using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SpreadSheetParser
{
    public partial class SheetParser_MainForm : Form
    {
        public enum EState
        {
            None,
            IsConnected,
            IsConnected_And_SelectTable,
        }

        public static SheetParser_MainForm isntance => _instance;
        private static SheetParser_MainForm _instance;

        public static SaveData_SheetSourceCollection pSheetSource_Selected { get; private set; }

        /// <summary>
        /// Key is SheetSource ID
        /// </summary>
        public static Dictionary<string, SheetSourceConnector> mapSheetSourceConnector { get; private set; } =
            new Dictionary<string, SheetSourceConnector>();

        delegate void SafeCallDelegate(string text);

        TypeData _pSheet_CurrentConnected;
        CodeFileBuilder _pCodeFileBuilder = new CodeFileBuilder();
        Config _pConfig;

        Dictionary<string, SaveData_SheetSource> _mapSaveSheetSource = new Dictionary<string, SaveData_SheetSource>();
        Dictionary<string, SaveData_SheetSourceCollection> _mapSaveSheetSourceCollection = new Dictionary<string, SaveData_SheetSourceCollection>();

        EState _eState;
        bool _bIsConnecting;
        bool _bIsUpdating_TableUI;
        bool _bIsLoading_CreateForm = false;

        public SheetParser_MainForm()
        {
            InitializeComponent();

            _instance = this;
        }

        public static void WriteConsole(string strText)
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

        public static void DoOpenFolder(string strPath)
        {
            WriteConsole($"폴더 열기 시도.. 경로{strPath}");
            try
            {
                System.Diagnostics.Process.Start(strPath);
                WriteConsole($"폴더 열기 성공.. 경로{strPath}");
            }
            catch (Exception pException)
            {
                WriteConsole($"폴더 열기 실패.. 경로{strPath}");
                WriteConsole($"에러:{ pException}");
            }
        }

        public delegate void delOnCheck_IsCorrectPath(string strPath, ref string strErrorMessage);

        public static bool DoShowFileBrowser_And_SavePath(bool bIsAbsolutePath, ref TextBox pTextBox_Path, delOnCheck_IsCorrectPath OnCheck_IsCorrect)
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

        public static bool DoShowFolderBrowser_And_SavePath(bool bIsAbsolutePath, ref TextBox pTextBox_Path)
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

        // ========================================================================================================

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetState(EState.None);
            _bIsLoading_CreateForm = true;

            _pConfig = SaveDataManager.LoadConfig();
            _mapSaveSheetSource = SaveDataManager.LoadSheet_SaveSheetSource(WriteConsole);
            _mapSaveSheetSourceCollection = SaveDataManager.LoadSheet_SaveSheetSourceCollection(WriteConsole);

            radioButton_SheetSource_GoogleSheet.Checked = true;
            comboBox_DependencyField.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_DependencyField_Sub.DropDownStyle = ComboBoxStyle.DropDownList;

            SaveData_SheetSourceCollection pSheetSourceLastEdit = GetSheetSource_LastEdit(_mapSaveSheetSourceCollection);
            if (_pConfig.bAutoConnect && pSheetSourceLastEdit != null)
            {
                WriteConsole("Config - 자동연결로 인해 연결을 시작합니다..");
                pSheetSourceLastEdit.DoInit(_mapSaveSheetSource);

                mapSheetSourceConnector.Clear();
                foreach (var pSaveSheetSource in pSheetSourceLastEdit.mapSaveSheetSource.Values)
                {
                    string strSheetSourceID = pSaveSheetSource.strSheetSourceID;
                    SheetSourceConnector pSheetSourceConnector = Create_SheetSourceConnector(strSheetSourceID, pSaveSheetSource.eSourceType);

                    mapSheetSourceConnector.Add(strSheetSourceID, pSheetSourceConnector);
                    listView_SheetSourceConnector.Items.Add(pSheetSourceConnector.ConvertListViewItem());
                }
            }

            // checkBox_AutoConnect.Checked = _pConfig.bAutoConnect;
            listView_Sheet.ItemCheck += CheckedListBox_TableList_ItemCheck;
            listView_Sheet.SelectedIndexChanged += CheckedListBox_SheetList_SelectedIndexChanged;

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
                    groupBox3_BuildSetting.Enabled = false;
                    groupBox_SelectedTable.Enabled = false;
                    break;

                case EState.IsConnected:
                    groupBox_2_1_TableSetting.Enabled = true;
                    groupBox3_BuildSetting.Enabled = true;
                    groupBox_SelectedTable.Enabled = false;

                    if (GetCurrentSelectedTable_OrNull() != null)
                        SetState(EState.IsConnected_And_SelectTable);

                    break;

                case EState.IsConnected_And_SelectTable:
                    groupBox_2_1_TableSetting.Enabled = true;
                    groupBox3_BuildSetting.Enabled = true;
                    groupBox_SelectedTable.Enabled = true;

                    var pWrapper = GetCurrentSelectedTable_OrNull();
                    if (pWrapper != null)
                        Update_Step_2_TableSetting(pWrapper);
                    else
                        SetState(EState.IsConnected);

                    break;
            }
        }

        private void AutoSaveAsync_CurrentSheet()
        {
            if (_bIsUpdating_TableUI)
                return;

            // pSheetSource_Selected.UpdateDate();
            WriteConsole("자동 저장 중.." + pSheetSource_Selected.strFileName);
            SaveDataManager.SaveSheet_Async(pSheetSource_Selected, AutoSaveDone);
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
            return (TypeData)listView_Sheet.SelectedItems.Cast<TypeData>().FirstOrDefault();
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

        private void button_AddSheetSource_Click(object sender, EventArgs e)
        {
            string strSheetSourceID = textBox_AddSheetSourceName.Text;
            if(mapSheetSourceConnector.ContainsKey(strSheetSourceID))
            {
                WriteConsole($"이미 추가된 시트.. \n{strSheetSourceID}");
                return;
            }

            SheetSourceConnector pConnector = null;
            if(radioButton_SheetSource_GoogleSheet.Checked)
                pConnector = Create_SheetSourceConnector(strSheetSourceID, ESheetSourceType.GoogleSpreadSheet);
            else if (radioButton_SheetSource_MSExcel.Checked)
                pConnector = Create_SheetSourceConnector(strSheetSourceID, ESheetSourceType.MSExcel);

            _bIsConnecting = true;
            pConnector?.ISheetSourceConnector_DoConnect_And_Parsing(OnFinishConnect);
        }

        private SheetSourceConnector Create_SheetSourceConnector(string strSheetID, ESheetSourceType eSheetSourceType)
        {
            SheetSourceConnector pConnector = SheetSourceConnector.DoCreate_SheetSource(eSheetSourceType, strSheetID);

            mapSheetSourceConnector.Add(strSheetID, pConnector);
            listView_SheetSourceConnector.Items.Add(pConnector.ConvertListViewItem());
            return pConnector;
        }

        private void button_OpenSheetSource_Click(object sender, EventArgs e)
        {
            if (radioButton_SheetSource_GoogleSheet.Checked)
                SheetSourceConnector.DoOpen_SheetSource(ESheetSourceType.GoogleSpreadSheet,textBox_AddSheetSourceName.Text, WriteConsole);
         
            if (radioButton_SheetSource_MSExcel.Checked)
                SheetSourceConnector.DoOpen_SheetSource(ESheetSourceType.MSExcel, textBox_AddSheetSourceName.Text, WriteConsole);
        }
    }
}
