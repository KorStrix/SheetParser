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
            this.textBox_URL = new System.Windows.Forms.TextBox();
            this.button_Connect = new System.Windows.Forms.Button();
            this.checkedListBox_TableList = new System.Windows.Forms.CheckedListBox();
            this.button_StartParsing = new System.Windows.Forms.Button();
            this.textBox_Console = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox_URL
            // 
            this.textBox_URL.Location = new System.Drawing.Point(42, 51);
            this.textBox_URL.Name = "textBox_URL";
            this.textBox_URL.Size = new System.Drawing.Size(100, 21);
            this.textBox_URL.TabIndex = 0;
            // 
            // button_Connect
            // 
            this.button_Connect.Location = new System.Drawing.Point(226, 51);
            this.button_Connect.Name = "button_Connect";
            this.button_Connect.Size = new System.Drawing.Size(75, 23);
            this.button_Connect.TabIndex = 1;
            this.button_Connect.Text = "button1";
            this.button_Connect.UseVisualStyleBackColor = true;
            this.button_Connect.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkedListBox_TableList
            // 
            this.checkedListBox_TableList.FormattingEnabled = true;
            this.checkedListBox_TableList.Location = new System.Drawing.Point(42, 118);
            this.checkedListBox_TableList.Name = "checkedListBox_TableList";
            this.checkedListBox_TableList.Size = new System.Drawing.Size(120, 84);
            this.checkedListBox_TableList.TabIndex = 2;
            // 
            // button_StartParsing
            // 
            this.button_StartParsing.Location = new System.Drawing.Point(226, 149);
            this.button_StartParsing.Name = "button_StartParsing";
            this.button_StartParsing.Size = new System.Drawing.Size(75, 23);
            this.button_StartParsing.TabIndex = 3;
            this.button_StartParsing.Text = "button2";
            this.button_StartParsing.UseVisualStyleBackColor = true;
            this.button_StartParsing.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox_Console
            // 
            this.textBox_Console.Location = new System.Drawing.Point(538, 220);
            this.textBox_Console.Name = "textBox_Console";
            this.textBox_Console.Size = new System.Drawing.Size(100, 21);
            this.textBox_Console.TabIndex = 4;
            // 
            // SpreadSheetParser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBox_Console);
            this.Controls.Add(this.button_StartParsing);
            this.Controls.Add(this.checkedListBox_TableList);
            this.Controls.Add(this.button_Connect);
            this.Controls.Add(this.textBox_URL);
            this.Name = "SpreadSheetParser";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_URL;
        private System.Windows.Forms.Button button_Connect;
        private System.Windows.Forms.CheckedListBox checkedListBox_TableList;
        private System.Windows.Forms.Button button_StartParsing;
        private System.Windows.Forms.TextBox textBox_Console;
    }
}

