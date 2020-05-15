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
    public class MSExcelConnector : ISheetConnector
    {
        public IReadOnlyDictionary<string, SheetData> mapWorkSheetData_Key_Is_SheetName => _mapWorkSheet;
        public string strSheetID { get; private set; }
        public ESpreadSheetType eSheetType => ESpreadSheetType.MSExcel;


        private readonly Dictionary<string, SheetData> _mapWorkSheet = new Dictionary<string, SheetData>();
        private readonly Dictionary<string, DataTable> _mapDataTable = new Dictionary<string, DataTable>();

        public async Task ISheetConnector_DoConnect_And_Parsing(string strSheetID, delOnFinishConnect OnFinishConnect)
        {
            await Open_Excel(SynchronizationContext.Current, strSheetID, OnFinishConnect);
        }

        private async Task Open_Excel(SynchronizationContext pSyncContext_Call, string strFileAbsolutePath_And_IncludeExtension, delOnFinishConnect OnFinishConnect)
        {
            foreach (var pSet in _mapDataTable.Values)
                pSet.Dispose();
            _mapDataTable.Clear();
            _mapWorkSheet.Clear();
            strSheetID = strFileAbsolutePath_And_IncludeExtension;
            Exception pException_OnError = null;

            await Task.Run(() =>
            {
                string strAbsolutePath = DoConvert_AbsolutePath(strFileAbsolutePath_And_IncludeExtension);

                try
                {
                    using (var stream = File.Open(strAbsolutePath, FileMode.Open, FileAccess.Read))
                    {
                        using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            var DataSet = reader.AsDataSet();
                            foreach (DataTable pSheet in DataSet.Tables)
                            {
                                string strTableName = pSheet.TableName;
                                _mapWorkSheet.Add(strTableName, new SheetData(this, strTableName));
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

            // strSheetID = Path.GetFileNameWithoutExtension(strFileAbsolutePath_And_IncludeExtension);
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

        public IList<IList<Object>> ISheetConnector_GetSheetData(string strSheetName)
        {
            if (_mapDataTable.TryGetValue(strSheetName, out var pSheet) == false)
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

        public static string DoConvert_AbsolutePath(string strPath)
        {
            if (Path.IsPathRooted(strPath))
                return strPath;

            var pCurrentURI = new Uri(Directory.GetCurrentDirectory());
            return $"{pCurrentURI.AbsolutePath}/../{strPath}";
        }
    }
}
