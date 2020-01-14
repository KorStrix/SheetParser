using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpreadSheetParser.SpreadSheetParser_MainForm;

namespace SpreadSheetParser
{
    static class SaveData_SheetHelper
    {
        static public void DoCheck_IsValid_Table(this SaveData_Sheet pSheetData)
        {
            bool bIsEnum = pSheetData.eType == SaveData_Sheet.EType.Enum;

            pSheetData.ParsingSheet(
            (listRow, strText, iRow, iColumn) =>
            {
                if (bIsEnum)
                {
                    Dictionary<int, EEnumHeaderType> mapEnumType = new Dictionary<int, EEnumHeaderType>();

                    EEnumHeaderType eType = EEnumHeaderType.EnumNone;
                    if (System.Enum.TryParse(strText, out eType))
                    {
                        // mapEnumType.Add(,eType)
                        if (eType == EEnumHeaderType.EnumType)
                        {
                            for (int i = iColumn; i < listRow.Count; i++)
                            {
                                string strTextOtherColumn = (string)listRow[i];
                                if (System.Enum.TryParse(strTextOtherColumn, out eType) == false)
                                {
                                    WriteConsole($"테이블 유효성 체크 - 이넘 파싱 에러");
                                    return;
                                }

                                if (mapEnumType.ContainsKey(iColumn) == false)
                                    mapEnumType.Add(iColumn, eType);
                            }
                        }

                        return;
                    }

                    if (mapEnumType.ContainsKey(iColumn) == false)
                        return;

                    switch (mapEnumType[iColumn])
                    {
                        case EEnumHeaderType.EnumType:
                        case EEnumHeaderType.EnumValue:
                            if (string.IsNullOrEmpty(strText))
                            {
                                WriteConsole($"테이블 유효성 체크 - 이넘 파싱 에러");
                                return;
                            }
                            break;
                    }
                }
                else
                {

                }
            });
        }

        static public void DoWork(this SaveData_Sheet pSheetData, CodeFileBuilder pCodeFileBuilder)
        {
            List<CommandLineArg> listCommandLine = Parsing_CommandLine(pSheetData.strCommandLine);

            bool bIsEnum = pSheetData.eType == SaveData_Sheet.EType.Enum;
            if (bIsEnum)
            {
                Parsing_OnEnum(pSheetData, pCodeFileBuilder);
            }
            else
            {
                Parsing_OnCode(pSheetData, pCodeFileBuilder, listCommandLine);
            }
        }

        private static void Parsing_OnCode(SaveData_Sheet pSheetData, CodeFileBuilder pCodeFileBuilder, List<CommandLineArg> listCommandLine)
        {
            var pCodeType = pCodeFileBuilder.AddCodeType(pSheetData.strFileName, pSheetData.eType);
            var mapFieldData_ConvertStringToEnum = pSheetData.listFieldData.Where((pFieldData) => pFieldData.bConvertStringToEnum).ToDictionary(((pFieldData) => pFieldData.strFieldName));
            var listFieldData_DeleteThisField_OnCode = pSheetData.listFieldData.Where((pFieldData) => pFieldData.bDeleteThisField_InCode).Select((pFieldData) => pFieldData.strFieldName);
            Dictionary<int, CodeTypeDeclaration> mapEnumType = new Dictionary<int, CodeTypeDeclaration>();

            pSheetData.ParsingSheet(
              (listRow, strText, iRow, iColumn) =>
              {
                  // 변수 선언 형식인경우
                  if (strText.Contains(":"))
                  {
                      string[] arrText = strText.Split(':');
                      string strFieldName = arrText[0];

                      if(mapFieldData_ConvertStringToEnum.ContainsKey(strFieldName))
                      {
                          FieldData pFieldData = mapFieldData_ConvertStringToEnum[strFieldName];
                          mapEnumType.Add(iColumn, pCodeFileBuilder.AddCodeType(pFieldData.strEnumName, SaveData_Sheet.EType.Enum));
                      }
                      else
                      {
                          // 삭제되는 코드인 경우
                          if (listFieldData_DeleteThisField_OnCode.Contains(strFieldName))
                              return;

                          pCodeType.AddField(new FieldData(strFieldName, arrText[1]));
                      }

                      return;
                  }

                  // 이넘 Column인 경우 이넘 생성
                  if (mapEnumType.ContainsKey(iColumn))
                  {
                      mapEnumType[iColumn].AddEnumField(new EnumFieldData(strText));
                      return;
                  }
              });

            Execute_CommandLine(pCodeType, listCommandLine);
        }

