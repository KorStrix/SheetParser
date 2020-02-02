using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Services;
using Google.Apis.Util.Store;

using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ExcelDataReader;
using System.Data;

namespace SpreadSheetParser
{
    public enum ESpreadSheetType
    {
        GoogleSpreadSheet,
        MSExcel,
    }

    public class SheetWrapper
    {
        public string strSheetName { get; private set; }

        public SheetWrapper(string strSheetName)
        {
            this.strSheetName = strSheetName;
        }

        public override string ToString()
        {
            return strSheetName;
        }
    }

    public class SpreadSheetConnector
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "Spread Sheet Parser";

        public delegate void delOnFinishConnect(string strSheetID, string strFileName, ESpreadSheetType eSheetType, List<SheetWrapper> listSheet, Exception pException_OnError);

        public bool bIsConnected => _pService != null && _pService.Spreadsheets != null;

        SheetsService _pService;
        CancellationTokenSource _pTokenSource = new CancellationTokenSource();
        ESpreadSheetType _eConnectedSheetType;
        Dictionary<string, DataTable> _mapWorkSheet = new Dictionary<string, DataTable>();
        string _strSheetID;

        public async Task DoConnect(string strSheetID, delOnFinishConnect OnFinishConnect, string strCredentialFilePath = "credentials.json")
        {
            List<SheetWrapper> listSheet = new List<SheetWrapper>();
            this._pService = null;
            this._eConnectedSheetType = ESpreadSheetType.GoogleSpreadSheet;
            this._strSheetID = strSheetID;
            UserCredential credential;
            Exception pException_OnError = null;

            try
            {
                using (var stream =
                    new FileStream(strCredentialFilePath, FileMode.Open, FileAccess.Read))
                {
                    string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        _pTokenSource.Token,
                        new FileDataStore(credPath, true)).Result;
                }

                _pService = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
            }
            catch (Exception pException)
            {
                OnFinishConnect(strSheetID, "", _eConnectedSheetType, listSheet, pException);
                return;
            }

            string strFileName = "";
            var pRequest_HandShake = _pService.Spreadsheets.Get(strSheetID);
            try
            {
                var pResponse = pRequest_HandShake.ExecuteAsync();
                await pResponse;

                strFileName = pResponse.Result.Properties.Title;
                for (int i = 0; i < pResponse.Result.Sheets.Count; i++)
                    listSheet.Add(new SheetWrapper(pResponse.Result.Sheets[i].Properties.Title));
            }
            catch (Exception pException)
            {
                pException_OnError = pException;
            }

            OnFinishConnect(strSheetID, strFileName, _eConnectedSheetType, listSheet, pException_OnError);
        }

        public void DoCancelConnect()
        {
            _pTokenSource.Cancel();
        }

        public IList<IList<Object>> GetExcelData(string strSheetName)
        {
            if (_eConnectedSheetType == ESpreadSheetType.GoogleSpreadSheet)
            {
                SpreadsheetsResource.ValuesResource.GetRequest pRequest =
                _pService.Spreadsheets.Values.Get(_strSheetID, strSheetName);

                var pResponse = pRequest.Execute();
                return pResponse.Values;
            }
            else
            {
                DataTable pSheet;
                if (_mapWorkSheet.TryGetValue(strSheetName, out pSheet) == false)
                {
                    return null;
                }
                List<IList<Object>> listData = new List<IList<object>>();

                var pRows = pSheet.Rows;
                foreach (DataRow pRow in pRows)
                {
                    List<Object> listRow = new List<Object>();
                    listData.Add(listRow);
                    listRow.AddRange(pRow.ItemArray.Select(p => p.ToString()));
                }

                return listData;
            }

            return null;
        }

        public void DoOpen_Excel(string strFileAbsolutePath_And_IncludeExtension, delOnFinishConnect OnFinishConnect)
        {
            Open_Excel(SynchronizationContext.Current, strFileAbsolutePath_And_IncludeExtension, OnFinishConnect);
        }

        private async void Open_Excel(SynchronizationContext pSyncContext_Call, string strFileAbsolutePath_And_IncludeExtension, delOnFinishConnect OnFinishConnect)
        {
            List<SheetWrapper> listSheet = new List<SheetWrapper>();
            this._eConnectedSheetType = ESpreadSheetType.MSExcel;
            this._strSheetID = strFileAbsolutePath_And_IncludeExtension;
            Exception pException_OnError = null;

            foreach (DataTable pData in _mapWorkSheet.Values)
                pData.Dispose();
            _mapWorkSheet.Clear();

            await Task.Run(() =>
            {
                string strAbsolutePath = DoMake_AbsolutePath(strFileAbsolutePath_And_IncludeExtension);

                try
                {
                    using (var stream = File.Open(strAbsolutePath, FileMode.Open, FileAccess.Read))
                    {
                        using (IExcelDataReader reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream))
                        {
                            var DataSet = reader.AsDataSet();
                            foreach (DataTable pSheet in DataSet.Tables)
                            {
                                _mapWorkSheet.Add(pSheet.TableName, pSheet);
                                listSheet.Add(new SheetWrapper(pSheet.TableName));
                            }
                        }
                    }
                }
                catch (Exception pException)
                {
                    pException_OnError = pException;
                }
            });

            string strFileName = Path.GetFileNameWithoutExtension(strFileAbsolutePath_And_IncludeExtension);
            pSyncContext_Call.Send(new SendOrPostCallback(o =>
            {
                OnFinishConnect(strFileAbsolutePath_And_IncludeExtension, strFileName, _eConnectedSheetType, listSheet, pException_OnError);
            }),
            null);
        }
        public static string DoMake_AbsolutePath(string strPath)
        {
            if (Path.IsPathRooted(strPath))
                return strPath;

            var pCurrentURI = new Uri(Directory.GetCurrentDirectory());
            return $"{pCurrentURI.AbsolutePath}/../{strPath}";
        }
    }
}
