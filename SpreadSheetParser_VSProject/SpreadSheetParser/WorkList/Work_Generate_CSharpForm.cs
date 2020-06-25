using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadSheetParser
{
    public partial class Work_Generate_CSharpForm : Form
    {
        BuildGenerateCSharpFile _pBuild;

        public Work_Generate_CSharpForm()
        {
            InitializeComponent();
        }

        public void DoInit(BuildGenerateCSharpFile pBuild)
        {
            _pBuild = null;

            checkBox_OpenFolder_AfterBuild.Checked = pBuild.bOpenPath_AfterBuild_CSharp;
            textBox_Path.Text = pBuild.strPath;
            textBox_FileName.Text = pBuild.strFileName;

            _pBuild = pBuild;
        }

        private void checkBox_OpenFolder_AfterBuild_CheckedChanged(object sender, EventArgs e)
        {
            if (_pBuild == null)
                return;

            _pBuild.bOpenPath_AfterBuild_CSharp = checkBox_OpenFolder_AfterBuild.Checked;
        }

        private void Button_OpenPath_Click(object sender, EventArgs e)
        {
            _pBuild.DoOpenFolder(textBox_Path.Text);
        }

        private void button_SavePath_Click(object sender, EventArgs e)
        {
            if (_pBuild == null)
                return;

            if (SheetParser_MainForm.DoShowFolderBrowser_And_SavePath(false, ref textBox_Path))
                _pBuild.strPath = textBox_Path.Text;
        }

        private void button_SaveAndClose_Click(object sender, EventArgs e)
        {
            _pBuild.strFileName = textBox_FileName.Text;
            _pBuild.DoAutoSaveAsync();
            Close();
        }
    }


    [Serializable]
    public class BuildGenerateCSharpFile : BuildBase
    {
        public string strPath;
        public string strFileName;
        public bool bOpenPath_AfterBuild_CSharp;

        protected override void OnCreateInstance(out Type pFormType, out Type pType)
        {
            pFormType = typeof(Work_Generate_CSharpForm);
            pType = GetType();
        }

        public override string GetDisplayString()
        {
            return "Generate CSharp File";
        }

        public override Task DoWork(CodeFileBuilder pCodeFileBuilder, SheetData[] arrSheetData, Action<string> OnPrintWorkProcess)
        {
            return Task.Run(() => pCodeFileBuilder.Generate_CSharpCode($"{GetRelative_To_AbsolutePath(strPath)}/{strFileName}"));
        }

        public override void DoWorkAfter()
        {
            if (bOpenPath_AfterBuild_CSharp)
                DoOpenFolder(strPath);
        }

        protected override void OnShowForm(Form pFormInstance)
        {
            Work_Generate_CSharpForm pForm = (Work_Generate_CSharpForm)pFormInstance;
            pForm.DoInit(this);
        }
    }

}