        private static void Parsing_OnEnum(SaveData_Sheet pSheetData, CodeFileBuilder pCodeFileBuilder)
        {
            Dictionary<int, EEnumHeaderType> mapEnumType = new Dictionary<int, EEnumHeaderType>();
            Dictionary<string, CodeTypeDeclaration> mapEnumValue = new Dictionary<string, CodeTypeDeclaration>();

            pSheetData.ParsingSheet(
                (listRow, strText, iRow, iColumn) =>
                {
                    EEnumHeaderType eType = EEnumHeaderType.EnumNone;
                    if (System.Enum.TryParse(strText, out eType))
                    {
                        if (eType == EEnumHeaderType.EnumType)
                        {
                            if (mapEnumType.ContainsKey(iColumn) == false)
                                mapEnumType.Add(iColumn, eType);

                            for (int i = iColumn; i < listRow.Count; i++)
                            {
                                string strTextOtherColumn = (string)listRow[i];
                                if (System.Enum.TryParse(strTextOtherColumn, out eType))
                                {
                                    if (mapEnumType.ContainsKey(i) == false)
                                        mapEnumType.Add(i, eType);
                                }
                            }
                        }

                        return;
                    }

                    eType = mapEnumType[iColumn];
                    if (eType != EEnumHeaderType.EnumType)
                        return;

                    if (mapEnumValue.ContainsKey(strText) == false)
                        mapEnumValue.Add(strText, pCodeFileBuilder.AddCodeType(strText, SaveData_Sheet.EType.Enum));

                    EnumFieldData pFieldData = new EnumFieldData();
                    for (int i = iColumn; i < listRow.Count; i++)
                    {
                        if (mapEnumType.TryGetValue(i, out eType))
                        {
                            string strNextText = (string)listRow[i];
                            switch (eType)
                            {
                                case EEnumHeaderType.EnumValue: pFieldData.strValue = strNextText; 
                                    break;

                                case EEnumHeaderType.NumberValue: pFieldData.iNumber = int.Parse(strNextText); break;
                                case EEnumHeaderType.Comment: pFieldData.strComment = strNextText; break;
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(pFieldData.strValue))
                        throw new System.Exception($"이넘인데 값이 없습니다 - 타입 : {mapEnumValue[strText].Name}");

                    mapEnumValue[strText].AddEnumField(pFieldData);
                });
        }

        private static List<CommandLineArg> Parsing_CommandLine(string strCommandLine)
        {
            return CommandLineParser.Parsing_CommandLine(strCommandLine,
                (string strCommandLineText, out bool bHasValue) =>
                {
                    ECommandLine eCommandLine;
                    bool bIsValid = Enum.TryParse(strCommandLineText, out eCommandLine);
                    switch (eCommandLine)
                    {
                        case ECommandLine.comment:
                        case ECommandLine.typename:
                            bHasValue = true;
                            break;

                        default:
                            bHasValue = false;
                            break;
                    }

                    return bIsValid;
                },

                (string strCommandLineText, CommandLineParser.Error eError) =>
                {
                    WriteConsole($"테이블 유효성 에러 Text : {strCommandLineText} Error : {eError}");
                    // iErrorCount++;
                });
        }

        static private void Execute_CommandLine(CodeTypeDeclaration pCodeType, List<CommandLineArg> listCommandLine)
        {
            for (int i = 0; i < listCommandLine.Count; i++)
            {
                ECommandLine eCommandLine = (ECommandLine)Enum.Parse(typeof(ECommandLine), listCommandLine[i].strArgName);
                switch (eCommandLine)
                {
                    case ECommandLine.comment:
                        pCodeType.AddComment(listCommandLine[i].strArgValue);
                        break;

                    case ECommandLine.typename:
                        pCodeType.Name = listCommandLine[i].strArgValue;
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
