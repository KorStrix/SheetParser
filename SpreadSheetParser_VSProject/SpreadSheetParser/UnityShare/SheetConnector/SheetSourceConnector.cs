using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SpreadSheetParser
{
    public enum ESheetSourceType
    {
        GoogleSpreadSheet,
        MSExcel,
    }
    
    public delegate void delOnFinishConnect(SheetSourceConnector iSourceConnector, Exception pException_OnError);

    public class SheetData
    {
        public SheetSourceConnector pSourceConnector { get; private set; }
        public string strSheetName { get; private set; }
        public string strSheetID { get; private set; }

        public SheetData(SheetSourceConnector pSourceConnector, string strSheetName)
        {
            this.pSourceConnector = pSourceConnector;
            this.strSheetName = strSheetName;
            this.strSheetID = strSheetName;
        }

        public SheetData(SheetSourceConnector pSourceConnector, string strSheetName, string strSheetID)
        {
            this.pSourceConnector = pSourceConnector;
            this.strSheetName = strSheetName;
            this.strSheetID = strSheetID;
        }


        public override string ToString()
        {
            return strSheetName;
        }

        public IList<IList<Object>> GetData()
        {
            return pSourceConnector.ISheetSourceConnector_GetSheetData(strSheetName);
        }
    }

    public abstract class SheetSourceConnector
    {
        public string strSheetSourceID { get; private set; }
        public abstract IReadOnlyDictionary<string, SheetData> mapWorkSheetData_Key_Is_SheetID { get; }
        public abstract ESheetSourceType eSheetSourceType { get; }

        protected SheetSourceConnector(string strSheetSourceID)
        {
            this.strSheetSourceID = strSheetSourceID;
        }


        public abstract Task ISheetSourceConnector_DoConnect_And_Parsing(delOnFinishConnect OnFinishConnect);

        public abstract IList<IList<Object>> ISheetSourceConnector_GetSheetData(string strSheetName);
        public abstract Task<IList<IList<Object>>> ISheetSourceConnector_GetSheetData_Async(string strSheetName);

        public static void DoOpen_SheetSource(ESheetSourceType eSheetSourceType, string strSheetSourceID, System.Action<string> OnException)
        {
            switch (eSheetSourceType)
            {
                case ESheetSourceType.GoogleSpreadSheet:
                    const string const_SheetURL = "https://docs.google.com/spreadsheets/d/";
                    System.Diagnostics.Process.Start($"{const_SheetURL}/{strSheetSourceID}");
                    break;

                case ESheetSourceType.MSExcel:
                    FileInfo pFileInfo = new FileInfo(DoMake_AbsolutePath(strSheetSourceID));
                    if (pFileInfo.Exists)
                        System.Diagnostics.Process.Start(strSheetSourceID);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(eSheetSourceType), eSheetSourceType, null);
            }
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
