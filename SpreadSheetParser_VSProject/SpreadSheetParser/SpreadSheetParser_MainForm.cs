using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SpreadSheetParser
{
    public partial class SpreadSheetParser_MainForm : Form
    {
        public enum ECommandLine
        {
            Error = -1,

            comment,
            ispartial,
            baseis,
        }

        public enum EState
        {
            None,
            IsConnected,
            IsConnected_And_SelectTable,
        }

        public enum EEnumHeaderType
        {
            EnumNone,

            EnumType,
            EnumValue,
            NumberValue,
            Comment,
        }

        static public SpreadSheetParser_MainForm isntance => _instance;
        static private SpreadSheetParser_MainForm _instance;

        static public SaveData_SpreadSheet pSpreadSheet_CurrentConnected { get; private set; }
        static public SpreadSheetConnector pSheetConnector { get; private set; } = new SpreadSheetConnector();

        delegate void SafeCallDelegate(string text);

        SaveData_Sheet _pSheet_CurrentConnected;
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

        static public void DoOpenPath(string strPath)
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

        private void ListView_Field_SelectedIndexChanged(object sender, EventArgs e)
        {
            FieldTypeData pFieldData = null;
            bool bEnable = listView_Field.SelectedIndices.Count > 0;
            if (bEnable)
            {
                pFieldData = (FieldTypeData)listView_Field.SelectedItems[0].Tag;
            }
            else
            {
                groupBox_2_2_SelectedField.Enabled = false;
                groupBox_2_2_SelectedField_Virtual.Enabled = false;
                checkBox_Field_ThisIsKey.Checked = false;
                checkBox_FieldKey_IsOverlap.Checked = false;

                textBox_FieldName.Text = "";
                textBox_Type.Text = "";
                comboBox_DependencyField.SelectedText = "";
                comboBox_DependencyField_Sub.SelectedText = "";

                return;
            }

            SaveData_Sheet pSheetData = (SaveData_Sheet)checkedListBox_SheetList.SelectedItem;
            if (pSheetData == null)
                return;

            bool bIsEnum = pSheetData.eType == SaveData_Sheet.EType.Enum;
            if (bIsEnum)
                return;

            comboBox_DependencyField.Items.Clear();
            comboBox_DependencyField.Items.AddRange(pSheetData.listFieldData.Where((pOption) => pOption.strFieldType == "string").Select((pOption) => pOption.strFieldName).ToArray());

            comboBox_DependencyField_Sub.Items.Clear();
            comboBox_DependencyField_Sub.Items.AddRange(pSheetData.listFieldData.Where((pOption) => pOption.strFieldType == "string").Select((pOption) => pOption.strFieldName).ToArray());

            groupBox_2_2_SelectedField.Enabled = true;
            groupBox_2_2_SelectedField_Virtual.Enabled = pFieldData.bIsVirtualField;

            textBox_FieldName.Text = pFieldData.strFieldName;
            textBox_Type.Text = pFieldData.strFieldType;

            checkBox_ConvertStringToEnum.Enabled = pFieldData.strFieldType == "string";
            if (checkBox_ConvertStringToEnum.Enabled)
                checkBox_ConvertStringToEnum.Checked = pFieldData.bConvertStringToEnum;
            else
                checkBox_ConvertStringToEnum.Checked = false;

            textBox_EnumName.Enabled = pFieldData.bConvertStringToEnum;
            if (pFieldData.bConvertStringToEnum)
                textBox_EnumName.Text = pFieldData.strEnumName;
            else
                textBox_EnumName.Text = "";


            checkBox_DeleteField_OnCode.Checked = pFieldData.bDeleteThisField_InCode;
            checkBox_IsHeaderField.Enabled = pFieldData.bDeleteThisField_InCode == false;
            if (checkBox_IsHeaderField.Enabled)
                checkBox_IsHeaderField.Checked = pSheetData.strHeaderFieldName == pFieldData.strFieldName;
            else
                checkBox_IsHeaderField.Checked = false;

            checkBox_Field_ThisIsKey.Checked = pFieldData.bIsKeyField;
            checkBox_FieldKey_IsOverlap.Enabled = pFieldData.bIsKeyField;
            if (checkBox_FieldKey_IsOverlap.Enabled)
                checkBox_FieldKey_IsOverlap.Checked = pFieldData.bIsOverlapKey;
            else
                checkBox_FieldKey_IsOverlap.Enabled = false;

            if (string.IsNullOrEmpty(pFieldData.strDependencyFieldName) == false)
                comboBox_DependencyField.SelectedIndex = comboBox_DependencyField.Items.IndexOf(pFieldData.strDependencyFieldName);

            if(string.IsNullOrEmpty(pFieldData.strDependencyFieldName_Sub) == false)
                comboBox_DependencyField_Sub.SelectedIndex = comboBox_DependencyField.Items.IndexOf(pFieldData.strDependencyFieldName_Sub);
        }

        private void CheckedListBox_SheetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView_Field.Items.Clear();

            ListView_Field_SelectedIndexChanged(null, null);
            SaveData_Sheet pSheetData = (SaveData_Sheet)checkedListBox_SheetList.SelectedItem;
            if (pSheetData == null)
                return;

            bool bIsEnum = pSheetData.eType == SaveData_Sheet.EType.Enum;
            if (bIsEnum)
                return;

            List<FieldTypeData> listFieldOption = pSheetData.listFieldData;
            HashSet<string> setRealField = new HashSet<string>();
            pSheetData.ParsingSheet((IList<object> listRow, string strText, int iRowIndex, int iColumnIndex) =>
            {
                if (strText.Contains(":") == false)
                    return;

                string[] arrText = strText.Split(':');
                string strField = arrText[0];
                setRealField.Add(strField);
                FieldTypeData[] arrExportOption = pSheetData.listFieldData.Where((pField) => pField.strFieldName == strField).ToArray();
                if (arrExportOption.Length == 0)
                {
                    arrExportOption = new FieldTypeData[1];
                    arrExportOption[0] = new FieldTypeData(strField, arrText[1]);
                    pSheetData.listFieldData.Add(arrExportOption[0]);
                }

                if(arrExportOption.Length > 1)
                {
                    for(int i = 1; i < arrExportOption.Length; i++)
                        pSheetData.listFieldData.Remove(arrExportOption[i]);
                }

                listView_Field.Items.Add(arrExportOption[0].ConvertListViewItem());
            });

            IEnumerable<FieldTypeData> pDeleteFieldOption = listFieldOption.Where((pFieldOption) => setRealField.Contains(pFieldOption.strFieldName) == false);
            if(pDeleteFieldOption.Count() != 0)
            {
                foreach (FieldTypeData pFieldOption in pDeleteFieldOption)
                {
                    pFieldOption.bIsVirtualField = true;
                    listView_Field.Items.Add(pFieldOption.ConvertListViewItem());
                }

                AutoSaveAsync_CurrentSheet();
            }
        }

        private void CheckedListBox_WorkList_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool bIsSelected = checkedListBox_WorkList.SelectedIndex != -1;
            groupBox_3_1_SelectedWork.Enabled = bIsSelected;
            button_RemoveWork.Enabled = bIsSelected;
        }

        private void CheckedListBox_WorkList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_bIsConnecting)
                return;

            pSpreadSheet_CurrentConnected.listSaveWork[e.Index].bEnable = e.NewValue == CheckState.Checked;
            AutoSaveAsync_CurrentSheet();
        }

        private void CheckedListBox_TableList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_bIsConnecting)
                return;

            pSpreadSheet_CurrentConnected.listTable[e.Index].bEnable = e.NewValue == CheckState.Checked;
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

            checkedListBox_WorkList.Items.Clear();
            checkedListBox_SheetList.Items.Clear();
            await pSheetConnector.DoConnect(strSheetID, OnFinishConnect);
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
                pSpreadSheet_CurrentConnected = _mapSaveData[strSheetID];
                List<SaveData_Sheet> listSavedTable = pSpreadSheet_CurrentConnected.listTable;

                for (int i = 0; i < listSheet.Count; i++)
                {
                    string strSheetName = listSheet[i].ToString();
                    if (listSavedTable.Where(x => (x.strSheetName == strSheetName)).Count() == 0)
                        listSavedTable.Add(new SaveData_Sheet(strSheetName));
                }
            }
            else
            {
                pSpreadSheet_CurrentConnected = new SaveData_SpreadSheet(strSheetID);
                _mapSaveData[pSpreadSheet_CurrentConnected.strSheetID] = pSpreadSheet_CurrentConnected;

                pSpreadSheet_CurrentConnected.listTable.Clear();
                for (int i = 0; i < listSheet.Count; i++)
                    pSpreadSheet_CurrentConnected.listTable.Add(new SaveData_Sheet(listSheet[i].ToString()));

                SaveDataManager.SaveSheet(pSpreadSheet_CurrentConnected);

                WriteConsole("새 파일을 만들었습니다.");
            }

            checkedListBox_SheetList.Items.Clear();
            List<SaveData_Sheet> listSheetSaved = pSpreadSheet_CurrentConnected.listTable;

            SaveData_Sheet[] arrSheetDelete = listSheetSaved.Where((pSheet) =>
                listSheet.Where((pSheetWrapper) => 
                    pSheetWrapper.ToString() == pSheet.strSheetName).Count() == 0).ToArray();

            if(arrSheetDelete.Length > 0)
            {
                for (int i = 0; i < arrSheetDelete.Length; i++)
                    listSheetSaved.Remove(arrSheetDelete[i]);

                AutoSaveAsync_CurrentSheet();
            }

            for (int i = 0; i < listSheetSaved.Count; i++)
                checkedListBox_SheetList.Items.Add(listSheetSaved[i], listSheetSaved[i].bEnable);

            checkedListBox_WorkList.Items.Clear();
            List<WorkBase> listWorkBase = pSpreadSheet_CurrentConnected.listSaveWork;
            listWorkBase.Sort((x, y) => x.iWorkOrder.CompareTo(y.iWorkOrder));
            for (int i = 0; i < listWorkBase.Count; i++)
                checkedListBox_WorkList.Items.Add(listWorkBase[i], listWorkBase[i].bEnable);

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
                    ((SaveData_Sheet)pItem).DoWork(_pCodeFileBuilder);
            }
            catch (Exception pException)
            {
                WriteConsole("빌드 실패.." + pException);
                return;
            }

            var listSheetData = checkedListBox_SheetList.CheckedItems.Cast<SaveData_Sheet>();
            var listWork = checkedListBox_WorkList.CheckedItems;
            foreach(WorkBase pWork in listWork)
            {
                try
                {
                    pWork.DoWork(_pCodeFileBuilder, listSheetData);
                }
                catch(Exception pException)
                {
                    WriteConsole($"빌드 중 에러 - Work : {pWork.pType} // Error : {pException}");
                    return;
                }
            }

            foreach (WorkBase pWork in listWork)
                pWork.DoWorkAfter();

            WriteConsole("빌드 완료");
        }

        private void button_CheckTable_Click(object sender, EventArgs e)
        {
            SaveData_Sheet pSheetData = GetCurrentSelectedTable_OrNull();
            WriteConsole("테이블 유효성 체크중.." + pSheetData.ToString());
            int iErrorCount = 0;

            try
            {
                SaveData_SheetHelper.DoCheck_IsValid_Table(pSheetData);
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

        private SaveData_SpreadSheet GetSheet_LastEdit(Dictionary<string, SaveData_SpreadSheet> mapSaveData)
        {
            WriteConsole("마지막에 수정한 Sheet 찾는 중..");

            DateTime date_LastEdit = DateTime.MinValue;
            SaveData_SpreadSheet pSheet_LastEdit = null;
            foreach (var pSheet in mapSaveData.Values)
            {
                if (date_LastEdit < pSheet.date_LastEdit)
                {
                    date_LastEdit = pSheet.date_LastEdit;
                    pSheet_LastEdit = pSheet;
                }
            }

            if (pSheet_LastEdit != null)
                WriteConsole(string.Format("마지막에 수정한 시트를 찾았다. 수정한 시간 {0}, SheetID {1}", pSheet_LastEdit.date_LastEdit, pSheet_LastEdit.strSheetID));
            else
                WriteConsole(string.Format("마지막에 수정한 시트를 못찾았다.."));

            return pSheet_LastEdit;
        }

        private void Button_OpenPath_SaveSheet_Click(object sender, EventArgs e)
        {
            string strSaveFolderPath = SaveDataManager.const_strSaveFolderPath;
            DoOpenPath(strSaveFolderPath.Remove(strSaveFolderPath.Length - 1, 1));
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

        private void checkBox_AutoConnect_CheckedChanged(object sender, EventArgs e)
        {
            if (_bIsLoading_CreateForm)
                return;

            _pConfig.bAutoConnect = checkBox_AutoConnect.Checked;
            AutoSaveAsync_Config();
        }

        private void AutoSaveAsync_CurrentSheet()
        {
            if (_bIsUpdating_TableUI)
                return;

            pSpreadSheet_CurrentConnected.UpdateDate();
            WriteConsole("자동 저장 중.." + pSpreadSheet_CurrentConnected.strSheetID);
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
            _bIsUpdating_TableUI = true;

            _pSheet_CurrentConnected = pSheetData;
            if (pSheetData == null)
                return;

            groupBox_SelectedTable.Text = pSheetData.ToString();
            textBox_TableFileName.Text = pSheetData.strFileName;
            textBox_CommandLine.Text = pSheetData.strCommandLine;

            switch (pSheetData.eType)
            {
                case SaveData_Sheet.EType.Class: radioButton_Class.Checked = true; break;
                case SaveData_Sheet.EType.Struct: radioButton_Struct.Checked = true; break;
                case SaveData_Sheet.EType.Enum: radioButton_Enum.Checked = true; break;
                case SaveData_Sheet.EType.Global: radioButton_Global.Checked = true; break;

            }

            _bIsUpdating_TableUI = false;
        }

        private void button_OpenLink_Click(object sender, EventArgs e)
        {
            const string const_SheetURL = "https://docs.google.com/spreadsheets/d/";

            System.Diagnostics.Process.Start($"{const_SheetURL}/{textBox_SheetID.Text}");
        }

        private void OnChangeValue_TypeRadioButton(object sender, EventArgs e)
        {
            if (_bIsUpdating_TableUI)
                return;

            if (radioButton_Class.Checked)
            {
                _pSheet_CurrentConnected.eType = SaveData_Sheet.EType.Class;
            }
            else if(radioButton_Struct.Checked)
            {
                _pSheet_CurrentConnected.eType = SaveData_Sheet.EType.Struct;
            }
            else if (radioButton_Enum.Checked)
            {
                _pSheet_CurrentConnected.eType = SaveData_Sheet.EType.Enum;
            }
            else if (radioButton_Global.Checked)
            {
                _pSheet_CurrentConnected.eType = SaveData_Sheet.EType.Global;
            }

            AutoSaveAsync_CurrentSheet();
        }

        private void button_AddWork_Click(object sender, EventArgs e)
        {
            WorkBase pWork = (WorkBase)comboBox_WorkList.SelectedItem;
            if (pWork == null)
                return;

            WorkBase pNewWork = pWork.CopyInstance();
            pNewWork.ShowForm();
            checkedListBox_WorkList.Items.Add(pNewWork);
            pSpreadSheet_CurrentConnected.listSaveWork.Add(pNewWork);
            Update_WorkListOrder();
        }

        private void button_EditWork_Click(object sender, EventArgs e)
        {
            WorkBase pWork = (WorkBase)checkedListBox_WorkList.SelectedItem;
            pWork.ShowForm();
        }

        private void button_RemoveWork_Click(object sender, EventArgs e)
        {
            pSpreadSheet_CurrentConnected.listSaveWork.RemoveAt(checkedListBox_WorkList.SelectedIndex);
            checkedListBox_WorkList.Items.RemoveAt(checkedListBox_WorkList.SelectedIndex);
            Update_WorkListOrder();
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

        private void button_WorkOrderUp_Click(object sender, EventArgs e)
        {
            int iSelectedIndex = checkedListBox_WorkList.SelectedIndex;
            object pSelectedItem = checkedListBox_WorkList.SelectedItem;

            if (iSelectedIndex == 0)
                return;

            if (pSelectedItem == null)
                return;

            checkedListBox_WorkList.SelectedIndex = -1;
            checkedListBox_WorkList.Items.RemoveAt(iSelectedIndex);
            checkedListBox_WorkList.Items.Insert(iSelectedIndex - 1, pSelectedItem);
            checkedListBox_WorkList.SelectedIndex = iSelectedIndex - 1;
            Update_WorkListOrder();
        }

        private void button_WorkOrderDown_Click(object sender, EventArgs e)
        {
            int iSelectedIndex = checkedListBox_WorkList.SelectedIndex;
            object pSelectedItem = checkedListBox_WorkList.SelectedItem;

            if (iSelectedIndex == checkedListBox_WorkList.Items.Count - 1)
                return;

            if (pSelectedItem == null)
                return;

            checkedListBox_WorkList.SelectedIndex = -1;
            checkedListBox_WorkList.Items.RemoveAt(iSelectedIndex);
            checkedListBox_WorkList.Items.Insert(iSelectedIndex + 1, pSelectedItem);
            checkedListBox_WorkList.SelectedIndex = iSelectedIndex + 1;
            Update_WorkListOrder();
        }

        void Update_WorkListOrder()
        {
            int iSortOrder = 0;
            foreach(WorkBase pWork in checkedListBox_WorkList.Items)
                pWork.iWorkOrder = iSortOrder++;

            AutoSaveAsync_CurrentSheet();
        }

        private void comboBox_SaveSheet_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_SheetID.Text = comboBox_SaveSheet.Text;
        }

        private void button_Add_VirtualField_Click(object sender, EventArgs e)
        {
            FieldTypeData pFieldOption = new FieldTypeData("None", "None");
            pFieldOption.bIsVirtualField = true;

            _pSheet_CurrentConnected.listFieldData.Add(pFieldOption);
            listView_Field.Items.Add(pFieldOption.ConvertListViewItem());

            AutoSaveAsync_CurrentSheet();
        }

        private void button_Remove_VirtualField_Click(object sender, EventArgs e)
        {
            if (listView_Field.SelectedItems.Count == 0)
                return;

            var pSelectedItem = listView_Field.SelectedItems[0];
            FieldTypeData pFieldOption = (FieldTypeData)pSelectedItem.Tag;

            _pSheet_CurrentConnected.listFieldData.Remove(pFieldOption);
            listView_Field.Items.Remove(pSelectedItem);

            AutoSaveAsync_CurrentSheet();
        }

        private void button_Save_Field_Click(object sender, EventArgs e)
        {
            if (listView_Field.SelectedItems.Count == 0)
                return;

            var pSelectedItem = listView_Field.SelectedItems[0];
            FieldTypeData pFieldData = (FieldTypeData)pSelectedItem.Tag;

            pFieldData.strDependencyFieldName = (string)comboBox_DependencyField.SelectedItem;
            pFieldData.strDependencyFieldName_Sub = (string)comboBox_DependencyField_Sub.SelectedItem;
            pFieldData.strFieldName = textBox_FieldName.Text;
            pFieldData.strFieldType = textBox_Type.Text;

            pFieldData.bConvertStringToEnum = checkBox_ConvertStringToEnum.Checked;
            pFieldData.strEnumName = textBox_EnumName.Text;


            pSelectedItem.Text = pFieldData.strFieldName;
            pFieldData.Reset_ListViewItem(pSelectedItem);

            AutoSaveAsync_CurrentSheet();
        }

        private void button_Check_TableAll_Click(object sender, EventArgs e)
        {

        }

        private void button_Save_FileName_Click(object sender, EventArgs e)
        {
            _pSheet_CurrentConnected.strFileName = textBox_TableFileName.Text;
            AutoSaveAsync_CurrentSheet();
        }

        private void buttonSave_CommandLine_Click(object sender, EventArgs e)
        {
            _pSheet_CurrentConnected.strCommandLine = textBox_CommandLine.Text;
            AutoSaveAsync_CurrentSheet();
        }

        private void checkBox_Field_NullOrEmtpy_IsError_CheckedChanged(object sender, EventArgs e)
        {
            if (listView_Field.SelectedItems.Count == 0)
                return;

            var pSelectedItem = listView_Field.SelectedItems[0];
            FieldTypeData pFieldData = (FieldTypeData)pSelectedItem.Tag;

            pFieldData.bIsKeyField = checkBox_Field_ThisIsKey.Checked;
            checkBox_FieldKey_IsOverlap.Enabled = pFieldData.bIsKeyField;
        }

        private void checkBox_DeleteField_OnAfterBuild_CheckedChanged(object sender, EventArgs e)
        {
            if (listView_Field.SelectedItems.Count == 0)
                return;

            var pSelectedItem = listView_Field.SelectedItems[0];
            FieldTypeData pFieldData = (FieldTypeData)pSelectedItem.Tag;

            pFieldData.bDeleteThisField_InCode = checkBox_DeleteField_OnCode.Checked;
        }

        private void checkBox_ConvertStringToEnum_CheckedChanged(object sender, EventArgs e)
        {
            if (listView_Field.SelectedItems.Count == 0)
                return;

            var pSelectedItem = listView_Field.SelectedItems[0];
            FieldTypeData pFieldData = (FieldTypeData)pSelectedItem.Tag;

            pFieldData.bConvertStringToEnum = checkBox_ConvertStringToEnum.Checked;
            textBox_EnumName.Enabled = checkBox_ConvertStringToEnum.Checked;
        }

        private void checkBox_IsHeaderField_CheckedChanged(object sender, EventArgs e)
        {
            if (listView_Field.SelectedItems.Count == 0)
                return;

            var pSelectedItem = listView_Field.SelectedItems[0];
            FieldTypeData pFieldData = (FieldTypeData)pSelectedItem.Tag;

            if (checkBox_IsHeaderField.Checked)
                _pSheet_CurrentConnected.strHeaderFieldName = pFieldData.strFieldName;
            else if(_pSheet_CurrentConnected.strHeaderFieldName == pFieldData.strFieldName)
                _pSheet_CurrentConnected.strHeaderFieldName = "";
        }

        private void checkBox_FieldKey_IsOverlap_CheckedChanged(object sender, EventArgs e)
        {
            if (listView_Field.SelectedItems.Count == 0)
                return;

            var pSelectedItem = listView_Field.SelectedItems[0];
            FieldTypeData pFieldData = (FieldTypeData)pSelectedItem.Tag;

            pFieldData.bIsOverlapKey = checkBox_FieldKey_IsOverlap.Checked;
        }

    }
}
