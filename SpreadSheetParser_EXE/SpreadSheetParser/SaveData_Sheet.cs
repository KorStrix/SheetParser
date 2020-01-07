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
    public class Config
    {
        public bool bAutoConnect = true;
        public bool bOpenPath_AfterBuild_Csharp;
        public bool bOpenPath_AfterBuild_CSV;
    }

    public class SaveData_SpreadSheet
    {
        public string strSheetID;
        public DateTime date_LastEdit;
        public List<SaveData_Sheet> listTable = new List<SaveData_Sheet>();
        public List<WorkBase> listSaveWork = new List<WorkBase>();

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

    public class SaveData_Sheet
    {
        public enum EType
        {
            Class,
            Struct,
            Enum,
        }

        public string strSheetName;
        public bool bEnable = true;

        public string strCommandLine;
        public EType eType;

        public SaveData_Sheet(string strSheetName)
        {
            this.strSheetName = strSheetName;
            bEnable = true;
            eType = EType.Class;
        }

        public override string ToString()
        {
            return strSheetName;
        }
    }

    public static class SaveDataManager
    {
        public static string const_strSaveFolderPath = Directory.GetCurrentDirectory() + "/SaveData/";

        static public void SaveConfig(string strFilePath, object pData)
        {
            JsonSaveManager.SaveData(pData, GetFilePath(strFilePath));
        }

        static public void SaveConfig(Config pData)
        {
            JsonSaveManager.SaveData(pData, GetFilePath("Config"));
        }

        static public void SaveConfig_Async(Config pData, System.Action<bool> OnFinishAsync)
        {
            JsonSaveManager.SaveData_Async(pData, GetFilePath("Config"), OnFinishAsync);
        }

        static public Config LoadConfig()
        {
            List<Config> listConfig = JsonSaveManager.LoadData<Config>(const_strSaveFolderPath);
            if (listConfig.Count > 0)
                return listConfig[0];
            else
                return new Config();
        }

        #region Sheet
        static public void SaveSheet(SaveData_SpreadSheet pSheet)
        {
            JsonSaveManager.SaveData(pSheet, GetFilePath(pSheet.strSheetID));
        }

        static public void SaveSheet_Async(SaveData_SpreadSheet pSheet, System.Action<bool> OnFinishAsync)
        {
            JsonSaveManager.SaveData_Async(pSheet, GetFilePath(pSheet.strSheetID), OnFinishAsync);
        }

        static public Dictionary<string, SaveData_SpreadSheet> LoadSheet(System.Action<string> OnError)
        {
            Dictionary<string, SaveData_SpreadSheet> mapSaveSheet = new Dictionary<string, SaveData_SpreadSheet>();
            List<SaveData_SpreadSheet> listSheet = JsonSaveManager.LoadData<SaveData_SpreadSheet>(const_strSaveFolderPath, OnError);

            for (int i = 0; i < listSheet.Count; i++)
            {
                var pData = listSheet[i];
                if (string.IsNullOrEmpty(pData.strSheetID))
                    continue;

                if (mapSaveSheet.ContainsKey(pData.strSheetID))
                {
                    SaveData_SpreadSheet pSheetAlreadyAdded = mapSaveSheet[pData.strSheetID];
                    if (pSheetAlreadyAdded.date_LastEdit < pData.date_LastEdit)
                        mapSaveSheet[pData.strSheetID] = pData;
                }
                else
                {
                    mapSaveSheet.Add(pData.strSheetID, pData);
                }
            }

            return mapSaveSheet;
        }
        #endregion Sheet

        private static string GetFilePath(string strFileName)
        {
            return const_strSaveFolderPath + strFileName + ".json";
        }
    }
}
