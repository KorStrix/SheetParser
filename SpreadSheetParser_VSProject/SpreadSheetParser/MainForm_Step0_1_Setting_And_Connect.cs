using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SpreadSheetParser
{
    public partial class SheetParser_MainForm
    {
        private void OnFinishConnect(SheetSourceConnector pSourceConnector, Exception pException_OnError)
        {
            if (pException_OnError != null)
            {
                WriteConsole("연결 실패 " + pException_OnError);
                return;
            }

            if (_mapSaveData.ContainsKey(pSourceConnector.strSheetSourceID))
            {
                pSheetSourceCurrentConnected = _mapSaveData[pSourceConnector.strSheetSourceID];
                pSheetSourceCurrentConnected.eSourceType = pSourceConnector.eSheetSourceType;
                List<TypeData> listSavedTable = pSheetSourceCurrentConnected.listTable;

                int iOrder = 0;
                foreach (KeyValuePair<string, SheetData> pSheet in pSourceConnector.mapWorkSheetData_Key_Is_SheetID)
                {
                    TypeData pTypeDataFind = listSavedTable.FirstOrDefault(x => (x.strSheetID == pSheet.Key));
                    if (pTypeDataFind == null)
                        listSavedTable.Add(new TypeData(pSourceConnector, pSheet.Value.strSheetID, pSheet.Value.strSheetName, iOrder));
                    else
                    {
                        // 이미 저장되있는 Sheet의 경우
                        // 웹에 있는 SheetName과 로컬의 SheetName이 다를 수 있기 때문에 갱신
                        pTypeDataFind.strSheetName = pSheet.Value.strSheetName;
                        pTypeDataFind.iOrder = iOrder;
                        pTypeDataFind.DoSetSheetSourceConnector(pSourceConnector);
                    }

                    iOrder++;
                }
            }
            else
            {
                pSheetSourceCurrentConnected = new SaveData_SheetSource(pSourceConnector.strSheetSourceID, pSourceConnector.eSheetSourceType);
                _mapSaveData[pSheetSourceCurrentConnected.strSheetSourceID] = pSheetSourceCurrentConnected;

                int iOrder = 0;
                pSheetSourceCurrentConnected.listTable.Clear();
                foreach (KeyValuePair<string, SheetData> pSheet in pSourceConnector.mapWorkSheetData_Key_Is_SheetID)
                    pSheetSourceCurrentConnected.listTable.Add(new TypeData(pSourceConnector, pSheet.Value.strSheetID, pSheet.Value.strSheetName, iOrder++));

                SaveDataManager.SaveSheet(pSheetSourceCurrentConnected);

                WriteConsole("새 파일을 만들었습니다.");
            }

            listView_Sheet.Items.Clear();
            List<TypeData> listSheetSaved = pSheetSourceCurrentConnected.listTable;
            listSheetSaved.Sort((x, y) => x.iOrder.CompareTo(y.iOrder));

            TypeData[] arrSheetDelete = listSheetSaved.Where((pSheet) =>
                pSourceConnector.mapWorkSheetData_Key_Is_SheetID.Values.Any(p => p.strSheetID == pSheet.strSheetID) == false).ToArray();

            if (arrSheetDelete.Length > 0)
            {
                for (int i = 0; i < arrSheetDelete.Length; i++)
                    listSheetSaved.Remove(arrSheetDelete[i]);

                AutoSaveAsync_CurrentSheet();
            }
            ClearFieldData(listSheetSaved);

            for (int i = 0; i < listSheetSaved.Count; i++)
                listView_Sheet.Items.Add(listSheetSaved[i].ConvertListViewItem());

            checkedListBox_WorkList.Items.Clear();
            List<WorkBase> listWorkBase = pSheetSourceCurrentConnected.listSaveWork;
            listWorkBase.Sort((x, y) => x.iWorkOrder.CompareTo(y.iWorkOrder));
            for (int i = 0; i < listWorkBase.Count; i++)
                checkedListBox_WorkList.Items.Add(listWorkBase[i], listWorkBase[i].bEnable);

            SetState(EState.IsConnected);
            WriteConsole("연결 성공");
            _bIsConnecting = false;
        }


        //private void button_SelectExcelFile_Click(object sender, EventArgs e)
        //{
        //    DoShowFileBrowser_And_SavePath(false, ref textBox_ExcelPath_ForConnect, 
        //        (string strFilePath, ref string strError) =>
        //        {
        //            if (strFilePath.Contains(".xlsx") == false)
        //                strError = "확장자가 xlsx여야만 합니다.";
        //        }
        //        );
        //}


        private static void ClearFieldData(List<TypeData> listSheetSaved)
        {
            for (int i = 0; i < listSheetSaved.Count; i++)
            {
                var listFieldGroup = listSheetSaved[i].listFieldData.GroupBy(p => p.strFieldName).ToList();
                foreach (var listField in listFieldGroup)
                {
                    int iCount = listField.Count();
                    foreach (var pField in listField.Reverse())
                    {
                        if (--iCount >= 1)
                            listSheetSaved[i].listFieldData.Remove(pField);
                    }
                }

                var listTemp = listSheetSaved[i].listFieldData.Where(p => p.bIsTemp);
                foreach (var pTempField in listTemp)
                    listSheetSaved[i].listFieldData.Remove(pTempField);
            }
        }

        private SaveData_SheetSource GetSheetSource_LastEdit(Dictionary<string, SaveData_SheetSource> mapSaveData)
        {
            WriteConsole("마지막에 수정한 Sheet 찾는 중..");

            DateTime date_LastEdit = DateTime.MinValue;
            SaveData_SheetSource pSheetSourceLastEdit = null;
            foreach (var pSheet in mapSaveData.Values)
            {
                if (date_LastEdit < pSheet.date_LastEdit)
                {
                    date_LastEdit = pSheet.date_LastEdit;
                    pSheetSourceLastEdit = pSheet;
                }
            }

            if (pSheetSourceLastEdit != null)
                WriteConsole($"마지막에 수정한 시트를 찾았다. 수정한 시간 {pSheetSourceLastEdit.date_LastEdit}, SheetID {pSheetSourceLastEdit.strSheetSourceID}");
            else
                WriteConsole($"마지막에 수정한 시트를 못찾았다..");

            return pSheetSourceLastEdit;
        }

        private void Button_OpenPath_SaveSheet_Click(object sender, EventArgs e)
        {
            string strSaveFolderPath = SaveDataManager.const_strSaveFolderPath;
            DoOpenFolder(strSaveFolderPath.Remove(strSaveFolderPath.Length - 1, 1));
        }

        private void checkBox_AutoConnect_CheckedChanged(object sender, EventArgs e)
        {
            if (_bIsLoading_CreateForm)
                return;

            _pConfig.bAutoConnect = checkBox_AutoConnect.Checked;
            AutoSaveAsync_Config();
        }
    }
}
