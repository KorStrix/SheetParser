namespace SpreadSheetParser
{
    partial class SpreadSheetParser
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
            this.checkedListBox_TableList = new System.Windows.Forms.CheckedListBox();
            this.button_StartParsing = new System.Windows.Forms.Button();
            this.textBox_Console = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_OpenPath_SaveSheet = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_SaveSheet = new System.Windows.Forms.ComboBox();
            this.groupBox2_TableSetting = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox_CommandLine = new System.Windows.Forms.TextBox();
            this.groupBox3_OutputSetting = new System.Windows.Forms.GroupBox();
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
            this.groupBox1.SuspendLayout();
            this.groupBox2_TableSetting.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3_OutputSetting.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_SheetID
            // 
            this.textBox_SheetID.Location = new System.Drawing.Point(138, 20);
            this.textBox_SheetID.Name = "textBox_SheetID";
            this.textBox_SheetID.Size = new System.Drawing.Size(389, 21);
            this.textBox_SheetID.TabIndex = 0;
            // 
            // button_Connect
            // 
            this.button_Connect.Location = new System.Drawing.Point(452, 47);
            this.button_Connect.Name = "button_Connect";
            this.button_Connect.Size = new System.Drawing.Size(75, 23);
            this.button_Connect.TabIndex = 1;
            this.button_Connect.Text = "연결";
            this.button_Connect.UseVisualStyleBackColor = true;
            this.button_Connect.Click += new System.EventHandler(this.button_Connect_Click);
            // 
            // checkedListBox_TableList
            // 
            this.checkedListBox_TableList.FormattingEnabled = true;
            this.checkedListBox_TableList.Location = new System.Drawing.Point(20, 31);
            this.checkedListBox_TableList.Name = "checkedListBox_TableList";
            this.checkedListBox_TableList.Size = new System.Drawing.Size(253, 276);
            this.checkedListBox_TableList.TabIndex = 2;
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
            this.textBox_Console.Size = new System.Drawing.Size(984, 108);
            this.textBox_Console.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_OpenPath_SaveSheet);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBox_SaveSheet);
            this.groupBox1.Controls.Add(this.textBox_SheetID);
            this.groupBox1.Controls.Add(this.button_Connect);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(533, 80);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "1. Connect";
            // 
            // button_OpenPath_SaveSheet
            // 
            this.button_OpenPath_SaveSheet.Location = new System.Drawing.Point(363, 47);
            this.button_OpenPath_SaveSheet.Name = "button_OpenPath_SaveSheet";
            this.button_OpenPath_SaveSheet.Size = new System.Drawing.Size(75, 23);
            this.button_OpenPath_SaveSheet.TabIndex = 5;
            this.button_OpenPath_SaveSheet.Text = "저장폴더";
            this.button_OpenPath_SaveSheet.UseVisualStyleBackColor = true;
            this.button_OpenPath_SaveSheet.Click += new System.EventHandler(this.Button_OpenPath_SaveSheet_Click);
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
            this.comboBox_SaveSheet.Location = new System.Drawing.Point(138, 50);
            this.comboBox_SaveSheet.Name = "comboBox_SaveSheet";
            this.comboBox_SaveSheet.Size = new System.Drawing.Size(219, 20);
            this.comboBox_SaveSheet.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2_TableSetting.Controls.Add(this.groupBox4);
            this.groupBox2_TableSetting.Controls.Add(this.checkedListBox_TableList);
            this.groupBox2_TableSetting.Location = new System.Drawing.Point(12, 98);
            this.groupBox2_TableSetting.Name = "groupBox2";
            this.groupBox2_TableSetting.Size = new System.Drawing.Size(533, 313);
            this.groupBox2_TableSetting.TabIndex = 6;
            this.groupBox2_TableSetting.TabStop = false;
            this.groupBox2_TableSetting.Text = "2. TableSetting";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Controls.Add(this.button2);
            this.groupBox4.Controls.Add(this.textBox_CommandLine);
            this.groupBox4.Location = new System.Drawing.Point(279, 20);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(248, 287);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Selected Table Setting";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "커맨드라인";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(131, 166);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(8, 166);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(82, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // textBox_CommandLine
            // 
            this.textBox_CommandLine.Location = new System.Drawing.Point(6, 121);
            this.textBox_CommandLine.Name = "textBox_CommandLine";
            this.textBox_CommandLine.Size = new System.Drawing.Size(207, 21);
            this.textBox_CommandLine.TabIndex = 6;
            // 
            // groupBox3
            // 
            this.groupBox3_OutputSetting.Controls.Add(this.button_OpenPath_CSV);
            this.groupBox3_OutputSetting.Controls.Add(this.button_OpenPath_Csharp);
            this.groupBox3_OutputSetting.Controls.Add(this.button_CSV_PathSetting);
            this.groupBox3_OutputSetting.Controls.Add(this.label5);
            this.groupBox3_OutputSetting.Controls.Add(this.textBox_CSV_Path);
            this.groupBox3_OutputSetting.Controls.Add(this.button_Csharp_PathSetting);
            this.groupBox3_OutputSetting.Controls.Add(this.label4);
            this.groupBox3_OutputSetting.Controls.Add(this.textBox_Csharp_Path);
            this.groupBox3_OutputSetting.Controls.Add(this.button_StartParsing);
            this.groupBox3_OutputSetting.Location = new System.Drawing.Point(551, 98);
            this.groupBox3_OutputSetting.Name = "groupBox3";
            this.groupBox3_OutputSetting.Size = new System.Drawing.Size(445, 313);
            this.groupBox3_OutputSetting.TabIndex = 7;
            this.groupBox3_OutputSetting.TabStop = false;
            this.groupBox3_OutputSetting.Text = "3. Output Setting";
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
            this.groupBox5.Location = new System.Drawing.Point(551, 12);
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
            this.checkBox_AutoConnect.Size = new System.Drawing.Size(72, 16);
            this.checkBox_AutoConnect.TabIndex = 7;
            this.checkBox_AutoConnect.Text = "자동연결";
            this.checkBox_AutoConnect.UseVisualStyleBackColor = true;
            this.checkBox_AutoConnect.CheckedChanged += new System.EventHandler(this.checkBox_AutoConnect_CheckedChanged);
            // 
            // SpreadSheetParser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 537);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3_OutputSetting);
            this.Controls.Add(this.groupBox2_TableSetting);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox_Console);
            this.Name = "SpreadSheetParser";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2_TableSetting.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
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
        private System.Windows.Forms.CheckedListBox checkedListBox_TableList;
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
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox4;
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
    }
}

