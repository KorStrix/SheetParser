using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SpreadSheetParser
{
    public partial class SpreadSheetParser_MainForm
    {
        private void button_Connect_Click(object sender, EventArgs e)
        {
            _bIsConnecting = true;

            string strSheetID = textBox_SheetID.Text;
            WriteConsole($"연결 시작 Sheet ID : {strSheetID}");

            //// 테스트 시트
            //// https://docs.google.com/spreadsheets/d/1_s89xLPwidVwRsmGS4bp3Y6huaLWoBDq7SUW7lYyxl4/edit#gid=0
            //strSheetID = "1_s89xLPwidVwRsmGS4bp3Y6huaLWoBDq7SUW7lYyxl4";

            checkedListBox_SheetList.Items.Clear();
            checkedListBox_WorkList.Items.Clear();
            pSheetConnector.ISheetConnector_DoConnect_And_Parsing(strSheetID, OnFinishConnect);
        }

        private void OnFinishConnect(ISheetConnector pConnector, Exception pException_OnError)
        {
            if (pException_OnError != null)
            {
                WriteConsole("연결 실패 " + pException_OnError);
                return;
            }

            if (_mapSaveData.ContainsKey(pConnector.strSheetID))
            {
                pSpreadSheet_CurrentConnected = _mapSaveData[pConnector.strSheetID];
                pSpreadSheet_CurrentConnected.eType = pConnector.eSheetType;
                List<TypeData> listSavedTable = pSpreadSheet_CurrentConnected.listTable;

                int iOrder = 0;
                foreach (KeyValuePair<string, SheetData> pSheet in pConnector.mapWorkSheetData_Key_Is_SheetID)
                {
                    TypeData pTypeDataFind = listSavedTable.FirstOrDefault(x => (x.strSheetID == pSheet.Key));
                    if (pTypeDataFind == null)
                        listSavedTable.Add(new TypeData(pSheet.Value.strSheetID, pSheet.Value.strSheetName, iOrder));
                    else
                    {
                        // 이미 저장되있는 Sheet의 경우
                        // 웹에 있는 SheetName과 로컬의 SheetName이 다를 수 있기 때문에 갱신
                        pTypeDataFind.strSheetName = pSheet.Value.strSheetName;
                        pTypeDataFind.iOrder = iOrder;
                    }

                    iOrder++;
                }
            }
            else
            {
                pSpreadSheet_CurrentConnected = new SaveData_SpreadSheet(pConnector.strSheetID, pConnector.eSheetType);
                _mapSaveData[pSpreadSheet_CurrentConnected.strSheetID] = pSpreadSheet_CurrentConnected;

                int iOrder = 0;
                pSpreadSheet_CurrentConnected.listTable.Clear();
                foreach (KeyValuePair<string, SheetData> pSheet in pConnector.mapWorkSheetData_Key_Is_SheetID)
                    pSpreadSheet_CurrentConnected.listTable.Add(new TypeData(pSheet.Value.strSheetID, pSheet.Value.strSheetName, iOrder++));

                SaveDataManager.SaveSheet(pSpreadSheet_CurrentConnected);

                WriteConsole("새 파일을 만들었습니다.");
            }

            checkedListBox_SheetList.Items.Clear();
            List<TypeData> listSheetSaved = pSpreadSheet_CurrentConnected.listTable;
            listSheetSaved.Sort((x, y) => x.iOrder.CompareTo(y.iOrder));

            TypeData[] arrSheetDelete = listSheetSaved.Where((pSheet) =>
                pConnector.mapWorkSheetData_Key_Is_SheetID.Values.Any(p => p.strSheetID == pSheet.strSheetID) == false).ToArray();

            if (arrSheetDelete.Length > 0)
            {
                for (int i = 0; i < arrSheetDelete.Length; i++)
                    listSheetSaved.Remove(arrSheetDelete[i]);

                AutoSaveAsync_CurrentSheet();
            }
            ClearFieldData(listSheetSaved);

            for (int i = 0; i < listSheetSaved.Count; i++)
                checkedListBox_SheetList.Items.Add(listSheetSaved[i], listSheetSaved[i].bEnable);

            checkedListBox_WorkList.Items.Clear();
            List<WorkBase> listWorkBase = pSpreadSheet_CurrentConnected.listSaveWork;
            listWorkBase.Sort((x, y) => x.iWorkOrder.CompareTo(y.iWorkOrder));
            for (int i = 0; i < listWorkBase.Count; i++)
                checkedListBox_WorkList.Items.Add(listWorkBase[i], listWorkBase[i].bEnable);

            SetState(EState.IsConnected);
            WriteConsole("연결 성공");
            _bIsConnecting = false;
        }


        private void button_SelectExcelFile_Click(object sender, EventArgs e)
        {
            DoShowFileBrowser_And_SavePath(false, ref textBox_ExcelPath_ForConnect, 
                (string strFilePath, ref string strError) =>
                {
                    if (strFilePath.Contains(".xlsx") == false)
                        strError = "확장자가 xlsx여야만 합니다.";
                }
                );
        }

        private void button_Connect_Excel_Click(object sender, EventArgs e)
        {
            _bIsConnecting = true;

            string strSheetID = textBox_ExcelPath_ForConnect.Text;
            WriteConsole($"연결 시작 Sheet ID : {strSheetID}");

            checkedListBox_WorkList.Items.Clear();
            checkedListBox_SheetList.Items.Clear();

            pSheetConnector.ISheetConnector_DoConnect_And_Parsing(textBox_ExcelPath_ForConnect.Text, OnFinishConnect);
        }

        private void button_OpenExcel_Click(object sender, EventArgs e)
        {
            FileInfo pFileInfo = new FileInfo(DoMake_AbsolutePath(textBox_ExcelPath_ForConnect.Text));
            if (pFileInfo.Exists)
                System.Diagnostics.Process.Start(textBox_ExcelPath_ForConnect.Text);
            else
                WriteConsole("Not Exists Excel File - " + textBox_ExcelPath_ForConnect.Text);
        }


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

        private SaveData_SpreadSheet GetSheet_LastEdit(Dictionary<string, SaveData_SpreadSheet> mapSaveData)
        {
            WriteConsole("마지막에 수정한 Sheet 찾는 중..");

            DateTime date_LastEdit = DateTime.MinValue;
            SaveData_SpreadSheet pSheet_LastEdit = null;
            foreach (var pSheet in mapSaveData.Values)
            {
                if (date_LastEdit < pSheet.date_LastEdit)
                {
                    date_LastEdit = pSheet.date_LastEdit;
                    pSheet_LastEdit = pSheet;
                }
            }

            if (pSheet_LastEdit != null)
                WriteConsole($"마지막에 수정한 시트를 찾았다. 수정한 시간 {pSheet_LastEdit.date_LastEdit}, SheetID {pSheet_LastEdit.strSheetID}");
            else
                WriteConsole($"마지막에 수정한 시트를 못찾았다..");

            return pSheet_LastEdit;
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

        private void button_OpenLink_Click(object sender, EventArgs e)
        {
            const string const_SheetURL = "https://docs.google.com/spreadsheets/d/";

            System.Diagnostics.Process.Start($"{const_SheetURL}/{textBox_SheetID.Text}");
        }

        private void comboBox_SaveSheet_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox_SheetID.Text = comboBox_SaveSheet.Text;
        }
    }
}
