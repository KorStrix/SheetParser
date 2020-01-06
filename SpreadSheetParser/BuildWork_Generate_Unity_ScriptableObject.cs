using System;
using System.CodeDom;
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
    public partial class BuildWork_Generate_Unity_ScriptableObject : Form
    {
        Work_Generate_Unity_ScriptableObject _pWork;

        public BuildWork_Generate_Unity_ScriptableObject()
        {
            InitializeComponent();
        }

        public void DoInit(Work_Generate_Unity_ScriptableObject pWork)
        {
            _pWork = null;

            textBox_EditorPath.Text = pWork.strPath;
            textBox_FileName.Text = pWork.strFileName;

            _pWork = pWork;
        }

        private void Button_OpenPath_Click(object sender, EventArgs e)
        {
            _pWork.DoOpenPath(textBox_EditorPath.Text);
        }

        private void button_SavePath_Click(object sender, EventArgs e)
        {
            if (_pWork == null)
                return;

            if (_pWork.DoShowFolderBrowser_And_SavePath(ref textBox_EditorPath))
                _pWork.strPath = textBox_EditorPath.Text;
        }

        private void button_SaveAndClose_Click(object sender, EventArgs e)
        {
            _pWork.strFileName = textBox_FileName.Text;
            _pWork.DoAutoSaveAsync();
            Close();
        }
    }


    [System.Serializable]
    public class Work_Generate_Unity_ScriptableObject : WorkBase
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
            return "Generate Unity SO";
        }

        public override void DoWork(CodeFileBuilder pCodeFileBuilder)
        {
            CodeTypeDeclarationCollection arrTypes = pCodeFileBuilder.pNameSpace.Types;
            foreach (CodeTypeDeclaration pType in arrTypes)
            {
                if (pType.IsClass == false)
                    continue;

                pType.AddBaseClass(typeof(UnityEngine.ScriptableObject));
            }

            pCodeFileBuilder.Generate_CSharpCode($"{strPath}/{strFileName}");
        }

        public override void DoWorkAfter()
        {
            if (bOpenPath_AfterBuild_CSharp)
                DoOpenPath(strPath);
        }

        protected override void OnShowForm(Form pFormInstance)
        {
            BuildWork_Generate_Unity_ScriptableObject pForm = (BuildWork_Generate_Unity_ScriptableObject)pFormInstance;
            pForm.DoInit(this);
        }
    }

}
