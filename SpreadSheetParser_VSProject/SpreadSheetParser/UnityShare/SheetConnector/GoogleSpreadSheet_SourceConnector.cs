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
using Google.Apis.Sheets.v4.Data;

namespace SpreadSheetParser
{
    public class GoogleSpreadSheet_SourceConnector : SheetSourceConnector
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        static readonly string[] Scopes = {SheetsService.Scope.SpreadsheetsReadonly};
        static readonly string ApplicationName = "Spread Sheet Parser";


        public override IReadOnlyDictionary<string, SheetConnectorWrapper> mapWorkSheetData_Key_Is_SheetID => _mapWorkSheet;
        public override ESheetSourceType eSheetSourceType => ESheetSourceType.GoogleSpreadSheet;

        public string strFileName { get; private set; }
        public bool bIsConnected => _pService != null && _pService.Spreadsheets != null;



        Dictionary<string, SheetConnectorWrapper> _mapWorkSheet = new Dictionary<string, SheetConnectorWrapper>();
        SheetsService _pService;

        readonly CancellationTokenSource _pTokenSource = new CancellationTokenSource();
        readonly string _strCredentialFilePath;


        public GoogleSpreadSheet_SourceConnector(string strSheetSourceID, string strCredentialFilePath = "credentials.json") : base(strSheetSourceID)
        {
            this._strCredentialFilePath = strCredentialFilePath;
        }

        public override async Task ISheetSourceConnector_DoConnect_And_Parsing(delOnFinishConnect OnFinishConnect)
        {
            _mapWorkSheet.Clear();
            _pService = null;
            Exception pException_OnError = null;

            try
            {
                UserCredential credential;
                using (FileStream stream =
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
            SpreadsheetsResource.GetRequest pRequest_HandShake = _pService.Spreadsheets.Get(strSheetSourceID);
            try
            {
                Task<Spreadsheet> pResponse = pRequest_HandShake.ExecuteAsync();
                await pResponse;

                strFileName = pResponse.Result.Properties.Title;
                IList<Sheet> listSheet = pResponse.Result.Sheets;
                _mapWorkSheet = listSheet.ToDictionary(p => p.Properties.SheetId.ToString(),
                    p => new SheetConnectorWrapper(this, p.Properties.Title, p.Properties.SheetId.ToString()));
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

        public override IList<IList<Object>> ISheetSourceConnector_GetSheetData(string strSheetName)
        {
            SpreadsheetsResource.ValuesResource.GetRequest pRequest =
                _pService.Spreadsheets.Values.Get(strSheetSourceID, strSheetName);

            return pRequest.Execute().Values;
        }

        public override Task<IList<IList<object>>> ISheetSourceConnector_GetSheetData_Async(string strSheetName)
        {
            SpreadsheetsResource.ValuesResource.GetRequest pRequest =
                _pService.Spreadsheets.Values.Get(strSheetSourceID, strSheetName);

            return pRequest.ExecuteAsync().ContinueWith(p => p.Result.Values);
        }
    }
}
