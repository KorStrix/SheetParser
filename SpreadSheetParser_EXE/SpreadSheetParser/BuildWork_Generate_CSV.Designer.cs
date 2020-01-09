namespace SpreadSheetParser
{
    partial class BuildWork_Generate_CSV
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox_Path = new System.Windows.Forms.TextBox();
            this.checkBox_OpenFolder_AfterBuild = new System.Windows.Forms.CheckBox();
            this.Button_OpenPath = new System.Windows.Forms.Button();
            this.button_SavePath = new System.Windows.Forms.Button();
            this.button_SaveAndClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox_Path
            // 
            this.textBox_Path.Location = new System.Drawing.Point(12, 141);
            this.textBox_Path.Name = "textBox_Path";
            this.textBox_Path.ReadOnly = true;
            this.textBox_Path.Size = new System.Drawing.Size(339, 21);
            this.textBox_Path.TabIndex = 0;
            // 
            // checkBox_OpenFolder_AfterBuild
            // 
            this.checkBox_OpenFolder_AfterBuild.AutoSize = true;
            this.checkBox_OpenFolder_AfterBuild.Location = new System.Drawing.Point(231, 122);
            this.checkBox_OpenFolder_AfterBuild.Name = "checkBox_OpenFolder_AfterBuild";
            this.checkBox_OpenFolder_AfterBuild.Size = new System.Drawing.Size(120, 16);
            this.checkBox_OpenFolder_AfterBuild.TabIndex = 1;
            this.checkBox_OpenFolder_AfterBuild.Text = "빌드 후 경로 열기";
            this.checkBox_OpenFolder_AfterBuild.UseVisualStyleBackColor = true;
            this.checkBox_OpenFolder_AfterBuild.CheckedChanged += new System.EventHandler(this.checkBox_OpenFolder_AfterBuild_CheckedChanged);
            // 
            // Button_OpenPath
            // 
            this.Button_OpenPath.Location = new System.Drawing.Point(276, 168);
            this.Button_OpenPath.Name = "Button_OpenPath";
            this.Button_OpenPath.Size = new System.Drawing.Size(75, 23);
            this.Button_OpenPath.TabIndex = 2;
            this.Button_OpenPath.Text = "경로 열기";
            this.Button_OpenPath.UseVisualStyleBackColor = true;
            this.Button_OpenPath.Click += new System.EventHandler(this.Button_OpenPath_Click);
            // 
            // button_SavePath
            // 
            this.button_SavePath.Location = new System.Drawing.Point(178, 168);
            this.button_SavePath.Name = "button_SavePath";
            this.button_SavePath.Size = new System.Drawing.Size(75, 23);
            this.button_SavePath.TabIndex = 3;
            this.button_SavePath.Text = "경로 세팅";
            this.button_SavePath.UseVisualStyleBackColor = true;
            this.button_SavePath.Click += new System.EventHandler(this.button_SavePath_Click);
            // 
            // button_SaveAndClose
            // 
            this.button_SaveAndClose.Location = new System.Drawing.Point(255, 12);
            this.button_SaveAndClose.Name = "button_SaveAndClose";
            this.button_SaveAndClose.Size = new System.Drawing.Size(117, 39);
            this.button_SaveAndClose.TabIndex = 4;
            this.button_SaveAndClose.Text = "로직 저장 후 닫기";
            this.button_SaveAndClose.UseVisualStyleBackColor = true;
            this.button_SaveAndClose.Click += new System.EventHandler(this.button_SaveAndClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 126);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "파일 출력 경로";
            // 
            // BuildWork_Generate_CSV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 261);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_SaveAndClose);
            this.Controls.Add(this.button_SavePath);
            this.Controls.Add(this.Button_OpenPath);
            this.Controls.Add(this.checkBox_OpenFolder_AfterBuild);
            this.Controls.Add(this.textBox_Path);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BuildWork_Generate_CSV";
            this.Text = "BuildWork_Generate_CSV";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_Path;
        private System.Windows.Forms.CheckBox checkBox_OpenFolder_AfterBuild;
        private System.Windows.Forms.Button Button_OpenPath;
        private System.Windows.Forms.Button button_SavePath;
        private System.Windows.Forms.Button button_SaveAndClose;
        private System.Windows.Forms.Label label1;
    }
}