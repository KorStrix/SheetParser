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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_SaveSheet = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox_CommandLine = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button_CSV_PathSetting = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_CSV_Path = new System.Windows.Forms.TextBox();
            this.button_Csharp_PathSetting = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_Csharp_Path = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_SheetID
            // 
            this.textBox_SheetID.Location = new System.Drawing.Point(138, 20);
            this.textBox_SheetID.Name = "textBox_SheetID";
            this.textBox_SheetID.Size = new System.Drawing.Size(283, 21);
            this.textBox_SheetID.TabIndex = 0;
            // 
            // button_Connect
            // 
            this.button_Connect.Location = new System.Drawing.Point(346, 47);
            this.button_Connect.Name = "button_Connect";
            this.button_Connect.Size = new System.Drawing.Size(75, 23);
            this.button_Connect.TabIndex = 1;
            this.button_Connect.Text = "Connect";
            this.button_Connect.UseVisualStyleBackColor = true;
            this.button_Connect.Click += new System.EventHandler(this.button_Connect_Click);
            // 
            // checkedListBox_TableList
            // 
            this.checkedListBox_TableList.FormattingEnabled = true;
            this.checkedListBox_TableList.Location = new System.Drawing.Point(20, 21);
            this.checkedListBox_TableList.Name = "checkedListBox_TableList";
            this.checkedListBox_TableList.Size = new System.Drawing.Size(176, 196);
            this.checkedListBox_TableList.TabIndex = 2;
            // 
            // button_StartParsing
            // 
            this.button_StartParsing.Location = new System.Drawing.Point(213, 255);
            this.button_StartParsing.Name = "button_StartParsing";
            this.button_StartParsing.Size = new System.Drawing.Size(124, 40);
            this.button_StartParsing.TabIndex = 3;
            this.button_StartParsing.Text = "파일 생성";
            this.button_StartParsing.UseVisualStyleBackColor = true;
            this.button_StartParsing.Click += new System.EventHandler(this.button_StartParsing_Click);
            // 
            // textBox_Console
            // 
            this.textBox_Console.Location = new System.Drawing.Point(12, 330);
            this.textBox_Console.Multiline = true;
            this.textBox_Console.Name = "textBox_Console";
            this.textBox_Console.ReadOnly = true;
            this.textBox_Console.Size = new System.Drawing.Size(427, 108);
            this.textBox_Console.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBox_SaveSheet);
            this.groupBox1.Controls.Add(this.textBox_SheetID);
            this.groupBox1.Controls.Add(this.button_Connect);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(427, 80);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "1. Connect";
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
            this.comboBox_SaveSheet.Size = new System.Drawing.Size(184, 20);
            this.comboBox_SaveSheet.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.checkedListBox_TableList);
            this.groupBox2.Location = new System.Drawing.Point(12, 98);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(427, 226);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "2. TableSetting";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Controls.Add(this.button2);
            this.groupBox4.Controls.Add(this.textBox_CommandLine);
            this.groupBox4.Location = new System.Drawing.Point(202, 20);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(219, 197);
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
            this.groupBox3.Controls.Add(this.button_CSV_PathSetting);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.textBox_CSV_Path);
            this.groupBox3.Controls.Add(this.button_Csharp_PathSetting);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.textBox_Csharp_Path);
            this.groupBox3.Controls.Add(this.button_StartParsing);
            this.groupBox3.Location = new System.Drawing.Point(445, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(343, 312);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "3. Output Setting";
            // 
            // button_CSV_PathSetting
            // 
            this.button_CSV_PathSetting.Location = new System.Drawing.Point(213, 130);
            this.button_CSV_PathSetting.Name = "button_CSV_PathSetting";
            this.button_CSV_PathSetting.Size = new System.Drawing.Size(124, 23);
            this.button_CSV_PathSetting.TabIndex = 10;
            this.button_CSV_PathSetting.Text = "경로 세팅";
            this.button_CSV_PathSetting.UseVisualStyleBackColor = true;
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
            this.textBox_CSV_Path.Size = new System.Drawing.Size(329, 21);
            this.textBox_CSV_Path.TabIndex = 8;
            // 
            // button_Csharp_PathSetting
            // 
            this.button_Csharp_PathSetting.Location = new System.Drawing.Point(213, 18);
            this.button_Csharp_PathSetting.Name = "button_Csharp_PathSetting";
            this.button_Csharp_PathSetting.Size = new System.Drawing.Size(124, 23);
            this.button_Csharp_PathSetting.TabIndex = 7;
            this.button_Csharp_PathSetting.Text = "경로 세팅";
            this.button_Csharp_PathSetting.UseVisualStyleBackColor = true;
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
            this.textBox_Csharp_Path.Size = new System.Drawing.Size(329, 21);
            this.textBox_Csharp_Path.TabIndex = 5;
            // 
            // SpreadSheetParser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox_Console);
            this.Name = "SpreadSheetParser";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_SaveSheet;
        private System.Windows.Forms.GroupBox groupBox3;
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
    }
}

