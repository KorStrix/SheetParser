using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadSheetParser
{
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
            List<Config> listConfig = JsonSaveManager.LoadData_List<Config>(const_strSaveFolderPath);
            if (listConfig.Count > 0)
                return listConfig[0];
            else
                return new Config();
        }

        static public void SaveSheet(SaveData_SpreadSheet pSheet)
        {
            JsonSaveManager.SaveData(pSheet, GetFilePath(pSheet.GetFileName()));
        }

        static public void SaveSheet_Async(SaveData_SpreadSheet pSheet, System.Action<bool> OnFinishAsync)
        {
            JsonSaveManager.SaveData_Async(pSheet, GetFilePath(pSheet.GetFileName()), OnFinishAsync);
        }

        static public Dictionary<string, SaveData_SpreadSheet> LoadSheet(System.Action<string> OnError)
        {
            Dictionary<string, SaveData_SpreadSheet> mapSaveSheet = new Dictionary<string, SaveData_SpreadSheet>();
            List<SaveData_SpreadSheet> listSheet = JsonSaveManager.LoadData_List<SaveData_SpreadSheet>(const_strSaveFolderPath, OnError);

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

        private static string GetFilePath(string strFileName)
        {
            return const_strSaveFolderPath + strFileName + ".json";
        }
    }
}
