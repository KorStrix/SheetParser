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
        static string ApplicationName = "Google Sheets API .NET Quickstart";

        public SheetsService pService { get; private set; } = null;

        string _strSheetID;

        public bool DoConnect(string strSheetID, out List<SheetWrapper> listSheet, out Exception pException_OnError)
        {
            listSheet = new List<SheetWrapper>();
            pException_OnError = null;

            this._strSheetID = strSheetID;
            UserCredential credential;

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
                    Console.WriteLine("Credential file saved to: " + credPath);
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
                return false;
            }

            var pRequest_HandShake = pService.Spreadsheets.Get(strSheetID);
            try
            {
                var TestResponse = pRequest_HandShake.Execute();
                for (int i = 0; i < TestResponse.Sheets.Count; i++)
                    listSheet.Add(new SheetWrapper(TestResponse.Sheets[i]));
            }
            catch (Exception pException)
            {
                pException_OnError = pException;
                return false;
            }

            return true;
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
