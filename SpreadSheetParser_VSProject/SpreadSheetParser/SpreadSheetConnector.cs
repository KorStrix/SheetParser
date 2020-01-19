using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace SpreadSheetParser
{
    public class SheetWrapper
    {
        public Sheet pSheet { get; private set; }

        public SheetWrapper(Sheet pSheet)
        {
            this.pSheet = pSheet;
        }

        public override string ToString()
        {
            if (pSheet == null)
                return "Error";

            return pSheet.Properties.Title;
        }
    }

    public class SpreadSheetConnector
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "Spread Sheet Parser";

        public delegate void delOnFinishConnect(string strSheetID, List<SheetWrapper> listSheet, Exception pException_OnError);

        public SheetsService pService { get; private set; } = null;

        string _strSheetID;

        public async Task DoConnect(string strSheetID, delOnFinishConnect OnFinishConnect)
        {
            List< SheetWrapper > listSheet = new List<SheetWrapper>();
            this._strSheetID = strSheetID;
            UserCredential credential;
            Exception pException_OnError = null;

            try
            {
                using (var stream =
                    new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
                {
                    string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                }

                pService = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
            }
            catch (Exception pException)
            {
                pException_OnError = pException;
                OnFinishConnect(strSheetID, listSheet, pException_OnError);
                return;
            }

            var pRequest_HandShake = pService.Spreadsheets.Get(strSheetID);
            try
            {
                var pResponse = pRequest_HandShake.ExecuteAsync();
                await pResponse;

                for (int i = 0; i < pResponse.Result.Sheets.Count; i++)
                    listSheet.Add(new SheetWrapper(pResponse.Result.Sheets[i]));
            }
            catch (Exception pException)
            {
                pException_OnError = pException;
            }

            OnFinishConnect(strSheetID, listSheet, pException_OnError);
        }

        public IList<IList<Object>> GetExcelData(string strSheetName)
        {
            SpreadsheetsResource.ValuesResource.GetRequest pRequest =
            pService.Spreadsheets.Values.Get(_strSheetID, strSheetName);

            var pResponse = pRequest.Execute();

            return pResponse.Values;
        }
    }
}
