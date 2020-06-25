using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadSheetParser
{
    public partial class Work_Generate_CSVForm : Form
    {
        BuildGenerateCsv _pBuild;

        public Work_Generate_CSVForm()
        {
            InitializeComponent();
        }

        public void DoInit(BuildGenerateCsv pBuild)
        {
            _pBuild = null;

            checkBox_OpenFolder_AfterBuild.Checked = pBuild.bOpenPath_AfterBuild_CSharp;
            textBox_Path.Text = pBuild.strExportPath;

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
                _pBuild.strExportPath = textBox_Path.Text;
        }

        private void button_SaveAndClose_Click(object sender, EventArgs e)
        {
            _pBuild.DoAutoSaveAsync();
            Close();
        }
    }


    [Serializable]
    public class BuildGenerateCsv : BuildBase
    {
        public string strExportPath;
        public bool bOpenPath_AfterBuild_CSharp;

        protected override void OnCreateInstance(out Type pFormType, out Type pType)
        {
            pFormType = typeof(Work_Generate_CSVForm);
            pType = GetType();
        }

        public override string GetDisplayString()
        {
            return "Generate CSV";
        }


        public override Task DoWork(CodeFileBuilder pCodeFileBuilder, SheetData[] arrSheetData, Action<string> OnPrintWorkProcess)
        {
            List<Task> listTask = new List<Task>();
            foreach (SheetData pSheet in arrSheetData)
            {
                listTask.Add(Task.Run(() =>
                {
                    StringBuilder pStrBuilder = new StringBuilder();
                    StreamWriter pFileWriter = new StreamWriter($"{GetRelative_To_AbsolutePath(strExportPath)}/{pSheet.strFileName.Trim()}.csv");

                    int iLastRowIndex = -1;
                    return pSheet.ParsingSheet_UseTask(
                        (listRow, strText, iRowIndex, iColumnIndex) =>
                        {
                            if (iLastRowIndex == -1)
                                iLastRowIndex = iRowIndex;

                            if (strText.Contains(':'))
                            {
                                string[] arrText = strText.Split(':');
                                strText = arrText[0];
                            }

                            if (iLastRowIndex != iRowIndex)
                            {
                                iLastRowIndex = iRowIndex;

                                pStrBuilder.Remove(pStrBuilder.Length - 1, 1);
                                pFileWriter.WriteLine(pStrBuilder.ToString());
                                pFileWriter.Flush();

                                pStrBuilder.Clear();
                            }

                            pStrBuilder.Append(strText);
                            pStrBuilder.Append(",");
                        }).ContinueWith((p) =>
                    {
                        pStrBuilder.Remove(pStrBuilder.Length - 1, 1);
                        pFileWriter.WriteLine(pStrBuilder.ToString());
                        pFileWriter.Flush();

                        pFileWriter.Close();
                        pFileWriter.Dispose();
                    });
                }));
            }
            // Task.WaitAll(listTask.ToArray());

            return Task.WhenAll(listTask);
        }

        public override void DoWorkAfter()
        {
            if (bOpenPath_AfterBuild_CSharp)
                DoOpenFolder(strExportPath);
        }

        protected override void OnShowForm(Form pFormInstance)
        {
            Work_Generate_CSVForm pForm = (Work_Generate_CSVForm)pFormInstance;
            pForm.DoInit(this);
        }
    }

}
