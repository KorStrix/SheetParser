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
            SheetData pSheetData = listView_Sheet.SelectedItems.Cast<SheetData>().FirstOrDefault();
            if (pSheetData == null)
                return;
            WriteConsole("선택한 시트만 코드 파일 생성중.." + pSheetData.strFileName);
            StartBuild(pSheetData);
        }

        private void button_StartParsing_Click(object sender, EventArgs e)
        {
            WriteConsole("코드 파일 생성중..");
            StartBuild(listView_Sheet.CheckedItems.Cast<SheetData>().ToArray());
        }

        private void StartBuild(params SheetData[] arrTypeData)
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
                foreach (SheetData pSheetData in arrTypeData)
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
            SheetData[] arrSheetData = listView_Sheet.CheckedItems.Cast<SheetData>().ToArray();
            CheckedListBox.CheckedItemCollection listWork = checkedListBox_BuildList.CheckedItems;
            foreach (BuildBase pWork in listWork)
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

            foreach (BuildBase pWork in listWork)
                pWork.DoWorkAfter();

            WriteConsole($"빌드 완료 \r\n {pTimer_Total.Elapsed}\r\n");
        }

        private void button_AddWork_Click(object sender, EventArgs e)
        {
            BuildBase pBuild = (BuildBase)comboBox_BuildList.SelectedItem;
            if (pBuild == null)
                return;

            BuildBase pNewBuild = pBuild.CopyInstance();
            pNewBuild.ShowForm();
            pCurrentProject.DoAdd_Build(pNewBuild);
        }

        private void button_WorkOrderUp_Click(object sender, EventArgs e)
        {
            int iSelectedIndex = checkedListBox_BuildList.SelectedIndex;
            object pSelectedItem = checkedListBox_BuildList.SelectedItem;

            if (iSelectedIndex == 0)
                return;

            if (pSelectedItem == null)
                return;

            checkedListBox_BuildList.SelectedIndex = -1;
            checkedListBox_BuildList.Items.RemoveAt(iSelectedIndex);
            checkedListBox_BuildList.Items.Insert(iSelectedIndex - 1, pSelectedItem);
            checkedListBox_BuildList.SelectedIndex = iSelectedIndex - 1;
            Update_BuildListOrder();
        }

        private void button_WorkOrderDown_Click(object sender, EventArgs e)
        {
            int iSelectedIndex = checkedListBox_BuildList.SelectedIndex;
            object pSelectedItem = checkedListBox_BuildList.SelectedItem;

            if (iSelectedIndex == checkedListBox_BuildList.Items.Count - 1)
                return;

            if (pSelectedItem == null)
                return;

            checkedListBox_BuildList.SelectedIndex = -1;
            checkedListBox_BuildList.Items.RemoveAt(iSelectedIndex);
            checkedListBox_BuildList.Items.Insert(iSelectedIndex + 1, pSelectedItem);
            checkedListBox_BuildList.SelectedIndex = iSelectedIndex + 1;
            Update_BuildListOrder();
        }

        void Update_BuildListOrder()
        {
            int iSortOrder = 0;
            foreach (BuildBase pWork in checkedListBox_BuildList.Items)
                pWork.iWorkOrder = iSortOrder++;

            AutoSaveAsync_CurrentProject();
        }

        private void button_EditWork_Click(object sender, EventArgs e)
        {
            BuildBase pBuild = (BuildBase)checkedListBox_BuildList.SelectedItem;
            pBuild.ShowForm();
        }

        private void button_RemoveWork_Click(object sender, EventArgs e)
        {
            pCurrentProject.DoRemove_Build(checkedListBox_BuildList.SelectedIndex);
        }

        private void CheckedListBoxBuildListSelectedIndexChanged(object sender, EventArgs e)
        {
            bool bIsSelected = checkedListBox_BuildList.SelectedIndex != -1;
            groupBox_3_1_SelectedBuild.Enabled = bIsSelected;
            button_RemoveWork.Enabled = bIsSelected;
        }

        private void CheckedListBoxBuildListItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_bIsConnecting)
                return;

            BuildBase pWork = pCurrentProject.GetWork_OrNull(e.Index);
            if(pWork != null)
                pWork.bEnable = e.NewValue == CheckState.Checked;

            AutoSaveAsync_CurrentProject();
        }

    }
}
