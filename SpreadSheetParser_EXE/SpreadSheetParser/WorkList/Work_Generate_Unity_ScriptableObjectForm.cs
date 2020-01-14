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
    public partial class Work_Generate_Unity_ScriptableObjectForm : Form
    {
        Work_Generate_Unity_ScriptableObject _pWork;

        public Work_Generate_Unity_ScriptableObjectForm()
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
        public string strUnityProjectPath;
        public string strUnityEditorPath;
        public bool bOpenPath_AfterBuild_CSharp;

        protected override void OnCreateInstance(out Type pFormType, out Type pType)
        {
            pFormType = typeof(Work_Generate_Unity_ScriptableObjectForm);
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

                SaveData_Sheet pSaveData = listSheetData.Where((pSaveDataSheet) => pSaveDataSheet.strFileName == pType.Name).FirstOrDefault();
                if (pSaveData == null || pSaveData.bIsPureClass)
                    continue;

                SpreadSheetParser_MainForm.WriteConsole($"UnitySO - Working SO {pType.Name}");

                Create_SO(pCodeFileBuilder, pNameSpace, pType, pSaveData);
                Create_SOContainer(pCodeFileBuilder, pNameSpace, pType, pSaveData);
            }

            pNameSpace.Types.Clear();
            foreach (CodeTypeDeclaration pType in arrTypes)
            {
                if (pType.IsClass)
                {
                    SaveData_Sheet pSaveData = listSheetData.Where((pSaveDataSheet) => pSaveDataSheet.strFileName == pType.Name).FirstOrDefault();
                    if (pSaveData == null || pSaveData.bIsPureClass == false)
                        continue;
                }

                SpreadSheetParser_MainForm.WriteConsole($"UnitySO - Working Others {pType.Name}");
                pNameSpace.Types.Add(pType);
            }

            if(pNameSpace.Types.Count != 0)
                pCodeFileBuilder.Generate_CSharpCode(pNameSpace, $"{GetRelative_To_AbsolutePath(strExportPath)}/Others");
        }

        private void Create_SO(CodeFileBuilder pCodeFileBuilder, CodeNamespace pNameSpace, CodeTypeDeclaration pType, SaveData_Sheet pSaveData)
        {
            pType.AddBaseClass(typeof(UnityEngine.ScriptableObject));
            pNameSpace.Types.Clear();
            pNameSpace.Types.Add(pType);

            var listVirtualFieldOption = pSaveData.listFieldData.Where(pExportOption => pExportOption.bIsVirtualField);
            foreach (var pVirtualField in listVirtualFieldOption)
                pType.AddField(pVirtualField);

            pCodeFileBuilder.Generate_CSharpCode(pNameSpace, $"{GetRelative_To_AbsolutePath(strExportPath)}/{pType.Name}");
        }

        private void Create_SOContainer(CodeFileBuilder pCodeFileBuilder, CodeNamespace pNameSpace, CodeTypeDeclaration pType, SaveData_Sheet pSaveData)
        {
            CodeTypeDeclaration pContainerType = new CodeTypeDeclaration(pType.Name + "_Container");
            pContainerType.AddBaseClass(typeof(UnityEngine.ScriptableObject));
            pNameSpace.Imports.Clear();
            pNameSpace.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            pNameSpace.Imports.Add(new CodeNamespaceImport("UnityEngine"));
            pNameSpace.Types.Clear();
            pNameSpace.Types.Add(pContainerType);

            pContainerType.AddField(new FieldData("listData", $"List<{pType.Name}>"));

            pCodeFileBuilder.Generate_CSharpCode(pNameSpace, $"{GetRelative_To_AbsolutePath(strExportPath)}/{pContainerType.Name}");
        }

        public override void DoWorkAfter()
        {
            const string const_BuildMethodeName = "UnitySO_Generator.DoBuild";

            System.Diagnostics.Process.Start(strUnityEditorPath, $"-quit -batchmode -executeMethod {const_BuildMethodeName}");

            if (bOpenPath_AfterBuild_CSharp)
                DoOpenPath(strExportPath);
        }

        protected override void OnShowForm(Form pFormInstance)
        {
            Work_Generate_Unity_ScriptableObjectForm pForm = (Work_Generate_Unity_ScriptableObjectForm)pFormInstance;
            pForm.DoInit(this);
        }
    }

}
