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
            ESpreadSheetType eSheetConnected = ESpreadSheetType.MSExcel;
            SpreadSheetConnector pConnector = new SpreadSheetConnector();
            bool bIsConnected = false;


            // Act
            await pConnector.DoConnect(strTestSheetID, (strSheetID, strFileName, eSheetType, listSheet, pException_OnError) => 
            {
                bIsConnected = strTestSheetID.Equals(strSheetID);
                eSheetConnected = eSheetType;
            });


            // Assert
            Assert.IsTrue(bIsConnected);
            Assert.AreEqual(eSheetConnected, ESpreadSheetType.GoogleSpreadSheet);
        }

        [Test]
        public async Task 시트컨넥터_MS엑셀_열기_테스트()
        {
            // Arrange
            string strTestExcelFileName = "TestExcel.xlsx";
            ESpreadSheetType eSheetConnected = ESpreadSheetType.GoogleSpreadSheet;
            SpreadSheetConnector pConnector = new SpreadSheetConnector();
            bool bIsConnected = false;


            // Act
            await pConnector.DoOpen_Excel(strTestExcelFileName, (strSheetID, strFileName, eSheetType, listSheet, pException_OnError) =>
            {
                bIsConnected = strTestExcelFileName.Equals(strSheetID);
                eSheetConnected = eSheetType;
            });


            // Assert
            Assert.IsTrue(bIsConnected);
            Assert.AreEqual(eSheetConnected, ESpreadSheetType.MSExcel);
        }
    }
}
