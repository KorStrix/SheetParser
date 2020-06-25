using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ExcelDataReader;
using System.Data;
using System.Linq;

namespace SpreadSheetParser
{
    public class MSExcel_SourceConnector : SheetSourceConnector
    {
        public override IReadOnlyDictionary<string, SheetConnectorWrapper> mapWorkSheetData_Key_Is_SheetID => _mapWorkSheet;
        public override ESheetSourceType eSheetSourceType => ESheetSourceType.MSExcel;


        private readonly Dictionary<string, SheetConnectorWrapper> _mapWorkSheet = new Dictionary<string, SheetConnectorWrapper>();
        private readonly Dictionary<string, DataTable> _mapDataTable = new Dictionary<string, DataTable>();

        public MSExcel_SourceConnector(string strFileAbsolutePath_And_IncludeExtension) : base(strFileAbsolutePath_And_IncludeExtension)
        {

        }

        public override async Task ISheetSourceConnector_DoConnect_And_Parsing(delOnFinishConnect OnFinishConnect)
        {
            await Open_Excel(SynchronizationContext.Current, strSheetSourceID, OnFinishConnect);
        }

        private async Task Open_Excel(SynchronizationContext pSyncContext_Call, string strFileAbsolutePath_And_IncludeExtension, delOnFinishConnect OnFinishConnect)
        {
            foreach (DataTable pSet in _mapDataTable.Values)
                pSet.Dispose();
            _mapDataTable.Clear();
            _mapWorkSheet.Clear();
            Exception pException_OnError = null;

            await Task.Run(() =>
            {
                string strAbsolutePath = DoConvert_AbsolutePath(strFileAbsolutePath_And_IncludeExtension);

                try
                {
                    using (FileStream stream = File.Open(strAbsolutePath, FileMode.Open, FileAccess.Read))
                    {
                        using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            DataSet DataSet = reader.AsDataSet();
                            foreach (DataTable pSheet in DataSet.Tables)
                            {
                                string strTableName = pSheet.TableName;
                                _mapWorkSheet.Add(strTableName, new SheetConnectorWrapper(this, strTableName));
                                _mapDataTable.Add(strTableName, pSheet);
                            }
                        }
                    }
                }
                catch (Exception pException)
                {
                    pException_OnError = pException;
                }
            });

            // strSheetSourceID = Path.GetFileNameWithoutExtension(strFileAbsolutePath_And_IncludeExtension);
            if (pSyncContext_Call == null)
            {
                OnFinishConnect(this, pException_OnError);
            }
            else
            {
                pSyncContext_Call.Send(o =>
                {
                    OnFinishConnect(this, pException_OnError);
                },
                null);
            }
        }

        public override IList<IList<Object>> ISheetSourceConnector_GetSheetData(string strSheetName)
        {
            if (_mapDataTable.TryGetValue(strSheetName, out DataTable pSheet) == false)
            {
                return null;
            }

            List<IList<Object>> listData = new List<IList<object>>();

            DataRowCollection pRows = pSheet.Rows;
            foreach (DataRow pRow in pRows)
            {
                List<Object> listRow = new List<Object>();
                listData.Add(listRow);
                listRow.AddRange(pRow.ItemArray.Select(p => p.ToString()));
            }

            return listData;
        }

        public override Task<IList<IList<Object>>> ISheetSourceConnector_GetSheetData_Async(string strSheetName)
        {
            return Task.Run(() => ISheetSourceConnector_GetSheetData(strSheetName));
        }

        public static string DoConvert_AbsolutePath(string strPath)
        {
            if (Path.IsPathRooted(strPath))
                return strPath;

            Uri pCurrentURI = new Uri(Directory.GetCurrentDirectory());
            return $"{pCurrentURI.AbsolutePath}/../{strPath}";
        }
    }
}
