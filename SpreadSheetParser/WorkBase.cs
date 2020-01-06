using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadSheetParser
{
    abstract public class WorkBase
    {
        public Type pType;
        public bool bEnable;

        [NonSerialized]
        private Type pFormType;

        protected WorkBase()
        {
            OnCreateInstance(out pFormType, out pType);
        }

        public void ShowForm()
        {
            Form pForm = (Form)Activator.CreateInstance(pFormType);
            pForm.Show();
            OnShowForm(pForm);
        }

        public bool DoShowFolderBrowser_And_SavePath(ref TextBox pTextBox_Path)
        {
            using (FolderBrowserDialog pDialog = new FolderBrowserDialog())
            {
                if (pDialog.ShowDialog() == DialogResult.OK)
                {
                    pTextBox_Path.Text = pDialog.SelectedPath;
                    return true;
                }
            }

            return false;
        }

        public void DoAutoSaveAsync()
        {
            SpreadSheetParser_MainForm.WriteConsole($"자동 저장 중.. {GetDisplayString()}");
            SaveDataManager.SaveSheet(SpreadSheetParser_MainForm.pSpreadSheet_CurrentConnected);
        }

        public void DoOpenPath(string strPath)
        {
            SpreadSheetParser_MainForm.DoOpenPath(strPath);
        }

        abstract public void DoWork(CodeFileBuilder pCodeFileBuilder);
        virtual public void DoWorkAfter() { }

        abstract protected void OnCreateInstance(out Type pFormType, out Type pType);
        abstract protected void OnShowForm(Form pFormInstance);

        abstract public string GetDisplayString();

        public override string ToString()
        {
            return GetDisplayString();
        }
    }


    // https://stackoverflow.com/questions/20995865/deserializing-json-to-abstract-class
    public class WorkJsonConverter : JsonConverter
    {
        public class BaseSpecifiedConcreteClassConverter : DefaultContractResolver
        {
            protected override JsonConverter ResolveContractConverter(Type objectType)
            {
                if (typeof(WorkBase).IsAssignableFrom(objectType) && !objectType.IsAbstract)
                    return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
                return base.ResolveContractConverter(objectType);
            }
        }

        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new BaseSpecifiedConcreteClassConverter() };

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(WorkBase));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            switch (jo["pType"].Value<string>())
            {
                case var pType when pType.Contains(nameof(Work_Generate_CSharpFile)):
                    return JsonConvert.DeserializeObject<Work_Generate_CSharpFile>(jo.ToString(), SpecifiedSubclassConversion);

                default:
                    throw new Exception();
            }
            throw new NotImplementedException();
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException(); // won't be called because CanWrite returns false
        }
    }
}
