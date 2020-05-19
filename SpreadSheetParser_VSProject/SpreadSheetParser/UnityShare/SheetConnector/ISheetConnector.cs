using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpreadSheetParser
{
    public enum ESpreadSheetType
    {
        GoogleSpreadSheet,
        MSExcel,
    }
    
    public delegate void delOnFinishConnect(ISheetConnector iConnector, Exception pException_OnError);

    public class SheetData
    {
        public ISheetConnector pConnector { get; private set; }
        public string strSheetName { get; private set; }
        public string strSheetID { get; private set; }

        public SheetData(ISheetConnector pConnector, string strSheetName)
        {
            this.pConnector = pConnector;
            this.strSheetName = strSheetName;
            this.strSheetID = strSheetName;
        }

        public SheetData(ISheetConnector pConnector, string strSheetName, string strSheetID)
        {
            this.pConnector = pConnector;
            this.strSheetName = strSheetName;
            this.strSheetID = strSheetID;
        }


        public override string ToString()
        {
            return strSheetName;
        }

        public IList<IList<Object>> GetData()
        {
            return pConnector.ISheetConnector_GetSheetData(strSheetName);
        }
    }

    public interface ISheetConnector
    {
        IReadOnlyDictionary<string, SheetData> mapWorkSheetData_Key_Is_SheetID { get; }
        string strSheetID { get; }
        ESpreadSheetType eSheetType { get; }


        Task ISheetConnector_DoConnect_And_Parsing(string strSheetID, delOnFinishConnect OnFinishConnect);

        IList<IList<Object>> ISheetConnector_GetSheetData(string strSheetName);

        Task<IList<IList<Object>>> ISheetConnector_GetSheetData_Async(string strSheetName);
    }

}
