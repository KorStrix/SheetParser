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

        public static void SaveConfig(string strFilePath, object pData)
        {
            JsonSaveManager.SaveData(pData, GetFilePath(strFilePath));
        }

        public static void SaveConfig(Config pData)
        {
            JsonSaveManager.SaveData(pData, GetFilePath("Config"));
        }

        public static void SaveConfig_Async(Config pData, Action<bool> OnFinishAsync)
        {
            JsonSaveManager.SaveData_Async(pData, GetFilePath("Config"), OnFinishAsync);
        }

        public static Config LoadConfig()
        {
            List<Config> listConfig = JsonSaveManager.LoadData_List<Config>(const_strSaveFolderPath);
            if (listConfig.Count > 0)
                return listConfig[0];
            else
                return new Config();
        }

        public static void SaveSheet(SaveData_SheetSource pSheetSource)
        {
            JsonSaveManager.SaveData(pSheetSource, GetFilePath(pSheetSource.GetFileName()));
        }

        public static void SaveSheet(SaveData_SheetSourceCollection pSheetSourceCollection)
        {
            JsonSaveManager.SaveData(pSheetSourceCollection, GetFilePath(pSheetSourceCollection.strFileName));
        }

        public static void SaveSheet_Async(SaveData_SheetSource pSheetSource, Action<bool> OnFinishAsync)
        {
            JsonSaveManager.SaveData_Async(pSheetSource, GetFilePath(pSheetSource.GetFileName()), OnFinishAsync);
        }


        public static void SaveSheet_Async(SaveData_SheetSourceCollection pSheetSourceCollection, Action<bool> OnFinishAsync)
        {
            JsonSaveManager.SaveData_Async(pSheetSourceCollection, GetFilePath(pSheetSourceCollection.strFileName), OnFinishAsync);
        }

        public static Dictionary<string, SaveData_SheetSourceCollection> LoadSheet_SaveSheetSourceCollection(Action<string> OnError)
        {
            Dictionary<string, SaveData_SheetSourceCollection> mapSaveSheetSourceCollection = new Dictionary<string, SaveData_SheetSourceCollection>();
            List<SaveData_SheetSourceCollection> listSheet = JsonSaveManager.LoadData_List<SaveData_SheetSourceCollection>(const_strSaveFolderPath, OnError);

            for (int i = 0; i < listSheet.Count; i++)
            {
                var pData = listSheet[i];
                if (string.IsNullOrEmpty(pData.strFileName))
                    continue;

                if (mapSaveSheetSourceCollection.ContainsKey(pData.strFileName))
                {
                    SaveData_SheetSourceCollection pSheetSourceAlreadyAdded = mapSaveSheetSourceCollection[pData.strFileName];
                    if (pSheetSourceAlreadyAdded.date_LastEdit < pData.date_LastEdit)
                        mapSaveSheetSourceCollection[pData.strFileName] = pData;
                }
                else
                {
                    mapSaveSheetSourceCollection.Add(pData.strFileName, pData);
                }
            }

            return mapSaveSheetSourceCollection;
        }

        public static Dictionary<string, SaveData_SheetSource> LoadSheet_SaveSheetSource(Action<string> OnError)
        {
            Dictionary<string, SaveData_SheetSource> mapSaveSheet = new Dictionary<string, SaveData_SheetSource>();
            List<SaveData_SheetSource> listSheet = JsonSaveManager.LoadData_List<SaveData_SheetSource>(const_strSaveFolderPath, OnError);

            for (int i = 0; i < listSheet.Count; i++)
            {
                var pData = listSheet[i];
                if (string.IsNullOrEmpty(pData.strSheetSourceID))
                    continue;

                if (mapSaveSheet.ContainsKey(pData.strSheetSourceID))
                {
                    SaveData_SheetSource pSheetSourceAlreadyAdded = mapSaveSheet[pData.strSheetSourceID];
                    if (pSheetSourceAlreadyAdded.date_LastEdit < pData.date_LastEdit)
                        mapSaveSheet[pData.strSheetSourceID] = pData;
                }
                else
                {
                    mapSaveSheet.Add(pData.strSheetSourceID, pData);
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
