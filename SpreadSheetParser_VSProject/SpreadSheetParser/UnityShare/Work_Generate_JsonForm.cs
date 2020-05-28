using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


#if !UNITY_EDITOR
using System.Windows.Forms;
#endif


namespace SpreadSheetParser
{
#if !UNITY_EDITOR
    public partial class Work_Generate_JsonForm : Form
    {
        BuildGenerateJson _pBuild;

        public Work_Generate_JsonForm()
        {
            InitializeComponent();
        }

        public void DoInit(BuildGenerateJson pBuild)
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
#endif

    [Serializable]
    public class BuildGenerateJson : BuildBase
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

        public override Task DoWork(CodeFileBuilder pCodeFileBuilder, TypeData[] arrSheetData, Action<string> OnPrintWorkProcess)
        {
            TypeDataList pTypeDataList = JsonSaveManager.LoadData<TypeDataList>($"{GetRelative_To_AbsolutePath(strExportPath)}/{nameof(TypeDataList)}.json", OnPrintWorkProcess);
            //if (pTypeDataList != null)
            //    pTypeDataList.listTypeData.ForEach(p => p.bEnable = false);

            //if (pTypeDataList == null)
            //    pTypeDataList = new TypeDataList(pSourceConnector.strSheetSourceID);

            List<Task> listTask = new List<Task>();
            foreach (var pSheet in arrSheetData)
            {
                if (pSheet.eType == ESheetType.Enum)
                    continue;

                listTask.Add(ProcessJson(OnPrintWorkProcess, pSheet, pTypeDataList));
            }

            return Task.WhenAll(listTask).ContinueWith((p) =>
            {
                pTypeDataList.listTypeData.Sort((x, y) => x.iOrder.CompareTo(y.iOrder));
                JsonSaveManager.SaveData(pTypeDataList, $"{GetRelative_To_AbsolutePath(strExportPath)}/{nameof(TypeDataList)}.json");
            });
        }

        private Task ProcessJson(Action<string> OnPrintWorkProcess, TypeData pSheet,
            TypeDataList pTypeDataList)
        {
            JObject pJson_Instance = new JObject();
            JArray pArray = new JArray();

            Dictionary<string, FieldTypeData> mapFieldData =
                pSheet.listFieldData.Where(p => p.bIsVirtualField == false).ToDictionary(p => p.strFieldName);
            Dictionary<int, string> mapMemberName = new Dictionary<int, string>();
            // Dictionary<int, string> mapMemberType = new Dictionary<int, string>();
            int iColumnStartIndex = -1;

            return pSheet.ParsingSheet_UseTask(
                ((listRow, strText, iRowIndex, iColumnIndex) =>
                {
                    if (strText.Contains(':')) // 변수 타입 파싱
                    {
                        if (mapMemberName.ContainsKey(iColumnIndex))
                            return;

                        string[] arrText = strText.Split(':');
                        mapMemberName.Add(iColumnIndex, arrText[0]);
                        // mapMemberType.Add(iColumnIndex, arrText[1]);

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
                        if (mapMemberName.ContainsKey(i) == false)
                            continue;

                        if (mapFieldData.TryGetValue(mapMemberName[i], out var pFieldTypeData) == false)
                        {
                            OnPrintWorkProcess?.Invoke(
                                $"{pSheet.strSheetID} - mapFieldData.ContainsKey({mapMemberName[i]}) Fail");
                            continue;
                        }

                        string strFieldName = pFieldTypeData.strFieldName;
                        string strValue = (string) listRow[i];
                        pObject.Add(strFieldName, strValue);
                    }

                    pArray.Add(pObject);
                })).ContinueWith((pTask) =>
                {
                    pJson_Instance.Add("array", pArray);

                    string strFileName = $"{pSheet.strFileName}.json";
                    JsonSaveManager.SaveData(pJson_Instance, $"{GetRelative_To_AbsolutePath(strExportPath)}/{strFileName}");

                    var pAlreadyExist = pTypeDataList.listTypeData.FirstOrDefault(p => p.strSheetID == pSheet.strSheetID);
                    if (pAlreadyExist != null)
                        pTypeDataList.listTypeData.Remove(pAlreadyExist);

                    pTypeDataList.listTypeData.Add(pSheet);
                });
        }

#if !UNITY_EDITOR
        public override void DoWorkAfter()
        {
            if (bOpenPath_AfterBuild_CSharp)
                DoOpenFolder(strExportPath);
        }

        protected override void OnShowForm(Form pFormInstance)
        {
            Work_Generate_JsonForm pForm = (Work_Generate_JsonForm)pFormInstance;
            pForm.DoInit(this);
        }
#endif
    }

}
