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

            if (pCurrentProject.mapSaveSheetSource.TryGetValue(pSourceConnector.strSheetSourceID, out SaveData_SheetSource pSheetSource))
            {
                pSheetSource.eSourceType = pSourceConnector.eSheetSourceType;
                List<TypeData> listSavedTable = pSheetSource.listSheet;

                int iOrder = 0;
                foreach (KeyValuePair<string, SheetData> pSheet in pSourceConnector.mapWorkSheetData_Key_Is_SheetID)
                {
                    TypeData pTypeDataFind = listSavedTable.FirstOrDefault(x => (x.strSheetID == pSheet.Key));
                    if (pTypeDataFind == null)
                    {
                        listSavedTable.Add(new TypeData(pSourceConnector, pSheet.Value.strSheetID, pSheet.Value.strSheetName, iOrder));
                    }
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
                pSheetSource = new SaveData_SheetSource(pSourceConnector.strSheetSourceID, pSourceConnector.eSheetSourceType);
                _mapSaveSheetSource[pSheetSource.strSheetSourceID] = pSheetSource;

                int iOrder = 0;
                pSheetSource.listSheet.Clear();
                foreach (KeyValuePair<string, SheetData> pSheet in pSourceConnector.mapWorkSheetData_Key_Is_SheetID)
                    pSheetSource.listSheet.Add(new TypeData(pSourceConnector, pSheet.Value.strSheetID, pSheet.Value.strSheetName, iOrder++));

                pCurrentProject.DoAdd_SheetSource(pSheetSource);
                pCurrentProject.DoSave();

                WriteConsole("새 파일을 만들었습니다.");
            }

            listView_Sheet.Items.Clear();
            pSheetSource.DoRefreshFieldData(pSourceConnector);
            for (int i = 0; i < pSheetSource.listSheet.Count; i++)
                listView_Sheet.Items.Add(pSheetSource.listSheet[i].ConvertListViewItem());

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


        private SaveData_Project GetSheetSource_LastEdit(Dictionary<string, SaveData_Project> mapSaveData)
        {
            WriteConsole("마지막에 작업한 프로젝트를 찾는 중..");

            DateTime date_LastEdit = DateTime.MinValue;
            SaveData_Project pSheetSourceLastEdit = null;
            foreach (var pSheet in mapSaveData.Values)
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

        //private void checkBox_AutoConnect_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (_bIsLoading_CreateForm)
        //        return;

        //    // _pConfig.bAutoConnect = checkBox_AutoConnect.Checked;
        //    AutoSaveAsync_Config();
        //}
    }
}
