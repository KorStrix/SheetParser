using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;

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
        public ISheetConnector pConnector{ get; private set; }
        public string strSheetName { get; private set; }

        public SheetData(ISheetConnector pConnector, string strSheetName)
        {
            this.strSheetName = strSheetName;
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
        IReadOnlyDictionary<string, SheetData> mapWorkSheetData_Key_Is_SheetName { get; }
        string strSheetID { get; }
        ESpreadSheetType eSheetType { get; }


        Task ISheetConnector_DoConnect_And_Parsing(string strSheetID, delOnFinishConnect OnFinishConnect);

        IList<IList<Object>> ISheetConnector_GetSheetData(string strSheetName);
    }

}
