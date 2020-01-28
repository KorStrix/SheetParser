namespace SpreadSheetParser
{
    partial class Work_Generate_Unity_ScriptableObjectForm
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
            this.button_SaveAndClose = new System.Windows.Forms.Button();
            this.checkBox_OpenFolder_AfterBuild = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button_SavePath_ExportPath = new System.Windows.Forms.Button();
            this.Button_OpenPath_ExportPath = new System.Windows.Forms.Button();
            this.textBox_ExportPath = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
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
            // checkBox_OpenFolder_AfterBuild
            // 
            this.checkBox_OpenFolder_AfterBuild.AutoSize = true;
            this.checkBox_OpenFolder_AfterBuild.Location = new System.Drawing.Point(231, 77);
            this.checkBox_OpenFolder_AfterBuild.Name = "checkBox_OpenFolder_AfterBuild";
            this.checkBox_OpenFolder_AfterBuild.Size = new System.Drawing.Size(120, 16);
            this.checkBox_OpenFolder_AfterBuild.TabIndex = 8;
            this.checkBox_OpenFolder_AfterBuild.Text = "빌드 후 경로 열기";
            this.checkBox_OpenFolder_AfterBuild.UseVisualStyleBackColor = true;
            this.checkBox_OpenFolder_AfterBuild.CheckedChanged += new System.EventHandler(this.checkBox_OpenFolder_AfterBuild_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 12);
            this.label3.TabIndex = 12;
            this.label3.Text = "파일 출력 경로";
            // 
            // button_SavePath_ExportPath
            // 
            this.button_SavePath_ExportPath.Location = new System.Drawing.Point(178, 126);
            this.button_SavePath_ExportPath.Name = "button_SavePath_ExportPath";
            this.button_SavePath_ExportPath.Size = new System.Drawing.Size(75, 23);
            this.button_SavePath_ExportPath.TabIndex = 11;
            this.button_SavePath_ExportPath.Text = "경로 세팅";
            this.button_SavePath_ExportPath.UseVisualStyleBackColor = true;
            this.button_SavePath_ExportPath.Click += new System.EventHandler(this.button_SavePath_ExportPath_Click);
            // 
            // Button_OpenPath_ExportPath
            // 
            this.Button_OpenPath_ExportPath.Location = new System.Drawing.Point(276, 126);
            this.Button_OpenPath_ExportPath.Name = "Button_OpenPath_ExportPath";
            this.Button_OpenPath_ExportPath.Size = new System.Drawing.Size(75, 23);
            this.Button_OpenPath_ExportPath.TabIndex = 10;
            this.Button_OpenPath_ExportPath.Text = "경로 열기";
            this.Button_OpenPath_ExportPath.UseVisualStyleBackColor = true;
            this.Button_OpenPath_ExportPath.Click += new System.EventHandler(this.Button_OpenPath_ExportPath_Click);
            // 
            // textBox_ExportPath
            // 
            this.textBox_ExportPath.Location = new System.Drawing.Point(12, 99);
            this.textBox_ExportPath.Name = "textBox_ExportPath";
            this.textBox_ExportPath.ReadOnly = true;
            this.textBox_ExportPath.Size = new System.Drawing.Size(339, 21);
            this.textBox_ExportPath.TabIndex = 9;
            // 
            // Work_Generate_Unity_ScriptableObjectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 261);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_SavePath_ExportPath);
            this.Controls.Add(this.Button_OpenPath_ExportPath);
            this.Controls.Add(this.textBox_ExportPath);
            this.Controls.Add(this.checkBox_OpenFolder_AfterBuild);
            this.Controls.Add(this.button_SaveAndClose);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Work_Generate_Unity_ScriptableObjectForm";
            this.Text = "BuildWork_Generate_Unity_ScriptableObject";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button_SaveAndClose;
        private System.Windows.Forms.CheckBox checkBox_OpenFolder_AfterBuild;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_SavePath_ExportPath;
        private System.Windows.Forms.Button Button_OpenPath_ExportPath;
        private System.Windows.Forms.TextBox textBox_ExportPath;
    }
}