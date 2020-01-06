namespace SpreadSheetParser
{
    partial class BuildWork_Generate_Unity_ScriptableObject
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
            this.textBox_EditorPath = new System.Windows.Forms.TextBox();
            this.Button_OpenPath = new System.Windows.Forms.Button();
            this.button_SavePath = new System.Windows.Forms.Button();
            this.button_SaveAndClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_FileName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox_EditorPath
            // 
            this.textBox_EditorPath.Location = new System.Drawing.Point(14, 68);
            this.textBox_EditorPath.Name = "textBox_EditorPath";
            this.textBox_EditorPath.ReadOnly = true;
            this.textBox_EditorPath.Size = new System.Drawing.Size(339, 21);
            this.textBox_EditorPath.TabIndex = 0;
            // 
            // Button_OpenPath
            // 
            this.Button_OpenPath.Location = new System.Drawing.Point(278, 95);
            this.Button_OpenPath.Name = "Button_OpenPath";
            this.Button_OpenPath.Size = new System.Drawing.Size(75, 23);
            this.Button_OpenPath.TabIndex = 2;
            this.Button_OpenPath.Text = "경로 열기";
            this.Button_OpenPath.UseVisualStyleBackColor = true;
            this.Button_OpenPath.Click += new System.EventHandler(this.Button_OpenPath_Click);
            // 
            // button_SavePath
            // 
            this.button_SavePath.Location = new System.Drawing.Point(180, 95);
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
            this.label1.Location = new System.Drawing.Point(12, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "UnityEditor Path";
            // 
            // textBox_FileName
            // 
            this.textBox_FileName.Location = new System.Drawing.Point(14, 146);
            this.textBox_FileName.Name = "textBox_FileName";
            this.textBox_FileName.Size = new System.Drawing.Size(339, 21);
            this.textBox_FileName.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "커맨드라인";
            // 
            // BuildWork_Generate_Unity_ScriptableObject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 261);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_FileName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_SaveAndClose);
            this.Controls.Add(this.button_SavePath);
            this.Controls.Add(this.Button_OpenPath);
            this.Controls.Add(this.textBox_EditorPath);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BuildWork_Generate_Unity_ScriptableObject";
            this.Text = "BuildWork_Generate_Unity_ScriptableObject";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_EditorPath;
        private System.Windows.Forms.Button Button_OpenPath;
        private System.Windows.Forms.Button button_SavePath;
        private System.Windows.Forms.Button button_SaveAndClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_FileName;
        private System.Windows.Forms.Label label2;
    }
}