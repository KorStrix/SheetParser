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
        Work_Generate_CSV _pWork;

        public Work_Generate_CSVForm()
        {
            InitializeComponent();
        }

        public void DoInit(Work_Generate_CSV pWork)
        {
            _pWork = null;

            checkBox_OpenFolder_AfterBuild.Checked = pWork.bOpenPath_AfterBuild_CSharp;
            textBox_Path.Text = pWork.strExportPath;

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

            if (_pWork.DoShowFolderBrowser_And_SavePath(false, ref textBox_Path))
                _pWork.strExportPath = textBox_Path.Text;
        }

        private void button_SaveAndClose_Click(object sender, EventArgs e)
        {
            _pWork.DoAutoSaveAsync();
            Close();
        }
    }


    [System.Serializable]
    public class Work_Generate_CSV : WorkBase
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

        public override void DoWork(CodeFileBuilder pCodeFileBuilder, IEnumerable<SaveData_Sheet> listSheetData)
        {
            StringBuilder pStrBuilder = new StringBuilder();

            foreach (var pSheet in listSheetData)
            { 
                pStrBuilder.Clear();

                StreamWriter pFileWriter = new StreamWriter($"{GetRelative_To_AbsolutePath(strExportPath)}/{pSheet.strFileName.Trim()}.csv");

                int iLastRowIndex = -1;
                pSheet.ParsingSheet(
                    (IList<object> listRow, string strText, int iRowIndex, int iColumnIndex) =>
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
                    });

                pStrBuilder.Remove(pStrBuilder.Length - 1, 1);
                pFileWriter.WriteLine(pStrBuilder.ToString());
                pFileWriter.Flush();

                pFileWriter.Close();
                pFileWriter.Dispose();
            }
        }

        public override void DoWorkAfter()
        {
            if (bOpenPath_AfterBuild_CSharp)
                DoOpenPath(strExportPath);
        }

        protected override void OnShowForm(Form pFormInstance)
        {
            Work_Generate_CSVForm pForm = (Work_Generate_CSVForm)pFormInstance;
            pForm.DoInit(this);
        }
    }

}
