namespace SpreadSheetParser
{
    partial class SpreadSheetParser_MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox_SheetID = new System.Windows.Forms.TextBox();
            this.button_Connect_Sheet = new System.Windows.Forms.Button();
            this.checkedListBox_SheetList = new System.Windows.Forms.CheckedListBox();
            this.button_StartParsing = new System.Windows.Forms.Button();
            this.textBox_Log = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_OpenGoogleSheetLink = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_SaveSheet = new System.Windows.Forms.ComboBox();
            this.button_OpenPath_SaveSheet = new System.Windows.Forms.Button();
            this.groupBox_2_1_TableSetting = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button_CommandLine = new System.Windows.Forms.Button();
            this.textBox_CommandLine = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button_Add_VirtualField = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButton_Enum = new System.Windows.Forms.RadioButton();
            this.radioButton_Global = new System.Windows.Forms.RadioButton();
            this.radioButton_Struct = new System.Windows.Forms.RadioButton();
            this.radioButton_Class = new System.Windows.Forms.RadioButton();
            this.button_Save_FileName = new System.Windows.Forms.Button();
            this.textBox_TableFileName = new System.Windows.Forms.TextBox();
            this.button_Check_TableSelected = new System.Windows.Forms.Button();
            this.groupBox_SelectedTable = new System.Windows.Forms.GroupBox();
            this.groupBox_2_2_SelectedField = new System.Windows.Forms.GroupBox();
            this.checkBox_FieldKey_IsOverlap = new System.Windows.Forms.CheckBox();
            this.checkBox_IsHeaderField = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.checkBox_ConvertStringToEnum = new System.Windows.Forms.CheckBox();
            this.textBox_EnumName = new System.Windows.Forms.TextBox();
            this.checkBox_DeleteField_OnCode = new System.Windows.Forms.CheckBox();
            this.button_Save_Field = new System.Windows.Forms.Button();
            this.groupBox_2_2_SelectedField_Virtual = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBox_DependencyField_Sub = new System.Windows.Forms.ComboBox();
            this.label_Type = new System.Windows.Forms.Label();
            this.textBox_Type = new System.Windows.Forms.TextBox();
            this.label_FieldName = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_FieldName = new System.Windows.Forms.TextBox();
            this.comboBox_DependencyField = new System.Windows.Forms.ComboBox();
            this.checkBox_Field_ThisIsKey = new System.Windows.Forms.CheckBox();
            this.button_Remove_VirtualField = new System.Windows.Forms.Button();
            this.listView_Field = new System.Windows.Forms.ListView();
            this.ColumnHeader_Name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader_Type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_IsVirtual = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button_Check_TableAll = new System.Windows.Forms.Button();
            this.groupBox3_WorkSetting = new System.Windows.Forms.GroupBox();
            this.groupBox_3_1_SelectedWork = new System.Windows.Forms.GroupBox();
            this.button_WorkOrderDown = new System.Windows.Forms.Button();
            this.button_WorkOrderUp = new System.Windows.Forms.Button();
            this.button_EditWork = new System.Windows.Forms.Button();
            this.button_RemoveWork = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox_WorkList = new System.Windows.Forms.ComboBox();
            this.button_AddWork = new System.Windows.Forms.Button();
            this.checkedListBox_WorkList = new System.Windows.Forms.CheckedListBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.checkBox_AutoConnect = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_LogClear = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.button_SelectExcelFile = new System.Windows.Forms.Button();
            this.button_OpenExcel = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.comboBox_ExcelPath_Saved = new System.Windows.Forms.ComboBox();
            this.textBox_ExcelPath_ForConnect = new System.Windows.Forms.TextBox();
            this.button_Connect_Excel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox_2_1_TableSetting.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox_SelectedTable.SuspendLayout();
            this.groupBox_2_2_SelectedField.SuspendLayout();
            this.groupBox_2_2_SelectedField_Virtual.SuspendLayout();
            this.groupBox3_WorkSetting.SuspendLayout();
            this.groupBox_3_1_SelectedWork.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_SheetID
            // 
            this.textBox_SheetID.Font = new System.Drawing.Font("굴림", 7F);
            this.textBox_SheetID.Location = new System.Drawing.Point(128, 15);
            this.textBox_SheetID.Name = "textBox_SheetID";
            this.textBox_SheetID.Size = new System.Drawing.Size(241, 18);
            this.textBox_SheetID.TabIndex = 0;
            // 
            // button_Connect_Sheet
            // 
            this.button_Connect_Sheet.Location = new System.Drawing.Point(375, 12);
            this.button_Connect_Sheet.Name = "button_Connect_Sheet";
            this.button_Connect_Sheet.Size = new System.Drawing.Size(75, 23);
            this.button_Connect_Sheet.TabIndex = 1;
            this.button_Connect_Sheet.Text = "연결";
            this.button_Connect_Sheet.UseVisualStyleBackColor = true;
            this.button_Connect_Sheet.Click += new System.EventHandler(this.button_Connect_Click);
            // 
            // checkedListBox_SheetList
            // 
            this.checkedListBox_SheetList.FormattingEnabled = true;
            this.checkedListBox_SheetList.HorizontalScrollbar = true;
            this.checkedListBox_SheetList.Location = new System.Drawing.Point(6, 15);
            this.checkedListBox_SheetList.Name = "checkedListBox_SheetList";
            this.checkedListBox_SheetList.Size = new System.Drawing.Size(297, 180);
            this.checkedListBox_SheetList.TabIndex = 2;
            this.checkedListBox_SheetList.SelectedIndexChanged += new System.EventHandler(this.checkedListBox_TableList_SelectedIndexChanged);
            // 
            // button_StartParsing
            // 
            this.button_StartParsing.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button_StartParsing.Location = new System.Drawing.Point(246, 252);
            this.button_StartParsing.Name = "button_StartParsing";
            this.button_StartParsing.Size = new System.Drawing.Size(106, 40);
            this.button_StartParsing.TabIndex = 3;
            this.button_StartParsing.Text = "작업 시작!!";
            this.button_StartParsing.UseVisualStyleBackColor = true;
            this.button_StartParsing.Click += new System.EventHandler(this.button_StartParsing_Click);
            // 
            // textBox_Log
            // 
            this.textBox_Log.Font = new System.Drawing.Font("굴림", 8F);
            this.textBox_Log.Location = new System.Drawing.Point(8, 20);
            this.textBox_Log.Multiline = true;
            this.textBox_Log.Name = "textBox_Log";
            this.textBox_Log.ReadOnly = true;
            this.textBox_Log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_Log.Size = new System.Drawing.Size(347, 273);
            this.textBox_Log.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.button_OpenGoogleSheetLink);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBox_SaveSheet);
            this.groupBox1.Controls.Add(this.textBox_SheetID);
            this.groupBox1.Controls.Add(this.button_Connect_Sheet);
            this.groupBox1.Location = new System.Drawing.Point(242, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(463, 69);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "1. Connect Google Sheet ";
            // 
            // button_OpenGoogleSheetLink
            // 
            this.button_OpenGoogleSheetLink.Location = new System.Drawing.Point(375, 37);
            this.button_OpenGoogleSheetLink.Name = "button_OpenGoogleSheetLink";
            this.button_OpenGoogleSheetLink.Size = new System.Drawing.Size(75, 23);
            this.button_OpenGoogleSheetLink.TabIndex = 6;
            this.button_OpenGoogleSheetLink.Text = "링크 열기";
            this.button_OpenGoogleSheetLink.UseVisualStyleBackColor = true;
            this.button_OpenGoogleSheetLink.Click += new System.EventHandler(this.button_OpenLink_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "연결할 Sheet ID";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "이전에 연결한 ID";
            // 
            // comboBox_SaveSheet
            // 
            this.comboBox_SaveSheet.Font = new System.Drawing.Font("굴림", 7F);
            this.comboBox_SaveSheet.FormattingEnabled = true;
            this.comboBox_SaveSheet.Location = new System.Drawing.Point(128, 39);
            this.comboBox_SaveSheet.Name = "comboBox_SaveSheet";
            this.comboBox_SaveSheet.Size = new System.Drawing.Size(241, 17);
            this.comboBox_SaveSheet.TabIndex = 2;
            this.comboBox_SaveSheet.SelectedIndexChanged += new System.EventHandler(this.comboBox_SaveSheet_SelectedIndexChanged);
            // 
            // button_OpenPath_SaveSheet
            // 
            this.button_OpenPath_SaveSheet.Location = new System.Drawing.Point(6, 39);
            this.button_OpenPath_SaveSheet.Name = "button_OpenPath_SaveSheet";
            this.button_OpenPath_SaveSheet.Size = new System.Drawing.Size(75, 23);
            this.button_OpenPath_SaveSheet.TabIndex = 5;
            this.button_OpenPath_SaveSheet.Text = "저장폴더";
            this.button_OpenPath_SaveSheet.UseVisualStyleBackColor = true;
            this.button_OpenPath_SaveSheet.Click += new System.EventHandler(this.Button_OpenPath_SaveSheet_Click);
            // 
            // groupBox_2_1_TableSetting
            // 
            this.groupBox_2_1_TableSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox_2_1_TableSetting.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox_2_1_TableSetting.Controls.Add(this.groupBox4);
            this.groupBox_2_1_TableSetting.Controls.Add(this.groupBox_SelectedTable);
            this.groupBox_2_1_TableSetting.Controls.Add(this.checkedListBox_SheetList);
            this.groupBox_2_1_TableSetting.Location = new System.Drawing.Point(12, 158);
            this.groupBox_2_1_TableSetting.Name = "groupBox_2_1_TableSetting";
            this.groupBox_2_1_TableSetting.Size = new System.Drawing.Size(693, 465);
            this.groupBox_2_1_TableSetting.TabIndex = 6;
            this.groupBox_2_1_TableSetting.TabStop = false;
            this.groupBox_2_1_TableSetting.Text = "2. Table Setting";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.button_CommandLine);
            this.groupBox4.Controls.Add(this.textBox_CommandLine);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.button_Add_VirtualField);
            this.groupBox4.Controls.Add(this.groupBox3);
            this.groupBox4.Controls.Add(this.button_Save_FileName);
            this.groupBox4.Controls.Add(this.textBox_TableFileName);
            this.groupBox4.Controls.Add(this.button_Check_TableSelected);
            this.groupBox4.Location = new System.Drawing.Point(6, 197);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(297, 256);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Table Setting";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 148);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 12);
            this.label3.TabIndex = 32;
            this.label3.Text = "Command Line";
            // 
            // button_CommandLine
            // 
            this.button_CommandLine.Location = new System.Drawing.Point(199, 161);
            this.button_CommandLine.Name = "button_CommandLine";
            this.button_CommandLine.Size = new System.Drawing.Size(82, 23);
            this.button_CommandLine.TabIndex = 29;
            this.button_CommandLine.Text = "Save";
            this.button_CommandLine.UseVisualStyleBackColor = true;
            this.button_CommandLine.Click += new System.EventHandler(this.buttonSave_CommandLine_Click);
            // 
            // textBox_CommandLine
            // 
            this.textBox_CommandLine.Location = new System.Drawing.Point(8, 163);
            this.textBox_CommandLine.Name = "textBox_CommandLine";
            this.textBox_CommandLine.Size = new System.Drawing.Size(178, 21);
            this.textBox_CommandLine.TabIndex = 30;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 104);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 28;
            this.label6.Text = "파일명";
            // 
            // button_Add_VirtualField
            // 
            this.button_Add_VirtualField.Location = new System.Drawing.Point(217, 13);
            this.button_Add_VirtualField.Name = "button_Add_VirtualField";
            this.button_Add_VirtualField.Size = new System.Drawing.Size(74, 54);
            this.button_Add_VirtualField.TabIndex = 15;
            this.button_Add_VirtualField.Text = "가상 필드 추가";
            this.button_Add_VirtualField.UseVisualStyleBackColor = true;
            this.button_Add_VirtualField.Click += new System.EventHandler(this.button_Add_VirtualField_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioButton_Enum);
            this.groupBox3.Controls.Add(this.radioButton_Global);
            this.groupBox3.Controls.Add(this.radioButton_Struct);
            this.groupBox3.Controls.Add(this.radioButton_Class);
            this.groupBox3.Location = new System.Drawing.Point(8, 20);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(129, 70);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Table Type";
            // 
            // radioButton_Enum
            // 
            this.radioButton_Enum.AutoSize = true;
            this.radioButton_Enum.Location = new System.Drawing.Point(6, 42);
            this.radioButton_Enum.Name = "radioButton_Enum";
            this.radioButton_Enum.Size = new System.Drawing.Size(56, 16);
            this.radioButton_Enum.TabIndex = 12;
            this.radioButton_Enum.Text = "Enum";
            this.radioButton_Enum.UseVisualStyleBackColor = true;
            this.radioButton_Enum.CheckedChanged += new System.EventHandler(this.OnChangeValue_TypeRadioButton);
            // 
            // radioButton_Global
            // 
            this.radioButton_Global.AutoSize = true;
            this.radioButton_Global.Location = new System.Drawing.Point(68, 42);
            this.radioButton_Global.Name = "radioButton_Global";
            this.radioButton_Global.Size = new System.Drawing.Size(59, 16);
            this.radioButton_Global.TabIndex = 12;
            this.radioButton_Global.Text = "Global";
            this.radioButton_Global.UseVisualStyleBackColor = true;
            this.radioButton_Global.CheckedChanged += new System.EventHandler(this.OnChangeValue_TypeRadioButton);
            // 
            // radioButton_Struct
            // 
            this.radioButton_Struct.AutoSize = true;
            this.radioButton_Struct.Location = new System.Drawing.Point(68, 20);
            this.radioButton_Struct.Name = "radioButton_Struct";
            this.radioButton_Struct.Size = new System.Drawing.Size(55, 16);
            this.radioButton_Struct.TabIndex = 11;
            this.radioButton_Struct.Text = "Struct";
            this.radioButton_Struct.UseVisualStyleBackColor = true;
            this.radioButton_Struct.CheckedChanged += new System.EventHandler(this.OnChangeValue_TypeRadioButton);
            // 
            // radioButton_Class
            // 
            this.radioButton_Class.AutoSize = true;
            this.radioButton_Class.Location = new System.Drawing.Point(6, 20);
            this.radioButton_Class.Name = "radioButton_Class";
            this.radioButton_Class.Size = new System.Drawing.Size(56, 16);
            this.radioButton_Class.TabIndex = 10;
            this.radioButton_Class.Text = "Class";
            this.radioButton_Class.UseVisualStyleBackColor = true;
            this.radioButton_Class.CheckedChanged += new System.EventHandler(this.OnChangeValue_TypeRadioButton);
            // 
            // button_Save_FileName
            // 
            this.button_Save_FileName.Location = new System.Drawing.Point(200, 117);
            this.button_Save_FileName.Name = "button_Save_FileName";
            this.button_Save_FileName.Size = new System.Drawing.Size(82, 23);
            this.button_Save_FileName.TabIndex = 25;
            this.button_Save_FileName.Text = "Save";
            this.button_Save_FileName.UseVisualStyleBackColor = true;
            this.button_Save_FileName.Click += new System.EventHandler(this.button_Save_FileName_Click);
            // 
            // textBox_TableFileName
            // 
            this.textBox_TableFileName.Location = new System.Drawing.Point(8, 119);
            this.textBox_TableFileName.Name = "textBox_TableFileName";
            this.textBox_TableFileName.Size = new System.Drawing.Size(177, 21);
            this.textBox_TableFileName.TabIndex = 26;
            // 
            // button_Check_TableSelected
            // 
            this.button_Check_TableSelected.Location = new System.Drawing.Point(114, 227);
            this.button_Check_TableSelected.Name = "button_Check_TableSelected";
            this.button_Check_TableSelected.Size = new System.Drawing.Size(177, 23);
            this.button_Check_TableSelected.TabIndex = 9;
            this.button_Check_TableSelected.Text = "Check Selected Table";
            this.button_Check_TableSelected.UseVisualStyleBackColor = true;
            this.button_Check_TableSelected.Click += new System.EventHandler(this.button_CheckTable_Click);
            // 
            // groupBox_SelectedTable
            // 
            this.groupBox_SelectedTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox_SelectedTable.Controls.Add(this.groupBox_2_2_SelectedField);
            this.groupBox_SelectedTable.Controls.Add(this.listView_Field);
            this.groupBox_SelectedTable.Location = new System.Drawing.Point(309, 12);
            this.groupBox_SelectedTable.Name = "groupBox_SelectedTable";
            this.groupBox_SelectedTable.Size = new System.Drawing.Size(378, 441);
            this.groupBox_SelectedTable.TabIndex = 9;
            this.groupBox_SelectedTable.TabStop = false;
            this.groupBox_SelectedTable.Text = "Selected Table Setting";
            // 
            // groupBox_2_2_SelectedField
            // 
            this.groupBox_2_2_SelectedField.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox_2_2_SelectedField.Controls.Add(this.checkBox_FieldKey_IsOverlap);
            this.groupBox_2_2_SelectedField.Controls.Add(this.checkBox_IsHeaderField);
            this.groupBox_2_2_SelectedField.Controls.Add(this.label7);
            this.groupBox_2_2_SelectedField.Controls.Add(this.checkBox_ConvertStringToEnum);
            this.groupBox_2_2_SelectedField.Controls.Add(this.textBox_EnumName);
            this.groupBox_2_2_SelectedField.Controls.Add(this.checkBox_DeleteField_OnCode);
            this.groupBox_2_2_SelectedField.Controls.Add(this.button_Save_Field);
            this.groupBox_2_2_SelectedField.Controls.Add(this.groupBox_2_2_SelectedField_Virtual);
            this.groupBox_2_2_SelectedField.Controls.Add(this.checkBox_Field_ThisIsKey);
            this.groupBox_2_2_SelectedField.Controls.Add(this.button_Remove_VirtualField);
            this.groupBox_2_2_SelectedField.Location = new System.Drawing.Point(6, 189);
            this.groupBox_2_2_SelectedField.Name = "groupBox_2_2_SelectedField";
            this.groupBox_2_2_SelectedField.Size = new System.Drawing.Size(366, 246);
            this.groupBox_2_2_SelectedField.TabIndex = 23;
            this.groupBox_2_2_SelectedField.TabStop = false;
            this.groupBox_2_2_SelectedField.Text = "2-2. Selected Field";
            // 
            // checkBox_FieldKey_IsOverlap
            // 
            this.checkBox_FieldKey_IsOverlap.AutoSize = true;
            this.checkBox_FieldKey_IsOverlap.Location = new System.Drawing.Point(6, 196);
            this.checkBox_FieldKey_IsOverlap.Name = "checkBox_FieldKey_IsOverlap";
            this.checkBox_FieldKey_IsOverlap.Size = new System.Drawing.Size(132, 16);
            this.checkBox_FieldKey_IsOverlap.TabIndex = 26;
            this.checkBox_FieldKey_IsOverlap.Text = "키가 중복될 수 있음";
            this.checkBox_FieldKey_IsOverlap.UseVisualStyleBackColor = true;
            this.checkBox_FieldKey_IsOverlap.CheckedChanged += new System.EventHandler(this.checkBox_FieldKey_IsOverlap_CheckedChanged);
            // 
            // checkBox_IsHeaderField
            // 
            this.checkBox_IsHeaderField.AutoSize = true;
            this.checkBox_IsHeaderField.Location = new System.Drawing.Point(6, 153);
            this.checkBox_IsHeaderField.Name = "checkBox_IsHeaderField";
            this.checkBox_IsHeaderField.Size = new System.Drawing.Size(128, 16);
            this.checkBox_IsHeaderField.TabIndex = 25;
            this.checkBox_IsHeaderField.Text = "이 필드값을 헤더로";
            this.checkBox_IsHeaderField.UseVisualStyleBackColor = true;
            this.checkBox_IsHeaderField.CheckedChanged += new System.EventHandler(this.checkBox_IsHeaderField_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(207, 157);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 12);
            this.label7.TabIndex = 23;
            this.label7.Text = "Enum Name";
            // 
            // checkBox_ConvertStringToEnum
            // 
            this.checkBox_ConvertStringToEnum.AutoSize = true;
            this.checkBox_ConvertStringToEnum.Location = new System.Drawing.Point(217, 132);
            this.checkBox_ConvertStringToEnum.Name = "checkBox_ConvertStringToEnum";
            this.checkBox_ConvertStringToEnum.Size = new System.Drawing.Size(104, 16);
            this.checkBox_ConvertStringToEnum.TabIndex = 24;
            this.checkBox_ConvertStringToEnum.Text = "Convert Enum";
            this.checkBox_ConvertStringToEnum.UseVisualStyleBackColor = true;
            this.checkBox_ConvertStringToEnum.CheckedChanged += new System.EventHandler(this.checkBox_ConvertStringToEnum_CheckedChanged);
            // 
            // textBox_EnumName
            // 
            this.textBox_EnumName.Location = new System.Drawing.Point(208, 172);
            this.textBox_EnumName.Name = "textBox_EnumName";
            this.textBox_EnumName.Size = new System.Drawing.Size(145, 21);
            this.textBox_EnumName.TabIndex = 22;
            // 
            // checkBox_DeleteField_OnCode
            // 
            this.checkBox_DeleteField_OnCode.AutoSize = true;
            this.checkBox_DeleteField_OnCode.Location = new System.Drawing.Point(6, 132);
            this.checkBox_DeleteField_OnCode.Name = "checkBox_DeleteField_OnCode";
            this.checkBox_DeleteField_OnCode.Size = new System.Drawing.Size(112, 16);
            this.checkBox_DeleteField_OnCode.TabIndex = 23;
            this.checkBox_DeleteField_OnCode.Text = "코드에서는 안씀";
            this.checkBox_DeleteField_OnCode.UseVisualStyleBackColor = true;
            this.checkBox_DeleteField_OnCode.CheckedChanged += new System.EventHandler(this.checkBox_DeleteField_OnAfterBuild_CheckedChanged);
            // 
            // button_Save_Field
            // 
            this.button_Save_Field.Location = new System.Drawing.Point(217, 215);
            this.button_Save_Field.Name = "button_Save_Field";
            this.button_Save_Field.Size = new System.Drawing.Size(42, 23);
            this.button_Save_Field.TabIndex = 22;
            this.button_Save_Field.Text = "저장";
            this.button_Save_Field.UseVisualStyleBackColor = true;
            this.button_Save_Field.Click += new System.EventHandler(this.button_Save_Field_Click);
            // 
            // groupBox_2_2_SelectedField_Virtual
            // 
            this.groupBox_2_2_SelectedField_Virtual.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox_2_2_SelectedField_Virtual.Controls.Add(this.label8);
            this.groupBox_2_2_SelectedField_Virtual.Controls.Add(this.comboBox_DependencyField_Sub);
            this.groupBox_2_2_SelectedField_Virtual.Controls.Add(this.label_Type);
            this.groupBox_2_2_SelectedField_Virtual.Controls.Add(this.textBox_Type);
            this.groupBox_2_2_SelectedField_Virtual.Controls.Add(this.label_FieldName);
            this.groupBox_2_2_SelectedField_Virtual.Controls.Add(this.label5);
            this.groupBox_2_2_SelectedField_Virtual.Controls.Add(this.textBox_FieldName);
            this.groupBox_2_2_SelectedField_Virtual.Controls.Add(this.comboBox_DependencyField);
            this.groupBox_2_2_SelectedField_Virtual.Location = new System.Drawing.Point(6, 20);
            this.groupBox_2_2_SelectedField_Virtual.Name = "groupBox_2_2_SelectedField_Virtual";
            this.groupBox_2_2_SelectedField_Virtual.Size = new System.Drawing.Size(354, 106);
            this.groupBox_2_2_SelectedField_Virtual.TabIndex = 17;
            this.groupBox_2_2_SelectedField_Virtual.TabStop = false;
            this.groupBox_2_2_SelectedField_Virtual.Text = "2-2. Virtual Field Option";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(195, 63);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(117, 12);
            this.label8.TabIndex = 23;
            this.label8.Text = "Dependency Field 2";
            // 
            // comboBox_DependencyField_Sub
            // 
            this.comboBox_DependencyField_Sub.FormattingEnabled = true;
            this.comboBox_DependencyField_Sub.Location = new System.Drawing.Point(195, 79);
            this.comboBox_DependencyField_Sub.Name = "comboBox_DependencyField_Sub";
            this.comboBox_DependencyField_Sub.Size = new System.Drawing.Size(152, 20);
            this.comboBox_DependencyField_Sub.TabIndex = 22;
            // 
            // label_Type
            // 
            this.label_Type.AutoSize = true;
            this.label_Type.Location = new System.Drawing.Point(199, 17);
            this.label_Type.Name = "label_Type";
            this.label_Type.Size = new System.Drawing.Size(34, 12);
            this.label_Type.TabIndex = 21;
            this.label_Type.Text = "Type";
            // 
            // textBox_Type
            // 
            this.textBox_Type.Location = new System.Drawing.Point(195, 32);
            this.textBox_Type.Name = "textBox_Type";
            this.textBox_Type.Size = new System.Drawing.Size(152, 21);
            this.textBox_Type.TabIndex = 20;
            // 
            // label_FieldName
            // 
            this.label_FieldName.AutoSize = true;
            this.label_FieldName.Location = new System.Drawing.Point(6, 17);
            this.label_FieldName.Name = "label_FieldName";
            this.label_FieldName.Size = new System.Drawing.Size(70, 12);
            this.label_FieldName.TabIndex = 19;
            this.label_FieldName.Text = "Field Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 12);
            this.label5.TabIndex = 18;
            this.label5.Text = "Dependency Field 1";
            // 
            // textBox_FieldName
            // 
            this.textBox_FieldName.Location = new System.Drawing.Point(8, 32);
            this.textBox_FieldName.Name = "textBox_FieldName";
            this.textBox_FieldName.Size = new System.Drawing.Size(152, 21);
            this.textBox_FieldName.TabIndex = 18;
            // 
            // comboBox_DependencyField
            // 
            this.comboBox_DependencyField.FormattingEnabled = true;
            this.comboBox_DependencyField.Location = new System.Drawing.Point(8, 79);
            this.comboBox_DependencyField.Name = "comboBox_DependencyField";
            this.comboBox_DependencyField.Size = new System.Drawing.Size(152, 20);
            this.comboBox_DependencyField.TabIndex = 7;
            // 
            // checkBox_Field_ThisIsKey
            // 
            this.checkBox_Field_ThisIsKey.AutoSize = true;
            this.checkBox_Field_ThisIsKey.Location = new System.Drawing.Point(6, 174);
            this.checkBox_Field_ThisIsKey.Name = "checkBox_Field_ThisIsKey";
            this.checkBox_Field_ThisIsKey.Size = new System.Drawing.Size(132, 16);
            this.checkBox_Field_ThisIsKey.TabIndex = 19;
            this.checkBox_Field_ThisIsKey.Text = "이 필드를 키로 캐싱";
            this.checkBox_Field_ThisIsKey.UseVisualStyleBackColor = true;
            this.checkBox_Field_ThisIsKey.CheckedChanged += new System.EventHandler(this.checkBox_Field_NullOrEmtpy_IsError_CheckedChanged);
            // 
            // button_Remove_VirtualField
            // 
            this.button_Remove_VirtualField.Location = new System.Drawing.Point(313, 215);
            this.button_Remove_VirtualField.Name = "button_Remove_VirtualField";
            this.button_Remove_VirtualField.Size = new System.Drawing.Size(40, 23);
            this.button_Remove_VirtualField.TabIndex = 14;
            this.button_Remove_VirtualField.Text = "삭제";
            this.button_Remove_VirtualField.UseVisualStyleBackColor = true;
            this.button_Remove_VirtualField.Click += new System.EventHandler(this.button_Remove_VirtualField_Click);
            // 
            // listView_Field
            // 
            this.listView_Field.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader_Name,
            this.ColumnHeader_Type,
            this.columnHeader_IsVirtual});
            this.listView_Field.HideSelection = false;
            this.listView_Field.Location = new System.Drawing.Point(6, 14);
            this.listView_Field.MultiSelect = false;
            this.listView_Field.Name = "listView_Field";
            this.listView_Field.Size = new System.Drawing.Size(366, 169);
            this.listView_Field.TabIndex = 12;
            this.listView_Field.UseCompatibleStateImageBehavior = false;
            this.listView_Field.View = System.Windows.Forms.View.Details;
            // 
            // ColumnHeader_Name
            // 
            this.ColumnHeader_Name.Text = "Name";
            this.ColumnHeader_Name.Width = 158;
            // 
            // ColumnHeader_Type
            // 
            this.ColumnHeader_Type.Text = "Type";
            this.ColumnHeader_Type.Width = 109;
            // 
            // columnHeader_IsVirtual
            // 
            this.columnHeader_IsVirtual.Text = "Virtual";
            this.columnHeader_IsVirtual.Width = 88;
            // 
            // button_Check_TableAll
            // 
            this.button_Check_TableAll.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button_Check_TableAll.Location = new System.Drawing.Point(246, 192);
            this.button_Check_TableAll.Name = "button_Check_TableAll";
            this.button_Check_TableAll.Size = new System.Drawing.Size(106, 23);
            this.button_Check_TableAll.TabIndex = 24;
            this.button_Check_TableAll.Text = "Check All Table";
            this.button_Check_TableAll.UseVisualStyleBackColor = true;
            this.button_Check_TableAll.Click += new System.EventHandler(this.button_Check_TableAll_Click);
            // 
            // groupBox3_WorkSetting
            // 
            this.groupBox3_WorkSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3_WorkSetting.AutoSize = true;
            this.groupBox3_WorkSetting.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox3_WorkSetting.Controls.Add(this.groupBox_3_1_SelectedWork);
            this.groupBox3_WorkSetting.Controls.Add(this.label4);
            this.groupBox3_WorkSetting.Controls.Add(this.comboBox_WorkList);
            this.groupBox3_WorkSetting.Controls.Add(this.button_AddWork);
            this.groupBox3_WorkSetting.Controls.Add(this.button_Check_TableAll);
            this.groupBox3_WorkSetting.Controls.Add(this.checkedListBox_WorkList);
            this.groupBox3_WorkSetting.Controls.Add(this.button_StartParsing);
            this.groupBox3_WorkSetting.Location = new System.Drawing.Point(711, 12);
            this.groupBox3_WorkSetting.Name = "groupBox3_WorkSetting";
            this.groupBox3_WorkSetting.Size = new System.Drawing.Size(361, 312);
            this.groupBox3_WorkSetting.TabIndex = 7;
            this.groupBox3_WorkSetting.TabStop = false;
            this.groupBox3_WorkSetting.Text = "3. Work Setting";
            // 
            // groupBox_3_1_SelectedWork
            // 
            this.groupBox_3_1_SelectedWork.AutoSize = true;
            this.groupBox_3_1_SelectedWork.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox_3_1_SelectedWork.Controls.Add(this.button_WorkOrderDown);
            this.groupBox_3_1_SelectedWork.Controls.Add(this.button_WorkOrderUp);
            this.groupBox_3_1_SelectedWork.Controls.Add(this.button_EditWork);
            this.groupBox_3_1_SelectedWork.Controls.Add(this.button_RemoveWork);
            this.groupBox_3_1_SelectedWork.Location = new System.Drawing.Point(8, 192);
            this.groupBox_3_1_SelectedWork.Name = "groupBox_3_1_SelectedWork";
            this.groupBox_3_1_SelectedWork.Size = new System.Drawing.Size(232, 100);
            this.groupBox_3_1_SelectedWork.TabIndex = 16;
            this.groupBox_3_1_SelectedWork.TabStop = false;
            this.groupBox_3_1_SelectedWork.Text = "3-1. Selected Work";
            // 
            // button_WorkOrderDown
            // 
            this.button_WorkOrderDown.Location = new System.Drawing.Point(6, 57);
            this.button_WorkOrderDown.Name = "button_WorkOrderDown";
            this.button_WorkOrderDown.Size = new System.Drawing.Size(113, 23);
            this.button_WorkOrderDown.TabIndex = 16;
            this.button_WorkOrderDown.Text = "작업 순위 내리기";
            this.button_WorkOrderDown.UseVisualStyleBackColor = true;
            this.button_WorkOrderDown.Click += new System.EventHandler(this.button_WorkOrderDown_Click);
            // 
            // button_WorkOrderUp
            // 
            this.button_WorkOrderUp.Location = new System.Drawing.Point(6, 20);
            this.button_WorkOrderUp.Name = "button_WorkOrderUp";
            this.button_WorkOrderUp.Size = new System.Drawing.Size(113, 23);
            this.button_WorkOrderUp.TabIndex = 15;
            this.button_WorkOrderUp.Text = "작업 순위 올리기";
            this.button_WorkOrderUp.UseVisualStyleBackColor = true;
            this.button_WorkOrderUp.Click += new System.EventHandler(this.button_WorkOrderUp_Click);
            // 
            // button_EditWork
            // 
            this.button_EditWork.Location = new System.Drawing.Point(144, 20);
            this.button_EditWork.Name = "button_EditWork";
            this.button_EditWork.Size = new System.Drawing.Size(82, 23);
            this.button_EditWork.TabIndex = 14;
            this.button_EditWork.Text = "작업 편집";
            this.button_EditWork.UseVisualStyleBackColor = true;
            this.button_EditWork.Click += new System.EventHandler(this.button_EditWork_Click);
            // 
            // button_RemoveWork
            // 
            this.button_RemoveWork.Location = new System.Drawing.Point(144, 55);
            this.button_RemoveWork.Name = "button_RemoveWork";
            this.button_RemoveWork.Size = new System.Drawing.Size(82, 23);
            this.button_RemoveWork.TabIndex = 15;
            this.button_RemoveWork.Text = "작업 삭제";
            this.button_RemoveWork.UseVisualStyleBackColor = true;
            this.button_RemoveWork.Click += new System.EventHandler(this.button_RemoveWork_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(249, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "추가할 작업이름";
            // 
            // comboBox_WorkList
            // 
            this.comboBox_WorkList.FormattingEnabled = true;
            this.comboBox_WorkList.Location = new System.Drawing.Point(249, 34);
            this.comboBox_WorkList.Name = "comboBox_WorkList";
            this.comboBox_WorkList.Size = new System.Drawing.Size(106, 20);
            this.comboBox_WorkList.TabIndex = 7;
            // 
            // button_AddWork
            // 
            this.button_AddWork.Location = new System.Drawing.Point(249, 60);
            this.button_AddWork.Name = "button_AddWork";
            this.button_AddWork.Size = new System.Drawing.Size(106, 40);
            this.button_AddWork.TabIndex = 13;
            this.button_AddWork.Text = "작업 추가";
            this.button_AddWork.UseVisualStyleBackColor = true;
            this.button_AddWork.Click += new System.EventHandler(this.button_AddWork_Click);
            // 
            // checkedListBox_WorkList
            // 
            this.checkedListBox_WorkList.FormattingEnabled = true;
            this.checkedListBox_WorkList.HorizontalScrollbar = true;
            this.checkedListBox_WorkList.Location = new System.Drawing.Point(8, 15);
            this.checkedListBox_WorkList.Name = "checkedListBox_WorkList";
            this.checkedListBox_WorkList.Size = new System.Drawing.Size(232, 164);
            this.checkedListBox_WorkList.TabIndex = 12;
            // 
            // groupBox5
            // 
            this.groupBox5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox5.Controls.Add(this.checkBox_AutoConnect);
            this.groupBox5.Controls.Add(this.button_OpenPath_SaveSheet);
            this.groupBox5.Location = new System.Drawing.Point(12, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(224, 140);
            this.groupBox5.TabIndex = 13;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "0. Config";
            // 
            // checkBox_AutoConnect
            // 
            this.checkBox_AutoConnect.AutoSize = true;
            this.checkBox_AutoConnect.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_AutoConnect.Location = new System.Drawing.Point(2, 17);
            this.checkBox_AutoConnect.Name = "checkBox_AutoConnect";
            this.checkBox_AutoConnect.Size = new System.Drawing.Size(216, 16);
            this.checkBox_AutoConnect.TabIndex = 7;
            this.checkBox_AutoConnect.Text = "프로그램 실행시 직전파일 자동연결";
            this.checkBox_AutoConnect.UseVisualStyleBackColor = true;
            this.checkBox_AutoConnect.CheckedChanged += new System.EventHandler(this.checkBox_AutoConnect_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox2.Controls.Add(this.button_LogClear);
            this.groupBox2.Controls.Add(this.textBox_Log);
            this.groupBox2.Location = new System.Drawing.Point(711, 330);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(361, 323);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Log";
            // 
            // button_LogClear
            // 
            this.button_LogClear.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button_LogClear.Location = new System.Drawing.Point(8, 296);
            this.button_LogClear.Name = "button_LogClear";
            this.button_LogClear.Size = new System.Drawing.Size(80, 21);
            this.button_LogClear.TabIndex = 25;
            this.button_LogClear.Text = "Clear Log";
            this.button_LogClear.UseVisualStyleBackColor = true;
            this.button_LogClear.Click += new System.EventHandler(this.button_LogClear_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.groupBox6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox6.Controls.Add(this.button_SelectExcelFile);
            this.groupBox6.Controls.Add(this.button_OpenExcel);
            this.groupBox6.Controls.Add(this.label9);
            this.groupBox6.Controls.Add(this.label10);
            this.groupBox6.Controls.Add(this.comboBox_ExcelPath_Saved);
            this.groupBox6.Controls.Add(this.textBox_ExcelPath_ForConnect);
            this.groupBox6.Controls.Add(this.button_Connect_Excel);
            this.groupBox6.Location = new System.Drawing.Point(242, 83);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(463, 69);
            this.groupBox6.TabIndex = 7;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "1. Open MS Excel";
            // 
            // button_SelectExcelFile
            // 
            this.button_SelectExcelFile.Location = new System.Drawing.Point(294, 12);
            this.button_SelectExcelFile.Name = "button_SelectExcelFile";
            this.button_SelectExcelFile.Size = new System.Drawing.Size(75, 23);
            this.button_SelectExcelFile.TabIndex = 7;
            this.button_SelectExcelFile.Text = "파일 선택";
            this.button_SelectExcelFile.UseVisualStyleBackColor = true;
            this.button_SelectExcelFile.Click += new System.EventHandler(this.button_SelectExcelFile_Click);
            // 
            // button_OpenExcel
            // 
            this.button_OpenExcel.Location = new System.Drawing.Point(375, 37);
            this.button_OpenExcel.Name = "button_OpenExcel";
            this.button_OpenExcel.Size = new System.Drawing.Size(75, 23);
            this.button_OpenExcel.TabIndex = 6;
            this.button_OpenExcel.Text = "파일 열기";
            this.button_OpenExcel.UseVisualStyleBackColor = true;
            this.button_OpenExcel.Click += new System.EventHandler(this.button_OpenExcel_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(18, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(105, 12);
            this.label9.TabIndex = 4;
            this.label9.Text = "연결할 Excel 경로";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(18, 40);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(109, 12);
            this.label10.TabIndex = 3;
            this.label10.Text = "이전에 연결한 경로";
            // 
            // comboBox_ExcelPath_Saved
            // 
            this.comboBox_ExcelPath_Saved.Font = new System.Drawing.Font("굴림", 7F);
            this.comboBox_ExcelPath_Saved.FormattingEnabled = true;
            this.comboBox_ExcelPath_Saved.Location = new System.Drawing.Point(128, 39);
            this.comboBox_ExcelPath_Saved.Name = "comboBox_ExcelPath_Saved";
            this.comboBox_ExcelPath_Saved.Size = new System.Drawing.Size(241, 17);
            this.comboBox_ExcelPath_Saved.TabIndex = 2;
            // 
            // textBox_ExcelPath_ForConnect
            // 
            this.textBox_ExcelPath_ForConnect.Font = new System.Drawing.Font("굴림", 7F);
            this.textBox_ExcelPath_ForConnect.Location = new System.Drawing.Point(128, 15);
            this.textBox_ExcelPath_ForConnect.Name = "textBox_ExcelPath_ForConnect";
            this.textBox_ExcelPath_ForConnect.Size = new System.Drawing.Size(160, 18);
            this.textBox_ExcelPath_ForConnect.TabIndex = 0;
            // 
            // button_Connect_Excel
            // 
            this.button_Connect_Excel.Location = new System.Drawing.Point(375, 12);
            this.button_Connect_Excel.Name = "button_Connect_Excel";
            this.button_Connect_Excel.Size = new System.Drawing.Size(75, 23);
            this.button_Connect_Excel.TabIndex = 1;
            this.button_Connect_Excel.Text = "열기";
            this.button_Connect_Excel.UseVisualStyleBackColor = true;
            this.button_Connect_Excel.Click += new System.EventHandler(this.button_Connect_Excel_Click);
            // 
            // SpreadSheetParser_MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 661);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox_2_1_TableSetting);
            this.Controls.Add(this.groupBox3_WorkSetting);
            this.Controls.Add(this.groupBox1);
            this.Name = "SpreadSheetParser_MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox_2_1_TableSetting.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox_SelectedTable.ResumeLayout(false);
            this.groupBox_2_2_SelectedField.ResumeLayout(false);
            this.groupBox_2_2_SelectedField.PerformLayout();
            this.groupBox_2_2_SelectedField_Virtual.ResumeLayout(false);
            this.groupBox_2_2_SelectedField_Virtual.PerformLayout();
            this.groupBox3_WorkSetting.ResumeLayout(false);
            this.groupBox3_WorkSetting.PerformLayout();
            this.groupBox_3_1_SelectedWork.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_SheetID;
        private System.Windows.Forms.Button button_Connect_Sheet;
        private System.Windows.Forms.CheckedListBox checkedListBox_SheetList;
        private System.Windows.Forms.Button button_StartParsing;
        private System.Windows.Forms.TextBox textBox_Log;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox_2_1_TableSetting;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3_WorkSetting;
        private System.Windows.Forms.GroupBox groupBox_SelectedTable;
        private System.Windows.Forms.Button button_OpenPath_SaveSheet;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox checkBox_AutoConnect;
        private System.Windows.Forms.Button button_Check_TableSelected;
        private System.Windows.Forms.Button button_OpenGoogleSheetLink;
        private System.Windows.Forms.RadioButton radioButton_Class;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButton_Enum;
        private System.Windows.Forms.RadioButton radioButton_Struct;
        private System.Windows.Forms.CheckedListBox checkedListBox_WorkList;
        private System.Windows.Forms.Button button_AddWork;
        private System.Windows.Forms.Button button_EditWork;
        private System.Windows.Forms.Button button_RemoveWork;
        private System.Windows.Forms.ComboBox comboBox_WorkList;
        private System.Windows.Forms.ComboBox comboBox_SaveSheet;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox_3_1_SelectedWork;
        private System.Windows.Forms.Button button_WorkOrderUp;
        private System.Windows.Forms.Button button_WorkOrderDown;
        private System.Windows.Forms.ListView listView_Field;
        private System.Windows.Forms.ColumnHeader ColumnHeader_Name;
        private System.Windows.Forms.ColumnHeader ColumnHeader_Type;
        private System.Windows.Forms.ColumnHeader columnHeader_IsVirtual;
        private System.Windows.Forms.GroupBox groupBox_2_2_SelectedField_Virtual;
        private System.Windows.Forms.Button button_Remove_VirtualField;
        private System.Windows.Forms.ComboBox comboBox_DependencyField;
        private System.Windows.Forms.Button button_Add_VirtualField;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label_FieldName;
        private System.Windows.Forms.TextBox textBox_FieldName;
        private System.Windows.Forms.Label label_Type;
        private System.Windows.Forms.TextBox textBox_Type;
        private System.Windows.Forms.Button button_Save_Field;
        private System.Windows.Forms.CheckBox checkBox_Field_ThisIsKey;
        private System.Windows.Forms.GroupBox groupBox_2_2_SelectedField;
        private System.Windows.Forms.Button button_Check_TableAll;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox_DeleteField_OnCode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button_Save_FileName;
        private System.Windows.Forms.TextBox textBox_TableFileName;
        private System.Windows.Forms.CheckBox checkBox_ConvertStringToEnum;
        private System.Windows.Forms.TextBox textBox_EnumName;
        private System.Windows.Forms.CheckBox checkBox_IsHeaderField;
        private System.Windows.Forms.RadioButton radioButton_Global;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox checkBox_FieldKey_IsOverlap;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_CommandLine;
        private System.Windows.Forms.TextBox textBox_CommandLine;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBox_DependencyField_Sub;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button button_OpenExcel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox comboBox_ExcelPath_Saved;
        private System.Windows.Forms.TextBox textBox_ExcelPath_ForConnect;
        private System.Windows.Forms.Button button_Connect_Excel;
        private System.Windows.Forms.Button button_SelectExcelFile;
        private System.Windows.Forms.Button button_LogClear;
    }
}

