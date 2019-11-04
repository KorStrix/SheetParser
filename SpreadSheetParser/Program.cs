using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadSheetParser
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new SpreadSheetParser());

            CodeFileBuilder pCodeFileBuilder = new CodeFileBuilder();

            string strTestEnum = "TestEnum";
            System.CodeDom.CodeTypeDeclaration pClass = pCodeFileBuilder.AddClass("TestClass", "Class Summary");
            pClass.AddField(new FieldData("TestField", strTestEnum, "Comment Test"));

            System.CodeDom.CodeTypeDeclaration pEnum = pCodeFileBuilder.AddEnum(strTestEnum, "Enum Summary");
            pEnum.AddEnumField(new EnumFieldData("Enum_TestField_minus1", -1, "Enum Comment Test 1"));
            pEnum.AddEnumField(new EnumFieldData("Enum_TestField_1"));
            pEnum.AddEnumField(new EnumFieldData("Enum_TestField_2"));


            pCodeFileBuilder.Set_Namespace("TestNameSpace");
            pCodeFileBuilder.Set_UsingNameList(new List<string>() { "System" });

            pCodeFileBuilder.GenerateCSharpCode("test");
        }
    }
}
