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
    public partial class BuildWork_Generate_CSharpForm : Form
    {
        Work_Generate_CSharpFile _pWork;

        public BuildWork_Generate_CSharpForm()
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
            _pWork.DoOpenPath(textBox_Path.Text);
        }

        private void button_SavePath_Click(object sender, EventArgs e)
        {
            if (_pWork == null)
                return;

            if (_pWork.DoShowFolderBrowser_And_SavePath(ref textBox_Path))
                _pWork.strPath = textBox_Path.Text;
        }

        private void button_SaveAndClose_Click(object sender, EventArgs e)
        {
            _pWork.strFileName = textBox_FileName.Text;
            _pWork.DoAutoSaveAsync();
            Close();
        }
    }


    [System.Serializable]
    public class Work_Generate_CSharpFile : WorkBase
    {
        public string strPath;
        public string strFileName;
        public bool bOpenPath_AfterBuild_CSharp;

        protected override void OnCreateInstance(out Type pFormType, out Type pType)
        {
            pFormType = typeof(BuildWork_Generate_CSharpForm);
            pType = GetType();
        }

        public override string GetDisplayString()
        {
            return "Generate CSharp File";
        }

        public override void DoWork(CodeFileBuilder pCodeFileBuilder)
        {
            pCodeFileBuilder.GenerateCSharpCode($"{strPath}/{strFileName}.cs");
        }

        public override void DoWorkAfter()
        {
            if (bOpenPath_AfterBuild_CSharp)
                DoOpenPath(strPath);
        }

        protected override void OnShowForm(Form pFormInstance)
        {
            BuildWork_Generate_CSharpForm pForm = (BuildWork_Generate_CSharpForm)pFormInstance;
            pForm.DoInit(this);
        }
    }

}
