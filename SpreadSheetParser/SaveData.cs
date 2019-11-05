using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SpreadSheetParser
{
    public class SaveData_SpreadSheet
    {
        public string strSheetID;
        public DateTime date_LastEdit;
        public List<SaveData_Table> listTable = new List<SaveData_Table>();

        public override string ToString()
        {
            return strSheetID;
        }
    }

    public class SaveData_Table
    {
        public string strSheetName;
        public bool bEnable = true;

        public string strCommandLine;

        public override string ToString()
        {
            return strSheetName;
        }
    }

    public static class SaveDataManager
    {
        static string const_strSaveFolderPath = Directory.GetCurrentDirectory() + "/SaveData/";

        static JsonSerializer _pSerializer = new JsonSerializer();

        static public void SaveSheet(SaveData_SpreadSheet pSheet)
        {
            string strJsonText = JsonConvert.SerializeObject(pSheet);

            if (Directory.Exists(const_strSaveFolderPath) == false)
                Directory.CreateDirectory(const_strSaveFolderPath);

            using (StreamWriter sw = new StreamWriter(const_strSaveFolderPath + pSheet.strSheetID + ".json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                _pSerializer.Serialize(writer, pSheet);
            }
        }

        static public Dictionary<string, SaveData_SpreadSheet> LoadSheet()
        {
            Dictionary<string, SaveData_SpreadSheet> mapSaveSheet = new Dictionary<string, SaveData_SpreadSheet>();

            if (Directory.Exists(const_strSaveFolderPath) == false)
                return mapSaveSheet;

            DirectoryInfo pDirectory = new DirectoryInfo(const_strSaveFolderPath);
            FileInfo[] arrFile = pDirectory.GetFiles();
            for(int i = 0; i < arrFile.Length; i++)
            {
                FileInfo pFile = arrFile[i];
                string strText = File.ReadAllText(pFile.FullName, Encoding.UTF8);

                SaveData_SpreadSheet pSaveData = null;
                try
                {
                    pSaveData = JsonConvert.DeserializeObject<SaveData_SpreadSheet>(strText);
                }
                catch
                {

                }

                if(pSaveData != null)
                    mapSaveSheet.Add(pSaveData.strSheetID, pSaveData);
            }

            return mapSaveSheet;
        }
    }
}
