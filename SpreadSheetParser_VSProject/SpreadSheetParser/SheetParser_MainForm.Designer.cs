namespace SpreadSheetParser
{
    partial class SheetParser_MainForm
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
            this.components = new System.ComponentModel.Container();
            this.button_StartBuild = new System.Windows.Forms.Button();
            this.textBox_Log = new System.Windows.Forms.TextBox();
            this.groupBox_2_1_TableSetting = new System.Windows.Forms.GroupBox();
            this.listView_Sheet = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_IsEnable = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            this.groupBox3_BuildSetting = new System.Windows.Forms.GroupBox();
            this.button_StartBuild_Selected = new System.Windows.Forms.Button();
            this.groupBox_3_1_SelectedBuild = new System.Windows.Forms.GroupBox();
            this.button_WorkOrderDown = new System.Windows.Forms.Button();
            this.button_BuildOrderUp = new System.Windows.Forms.Button();
            this.button_EditWork = new System.Windows.Forms.Button();
            this.button_RemoveWork = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox_BuildList = new System.Windows.Forms.ComboBox();
            this.button_AddWork = new System.Windows.Forms.Button();
            this.checkedListBox_BuildList = new System.Windows.Forms.CheckedListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_LogClear = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.button_OpenSheetSource = new System.Windows.Forms.Button();
            this.button_AddSheetSource = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.radioButton_SheetSource_MSExcel = new System.Windows.Forms.RadioButton();
            this.radioButton_SheetSource_GoogleSheet = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_AddSheetSourceName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.listView_SheetSourceConnector = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem_File_New = new System.Windows.Forms.MenuItem();
            this.menuItem_File_Open = new System.Windows.Forms.MenuItem();
            this.menuItem_File_Save = new System.Windows.Forms.MenuItem();
            this.menuItem_File_SaveAs = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem_Tools_OpenSaveDataFolder = new System.Windows.Forms.MenuItem();
            this.groupBox_2_1_TableSetting.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox_SelectedTable.SuspendLayout();
            this.groupBox_2_2_SelectedField.SuspendLayout();
            this.groupBox_2_2_SelectedField_Virtual.SuspendLayout();
            this.groupBox3_BuildSetting.SuspendLayout();
            this.groupBox_3_1_SelectedBuild.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_StartBuild
            // 
            this.button_StartBuild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_StartBuild.Location = new System.Drawing.Point(400, 126);
            this.button_StartBuild.Name = "button_StartBuild";
            this.button_StartBuild.Size = new System.Drawing.Size(74, 40);
            this.button_StartBuild.TabIndex = 3;
            this.button_StartBuild.Text = "빌드 시작!!";
            this.button_StartBuild.UseVisualStyleBackColor = true;
            this.button_StartBuild.Click += new System.EventHandler(this.button_StartParsing_Click);
            // 
            // textBox_Log
            // 
            this.textBox_Log.Font = new System.Drawing.Font("굴림", 8F);
            this.textBox_Log.Location = new System.Drawing.Point(8, 19);
            this.textBox_Log.Multiline = true;
            this.textBox_Log.Name = "textBox_Log";
            this.textBox_Log.ReadOnly = true;
            this.textBox_Log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_Log.Size = new System.Drawing.Size(327, 212);
            this.textBox_Log.TabIndex = 4;
            // 
            // groupBox_2_1_TableSetting
            // 
            this.groupBox_2_1_TableSetting.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.groupBox_2_1_TableSetting.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox_2_1_TableSetting.Controls.Add(this.listView_Sheet);
            this.groupBox_2_1_TableSetting.Controls.Add(this.groupBox4);
            this.groupBox_2_1_TableSetting.Controls.Add(this.groupBox_SelectedTable);
            this.groupBox_2_1_TableSetting.Location = new System.Drawing.Point(362, 1);
            this.groupBox_2_1_TableSetting.Name = "groupBox_2_1_TableSetting";
            this.groupBox_2_1_TableSetting.Size = new System.Drawing.Size(676, 467);
            this.groupBox_2_1_TableSetting.TabIndex = 6;
            this.groupBox_2_1_TableSetting.TabStop = false;
            this.groupBox_2_1_TableSetting.Text = "2. Table Setting";
            // 
            // listView_Sheet
            // 
            this.listView_Sheet.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader_IsEnable,
            this.columnHeader2});
            this.listView_Sheet.HideSelection = false;
            this.listView_Sheet.Location = new System.Drawing.Point(7, 20);
            this.listView_Sheet.MultiSelect = false;
            this.listView_Sheet.Name = "listView_Sheet";
            this.listView_Sheet.Size = new System.Drawing.Size(279, 196);
            this.listView_Sheet.TabIndex = 24;
            this.listView_Sheet.UseCompatibleStateImageBehavior = false;
            this.listView_Sheet.View = System.Windows.Forms.View.Details;
            this.listView_Sheet.SelectedIndexChanged += new System.EventHandler(this.listView_Sheet_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.DisplayIndex = 2;
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 217;
            // 
            // columnHeader_IsEnable
            // 
            this.columnHeader_IsEnable.DisplayIndex = 0;
            this.columnHeader_IsEnable.Text = "Enable";
            this.columnHeader_IsEnable.Width = 52;
            // 
            // columnHeader2
            // 
            this.columnHeader2.DisplayIndex = 1;
            this.columnHeader2.Text = "Type";
            this.columnHeader2.Width = 68;
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
            this.groupBox4.Location = new System.Drawing.Point(7, 222);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(279, 239);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "2-1. Table Setting";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 12);
            this.label3.TabIndex = 32;
            this.label3.Text = "Command Line";
            // 
            // button_CommandLine
            // 
            this.button_CommandLine.Location = new System.Drawing.Point(190, 144);
            this.button_CommandLine.Name = "button_CommandLine";
            this.button_CommandLine.Size = new System.Drawing.Size(82, 23);
            this.button_CommandLine.TabIndex = 29;
            this.button_CommandLine.Text = "Save";
            this.button_CommandLine.UseVisualStyleBackColor = true;
            this.button_CommandLine.Click += new System.EventHandler(this.buttonSave_CommandLine_Click);
            // 
            // textBox_CommandLine
            // 
            this.textBox_CommandLine.Location = new System.Drawing.Point(7, 145);
            this.textBox_CommandLine.Name = "textBox_CommandLine";
            this.textBox_CommandLine.Size = new System.Drawing.Size(178, 21);
            this.textBox_CommandLine.TabIndex = 30;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 86);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 28;
            this.label6.Text = "파일명";
            // 
            // button_Add_VirtualField
            // 
            this.button_Add_VirtualField.Location = new System.Drawing.Point(199, 10);
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
            this.groupBox3.Location = new System.Drawing.Point(7, 16);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(129, 63);
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
            this.button_Save_FileName.Location = new System.Drawing.Point(190, 100);
            this.button_Save_FileName.Name = "button_Save_FileName";
            this.button_Save_FileName.Size = new System.Drawing.Size(82, 23);
            this.button_Save_FileName.TabIndex = 25;
            this.button_Save_FileName.Text = "Save";
            this.button_Save_FileName.UseVisualStyleBackColor = true;
            this.button_Save_FileName.Click += new System.EventHandler(this.button_Save_FileName_Click);
            // 
            // textBox_TableFileName
            // 
            this.textBox_TableFileName.Location = new System.Drawing.Point(7, 101);
            this.textBox_TableFileName.Name = "textBox_TableFileName";
            this.textBox_TableFileName.Size = new System.Drawing.Size(177, 21);
            this.textBox_TableFileName.TabIndex = 26;
            // 
            // button_Check_TableSelected
            // 
            this.button_Check_TableSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Check_TableSelected.Location = new System.Drawing.Point(95, 209);
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
            this.groupBox_SelectedTable.Location = new System.Drawing.Point(292, 10);
            this.groupBox_SelectedTable.Name = "groupBox_SelectedTable";
            this.groupBox_SelectedTable.Size = new System.Drawing.Size(378, 451);
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
            this.groupBox_2_2_SelectedField.Location = new System.Drawing.Point(6, 212);
            this.groupBox_2_2_SelectedField.Name = "groupBox_2_2_SelectedField";
            this.groupBox_2_2_SelectedField.Size = new System.Drawing.Size(360, 232);
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
            this.checkBox_ConvertStringToEnum.Location = new System.Drawing.Point(208, 131);
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
            this.button_Save_Field.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Save_Field.Location = new System.Drawing.Point(217, 201);
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
            this.groupBox_2_2_SelectedField_Virtual.Size = new System.Drawing.Size(347, 106);
            this.groupBox_2_2_SelectedField_Virtual.TabIndex = 17;
            this.groupBox_2_2_SelectedField_Virtual.TabStop = false;
            this.groupBox_2_2_SelectedField_Virtual.Text = "2-2. Virtual Field Option";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(189, 63);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(117, 12);
            this.label8.TabIndex = 23;
            this.label8.Text = "Dependency Field 2";
            // 
            // comboBox_DependencyField_Sub
            // 
            this.comboBox_DependencyField_Sub.FormattingEnabled = true;
            this.comboBox_DependencyField_Sub.Location = new System.Drawing.Point(189, 79);
            this.comboBox_DependencyField_Sub.Name = "comboBox_DependencyField_Sub";
            this.comboBox_DependencyField_Sub.Size = new System.Drawing.Size(152, 20);
            this.comboBox_DependencyField_Sub.TabIndex = 22;
            // 
            // label_Type
            // 
            this.label_Type.AutoSize = true;
            this.label_Type.Location = new System.Drawing.Point(191, 14);
            this.label_Type.Name = "label_Type";
            this.label_Type.Size = new System.Drawing.Size(34, 12);
            this.label_Type.TabIndex = 21;
            this.label_Type.Text = "Type";
            // 
            // textBox_Type
            // 
            this.textBox_Type.Location = new System.Drawing.Point(189, 32);
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
            this.button_Remove_VirtualField.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Remove_VirtualField.Location = new System.Drawing.Point(313, 201);
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
            this.listView_Field.Size = new System.Drawing.Size(366, 192);
            this.listView_Field.TabIndex = 12;
            this.listView_Field.UseCompatibleStateImageBehavior = false;
            this.listView_Field.View = System.Windows.Forms.View.Details;
            // 
            // ColumnHeader_Name
            // 
            this.ColumnHeader_Name.Text = "Name";
            this.ColumnHeader_Name.Width = 173;
            // 
            // ColumnHeader_Type
            // 
            this.ColumnHeader_Type.Text = "Type";
            this.ColumnHeader_Type.Width = 99;
            // 
            // columnHeader_IsVirtual
            // 
            this.columnHeader_IsVirtual.Text = "Virtual";
            this.columnHeader_IsVirtual.Width = 88;
            // 
            // button_Check_TableAll
            // 
            this.button_Check_TableAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Check_TableAll.Location = new System.Drawing.Point(224, 126);
            this.button_Check_TableAll.Name = "button_Check_TableAll";
            this.button_Check_TableAll.Size = new System.Drawing.Size(74, 40);
            this.button_Check_TableAll.TabIndex = 24;
            this.button_Check_TableAll.Text = "Check  All Table";
            this.button_Check_TableAll.UseVisualStyleBackColor = true;
            this.button_Check_TableAll.Click += new System.EventHandler(this.button_Check_TableAll_Click);
            // 
            // groupBox3_BuildSetting
            // 
            this.groupBox3_BuildSetting.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.groupBox3_BuildSetting.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox3_BuildSetting.Controls.Add(this.button_StartBuild_Selected);
            this.groupBox3_BuildSetting.Controls.Add(this.groupBox_3_1_SelectedBuild);
            this.groupBox3_BuildSetting.Controls.Add(this.label4);
            this.groupBox3_BuildSetting.Controls.Add(this.comboBox_BuildList);
            this.groupBox3_BuildSetting.Controls.Add(this.button_AddWork);
            this.groupBox3_BuildSetting.Controls.Add(this.button_Check_TableAll);
            this.groupBox3_BuildSetting.Controls.Add(this.checkedListBox_BuildList);
            this.groupBox3_BuildSetting.Controls.Add(this.button_StartBuild);
            this.groupBox3_BuildSetting.Location = new System.Drawing.Point(362, 468);
            this.groupBox3_BuildSetting.Name = "groupBox3_BuildSetting";
            this.groupBox3_BuildSetting.Size = new System.Drawing.Size(480, 172);
            this.groupBox3_BuildSetting.TabIndex = 7;
            this.groupBox3_BuildSetting.TabStop = false;
            this.groupBox3_BuildSetting.Text = "3. Work Setting";
            // 
            // button_StartBuild_Selected
            // 
            this.button_StartBuild_Selected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_StartBuild_Selected.Location = new System.Drawing.Point(304, 126);
            this.button_StartBuild_Selected.Name = "button_StartBuild_Selected";
            this.button_StartBuild_Selected.Size = new System.Drawing.Size(90, 40);
            this.button_StartBuild_Selected.TabIndex = 25;
            this.button_StartBuild_Selected.Text = "선택한 시트만  빌드 시작!";
            this.button_StartBuild_Selected.UseVisualStyleBackColor = true;
            this.button_StartBuild_Selected.Click += new System.EventHandler(this.button_StartParsing_Selected_Click);
            // 
            // groupBox_3_1_SelectedBuild
            // 
            this.groupBox_3_1_SelectedBuild.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox_3_1_SelectedBuild.Controls.Add(this.button_WorkOrderDown);
            this.groupBox_3_1_SelectedBuild.Controls.Add(this.button_BuildOrderUp);
            this.groupBox_3_1_SelectedBuild.Controls.Add(this.button_EditWork);
            this.groupBox_3_1_SelectedBuild.Controls.Add(this.button_RemoveWork);
            this.groupBox_3_1_SelectedBuild.Location = new System.Drawing.Point(223, 15);
            this.groupBox_3_1_SelectedBuild.Name = "groupBox_3_1_SelectedBuild";
            this.groupBox_3_1_SelectedBuild.Size = new System.Drawing.Size(250, 91);
            this.groupBox_3_1_SelectedBuild.TabIndex = 16;
            this.groupBox_3_1_SelectedBuild.TabStop = false;
            this.groupBox_3_1_SelectedBuild.Text = "3-1. Selected Work";
            // 
            // button_WorkOrderDown
            // 
            this.button_WorkOrderDown.Location = new System.Drawing.Point(6, 57);
            this.button_WorkOrderDown.Name = "button_WorkOrderDown";
            this.button_WorkOrderDown.Size = new System.Drawing.Size(113, 23);
            this.button_WorkOrderDown.TabIndex = 16;
            this.button_WorkOrderDown.Text = "빌드 순위 내리기";
            this.button_WorkOrderDown.UseVisualStyleBackColor = true;
            this.button_WorkOrderDown.Click += new System.EventHandler(this.button_WorkOrderDown_Click);
            // 
            // button_BuildOrderUp
            // 
            this.button_BuildOrderUp.Location = new System.Drawing.Point(6, 20);
            this.button_BuildOrderUp.Name = "button_BuildOrderUp";
            this.button_BuildOrderUp.Size = new System.Drawing.Size(113, 23);
            this.button_BuildOrderUp.TabIndex = 15;
            this.button_BuildOrderUp.Text = "빌드 순위 올리기";
            this.button_BuildOrderUp.UseVisualStyleBackColor = true;
            this.button_BuildOrderUp.Click += new System.EventHandler(this.button_WorkOrderUp_Click);
            // 
            // button_EditWork
            // 
            this.button_EditWork.Location = new System.Drawing.Point(162, 20);
            this.button_EditWork.Name = "button_EditWork";
            this.button_EditWork.Size = new System.Drawing.Size(82, 23);
            this.button_EditWork.TabIndex = 14;
            this.button_EditWork.Text = "빌드 편집";
            this.button_EditWork.UseVisualStyleBackColor = true;
            this.button_EditWork.Click += new System.EventHandler(this.button_EditWork_Click);
            // 
            // button_RemoveWork
            // 
            this.button_RemoveWork.Location = new System.Drawing.Point(162, 57);
            this.button_RemoveWork.Name = "button_RemoveWork";
            this.button_RemoveWork.Size = new System.Drawing.Size(82, 23);
            this.button_RemoveWork.TabIndex = 15;
            this.button_RemoveWork.Text = "빌드 삭제";
            this.button_RemoveWork.UseVisualStyleBackColor = true;
            this.button_RemoveWork.Click += new System.EventHandler(this.button_RemoveWork_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 129);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "추가할 빌드이름";
            // 
            // comboBox_BuildList
            // 
            this.comboBox_BuildList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBox_BuildList.FormattingEnabled = true;
            this.comboBox_BuildList.Location = new System.Drawing.Point(6, 144);
            this.comboBox_BuildList.Name = "comboBox_BuildList";
            this.comboBox_BuildList.Size = new System.Drawing.Size(125, 20);
            this.comboBox_BuildList.TabIndex = 7;
            // 
            // button_AddWork
            // 
            this.button_AddWork.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_AddWork.Location = new System.Drawing.Point(137, 143);
            this.button_AddWork.Name = "button_AddWork";
            this.button_AddWork.Size = new System.Drawing.Size(77, 21);
            this.button_AddWork.TabIndex = 13;
            this.button_AddWork.Text = "작업 추가";
            this.button_AddWork.UseVisualStyleBackColor = true;
            this.button_AddWork.Click += new System.EventHandler(this.button_AddWork_Click);
            // 
            // checkedListBox_BuildList
            // 
            this.checkedListBox_BuildList.FormattingEnabled = true;
            this.checkedListBox_BuildList.HorizontalScrollbar = true;
            this.checkedListBox_BuildList.Location = new System.Drawing.Point(8, 15);
            this.checkedListBox_BuildList.Name = "checkedListBox_BuildList";
            this.checkedListBox_BuildList.Size = new System.Drawing.Size(206, 100);
            this.checkedListBox_BuildList.TabIndex = 12;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.groupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox2.Controls.Add(this.button_LogClear);
            this.groupBox2.Controls.Add(this.textBox_Log);
            this.groupBox2.Location = new System.Drawing.Point(5, 380);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(344, 261);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Log";
            // 
            // button_LogClear
            // 
            this.button_LogClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_LogClear.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.button_LogClear.Location = new System.Drawing.Point(258, 234);
            this.button_LogClear.Name = "button_LogClear";
            this.button_LogClear.Size = new System.Drawing.Size(80, 21);
            this.button_LogClear.TabIndex = 25;
            this.button_LogClear.Text = "Clear Log";
            this.button_LogClear.UseVisualStyleBackColor = true;
            this.button_LogClear.Click += new System.EventHandler(this.button_LogClear_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.groupBox7.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox7.Controls.Add(this.groupBox8);
            this.groupBox7.Controls.Add(this.groupBox1);
            this.groupBox7.Controls.Add(this.listView_SheetSourceConnector);
            this.groupBox7.Location = new System.Drawing.Point(4, 1);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(344, 373);
            this.groupBox7.TabIndex = 26;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "1. Sheet Source Setting";
            // 
            // groupBox8
            // 
            this.groupBox8.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox8.Controls.Add(this.button_OpenSheetSource);
            this.groupBox8.Controls.Add(this.button_AddSheetSource);
            this.groupBox8.Controls.Add(this.groupBox6);
            this.groupBox8.Controls.Add(this.label1);
            this.groupBox8.Controls.Add(this.textBox_AddSheetSourceName);
            this.groupBox8.Location = new System.Drawing.Point(6, 222);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(228, 142);
            this.groupBox8.TabIndex = 27;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "1-1. Add Sheet Source";
            // 
            // button_OpenSheetSource
            // 
            this.button_OpenSheetSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_OpenSheetSource.Location = new System.Drawing.Point(6, 112);
            this.button_OpenSheetSource.Name = "button_OpenSheetSource";
            this.button_OpenSheetSource.Size = new System.Drawing.Size(69, 23);
            this.button_OpenSheetSource.TabIndex = 35;
            this.button_OpenSheetSource.Text = "소스 열기";
            this.button_OpenSheetSource.UseVisualStyleBackColor = true;
            this.button_OpenSheetSource.Click += new System.EventHandler(this.button_OpenSheetSource_Click);
            // 
            // button_AddSheetSource
            // 
            this.button_AddSheetSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_AddSheetSource.Location = new System.Drawing.Point(152, 113);
            this.button_AddSheetSource.Name = "button_AddSheetSource";
            this.button_AddSheetSource.Size = new System.Drawing.Size(69, 23);
            this.button_AddSheetSource.TabIndex = 16;
            this.button_AddSheetSource.Text = "소스 추가";
            this.button_AddSheetSource.UseVisualStyleBackColor = true;
            this.button_AddSheetSource.Click += new System.EventHandler(this.button_AddSheetSource_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.radioButton_SheetSource_MSExcel);
            this.groupBox6.Controls.Add(this.radioButton_SheetSource_GoogleSheet);
            this.groupBox6.Location = new System.Drawing.Point(6, 18);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(204, 36);
            this.groupBox6.TabIndex = 13;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Sheet Source Type";
            // 
            // radioButton_SheetSource_MSExcel
            // 
            this.radioButton_SheetSource_MSExcel.AutoSize = true;
            this.radioButton_SheetSource_MSExcel.Location = new System.Drawing.Point(120, 15);
            this.radioButton_SheetSource_MSExcel.Name = "radioButton_SheetSource_MSExcel";
            this.radioButton_SheetSource_MSExcel.Size = new System.Drawing.Size(78, 16);
            this.radioButton_SheetSource_MSExcel.TabIndex = 11;
            this.radioButton_SheetSource_MSExcel.Text = "MS Excel";
            this.radioButton_SheetSource_MSExcel.UseVisualStyleBackColor = true;
            // 
            // radioButton_SheetSource_GoogleSheet
            // 
            this.radioButton_SheetSource_GoogleSheet.AutoSize = true;
            this.radioButton_SheetSource_GoogleSheet.Location = new System.Drawing.Point(6, 15);
            this.radioButton_SheetSource_GoogleSheet.Name = "radioButton_SheetSource_GoogleSheet";
            this.radioButton_SheetSource_GoogleSheet.Size = new System.Drawing.Size(99, 16);
            this.radioButton_SheetSource_GoogleSheet.TabIndex = 10;
            this.radioButton_SheetSource_GoogleSheet.Text = "Google Sheet";
            this.radioButton_SheetSource_GoogleSheet.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 12);
            this.label1.TabIndex = 34;
            this.label1.Text = "시트 소스 명";
            // 
            // textBox_AddSheetSourceName
            // 
            this.textBox_AddSheetSourceName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_AddSheetSourceName.Location = new System.Drawing.Point(8, 77);
            this.textBox_AddSheetSourceName.Name = "textBox_AddSheetSourceName";
            this.textBox_AddSheetSourceName.Size = new System.Drawing.Size(213, 21);
            this.textBox_AddSheetSourceName.TabIndex = 33;
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Location = new System.Drawing.Point(240, 222);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(95, 142);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "1-2. Selected SheetSource";
            // 
            // button4
            // 
            this.button4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button4.Location = new System.Drawing.Point(6, 75);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(82, 23);
            this.button4.TabIndex = 14;
            this.button4.Text = "작업 편집";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button5.Location = new System.Drawing.Point(6, 113);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(82, 23);
            this.button5.TabIndex = 15;
            this.button5.Text = "작업 삭제";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // listView_SheetSourceConnector
            // 
            this.listView_SheetSourceConnector.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader3,
            this.columnHeader5});
            this.listView_SheetSourceConnector.HideSelection = false;
            this.listView_SheetSourceConnector.Location = new System.Drawing.Point(6, 20);
            this.listView_SheetSourceConnector.MultiSelect = false;
            this.listView_SheetSourceConnector.Name = "listView_SheetSourceConnector";
            this.listView_SheetSourceConnector.Size = new System.Drawing.Size(332, 196);
            this.listView_SheetSourceConnector.TabIndex = 25;
            this.listView_SheetSourceConnector.UseCompatibleStateImageBehavior = false;
            this.listView_SheetSourceConnector.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader4
            // 
            this.columnHeader4.DisplayIndex = 2;
            this.columnHeader4.Text = "Name";
            this.columnHeader4.Width = 293;
            // 
            // columnHeader3
            // 
            this.columnHeader3.DisplayIndex = 0;
            this.columnHeader3.Text = "Enable";
            this.columnHeader3.Width = 52;
            // 
            // columnHeader5
            // 
            this.columnHeader5.DisplayIndex = 1;
            this.columnHeader5.Text = "Type";
            this.columnHeader5.Width = 111;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem_File_New,
            this.menuItem_File_Open,
            this.menuItem_File_Save,
            this.menuItem_File_SaveAs});
            this.menuItem1.Text = "File";
            // 
            // menuItem_File_New
            // 
            this.menuItem_File_New.Index = 0;
            this.menuItem_File_New.Text = "New";
            this.menuItem_File_New.Click += new System.EventHandler(this.menuItem_File_New_Click);
            // 
            // menuItem_File_Open
            // 
            this.menuItem_File_Open.Index = 1;
            this.menuItem_File_Open.Text = "Open";
            this.menuItem_File_Open.Click += new System.EventHandler(this.menuItem_File_Open_Click);
            // 
            // menuItem_File_Save
            // 
            this.menuItem_File_Save.Index = 2;
            this.menuItem_File_Save.Text = "Save";
            this.menuItem_File_Save.Click += new System.EventHandler(this.menuItem_File_Save_Click);
            // 
            // menuItem_File_SaveAs
            // 
            this.menuItem_File_SaveAs.Index = 3;
            this.menuItem_File_SaveAs.Text = "Save As";
            this.menuItem_File_SaveAs.Click += new System.EventHandler(this.menuItem_File_SaveAs_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem_Tools_OpenSaveDataFolder});
            this.menuItem2.Text = "Tools";
            // 
            // menuItem_Tools_OpenSaveDataFolder
            // 
            this.menuItem_Tools_OpenSaveDataFolder.Index = 0;
            this.menuItem_Tools_OpenSaveDataFolder.Text = "Open SaveData";
            this.menuItem_Tools_OpenSaveDataFolder.Click += new System.EventHandler(this.menuItem_Tools_OpenSaveDataFolder_Click);
            // 
            // SheetParser_MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1044, 653);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3_BuildSetting);
            this.Controls.Add(this.groupBox_2_1_TableSetting);
            this.Menu = this.mainMenu1;
            this.Name = "SheetParser_MainForm";
            this.Text = "Sheet Parser";
            this.Load += new System.EventHandler(this.MainForm_Load);
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
            this.groupBox3_BuildSetting.ResumeLayout(false);
            this.groupBox3_BuildSetting.PerformLayout();
            this.groupBox_3_1_SelectedBuild.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_StartBuild;
        private System.Windows.Forms.TextBox textBox_Log;
        private System.Windows.Forms.GroupBox groupBox_2_1_TableSetting;
        private System.Windows.Forms.GroupBox groupBox3_BuildSetting;
        private System.Windows.Forms.GroupBox groupBox_SelectedTable;
        private System.Windows.Forms.Button button_Check_TableSelected;
        private System.Windows.Forms.RadioButton radioButton_Class;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButton_Enum;
        private System.Windows.Forms.RadioButton radioButton_Struct;
        private System.Windows.Forms.CheckedListBox checkedListBox_BuildList;
        private System.Windows.Forms.Button button_AddWork;
        private System.Windows.Forms.Button button_EditWork;
        private System.Windows.Forms.Button button_RemoveWork;
        private System.Windows.Forms.ComboBox comboBox_BuildList;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox_3_1_SelectedBuild;
        private System.Windows.Forms.Button button_BuildOrderUp;
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
        private System.Windows.Forms.Button button_LogClear;
        private System.Windows.Forms.Button button_StartBuild_Selected;
        private System.Windows.Forms.ListView listView_Sheet;
        private System.Windows.Forms.ColumnHeader columnHeader_IsEnable;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.ListView listView_SheetSourceConnector;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.RadioButton radioButton_SheetSource_MSExcel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_AddSheetSourceName;
        private System.Windows.Forms.RadioButton radioButton_SheetSource_GoogleSheet;
        private System.Windows.Forms.Button button_AddSheetSource;
        private System.Windows.Forms.Button button_OpenSheetSource;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem_File_New;
        private System.Windows.Forms.MenuItem menuItem_File_Open;
        private System.Windows.Forms.MenuItem menuItem_File_Save;
        private System.Windows.Forms.MenuItem menuItem_File_SaveAs;
        private System.Windows.Forms.MenuItem menuItem_Tools_OpenSaveDataFolder;
    }
}

