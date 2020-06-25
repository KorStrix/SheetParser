using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SpreadSheetParser
{
    public partial class SheetParser_MainForm
    {
        private void OnFinishConnect_SheetSource(SheetSourceConnector pSourceConnector, Exception pException_OnError)
        {
            if (pException_OnError != null)
            {
                WriteConsole($"시트소스 : {pSourceConnector.strSheetSourceID} 연결 실패 " + pException_OnError);
                return;
            }

            // 시트소스가 이미 로컬 세이브 데이터에 있었던 경우
            if (TryGetSheetSourceData(pSourceConnector.strSheetSourceID, out SaveData_SheetSource pSheetSource))
            {
                pSheetSource.eSourceType = pSourceConnector.eSheetSourceType;
                List<SheetData> listSavedTable = pSheetSource.listSheet;

                int iOrder = 0;
                foreach (KeyValuePair<string, SheetConnectorWrapper> pSheet in pSourceConnector.mapWorkSheetData_Key_Is_SheetID)
                {
                    SheetData pSheetDataFind = listSavedTable.FirstOrDefault(x => (x.strSheetID == pSheet.Key));
                    if (pSheetDataFind == null)
                    {
                        listSavedTable.Add(new SheetData(pSourceConnector, pSheet.Value.strSheetID, pSheet.Value.strSheetName, iOrder));
                    }
                    else
                    {
                        // 이미 로컬에 저장되있는 Sheet의 경우
                        // 웹에 있는 SheetName과 로컬의 SheetName이 다를 수 있기 때문에 갱신
                        pSheetDataFind.strSheetName = pSheet.Value.strSheetName;
                        pSheetDataFind.iOrder = iOrder;
                        pSheetDataFind.DoSetSheetSourceConnector(pSourceConnector);
                    }

                    iOrder++;
                }
            }
            else
            {
                pSheetSource = new SaveData_SheetSource(pSourceConnector.strSheetSourceID, pSourceConnector.eSheetSourceType);
                _mapSaveSheetSource[pSheetSource.strSheetSourceID] = pSheetSource;

                int iOrder = 0;
                foreach (KeyValuePair<string, SheetConnectorWrapper> pSheet in pSourceConnector.mapWorkSheetData_Key_Is_SheetID)
                    pSheetSource.listSheet.Add(new SheetData(pSourceConnector, pSheet.Value.strSheetID, pSheet.Value.strSheetName, iOrder++));

                try
                {
                    pSheetSource.DoSave();
                    pCurrentProject.DoAdd_SheetSource(pSheetSource);
                    pCurrentProject.DoSave();
                }
                catch (Exception e)
                {
                    WriteConsole("여기해야함!!");
                }

                WriteConsole("새 프로젝트를 만들었습니다.");
            }

            UpdateUI_SheetList(pSourceConnector, pSheetSource);

            SetState(EState.IsConnected);
            WriteConsole($"시트소스 {pSourceConnector.strSheetSourceID} 연결 성공");
            _bIsConnecting = false;
        }

        private void UpdateUI_SheetList(SheetSourceConnector pSourceConnector, SaveData_SheetSource pSheetSource)
        {
            // 멀티 시트 소스라 클리어 안해도 됨, 대신 해당 시트 소스 단위로 중복체크 해야함
            // listView_Sheet.Items.Clear(); 
            WriteConsole("여기 중복 체크 해야함");

            pSheetSource.DoRefreshFieldData(pSourceConnector);
            pSheetSource.listSheet.ForEach(p => listView_Sheet.Items.RemoveByKey(p.strSheetName));
            for (int i = 0; i < pSheetSource.listSheet.Count; i++)
                listView_Sheet.Items.Add(pSheetSource.listSheet[i].ConvertListViewItem());
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


        private SaveData_Project GetSheetSource_LastEdit(Dictionary<string, SaveData_Project> mapSaveData)
        {
            WriteConsole("마지막에 작업한 프로젝트를 찾는 중..");

            DateTime date_LastEdit = DateTime.MinValue;
            SaveData_Project pSheetSourceLastEdit = null;
            foreach (SaveData_Project pSheet in mapSaveData.Values)
            {
                if (date_LastEdit < pSheet.date_LastEdit)
                {
                    date_LastEdit = pSheet.date_LastEdit;
                    pSheetSourceLastEdit = pSheet;
                }
            }

            if (pSheetSourceLastEdit != null)
                WriteConsole($"마지막에 수정한 프로젝트를 찾았다. 수정한 시간 {pSheetSourceLastEdit.date_LastEdit}, SheetID {pSheetSourceLastEdit.strProjectName}");
            else
                WriteConsole($"마지막에 수정한 프로젝트를 못찾았다..");

            return pSheetSourceLastEdit;
        }

        private void UpdateWorkList(SaveData_Project pProject)
        {
            List<BuildBase> listWorkBase = pProject.listSaveBuild;
            listWorkBase.Sort((x, y) => x.iWorkOrder.CompareTo(y.iWorkOrder));
            for (int i = 0; i < listWorkBase.Count; i++)
                checkedListBox_BuildList.Items.Add(listWorkBase[i], listWorkBase[i].bEnable);
        }

        private void Button_OpenPath_SaveSheet_Click(object sender, EventArgs e)
        {
            string strSaveFolderPath = SaveDataManager.const_strSaveFolderPath;
            DoOpenFolder(strSaveFolderPath.Remove(strSaveFolderPath.Length - 1, 1));
        }

        private void button_Remove_SelectedSheetSource_Click(object sender, EventArgs e)
        {
            SheetSourceConnector pSource = GetCurrentSelected_SheetSource_OrNull();
            if (pSource == null)
                return;

            IEnumerable<ListViewItem> arrRemoveItem = listView_Sheet.Items.
                Cast<System.Windows.Forms.ListViewItem>().
                Where(pViewItem => (pViewItem.Tag as SheetData).pSheetSourceConnector == pSource);
            
            foreach(var pItem in arrRemoveItem)
                listView_Sheet.Items.Remove(pItem);

            pCurrentProject.DoRemove_SheetSource(pSource.strSheetSourceID);
            pCurrentProject.DoSave();

            WriteConsole($"소스 {pSource.strSheetSourceID} 삭제");
        }

        //private void checkBox_AutoConnect_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (_bIsLoading_CreateForm)
        //        return;

        //    // _pConfig.bAutoConnect = checkBox_AutoConnect.Checked;
        //    AutoSaveAsync_Config();
        //}
    }
}
