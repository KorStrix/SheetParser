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
            this.button_Connect = new System.Windows.Forms.Button();
            this.checkedListBox_SheetList = new System.Windows.Forms.CheckedListBox();
            this.button_StartParsing = new System.Windows.Forms.Button();
            this.textBox_Console = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_OpenLink = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_SaveSheet = new System.Windows.Forms.ComboBox();
            this.button_OpenPath_SaveSheet = new System.Windows.Forms.Button();
            this.groupBox2_TableSetting = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox_SelectedTable = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButton_Enum = new System.Windows.Forms.RadioButton();
            this.radioButton_Struct = new System.Windows.Forms.RadioButton();
            this.radioButton_Class = new System.Windows.Forms.RadioButton();
            this.button_CheckTable = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.button_Save_TableCommandLine = new System.Windows.Forms.Button();
            this.button_Cancel_TableCommandLine = new System.Windows.Forms.Button();
            this.textBox_CommandLine = new System.Windows.Forms.TextBox();
            this.groupBox3_OutputSetting = new System.Windows.Forms.GroupBox();
            this.button_Save_FileName_Csharp = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_FileName_Csharp = new System.Windows.Forms.TextBox();
            this.checkBox_OpenFolder_AfterBuild_Csharp = new System.Windows.Forms.CheckBox();
            this.checkBox_OpenFolder_AfterBuild_CSV = new System.Windows.Forms.CheckBox();
            this.button_OpenPath_CSV = new System.Windows.Forms.Button();
            this.button_OpenPath_Csharp = new System.Windows.Forms.Button();
            this.button_CSV_PathSetting = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_CSV_Path = new System.Windows.Forms.TextBox();
            this.button_Csharp_PathSetting = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_Csharp_Path = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.checkBox_AutoConnect = new System.Windows.Forms.CheckBox();
            this.button_Info = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2_TableSetting.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox_SelectedTable.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox3_OutputSetting.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_SheetID
            // 
            this.textBox_SheetID.Location = new System.Drawing.Point(116, 20);
            this.textBox_SheetID.Name = "textBox_SheetID";
            this.textBox_SheetID.Size = new System.Drawing.Size(400, 21);
            this.textBox_SheetID.TabIndex = 0;
            // 
            // button_Connect
            // 
            this.button_Connect.Location = new System.Drawing.Point(522, 20);
            this.button_Connect.Name = "button_Connect";
            this.button_Connect.Size = new System.Drawing.Size(75, 23);
            this.button_Connect.TabIndex = 1;
            this.button_Connect.Text = "연결";
            this.button_Connect.UseVisualStyleBackColor = true;
            this.button_Connect.Click += new System.EventHandler(this.button_Connect_Click);
            // 
            // checkedListBox_SheetList
            // 
            this.checkedListBox_SheetList.FormattingEnabled = true;
            this.checkedListBox_SheetList.HorizontalScrollbar = true;
            this.checkedListBox_SheetList.Location = new System.Drawing.Point(20, 31);
            this.checkedListBox_SheetList.Name = "checkedListBox_SheetList";
            this.checkedListBox_SheetList.Size = new System.Drawing.Size(253, 276);
            this.checkedListBox_SheetList.TabIndex = 2;
            this.checkedListBox_SheetList.SelectedIndexChanged += new System.EventHandler(this.checkedListBox_TableList_SelectedIndexChanged);
            // 
            // button_StartParsing
            // 
            this.button_StartParsing.Location = new System.Drawing.Point(315, 267);
            this.button_StartParsing.Name = "button_StartParsing";
            this.button_StartParsing.Size = new System.Drawing.Size(124, 40);
            this.button_StartParsing.TabIndex = 3;
            this.button_StartParsing.Text = "파일 생성";
            this.button_StartParsing.UseVisualStyleBackColor = true;
            this.button_StartParsing.Click += new System.EventHandler(this.button_StartParsing_Click);
            // 
            // textBox_Console
            // 
            this.textBox_Console.Location = new System.Drawing.Point(12, 417);
            this.textBox_Console.Multiline = true;
            this.textBox_Console.Name = "textBox_Console";
            this.textBox_Console.ReadOnly = true;
            this.textBox_Console.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_Console.Size = new System.Drawing.Size(1060, 108);
            this.textBox_Console.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_OpenLink);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBox_SaveSheet);
            this.groupBox1.Controls.Add(this.textBox_SheetID);
            this.groupBox1.Controls.Add(this.button_Connect);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(609, 80);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "1. Connect";
            // 
            // button_OpenLink
            // 
            this.button_OpenLink.Location = new System.Drawing.Point(522, 47);
            this.button_OpenLink.Name = "button_OpenLink";
            this.button_OpenLink.Size = new System.Drawing.Size(75, 23);
            this.button_OpenLink.TabIndex = 6;
            this.button_OpenLink.Text = "링크 열기";
            this.button_OpenLink.UseVisualStyleBackColor = true;
            this.button_OpenLink.Click += new System.EventHandler(this.button_OpenLink_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "연결할 Sheet ID";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "이전에 연결한 ID";
            // 
            // comboBox_SaveSheet
            // 
            this.comboBox_SaveSheet.FormattingEnabled = true;
            this.comboBox_SaveSheet.Location = new System.Drawing.Point(116, 50);
            this.comboBox_SaveSheet.Name = "comboBox_SaveSheet";
            this.comboBox_SaveSheet.Size = new System.Drawing.Size(400, 20);
            this.comboBox_SaveSheet.TabIndex = 2;
            // 
            // button_OpenPath_SaveSheet
            // 
            this.button_OpenPath_SaveSheet.Location = new System.Drawing.Point(8, 48);
            this.button_OpenPath_SaveSheet.Name = "button_OpenPath_SaveSheet";
            this.button_OpenPath_SaveSheet.Size = new System.Drawing.Size(75, 23);
            this.button_OpenPath_SaveSheet.TabIndex = 5;
            this.button_OpenPath_SaveSheet.Text = "저장폴더";
            this.button_OpenPath_SaveSheet.UseVisualStyleBackColor = true;
            this.button_OpenPath_SaveSheet.Click += new System.EventHandler(this.Button_OpenPath_SaveSheet_Click);
            // 
            // groupBox2_TableSetting
            // 
            this.groupBox2_TableSetting.Controls.Add(this.groupBox2);
            this.groupBox2_TableSetting.Controls.Add(this.groupBox_SelectedTable);
            this.groupBox2_TableSetting.Controls.Add(this.checkedListBox_SheetList);
            this.groupBox2_TableSetting.Location = new System.Drawing.Point(12, 98);
            this.groupBox2_TableSetting.Name = "groupBox2_TableSetting";
            this.groupBox2_TableSetting.Size = new System.Drawing.Size(609, 313);
            this.groupBox2_TableSetting.TabIndex = 6;
            this.groupBox2_TableSetting.TabStop = false;
            this.groupBox2_TableSetting.Text = "2. TableSetting";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Location = new System.Drawing.Point(279, 221);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(324, 86);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "All Checked Table";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(236, 55);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(82, 23);
            this.button3.TabIndex = 10;
            this.button3.Text = "Check";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // groupBox_SelectedTable
            // 
            this.groupBox_SelectedTable.Controls.Add(this.button_Info);
            this.groupBox_SelectedTable.Controls.Add(this.groupBox3);
            this.groupBox_SelectedTable.Controls.Add(this.button_CheckTable);
            this.groupBox_SelectedTable.Controls.Add(this.label3);
            this.groupBox_SelectedTable.Controls.Add(this.button_Save_TableCommandLine);
            this.groupBox_SelectedTable.Controls.Add(this.button_Cancel_TableCommandLine);
            this.groupBox_SelectedTable.Controls.Add(this.textBox_CommandLine);
            this.groupBox_SelectedTable.Location = new System.Drawing.Point(279, 20);
            this.groupBox_SelectedTable.Name = "groupBox_SelectedTable";
            this.groupBox_SelectedTable.Size = new System.Drawing.Size(324, 195);
            this.groupBox_SelectedTable.TabIndex = 9;
            this.groupBox_SelectedTable.TabStop = false;
            this.groupBox_SelectedTable.Text = "Selected Table Setting";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioButton_Enum);
            this.groupBox3.Controls.Add(this.radioButton_Struct);
            this.groupBox3.Controls.Add(this.radioButton_Class);
            this.groupBox3.Location = new System.Drawing.Point(8, 20);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(199, 51);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Type";
            // 
            // radioButton_Enum
            // 
            this.radioButton_Enum.AutoSize = true;
            this.radioButton_Enum.Location = new System.Drawing.Point(136, 20);
            this.radioButton_Enum.Name = "radioButton_Enum";
            this.radioButton_Enum.Size = new System.Drawing.Size(56, 16);
            this.radioButton_Enum.TabIndex = 12;
            this.radioButton_Enum.Text = "Enum";
            this.radioButton_Enum.UseVisualStyleBackColor = true;
            this.radioButton_Enum.CheckedChanged += new System.EventHandler(this.radioButton_Enum_CheckedChanged);
            // 
            // radioButton_Struct
            // 
            this.radioButton_Struct.AutoSize = true;
            this.radioButton_Struct.Location = new System.Drawing.Point(71, 20);
            this.radioButton_Struct.Name = "radioButton_Struct";
            this.radioButton_Struct.Size = new System.Drawing.Size(55, 16);
            this.radioButton_Struct.TabIndex = 11;
            this.radioButton_Struct.Text = "Struct";
            this.radioButton_Struct.UseVisualStyleBackColor = true;
            this.radioButton_Struct.CheckedChanged += new System.EventHandler(this.radioButton_Struct_CheckedChanged);
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
            this.radioButton_Class.CheckedChanged += new System.EventHandler(this.radioButton_class_CheckedChanged);
            // 
            // button_CheckTable
            // 
            this.button_CheckTable.Location = new System.Drawing.Point(236, 166);
            this.button_CheckTable.Name = "button_CheckTable";
            this.button_CheckTable.Size = new System.Drawing.Size(82, 23);
            this.button_CheckTable.TabIndex = 9;
            this.button_CheckTable.Text = "Check";
            this.button_CheckTable.UseVisualStyleBackColor = true;
            this.button_CheckTable.Click += new System.EventHandler(this.button_CheckTable_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "CommandLine";
            // 
            // button_Save_TableCommandLine
            // 
            this.button_Save_TableCommandLine.Location = new System.Drawing.Point(236, 137);
            this.button_Save_TableCommandLine.Name = "button_Save_TableCommandLine";
            this.button_Save_TableCommandLine.Size = new System.Drawing.Size(82, 23);
            this.button_Save_TableCommandLine.TabIndex = 6;
            this.button_Save_TableCommandLine.Text = "Save";
            this.button_Save_TableCommandLine.UseVisualStyleBackColor = true;
            this.button_Save_TableCommandLine.Click += new System.EventHandler(this.button_TableSave_Click);
            // 
            // button_Cancel_TableCommandLine
            // 
            this.button_Cancel_TableCommandLine.Location = new System.Drawing.Point(8, 137);
            this.button_Cancel_TableCommandLine.Name = "button_Cancel_TableCommandLine";
            this.button_Cancel_TableCommandLine.Size = new System.Drawing.Size(82, 23);
            this.button_Cancel_TableCommandLine.TabIndex = 7;
            this.button_Cancel_TableCommandLine.Text = "Cancel";
            this.button_Cancel_TableCommandLine.UseVisualStyleBackColor = true;
            this.button_Cancel_TableCommandLine.Click += new System.EventHandler(this.button_Cancel_TableCommandLine_Click);
            // 
            // textBox_CommandLine
            // 
            this.textBox_CommandLine.Location = new System.Drawing.Point(6, 110);
            this.textBox_CommandLine.Name = "textBox_CommandLine";
            this.textBox_CommandLine.Size = new System.Drawing.Size(312, 21);
            this.textBox_CommandLine.TabIndex = 6;
            // 
            // groupBox3_OutputSetting
            // 
            this.groupBox3_OutputSetting.Controls.Add(this.button_Save_FileName_Csharp);
            this.groupBox3_OutputSetting.Controls.Add(this.label6);
            this.groupBox3_OutputSetting.Controls.Add(this.textBox_FileName_Csharp);
            this.groupBox3_OutputSetting.Controls.Add(this.checkBox_OpenFolder_AfterBuild_Csharp);
            this.groupBox3_OutputSetting.Controls.Add(this.checkBox_OpenFolder_AfterBuild_CSV);
            this.groupBox3_OutputSetting.Controls.Add(this.button_OpenPath_CSV);
            this.groupBox3_OutputSetting.Controls.Add(this.button_OpenPath_Csharp);
            this.groupBox3_OutputSetting.Controls.Add(this.button_CSV_PathSetting);
            this.groupBox3_OutputSetting.Controls.Add(this.label5);
            this.groupBox3_OutputSetting.Controls.Add(this.textBox_CSV_Path);
            this.groupBox3_OutputSetting.Controls.Add(this.button_Csharp_PathSetting);
            this.groupBox3_OutputSetting.Controls.Add(this.label4);
            this.groupBox3_OutputSetting.Controls.Add(this.textBox_Csharp_Path);
            this.groupBox3_OutputSetting.Controls.Add(this.button_StartParsing);
            this.groupBox3_OutputSetting.Location = new System.Drawing.Point(627, 98);
            this.groupBox3_OutputSetting.Name = "groupBox3_OutputSetting";
            this.groupBox3_OutputSetting.Size = new System.Drawing.Size(445, 313);
            this.groupBox3_OutputSetting.TabIndex = 7;
            this.groupBox3_OutputSetting.TabStop = false;
            this.groupBox3_OutputSetting.Text = "3. Output Setting";
            // 
            // button_Save_FileName_Csharp
            // 
            this.button_Save_FileName_Csharp.Location = new System.Drawing.Point(357, 76);
            this.button_Save_FileName_Csharp.Name = "button_Save_FileName_Csharp";
            this.button_Save_FileName_Csharp.Size = new System.Drawing.Size(82, 23);
            this.button_Save_FileName_Csharp.TabIndex = 19;
            this.button_Save_FileName_Csharp.Text = "파일명 저장";
            this.button_Save_FileName_Csharp.UseVisualStyleBackColor = true;
            this.button_Save_FileName_Csharp.Click += new System.EventHandler(this.button_Save_FileName_Csharp_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(84, 79);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 12);
            this.label6.TabIndex = 15;
            this.label6.Text = "C# 파일명";
            // 
            // textBox_FileName_Csharp
            // 
            this.textBox_FileName_Csharp.Location = new System.Drawing.Point(150, 76);
            this.textBox_FileName_Csharp.Name = "textBox_FileName_Csharp";
            this.textBox_FileName_Csharp.Size = new System.Drawing.Size(178, 21);
            this.textBox_FileName_Csharp.TabIndex = 14;
            // 
            // checkBox_OpenFolder_AfterBuild_Csharp
            // 
            this.checkBox_OpenFolder_AfterBuild_Csharp.AutoSize = true;
            this.checkBox_OpenFolder_AfterBuild_Csharp.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_OpenFolder_AfterBuild_Csharp.Location = new System.Drawing.Point(238, 22);
            this.checkBox_OpenFolder_AfterBuild_Csharp.Name = "checkBox_OpenFolder_AfterBuild_Csharp";
            this.checkBox_OpenFolder_AfterBuild_Csharp.Size = new System.Drawing.Size(72, 16);
            this.checkBox_OpenFolder_AfterBuild_Csharp.TabIndex = 13;
            this.checkBox_OpenFolder_AfterBuild_Csharp.Text = "자동연결";
            this.checkBox_OpenFolder_AfterBuild_Csharp.UseVisualStyleBackColor = true;
            this.checkBox_OpenFolder_AfterBuild_Csharp.CheckedChanged += new System.EventHandler(this.checkBox_OpenFolder_AfterBuild_Csharp_CheckedChanged);
            // 
            // checkBox_OpenFolder_AfterBuild_CSV
            // 
            this.checkBox_OpenFolder_AfterBuild_CSV.AutoSize = true;
            this.checkBox_OpenFolder_AfterBuild_CSV.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_OpenFolder_AfterBuild_CSV.Location = new System.Drawing.Point(238, 134);
            this.checkBox_OpenFolder_AfterBuild_CSV.Name = "checkBox_OpenFolder_AfterBuild_CSV";
            this.checkBox_OpenFolder_AfterBuild_CSV.Size = new System.Drawing.Size(72, 16);
            this.checkBox_OpenFolder_AfterBuild_CSV.TabIndex = 8;
            this.checkBox_OpenFolder_AfterBuild_CSV.Text = "자동연결";
            this.checkBox_OpenFolder_AfterBuild_CSV.UseVisualStyleBackColor = true;
            this.checkBox_OpenFolder_AfterBuild_CSV.CheckedChanged += new System.EventHandler(this.checkBox_OpenFolder_AfterBuild_CSV_CheckedChanged);
            // 
            // button_OpenPath_CSV
            // 
            this.button_OpenPath_CSV.Location = new System.Drawing.Point(150, 130);
            this.button_OpenPath_CSV.Name = "button_OpenPath_CSV";
            this.button_OpenPath_CSV.Size = new System.Drawing.Size(82, 23);
            this.button_OpenPath_CSV.TabIndex = 12;
            this.button_OpenPath_CSV.Text = "경로 열기";
            this.button_OpenPath_CSV.UseVisualStyleBackColor = true;
            this.button_OpenPath_CSV.Click += new System.EventHandler(this.Button_OpenPath_CSV_Click);
            // 
            // button_OpenPath_Csharp
            // 
            this.button_OpenPath_Csharp.Location = new System.Drawing.Point(150, 18);
            this.button_OpenPath_Csharp.Name = "button_OpenPath_Csharp";
            this.button_OpenPath_Csharp.Size = new System.Drawing.Size(82, 23);
            this.button_OpenPath_Csharp.TabIndex = 11;
            this.button_OpenPath_Csharp.Text = "경로 열기";
            this.button_OpenPath_Csharp.UseVisualStyleBackColor = true;
            this.button_OpenPath_Csharp.Click += new System.EventHandler(this.Button_OpenPath_Csharp_Click);
            // 
            // button_CSV_PathSetting
            // 
            this.button_CSV_PathSetting.Location = new System.Drawing.Point(357, 130);
            this.button_CSV_PathSetting.Name = "button_CSV_PathSetting";
            this.button_CSV_PathSetting.Size = new System.Drawing.Size(82, 23);
            this.button_CSV_PathSetting.TabIndex = 10;
            this.button_CSV_PathSetting.Text = "경로 세팅";
            this.button_CSV_PathSetting.UseVisualStyleBackColor = true;
            this.button_CSV_PathSetting.Click += new System.EventHandler(this.Button_CSV_PathSetting_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 135);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(109, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "3-2. CSV File Path";
            // 
            // textBox_CSV_Path
            // 
            this.textBox_CSV_Path.Location = new System.Drawing.Point(8, 161);
            this.textBox_CSV_Path.Name = "textBox_CSV_Path";
            this.textBox_CSV_Path.Size = new System.Drawing.Size(431, 21);
            this.textBox_CSV_Path.TabIndex = 8;
            // 
            // button_Csharp_PathSetting
            // 
            this.button_Csharp_PathSetting.Location = new System.Drawing.Point(357, 18);
            this.button_Csharp_PathSetting.Name = "button_Csharp_PathSetting";
            this.button_Csharp_PathSetting.Size = new System.Drawing.Size(82, 23);
            this.button_Csharp_PathSetting.TabIndex = 7;
            this.button_Csharp_PathSetting.Text = "경로 세팅";
            this.button_Csharp_PathSetting.UseVisualStyleBackColor = true;
            this.button_Csharp_PathSetting.Click += new System.EventHandler(this.Button_Csharp_PathSetting_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "3-1. C# File Path";
            // 
            // textBox_Csharp_Path
            // 
            this.textBox_Csharp_Path.Location = new System.Drawing.Point(8, 49);
            this.textBox_Csharp_Path.Name = "textBox_Csharp_Path";
            this.textBox_Csharp_Path.Size = new System.Drawing.Size(431, 21);
            this.textBox_Csharp_Path.TabIndex = 5;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.checkBox_AutoConnect);
            this.groupBox5.Controls.Add(this.button_OpenPath_SaveSheet);
            this.groupBox5.Location = new System.Drawing.Point(627, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(445, 80);
            this.groupBox5.TabIndex = 13;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "0. Config";
            // 
            // checkBox_AutoConnect
            // 
            this.checkBox_AutoConnect.AutoSize = true;
            this.checkBox_AutoConnect.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_AutoConnect.Location = new System.Drawing.Point(8, 25);
            this.checkBox_AutoConnect.Name = "checkBox_AutoConnect";
            this.checkBox_AutoConnect.Size = new System.Drawing.Size(200, 16);
            this.checkBox_AutoConnect.TabIndex = 7;
            this.checkBox_AutoConnect.Text = "프로그램 실행시 Sheet 자동연결";
            this.checkBox_AutoConnect.UseVisualStyleBackColor = true;
            this.checkBox_AutoConnect.CheckedChanged += new System.EventHandler(this.checkBox_AutoConnect_CheckedChanged);
            // 
            // button_Info
            // 
            this.button_Info.Location = new System.Drawing.Point(100, 86);
            this.button_Info.Name = "button_Info";
            this.button_Info.Size = new System.Drawing.Size(60, 23);
            this.button_Info.TabIndex = 12;
            this.button_Info.Text = "Info";
            this.button_Info.UseVisualStyleBackColor = true;
            this.button_Info.Click += new System.EventHandler(this.button_Info_Click);
            // 
            // SpreadSheetParser_MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 537);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3_OutputSetting);
            this.Controls.Add(this.groupBox2_TableSetting);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox_Console);
            this.Name = "SpreadSheetParser_MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2_TableSetting.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox_SelectedTable.ResumeLayout(false);
            this.groupBox_SelectedTable.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox3_OutputSetting.ResumeLayout(false);
            this.groupBox3_OutputSetting.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_SheetID;
        private System.Windows.Forms.Button button_Connect;
        private System.Windows.Forms.CheckedListBox checkedListBox_SheetList;
        private System.Windows.Forms.Button button_StartParsing;
        private System.Windows.Forms.TextBox textBox_Console;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2_TableSetting;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_SaveSheet;
        private System.Windows.Forms.GroupBox groupBox3_OutputSetting;
        private System.Windows.Forms.TextBox textBox_Csharp_Path;
        private System.Windows.Forms.TextBox textBox_CommandLine;
        private System.Windows.Forms.Button button_Cancel_TableCommandLine;
        private System.Windows.Forms.Button button_Save_TableCommandLine;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox_SelectedTable;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_Csharp_PathSetting;
        private System.Windows.Forms.Button button_CSV_PathSetting;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_CSV_Path;
        private System.Windows.Forms.Button button_OpenPath_CSV;
        private System.Windows.Forms.Button button_OpenPath_Csharp;
        private System.Windows.Forms.Button button_OpenPath_SaveSheet;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox checkBox_AutoConnect;
        private System.Windows.Forms.Button button_CheckTable;
        private System.Windows.Forms.CheckBox checkBox_OpenFolder_AfterBuild_Csharp;
        private System.Windows.Forms.CheckBox checkBox_OpenFolder_AfterBuild_CSV;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_FileName_Csharp;
        private System.Windows.Forms.Button button_Save_FileName_Csharp;
        private System.Windows.Forms.Button button_OpenLink;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButton_Class;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButton_Enum;
        private System.Windows.Forms.RadioButton radioButton_Struct;
        private System.Windows.Forms.Button button_Info;
    }
}

