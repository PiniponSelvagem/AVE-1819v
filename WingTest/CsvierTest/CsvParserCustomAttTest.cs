using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace Csvier.Test {

    [TestClass]
    public class CsvParserCustomAttTest : CsvBase {
        
        [TestMethod]
        public void LoadParse_Multiple() {
            // Arrange
            DateTime valueDate = new DateTime(2000, 1, 1); int dateInc = 1; //ValueDate
            int valueStart=10, intInc=20;  //ValueInt1
            CsvParser testInfo = CreateCsvParser()
                .CsvParserAutoCreate();

            // Act
            TestInfo[] testInfoItems = LoadParse_TestInfo(testInfo);

            // Assert
            AssertFullFilteredSample<DateTime>(testInfoItems, typeof(PropertyInfo), "ValueDate", valueDate, x => (DateTime)x.AddDays(dateInc));
            AssertFullFilteredSample<int>(testInfoItems, typeof(PropertyInfo), "ValueInt1", valueStart, x => x+intInc);
        }



        /* -----------------------------------
         * -------- PROPERTIES TESTS ----------
         * ----------------------------------- */

        [TestMethod]
        public void PropArg_Multiple() {
            // Arrange
            int valueStart=10, inc=20;
            CsvParser testInfo = CreateCsvParser()
                .CsvParserAutoCreate();

            // Act
            TestInfo[] testInfoItems = LoadParse_TestInfo(testInfo);

            // Assert
            AssertFullFilteredSample<int>(testInfoItems, typeof(PropertyInfo), "ValueInt2", valueStart, x => x+inc);
            AssertFullFilteredSample<double>(testInfoItems, typeof(PropertyInfo), "ValueDouble", valueStart+0.01, x => Math.Round(x+=inc, 2));
        }


        /* -----------------------------------
         * ----------- FIELD TESTS -----------
         * ----------------------------------- */

        [TestMethod]
        public void FieldArg_Multiple() {
            // Arrange
            int valueStart=10, inc=20;
            CsvParser testInfo = CreateCsvParser()
                .CsvParserAutoCreate();

            // Act
            TestInfo[] testInfoItems = LoadParse_TestInfo(testInfo);

            // Assert
            AssertFullFilteredSample<int>(testInfoItems, typeof(FieldInfo), "fieldValueInt", valueStart, x => x+inc);
            AssertFullFilteredSample<double>(testInfoItems, typeof(FieldInfo), "fieldValueDouble", valueStart+0.01, x => Math.Round(x+=inc, 2));
        }
    }
}
