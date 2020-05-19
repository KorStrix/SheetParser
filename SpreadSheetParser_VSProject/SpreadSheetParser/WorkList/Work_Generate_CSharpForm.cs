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
        Work_Generate_CSharpFile _pWork;

        public Work_Generate_CSharpForm()
        {
            InitializeComponent();
        }

        public void DoInit(Work_Generate_CSharpFile pWork)
        {
            _pWork = null;

            checkBox_OpenFolder_AfterBuild.Checked = pWork.bOpenPath_AfterBuild_CSharp;
            textBox_Path.Text = pWork.strPath;
            textBox_FileName.Text = pWork.strFileName;

            _pWork = pWork;
        }

        private void checkBox_OpenFolder_AfterBuild_CheckedChanged(object sender, EventArgs e)
        {
            if (_pWork == null)
                return;

            _pWork.bOpenPath_AfterBuild_CSharp = checkBox_OpenFolder_AfterBuild.Checked;
        }

        private void Button_OpenPath_Click(object sender, EventArgs e)
        {
            _pWork.DoOpenFolder(textBox_Path.Text);
        }

        private void button_SavePath_Click(object sender, EventArgs e)
        {
            if (_pWork == null)
                return;

            if (SpreadSheetParser_MainForm.DoShowFolderBrowser_And_SavePath(false, ref textBox_Path))
                _pWork.strPath = textBox_Path.Text;
        }

        private void button_SaveAndClose_Click(object sender, EventArgs e)
        {
            _pWork.strFileName = textBox_FileName.Text;
            _pWork.DoAutoSaveAsync();
            Close();
        }
    }


    [Serializable]
    public class Work_Generate_CSharpFile : WorkBase
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

        public override Task DoWork(CodeFileBuilder pCodeFileBuilder, ISheetConnector pConnector, TypeData[] arrSheetData, Action<string> OnPrintWorkProcess)
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
