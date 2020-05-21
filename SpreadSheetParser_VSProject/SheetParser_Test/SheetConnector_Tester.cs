using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using SpreadSheetParser;

namespace SheetParser_Test
{
    [TestFixture]
    public class SheetConnector_Tester
    {
        [Test]
        public async Task 시트컨넥터_구글시트_연결_테스트()
        {
            // Arrange
            // 테스트 시트
            // https://docs.google.com/spreadsheets/d/1_s89xLPwidVwRsmGS4bp3Y6huaLWoBDq7SUW7lYyxl4/edit#gid=0
            string strTestSheetID = "1_s89xLPwidVwRsmGS4bp3Y6huaLWoBDq7SUW7lYyxl4";
            SheetSourceConnector pSourceConnector = new GoogleSpreadSheet_SourceConnector(GetDirectory_ForTestProject() + "/credentials.json");
            bool bIsConnected = false;


            // Act
            await pSourceConnector.ISheetSourceConnector_DoConnect_And_Parsing(strTestSheetID, (iConnector, pException_OnError) => 
            {
                bIsConnected = pException_OnError == null && strTestSheetID.Equals(iConnector.strSheetSourceID) && iConnector.mapWorkSheetData_Key_Is_SheetID.Count > 0;
            });


            // Assert
            Assert.IsTrue(bIsConnected);
        }

        [Test]
        public async Task 시트컨넥터_MS엑셀_열기_테스트()
        {
            // Arrange
            string strTestExcelFileName = "TestExcel.xlsx";
            SheetSourceConnector pSourceConnector = new MSExcel_SourceConnector();
            bool bIsConnected = false;

            // Act
            await pSourceConnector.ISheetSourceConnector_DoConnect_And_Parsing(GetDirectory_ForTestProject() + strTestExcelFileName, (iConnector, pException_OnError) =>
            {
                bIsConnected = pException_OnError == null && iConnector.mapWorkSheetData_Key_Is_SheetID.Count > 0;
            });


            // Assert
            Assert.IsTrue(bIsConnected);
        }

        private static string GetDirectory_ForTestProject()
        {
            // Get the executing directory of the tests 
            string dir = NUnit.Framework.TestContext.CurrentContext.TestDirectory;
            // Infer the project directory from there...2 levels up (depending on project type - for asp.net omit the latter Parent for a single level up)
            dir = System.IO.Directory.GetParent(dir).Parent.FullName;
            return dir + "/";
        }
    }
}
