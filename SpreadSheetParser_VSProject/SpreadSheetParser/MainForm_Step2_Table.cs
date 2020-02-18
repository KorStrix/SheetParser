using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadSheetParser
{
    public partial class SpreadSheetParser_MainForm
    {
        private void ListView_Field_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView_Field.Items.Clear();

            FieldTypeData pFieldData = null;
            bool bEnable = listView_Field.SelectedIndices.Count > 0;
            if (bEnable)
            {
                pFieldData = (FieldTypeData)listView_Field.SelectedItems[0].Tag;
            }
            else
            {
                groupBox_2_2_SelectedField.Enabled = false;
                groupBox_2_2_SelectedField_Virtual.Enabled = false;
                checkBox_Field_ThisIsKey.Checked = false;
                checkBox_FieldKey_IsOverlap.Checked = false;

                textBox_FieldName.Text = "";
                textBox_Type.Text = "";
                comboBox_DependencyField.SelectedText = "";
                comboBox_DependencyField_Sub.SelectedText = "";

                return;
            }

            TypeData pSheetData = (TypeData)checkedListBox_SheetList.SelectedItem;
            if (pSheetData == null)
                return;

            bool bIsEnum = pSheetData.eType == ESheetType.Enum;
            if (bIsEnum)
                return;

            comboBox_DependencyField.Items.Clear();
            comboBox_DependencyField.Items.AddRange(pSheetData.listFieldData.Where((pOption) => pOption.strFieldType == "string").Select((pOption) => pOption.strFieldName).ToArray());

            comboBox_DependencyField_Sub.Items.Clear();
            comboBox_DependencyField_Sub.Items.AddRange(pSheetData.listFieldData.Where((pOption) => pOption.strFieldType == "string").Select((pOption) => pOption.strFieldName).ToArray());

            groupBox_2_2_SelectedField.Enabled = true;
            groupBox_2_2_SelectedField_Virtual.Enabled = pFieldData.bIsVirtualField;

            textBox_FieldName.Text = pFieldData.strFieldName;
            textBox_Type.Text = pFieldData.strFieldType;

            checkBox_ConvertStringToEnum.Enabled = pFieldData.strFieldType == "string";
            if (checkBox_ConvertStringToEnum.Enabled)
                checkBox_ConvertStringToEnum.Checked = pFieldData.bConvertStringToEnum;
            else
                checkBox_ConvertStringToEnum.Checked = false;

            textBox_EnumName.Enabled = pFieldData.bConvertStringToEnum;
            if (pFieldData.bConvertStringToEnum)
                textBox_EnumName.Text = pFieldData.strEnumName;
            else
                textBox_EnumName.Text = "";


            checkBox_DeleteField_OnCode.Checked = pFieldData.bDeleteThisField_InCode;
            checkBox_IsHeaderField.Enabled = pFieldData.bDeleteThisField_InCode == false;
            if (checkBox_IsHeaderField.Enabled)
                checkBox_IsHeaderField.Checked = pSheetData.strHeaderFieldName == pFieldData.strFieldName;
            else
                checkBox_IsHeaderField.Checked = false;

            checkBox_Field_ThisIsKey.Checked = pFieldData.bIsKeyField;
            checkBox_FieldKey_IsOverlap.Enabled = pFieldData.bIsKeyField;
            if (checkBox_FieldKey_IsOverlap.Enabled)
                checkBox_FieldKey_IsOverlap.Checked = pFieldData.bIsOverlapKey;
            else
                checkBox_FieldKey_IsOverlap.Enabled = false;

            if (string.IsNullOrEmpty(pFieldData.strDependencyFieldName) == false)
                comboBox_DependencyField.SelectedIndex = comboBox_DependencyField.Items.IndexOf(pFieldData.strDependencyFieldName);

            if (string.IsNullOrEmpty(pFieldData.strDependencyFieldName_Sub) == false)
                comboBox_DependencyField_Sub.SelectedIndex = comboBox_DependencyField.Items.IndexOf(pFieldData.strDependencyFieldName_Sub);
        }

        private void CheckedListBox_SheetList_SelectedIndexChanged(object sender, EventArgs e)
        {

            ListView_Field_SelectedIndexChanged(null, null);
            TypeData pSheetData = (TypeData)checkedListBox_SheetList.SelectedItem;
            if (pSheetData == null)
                return;

            bool bIsEnum = pSheetData.eType == ESheetType.Enum;
            if (bIsEnum)
                return;

            UpdateSheetData(pSheetData);
        }

        private void UpdateSheetData(TypeData pSheetData)
        {
            int iDefinedTypeRow = -1;
            List<FieldTypeData> listFieldOption = pSheetData.listFieldData;
            HashSet<string> setRealField = new HashSet<string>();
            pSheetData.ParsingSheet(pSheetConnector,
            ((IList<object> listRow, string strText, int iRowIndex, int iColumnIndex) =>
            {
                if (strText.Contains(":") == false)
                    return;

                if (iDefinedTypeRow == -1)
                    iDefinedTypeRow = iRowIndex;

                if (iDefinedTypeRow != iRowIndex)
                    return;

                string[] arrText = strText.Split(':');
                string strField = arrText[0];
                setRealField.Add(strField);
                FieldTypeData[] arrFieldData = pSheetData.listFieldData.Where(((pField) => (pField.strFieldName == strField))).ToArray();
                if (arrFieldData.Length == 0)
                {
                    arrFieldData = new FieldTypeData[1];
                    arrFieldData[0] = new FieldTypeData(strField, arrText[1]);
                    pSheetData.listFieldData.Add(arrFieldData[0]);
                }

                if (arrFieldData.Length > 1)
                {
                    for (int i = 1; i < arrFieldData.Length; i++)
                        pSheetData.listFieldData.Remove(arrFieldData[i]);
                }

                listView_Field.Items.Add(arrFieldData[0].ConvertListViewItem());
            }));

            IEnumerable<FieldTypeData> pDeleteFieldOption = listFieldOption.Where((pFieldOption) => setRealField.Contains(pFieldOption.strFieldName) == false);
            if (pDeleteFieldOption.Count() != 0)
            {
                foreach (FieldTypeData pFieldOption in pDeleteFieldOption)
                {
                    pFieldOption.bIsVirtualField = true;
                    listView_Field.Items.Add(pFieldOption.ConvertListViewItem());
                }

                AutoSaveAsync_CurrentSheet();
            }
        }

        private void Update_Step_2_TableSetting(TypeData pSheetData)
        {
            _bIsUpdating_TableUI = true;

            _pSheet_CurrentConnected = pSheetData;
            if (pSheetData == null)
                return;

            groupBox_SelectedTable.Text = pSheetData.ToString();
            textBox_TableFileName.Text = pSheetData.strFileName;
            textBox_CommandLine.Text = pSheetData.strCommandLine;

            switch (pSheetData.eType)
            {
                case ESheetType.Class: radioButton_Class.Checked = true; break;
                case ESheetType.Struct: radioButton_Struct.Checked = true; break;
                case ESheetType.Enum: radioButton_Enum.Checked = true; break;
                case ESheetType.Global: radioButton_Global.Checked = true; break;

            }

            _bIsUpdating_TableUI = false;
        }

        private void OnChangeValue_TypeRadioButton(object sender, EventArgs e)
        {
            if (_bIsUpdating_TableUI)
                return;

            if (radioButton_Class.Checked)
            {
                _pSheet_CurrentConnected.eType = ESheetType.Class;
            }
            else if (radioButton_Struct.Checked)
            {
                _pSheet_CurrentConnected.eType = ESheetType.Struct;
            }
            else if (radioButton_Enum.Checked)
            {
                _pSheet_CurrentConnected.eType = ESheetType.Enum;
            }
            else if (radioButton_Global.Checked)
            {
                _pSheet_CurrentConnected.eType = ESheetType.Global;
            }

            AutoSaveAsync_CurrentSheet();
        }

        private void button_CheckTable_Click(object sender, EventArgs e)
        {
            TypeData pSheetData = GetCurrentSelectedTable_OrNull();
            WriteConsole("테이블 유효성 체크중.." + pSheetData.ToString());
            int iErrorCount = 0;

            try
            {
                pSheetData.DoCheck_IsValid_Table(pSheetConnector, WriteConsole);
            }
            catch (Exception pException)
            {
                WriteConsole("테이블 유효성 - 치명적인 에러 " + pException);
                return;
            }

            if (iErrorCount > 0)
                WriteConsole($"테이블 유효성 체크 - 에러, 개수 : {iErrorCount}");
            else
                WriteConsole("테이블 유효성 체크 - 이상없음");
        }

        private void button_Add_VirtualField_Click(object sender, EventArgs e)
        {
            FieldTypeData pFieldOption = new FieldTypeData("None", "None");
            pFieldOption.bIsVirtualField = true;

            _pSheet_CurrentConnected.listFieldData.Add(pFieldOption);
            listView_Field.Items.Add(pFieldOption.ConvertListViewItem());

            AutoSaveAsync_CurrentSheet();
        }

        private void button_Remove_VirtualField_Click(object sender, EventArgs e)
        {
            if (listView_Field.SelectedItems.Count == 0)
                return;

            var pSelectedItem = listView_Field.SelectedItems[0];
            FieldTypeData pFieldOption = (FieldTypeData)pSelectedItem.Tag;

            _pSheet_CurrentConnected.listFieldData.Remove(pFieldOption);
            listView_Field.Items.Remove(pSelectedItem);

            AutoSaveAsync_CurrentSheet();
        }

        private void button_Save_Field_Click(object sender, EventArgs e)
        {
            if (listView_Field.SelectedItems.Count == 0)
                return;

            var pSelectedItem = listView_Field.SelectedItems[0];
            FieldTypeData pFieldData = (FieldTypeData)pSelectedItem.Tag;

            pFieldData.strDependencyFieldName = (string)comboBox_DependencyField.SelectedItem;
            pFieldData.strDependencyFieldName_Sub = (string)comboBox_DependencyField_Sub.SelectedItem;
            pFieldData.strFieldName = textBox_FieldName.Text;
            pFieldData.strFieldType = textBox_Type.Text;

            pFieldData.bConvertStringToEnum = checkBox_ConvertStringToEnum.Checked;
            pFieldData.strEnumName = textBox_EnumName.Text;


            pSelectedItem.Text = pFieldData.strFieldName;
            pFieldData.Reset_ListViewItem(pSelectedItem);

            AutoSaveAsync_CurrentSheet();
        }

        private void button_Check_TableAll_Click(object sender, EventArgs e)
        {

        }

        private void button_Save_FileName_Click(object sender, EventArgs e)
        {
            _pSheet_CurrentConnected.strFileName = textBox_TableFileName.Text;
            AutoSaveAsync_CurrentSheet();
        }

        private void buttonSave_CommandLine_Click(object sender, EventArgs e)
        {
            _pSheet_CurrentConnected.strCommandLine = textBox_CommandLine.Text;
            AutoSaveAsync_CurrentSheet();
        }

        private void checkBox_Field_NullOrEmtpy_IsError_CheckedChanged(object sender, EventArgs e)
        {
            if (listView_Field.SelectedItems.Count == 0)
                return;

            var pSelectedItem = listView_Field.SelectedItems[0];
            FieldTypeData pFieldData = (FieldTypeData)pSelectedItem.Tag;

            pFieldData.bIsKeyField = checkBox_Field_ThisIsKey.Checked;
            checkBox_FieldKey_IsOverlap.Enabled = pFieldData.bIsKeyField;
        }

        private void checkBox_DeleteField_OnAfterBuild_CheckedChanged(object sender, EventArgs e)
        {
            if (listView_Field.SelectedItems.Count == 0)
                return;

            var pSelectedItem = listView_Field.SelectedItems[0];
            FieldTypeData pFieldData = (FieldTypeData)pSelectedItem.Tag;

            pFieldData.bDeleteThisField_InCode = checkBox_DeleteField_OnCode.Checked;
        }

        private void checkBox_ConvertStringToEnum_CheckedChanged(object sender, EventArgs e)
        {
            if (listView_Field.SelectedItems.Count == 0)
                return;

            var pSelectedItem = listView_Field.SelectedItems[0];
            FieldTypeData pFieldData = (FieldTypeData)pSelectedItem.Tag;

            pFieldData.bConvertStringToEnum = checkBox_ConvertStringToEnum.Checked;
            textBox_EnumName.Enabled = checkBox_ConvertStringToEnum.Checked;
        }

        private void checkBox_IsHeaderField_CheckedChanged(object sender, EventArgs e)
        {
            if (listView_Field.SelectedItems.Count == 0)
                return;

            var pSelectedItem = listView_Field.SelectedItems[0];
            FieldTypeData pFieldData = (FieldTypeData)pSelectedItem.Tag;

            if (checkBox_IsHeaderField.Checked)
                _pSheet_CurrentConnected.strHeaderFieldName = pFieldData.strFieldName;
            else if (_pSheet_CurrentConnected.strHeaderFieldName == pFieldData.strFieldName)
                _pSheet_CurrentConnected.strHeaderFieldName = "";
        }

        private void CheckedListBox_TableList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_bIsConnecting)
                return;

            pSpreadSheet_CurrentConnected.listTable[e.Index].bEnable = e.NewValue == CheckState.Checked;
            AutoSaveAsync_CurrentSheet();
        }

        private void checkBox_FieldKey_IsOverlap_CheckedChanged(object sender, EventArgs e)
        {
            if (listView_Field.SelectedItems.Count == 0)
                return;

            var pSelectedItem = listView_Field.SelectedItems[0];
            FieldTypeData pFieldData = (FieldTypeData)pSelectedItem.Tag;

            pFieldData.bIsOverlapKey = checkBox_FieldKey_IsOverlap.Checked;
        }

        private void checkedListBox_TableList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetState(EState.IsConnected_And_SelectTable);
        }

    }
}
