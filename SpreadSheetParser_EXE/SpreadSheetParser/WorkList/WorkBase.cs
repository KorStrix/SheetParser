using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadSheetParser
{
    abstract public class WorkBase
    {
        public int iWorkOrder;
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

        public WorkBase CopyInstance()
        {
            return (WorkBase)this.MemberwiseClone();
        }

        public bool DoShowFileBrowser_And_SavePath(bool bIsAbsolutePath, ref TextBox pTextBox_Path, System.Func<string, bool> OnCheck_IsCorrect, string strOnErrorMsg)
        {
            if (OnCheck_IsCorrect == null)
                OnCheck_IsCorrect = OnCheck_IsCorrect_Default;

            using (OpenFileDialog pDialog = new OpenFileDialog())
            {
                if (pDialog.ShowDialog() == DialogResult.OK)
                {
                    if(OnCheck_IsCorrect(pDialog.FileName))
                    {
                        if(bIsAbsolutePath)
                            pTextBox_Path.Text = pDialog.FileName;
                        else
                            pTextBox_Path.Text = MakeRelativePath(pDialog.FileName);
                        return true;
                    }
                    else
                    {
                        MessageBox.Show(strOnErrorMsg, null, MessageBoxButtons.OK);
                    }
                }
            }

            return false;
        }

        public bool DoShowFolderBrowser_And_SavePath(bool bIsAbsolutePath, ref TextBox pTextBox_Path)
        {
            using (FolderBrowserDialog pDialog = new FolderBrowserDialog())
            {
                if (pDialog.ShowDialog() == DialogResult.OK)
                {
                    if(bIsAbsolutePath)
                        pTextBox_Path.Text = pDialog.SelectedPath;
                    else
                        pTextBox_Path.Text = MakeRelativePath(pDialog.SelectedPath);

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
            if(Path.IsPathRooted(strPath))
                SpreadSheetParser_MainForm.DoOpenPath(strPath);
            else
                SpreadSheetParser_MainForm.DoOpenPath(GetRelative_To_AbsolutePath() + strPath);
        }

        abstract public void DoWork(CodeFileBuilder pCodeFileBuilder, IEnumerable<SaveData_Sheet> listSheetData);
        virtual public void DoWorkAfter() { }

        abstract protected void OnCreateInstance(out Type pFormType, out Type pType);
        abstract protected void OnShowForm(Form pFormInstance);

        abstract public string GetDisplayString();

        public override string ToString()
        {
            return GetDisplayString();
        }


        // https://stackoverflow.com/questions/13266756/absolute-to-relative-path
        public static string MakeRelativePath(string filePath)
        {
            var pFileURI = new Uri(filePath);
            var pCurrentURI = new Uri(Directory.GetCurrentDirectory());

            return pCurrentURI.MakeRelativeUri(pFileURI).ToString();
        }

        public string GetRelative_To_AbsolutePath()
        {
            return $"{new Uri(Directory.GetCurrentDirectory()).AbsolutePath}{"/../"}";
        }

        bool OnCheck_IsCorrect_Default(string strFileName)
        {
            return true;
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
            string strTypeName = jo["pType"].Value<string>();

            foreach(var pType in GetEnumerableOfType(typeof(WorkBase)))
            {
                if(strTypeName.Contains(pType.Name))
                {
                    return JsonConvert.DeserializeObject(jo.ToString(), pType, SpecifiedSubclassConversion);
                }
            }

            return null;
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException(); // won't be called because CanWrite returns false
        }

        public static IEnumerable<Type> GetEnumerableOfType(System.Type pBaseType)
        {
            List<Type> objects = new List<Type>();
            foreach (Type type in
                Assembly.GetAssembly(pBaseType).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(pBaseType)))
            {
                objects.Add(type);
            }
            return objects;
        }
    }
}
