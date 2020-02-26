﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadSheetParser
{
    public partial class SpreadSheetParser_MainForm
    {
        private void button_StartParsing_Selected_Click(object sender, EventArgs e)
        {
            TypeData pSheetData = (TypeData)checkedListBox_SheetList.SelectedItem;
            if(pSheetData != null)
            {
                WriteConsole("선택한 시트만 코드 파일 생성중.." + pSheetData.strFileName);
                _pCodeFileBuilder = new CodeFileBuilder();

                listView_Field.Items.Clear();
                UpdateSheetData(pSheetData, false, true);
                pSheetData.DoWork(pSheetConnector, _pCodeFileBuilder, WriteConsole);
            }

            var listWork = checkedListBox_WorkList.CheckedItems;
            foreach (WorkBase pWork in listWork)
            {
                try
                {
                    pWork.DoWork(_pCodeFileBuilder, pSheetConnector, new TypeData[] { pSheetData }, SpreadSheetParser_MainForm.WriteConsole);
                }
                catch (Exception pException)
                {
                    WriteConsole($"빌드 중 에러 - Work : {pWork.pType} // Error : {pException}");
                    return;
                }
            }

            foreach (WorkBase pWork in listWork)
                pWork.DoWorkAfter();

            WriteConsole("빌드 완료");
        }

        private void button_StartParsing_Click(object sender, EventArgs e)
        {
            WriteConsole("코드 파일 생성중..");
            _pCodeFileBuilder = new CodeFileBuilder();

            Stopwatch pTimer_Total = new Stopwatch();
            pTimer_Total.Start();

            Stopwatch pTimer = new Stopwatch();
            try
            {
                int iLoopCount = 0;
                int iCount = checkedListBox_SheetList.CheckedItems.Count;
                foreach (var pItem in checkedListBox_SheetList.CheckedItems)
                {
                    pTimer.Restart();

                    TypeData pSheetData = (TypeData)pItem;
                    listView_Field.Items.Clear();
                    UpdateSheetData(pSheetData, false, ++iLoopCount == iCount);
                    pSheetData.DoWork(pSheetConnector, _pCodeFileBuilder, WriteConsole);

                    pTimer.Stop();
                    WriteConsole(pSheetData.strFileName + $" 작업완료 \r\n{pTimer.Elapsed}\r\n");
                }
            }
            catch (Exception pException)
            {
                WriteConsole("빌드 실패.." + pException);
                return;
            }

            var listSheetData = checkedListBox_SheetList.CheckedItems.Cast<TypeData>();
            var listWork = checkedListBox_WorkList.CheckedItems;
            foreach (WorkBase pWork in listWork)
            {
                try
                {
                    pWork.DoWork(_pCodeFileBuilder, pSheetConnector, listSheetData, SpreadSheetParser_MainForm.WriteConsole);
                }
                catch (Exception pException)
                {
                    WriteConsole($"빌드 중 에러 - Work : {pWork.pType} // Error : {pException}");
                    return;
                }
            }

            foreach (WorkBase pWork in listWork)
                pWork.DoWorkAfter();

            WriteConsole($"빌드 완료 \r\n {pTimer_Total.Elapsed}\r\n");
        }

        private void button_AddWork_Click(object sender, EventArgs e)
        {
            WorkBase pWork = (WorkBase)comboBox_WorkList.SelectedItem;
            if (pWork == null)
                return;

            WorkBase pNewWork = pWork.CopyInstance();
            pNewWork.ShowForm();
            checkedListBox_WorkList.Items.Add(pNewWork, true);
            pSpreadSheet_CurrentConnected.listSaveWork.Add(pNewWork);
            Update_WorkListOrder();
        }

        private void button_WorkOrderUp_Click(object sender, EventArgs e)
        {
            int iSelectedIndex = checkedListBox_WorkList.SelectedIndex;
            object pSelectedItem = checkedListBox_WorkList.SelectedItem;

            if (iSelectedIndex == 0)
                return;

            if (pSelectedItem == null)
                return;

            checkedListBox_WorkList.SelectedIndex = -1;
            checkedListBox_WorkList.Items.RemoveAt(iSelectedIndex);
            checkedListBox_WorkList.Items.Insert(iSelectedIndex - 1, pSelectedItem);
            checkedListBox_WorkList.SelectedIndex = iSelectedIndex - 1;
            Update_WorkListOrder();
        }

        private void button_WorkOrderDown_Click(object sender, EventArgs e)
        {
            int iSelectedIndex = checkedListBox_WorkList.SelectedIndex;
            object pSelectedItem = checkedListBox_WorkList.SelectedItem;

            if (iSelectedIndex == checkedListBox_WorkList.Items.Count - 1)
                return;

            if (pSelectedItem == null)
                return;

            checkedListBox_WorkList.SelectedIndex = -1;
            checkedListBox_WorkList.Items.RemoveAt(iSelectedIndex);
            checkedListBox_WorkList.Items.Insert(iSelectedIndex + 1, pSelectedItem);
            checkedListBox_WorkList.SelectedIndex = iSelectedIndex + 1;
            Update_WorkListOrder();
        }

        void Update_WorkListOrder()
        {
            int iSortOrder = 0;
            foreach (WorkBase pWork in checkedListBox_WorkList.Items)
                pWork.iWorkOrder = iSortOrder++;

            AutoSaveAsync_CurrentSheet();
        }

        private void button_EditWork_Click(object sender, EventArgs e)
        {
            WorkBase pWork = (WorkBase)checkedListBox_WorkList.SelectedItem;
            pWork.ShowForm();
        }

        private void button_RemoveWork_Click(object sender, EventArgs e)
        {
            pSpreadSheet_CurrentConnected.listSaveWork.RemoveAt(checkedListBox_WorkList.SelectedIndex);
            checkedListBox_WorkList.Items.RemoveAt(checkedListBox_WorkList.SelectedIndex);
            Update_WorkListOrder();
        }

        private void CheckedListBox_WorkList_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool bIsSelected = checkedListBox_WorkList.SelectedIndex != -1;
            groupBox_3_1_SelectedWork.Enabled = bIsSelected;
            button_RemoveWork.Enabled = bIsSelected;
        }

        private void CheckedListBox_WorkList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_bIsConnecting)
                return;

            pSpreadSheet_CurrentConnected.listSaveWork[e.Index].bEnable = e.NewValue == CheckState.Checked;
            AutoSaveAsync_CurrentSheet();
        }

    }
}
