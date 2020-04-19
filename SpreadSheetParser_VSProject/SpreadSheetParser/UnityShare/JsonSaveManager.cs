using System.Collections.Generic;
using System.IO;
using System.Text;

using Newtonsoft.Json;

#if !UNITY_EDITOR
using Microsoft.IO;
#endif

namespace SpreadSheetParser
{
    public static class JsonSaveManager
    {
        static JsonSerializer _pSerializer = JsonSerializer.CreateDefault(Create_JsonSerializerSetting());
        static JsonSerializerSettings _pSetting = Create_JsonSerializerSetting();

        static HashSet<string> _set_AsyncSave = new HashSet<string>();
        static JsonSerializerSettings Create_JsonSerializerSetting()
        {
            JsonSerializerSettings pSetting = new JsonSerializerSettings();
            pSetting.Formatting = Formatting.Indented;
            pSetting.MissingMemberHandling = MissingMemberHandling.Ignore;
            pSetting.Converters.Add(new WorkJsonConverter());

            return pSetting;
        }

        static public void SaveData(object pData, string strFilePath)
        {
            Check_ExistsFolderPath(strFilePath);

            using (StreamWriter sw = new StreamWriter(strFilePath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                _pSerializer.Serialize(writer, pData);
            }
        }

#if !UNITY_EDITOR
        static public async void SaveData_Async(object pData, string strFilePath, System.Action<bool> OnFinishAsync)
        {
            if (_set_AsyncSave.Contains(strFilePath))
                return;
            _set_AsyncSave.Add(strFilePath);

            Check_ExistsFolderPath(strFilePath);

            try
            {
                using (var file = File.Open(strFilePath, FileMode.Create))
                {
                    // create this in the constructor, stream manages can be reused
                    // see details in this answer https://stackoverflow.com/a/42599288/185498
                    var streamManager = new RecyclableMemoryStreamManager();

                    using (var memoryStream = streamManager.GetStream()) // RecyclableMemoryStream will be returned, it inherits MemoryStream, however prevents data allocation into the LOH
                    {
                        using (var writer = new StreamWriter(memoryStream))
                        {
                            JsonSerializerSettings pSetting = new JsonSerializerSettings();
                            pSetting.Formatting = Formatting.Indented;

                            _pSerializer.Serialize(writer, pData);

                            await writer.FlushAsync().ConfigureAwait(false);

                            memoryStream.Seek(0, SeekOrigin.Begin);

                            await memoryStream.CopyToAsync(file).ConfigureAwait(false);
                        }
                    }

                    await file.FlushAsync().ConfigureAwait(false);
                    OnFinishAsync?.Invoke(true);
                }
            } 
            catch
            {
                OnFinishAsync?.Invoke(false);
            }

            _set_AsyncSave.Remove(strFilePath);
        }
#endif

        public static T LoadData<T>(string strFilePath, System.Action<string> OnError = null)
           where T : class
        {
            if (File.Exists(strFilePath) == false)
                return null;

            string strText = File.ReadAllText(strFilePath, Encoding.UTF8);
            T pData = null;
            try
            {
                pData = JsonConvert.DeserializeObject<T>(strText, _pSetting);
            }
            catch (System.Exception e)
            {
                OnError?.Invoke($"Load Data Parsing Error - \nFileName : {strFilePath} Error : {e}");
            }

            return pData;
        }

        public static List<T> LoadData_List<T>(string strFolderPath, System.Action<string> OnError = null)
            where T : class
        {
            List<T> listData = new List<T>();
            if (Directory.Exists(strFolderPath) == false)
                return listData;

            DirectoryInfo pDirectory = new DirectoryInfo(strFolderPath);
            FileInfo[] arrFile = pDirectory.GetFiles();
            for (int i = 0; i < arrFile.Length; i++)
            {
                T pData = pData = LoadData<T>(arrFile[i].FullName);
                if (pData == null)
                    continue;

                listData.Add(pData);
            }

            return listData;
        }


        private static void Check_ExistsFolderPath(string strFilePath)
        {
            string strFolderPath = Path.GetDirectoryName(strFilePath);
            if (Directory.Exists(strFolderPath) == false)
                Directory.CreateDirectory(strFolderPath);
        }
    }
}
