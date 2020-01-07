using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadSheetParser
{
    static class Program_Example_CodeDomCustomize
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main_Example()
        {
            CodeFileBuilder pCodeFileBuilder = new CodeFileBuilder();

            string strTestEnum = "TestEnum";
            System.CodeDom.CodeTypeDeclaration pClass = pCodeFileBuilder.AddCodeType("TestClass", "Class Summary");
            pClass.IsClass = true;
            pClass.AddField(new FieldData("TestField", strTestEnum, "Comment Test"));

            System.CodeDom.CodeTypeDeclaration pEnum = pCodeFileBuilder.AddCodeType(strTestEnum, "Enum Summary");
            pEnum.IsEnum = true;
            pEnum.AddEnumField(new EnumFieldData("Enum_TestField_minus1", -1, "Enum Comment Test 1"));
            pEnum.AddEnumField(new EnumFieldData("Enum_TestField_1"));
            pEnum.AddEnumField(new EnumFieldData("Enum_TestField_2"));


            pCodeFileBuilder.Set_Namespace("TestNameSpace");
            pCodeFileBuilder.Set_UsingList("System");

            pCodeFileBuilder.Generate_CSharpCode("test");
        }
    }
}
