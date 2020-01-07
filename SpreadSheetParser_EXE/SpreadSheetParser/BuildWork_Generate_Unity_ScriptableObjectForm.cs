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
    public partial class BuildWork_Generate_Unity_ScriptableObjectForm : Form
    {
        Work_Generate_Unity_ScriptableObject _pWork;

        public BuildWork_Generate_Unity_ScriptableObjectForm()
        {
            InitializeComponent();
        }

        public void DoInit(Work_Generate_Unity_ScriptableObject pWork)
        {
            _pWork = null;
            
            checkBox_OpenFolder_AfterBuild.Checked = pWork.bOpenPath_AfterBuild_CSharp;
            textBox_EditorPath.Text = pWork.strUnityEditorPath;
            textBox_ExportPath.Text = pWork.strExportPath;

            _pWork = pWork;
        }

        private void Button_OpenPath_Click(object sender, EventArgs e)
        {
            _pWork.DoOpenPath(textBox_EditorPath.Text);
        }

        private void button_SavePath_EditorClick(object sender, EventArgs e)
        {
            if (_pWork == null)
                return;

            if (_pWork.DoShowFileBrowser_And_SavePath(true, ref textBox_EditorPath, (strFileName) => strFileName.Contains("Unity.exe"), "Unity 실행프로그램이 아닙니다"))
                _pWork.strUnityEditorPath = textBox_EditorPath.Text;
        }

        private void checkBox_OpenFolder_AfterBuild_CheckedChanged(object sender, EventArgs e)
        {
            if (_pWork == null)
                return;

            _pWork.bOpenPath_AfterBuild_CSharp = checkBox_OpenFolder_AfterBuild.Checked;
        }

        private void button_SavePath_ExportPath_Click(object sender, EventArgs e)
        {
            if (_pWork == null)
                return;

            if (_pWork.DoShowFolderBrowser_And_SavePath(false, ref textBox_ExportPath))
                _pWork.strExportPath = textBox_ExportPath.Text;
        }

        private void Button_OpenPath_ExportPath_Click(object sender, EventArgs e)
        {
            _pWork.DoOpenPath(textBox_ExportPath.Text);
        }

        private void button_SaveAndClose_Click(object sender, EventArgs e)
        {
            _pWork.DoAutoSaveAsync();
            Close();
        }
    }


    [System.Serializable]
    public class Work_Generate_Unity_ScriptableObject : WorkBase
    {
        public string strExportPath;
        public string strUnityEditorPath;
        public bool bOpenPath_AfterBuild_CSharp;

        protected override void OnCreateInstance(out Type pFormType, out Type pType)
        {
            pFormType = typeof(BuildWork_Generate_Unity_ScriptableObjectForm);
            pType = GetType();
        }

        public override string GetDisplayString()
        {
            return "Generate Unity SO";
        }

        public override void DoWork(CodeFileBuilder pCodeFileBuilder, IEnumerable<SaveData_Sheet> listSheetData)
        {
            CodeTypeDeclarationCollection arrTypes = pCodeFileBuilder.GetCodeTypeDeclarationCollection();
            CodeNamespace pNameSpace = new CodeNamespace();
            pNameSpace.Imports.Add(new CodeNamespaceImport("UnityEngine"));

            foreach (CodeTypeDeclaration pType in arrTypes)
            {
                if (pType.IsClass == false)
                    continue;

                pType.AddBaseClass(typeof(UnityEngine.ScriptableObject));
                pNameSpace.Types.Clear();
                pNameSpace.Types.Add(pType);

                Uri pURI = new Uri($"{strExportPath}/{pType.Name}");
                pCodeFileBuilder.Generate_CSharpCode(pNameSpace, pURI.AbsolutePath);
            }

            pNameSpace.Types.Clear();
            foreach (CodeTypeDeclaration pType in arrTypes)
            {
                if (pType.IsClass)
                    continue;

                pNameSpace.Types.Add(pType);
            }

            pCodeFileBuilder.Generate_CSharpCode(pNameSpace, $"{strExportPath}/Others");
        }

        public override void DoWorkAfter()
        {
            System.Diagnostics.Process.Start(strUnityEditorPath, "-quit -batchmode");

            if (bOpenPath_AfterBuild_CSharp)
                DoOpenPath(strExportPath);
        }

        protected override void OnShowForm(Form pFormInstance)
        {
            BuildWork_Generate_Unity_ScriptableObjectForm pForm = (BuildWork_Generate_Unity_ScriptableObjectForm)pFormInstance;
            pForm.DoInit(this);
        }
    }

}
