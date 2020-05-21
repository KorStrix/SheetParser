using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadSheetParser
{
    public partial class SheetParser_MainForm
    {
        private void button_StartParsing_Selected_Click(object sender, EventArgs e)
        {
            TypeData pSheetData = listView_Sheet.SelectedItems.Cast<TypeData>().FirstOrDefault();
            if (pSheetData == null)
                return;
            WriteConsole("선택한 시트만 코드 파일 생성중.." + pSheetData.strFileName);
            StartBuild(pSheetData);
        }

        private void button_StartParsing_Click(object sender, EventArgs e)
        {
            WriteConsole("코드 파일 생성중..");
            StartBuild(listView_Sheet.CheckedItems.Cast<TypeData>().ToArray());
        }

        private void StartBuild(params TypeData[] arrTypeData)
        {
            _pCodeFileBuilder = new CodeFileBuilder();

            List<Task> listTask = new List<Task>();
            Stopwatch pTimer_Total = new Stopwatch();
            pTimer_Total.Start();

            Stopwatch pTimer = new Stopwatch();
            try
            {
                int iLoopCount = 0;
                int iCount = arrTypeData.Length;
                foreach (var pSheetData in arrTypeData)
                {
                    pTimer.Restart();

                    // 왜 Update했는지 까먹었다.. 일단 상관없어보이니 주석처리
                    // listView_Field.Items.Clear();
                    // UpdateSheetData(pSheetData, false, ++iLoopCount == iCount);

                    // 동기버전
                    // pSheetData.DoWork(pSheetSourceConnector, _pCodeFileBuilder, WriteConsole);

                    // 비동기버전
                    listTask.Add(pSheetData.DoWork(_pCodeFileBuilder, WriteConsole));

                    pTimer.Stop();
                    WriteConsole(pSheetData.strFileName + $" 작업완료 \r\n{pTimer.Elapsed}\r\n");
                }
            }
            catch (Exception pException)
            {
                WriteConsole("빌드 실패.." + pException);
                return;
            }


            Task.WaitAll(listTask.ToArray());


            listTask.Clear();
            var arrSheetData = listView_Sheet.CheckedItems.Cast<TypeData>().ToArray();
            var listWork = checkedListBox_WorkList.CheckedItems;
            foreach (WorkBase pWork in listWork)
            {
                try
                {
                    pTimer.Restart();

                    listTask.Add(pWork.DoWork(_pCodeFileBuilder, arrSheetData, WriteConsole));
                }
                catch (Exception pException)
                {
                    WriteConsole($"빌드 중 에러 - Work : {pWork.pType} // Error : {pException}");
                    return;
                }
                finally
                {
                    pTimer.Stop();
                    WriteConsole(pWork.ToString() + $" 작업완료 \r\n{pTimer.Elapsed}\r\n");
                }
            }
            
            Task.WaitAll(listTask.ToArray());

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
            pSheetSourceCurrentConnected.listSaveWork.Add(pNewWork);
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
            pSheetSourceCurrentConnected.listSaveWork.RemoveAt(checkedListBox_WorkList.SelectedIndex);
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

            if(e.Index < pSheetSourceCurrentConnected.listSaveWork.Count)
                pSheetSourceCurrentConnected.listSaveWork[e.Index].bEnable = e.NewValue == CheckState.Checked;
            AutoSaveAsync_CurrentSheet();
        }

    }
}
