using Microsoft.IO;
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
        public string strOutputPath_Csharp = Directory.GetCurrentDirectory();
        public string strOutputPath_CSV = Directory.GetCurrentDirectory();

        public SaveData_SpreadSheet(string strSheetID)
        {
            this.strSheetID = strSheetID;
            UpdateDate();
        }

        public void UpdateDate()
        {
            date_LastEdit = System.DateTime.Now;
        }

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
        public static string const_strSaveFolderPath = Directory.GetCurrentDirectory() + "/SaveData/";

        static JsonSerializer _pSerializer = new JsonSerializer();

        static public void SaveSheet(SaveData_SpreadSheet pSheet)
        {
            if (Directory.Exists(const_strSaveFolderPath) == false)
                Directory.CreateDirectory(const_strSaveFolderPath);

            using (StreamWriter sw = new StreamWriter(CreateFileName(pSheet)))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                _pSerializer.Serialize(writer, pSheet);
            }
        }

        static public async void SaveSheet_Async(SaveData_SpreadSheet pSheet, System.Action OnFinishAsync)
        {
            if (Directory.Exists(const_strSaveFolderPath) == false)
                Directory.CreateDirectory(const_strSaveFolderPath);

            // create this in the constructor, stream manages can be reused
            // see details in this answer https://stackoverflow.com/a/42599288/185498
            var streamManager = new RecyclableMemoryStreamManager();

            using (var file = File.Open(CreateFileName(pSheet), FileMode.Create))
            {
                using (var memoryStream = streamManager.GetStream()) // RecyclableMemoryStream will be returned, it inherits MemoryStream, however prevents data allocation into the LOH
                {
                    using (var writer = new StreamWriter(memoryStream))
                    {
                        var serializer = JsonSerializer.CreateDefault();

                        serializer.Serialize(writer, pSheet);

                        await writer.FlushAsync().ConfigureAwait(false);

                        memoryStream.Seek(0, SeekOrigin.Begin);

                        await memoryStream.CopyToAsync(file).ConfigureAwait(false);
                    }
                }

                await file.FlushAsync().ConfigureAwait(false);
                OnFinishAsync?.Invoke();
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

                if (pSaveData == null)
                    continue;

                if(mapSaveSheet.ContainsKey(pSaveData.strSheetID))
                {
                    SaveData_SpreadSheet pSheetAlreadyAdded = mapSaveSheet[pSaveData.strSheetID];
                    if (pSheetAlreadyAdded.date_LastEdit < pSaveData.date_LastEdit)
                        mapSaveSheet[pSaveData.strSheetID] = pSaveData;
                }
                else
                {
                    mapSaveSheet.Add(pSaveData.strSheetID, pSaveData);
                }
            }

            return mapSaveSheet;
        }


        private static string CreateFileName(SaveData_SpreadSheet pSheet)
        {
            return const_strSaveFolderPath + pSheet.strSheetID + ".json";
        }

    }
}
