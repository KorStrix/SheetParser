using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#if !UNITY_EDITOR
using System.Windows.Forms;
#endif

namespace SpreadSheetParser
{
#if !UNITY_EDITOR
    public partial class Work_Generate_JsonForm : Form
    {
        Work_Generate_Json _pWork;

        public Work_Generate_JsonForm()
        {
            InitializeComponent();
        }

        public void DoInit(Work_Generate_Json pWork)
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
#endif

    [System.Serializable]
    public class Work_Generate_Json : WorkBase
    {
        public string strExportPath;
        public bool bOpenPath_AfterBuild_CSharp;

#if !UNITY_EDITOR
        protected override void OnCreateInstance(out Type pFormType, out Type pType)
        {
            pFormType = typeof(Work_Generate_JsonForm);
            pType = GetType();
        }
#endif

        public override string GetDisplayString()
        {
            return "Generate Json";
        }

        public override void DoWork(CodeFileBuilder pCodeFileBuilder, SpreadSheetConnector pConnector, IEnumerable<TypeData> listSheetData, System.Action<string> OnPrintWorkProcess)
        {
            TypeDataList pTypeDataList = new TypeDataList();
            foreach (var pSheet in listSheetData)
            {
                if (pSheet.eType == ESheetType.Enum)
                    return;

                JObject pJson_Instance = new JObject();
                JArray pArray = new JArray();

                Dictionary<string, FieldTypeData> mapFieldData = pSheet.listFieldData.Where(p => p.bIsVirtualField == false).ToDictionary(p => p.strFieldName);
                Dictionary<int, string> mapMemberName = new Dictionary<int, string>();
                Dictionary<int, string> mapMemberType = new Dictionary<int, string>();
                int iColumnStartIndex = -1;

                pSheet.ParsingSheet(pConnector,
                ((IList<object> listRow, string strText, int iRowIndex, int iColumnIndex) =>
                {
                    if (strText.Contains(':')) // 변수 타입 파싱
                    {
                        if (mapMemberName.ContainsKey(iColumnIndex))
                            return;

                        string[] arrText = strText.Split(':');
                        mapMemberName.Add(iColumnIndex, arrText[0]);
                        mapMemberType.Add(iColumnIndex, arrText[1]);

                        if (iColumnStartIndex == -1)
                            iColumnStartIndex = iColumnIndex;

                        return;
                    }

                    if (iColumnIndex != iColumnStartIndex)
                        return;

                    JObject pObject = new JObject();

                    // 실제 변수값
                    for (int i = iColumnIndex; i < listRow.Count; i++)
                    {
                        if(mapMemberName.ContainsKey(i))
                        {
                            FieldTypeData pFieldTypeData;
                            if(mapFieldData.TryGetValue(mapMemberName[i], out pFieldTypeData) == false)
                            {
                                OnPrintWorkProcess?.Invoke($"{pSheet.strSheetName} - mapFieldData.ContainsKey({mapMemberName[i]}) Fail");
                                continue;
                            }

                            string strFieldName = pFieldTypeData.strFieldName;
                            string strValue = (string)listRow[i];
                            pObject.Add(strFieldName, strValue);
                        }
                    }

                    pArray.Add(pObject);
                }));

                if (pArray.Count == 0)
                    continue;

                pJson_Instance.Add("array", pArray);

                string strFileName = $"{pSheet.strFileName}.json";
                JsonSaveManager.SaveData(pJson_Instance, $"{GetRelative_To_AbsolutePath(strExportPath)}/{strFileName}");

                pTypeDataList.listTypeData.Add(pSheet);
            }

            JsonSaveManager.SaveData(pTypeDataList, $"{GetRelative_To_AbsolutePath(strExportPath)}/{nameof(TypeDataList)}.json");
        }

#if !UNITY_EDITOR
        public override void DoWorkAfter()
        {
            if (bOpenPath_AfterBuild_CSharp)
                DoOpenPath(strExportPath);
        }

        protected override void OnShowForm(Form pFormInstance)
        {
            Work_Generate_JsonForm pForm = (Work_Generate_JsonForm)pFormInstance;
            pForm.DoInit(this);
        }
#endif
    }

}
