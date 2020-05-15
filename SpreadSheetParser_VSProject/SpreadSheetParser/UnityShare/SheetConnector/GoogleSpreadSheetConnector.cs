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
using Google.Apis.Sheets.v4.Data;

namespace SpreadSheetParser
{
    public class GoogleSpreadSheetConnector : ISheetConnector
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        static readonly string[] Scopes = {SheetsService.Scope.SpreadsheetsReadonly};
        static readonly string ApplicationName = "Spread Sheet Parser";


        public IReadOnlyDictionary<string, SheetData> mapWorkSheetData_Key_Is_SheetName => _mapWorkSheet;
        public ESpreadSheetType eSheetType => ESpreadSheetType.GoogleSpreadSheet;

        public string strFileName { get; private set; }
        public string strSheetID { get; private set; }
        public bool bIsConnected => _pService != null && _pService.Spreadsheets != null;



        Dictionary<string, SheetData> _mapWorkSheet = new Dictionary<string, SheetData>();
        SheetsService _pService;
        readonly CancellationTokenSource _pTokenSource = new CancellationTokenSource();
        readonly string _strCredentialFilePath;




        public GoogleSpreadSheetConnector(string strCredentialFilePath = "credentials.json")
        {
            this._strCredentialFilePath = strCredentialFilePath;
        }

        public async Task ISheetConnector_DoConnect_And_Parsing(string strSheetID, delOnFinishConnect OnFinishConnect)
        {
            _mapWorkSheet.Clear();
            this.strSheetID = strSheetID;

            _pService = null;
            Exception pException_OnError = null;

            try
            {
                UserCredential credential;
                using (var stream =
                    new FileStream(_strCredentialFilePath, FileMode.Open, FileAccess.Read))
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
                OnFinishConnect(this, pException);
                return;
            }

            strFileName = "";
            var pRequest_HandShake = _pService.Spreadsheets.Get(strSheetID);
            try
            {
                var pResponse = pRequest_HandShake.ExecuteAsync();
                await pResponse;

                strFileName = pResponse.Result.Properties.Title;
                IList<Sheet> listSheet = pResponse.Result.Sheets;
                for (int i = 0; i < listSheet.Count; i++)
                    _mapWorkSheet.Add(listSheet[i].Properties.Title, new SheetData(this, listSheet[i].Properties.Title));
            }
            catch (Exception pException)
            {
                pException_OnError = pException;
            }

            OnFinishConnect(this, pException_OnError);
        }


        public void DoCancelConnect()
        {
            _pTokenSource.Cancel();
        }

        public IList<IList<Object>> ISheetConnector_GetSheetData(string strSheetName)
        {
            SpreadsheetsResource.ValuesResource.GetRequest pRequest =
                _pService.Spreadsheets.Values.Get(strSheetID, strSheetName);

            var pResponse = pRequest.Execute();
            return pResponse.Values;
        }
    }
}
