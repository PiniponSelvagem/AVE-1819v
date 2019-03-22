using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Csvier.Exceptions;

namespace Csvier.Test {

    [TestClass]
    public class CsvParserExplicitTest : CsvBase {

        [TestMethod]
        [ExpectedException(typeof(ConstructorNotFoundCsvException))]
        public void LoadParse_WithIncorrectCtorArgName() {
            // Arrange
            CsvParser testInfo = CreateCsvParser("INVALID", 1);

            // Act
            TestInfo[] testInfoItems = LoadParse_TestInfo(testInfo);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastCsvException))]
        public void LoadParse_WithInvalidCastForCtorArgType() {
            // Arrange
            CsvParser testInfo = CreateCsvParser("valueInt1", 4);

            // Act
            TestInfo[] testInfoItems = LoadParse_TestInfo(testInfo);
        }

        [TestMethod]
        public void LoadParse_Single() {
            // Arrange
            int valueStart=10, inc=20;
            CsvParser testInfo = CreateCsvParser("valueInt1", 1);

            // Act
            TestInfo[] testInfoItems = LoadParse_TestInfo(testInfo);

            // Assert
            AssertFullFilteredSample<int>(testInfoItems, typeof(PropertyInfo), "ValueInt1", valueStart, x => x+inc);
        }

        [TestMethod]
        public void LoadParse_Multiple() {
            // Arrange
            DateTime valueDate = new DateTime(2000, 1, 1); int dateInc = 1; //ValueDate
            int valueStart=10, intInc=20;  //ValueInt1
            CsvParser testInfo = CreateCsvParser(new string[]{"valueDate", "valueInt1"}, new int[] {0, 1});

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
        [ExpectedException(typeof(PropertyNotFoundCsvException))]
        public void PropArg_WithIncorrectArgName() {
            // Arrange
            CsvParser testInfo = CreateCsvParser("valueInt1", 1)
                .PropArg("INVALID", 1);

            // Act
            TestInfo[] testInfoItems = LoadParse_TestInfo(testInfo);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastCsvException))]
        public void PropArg_WithInvalidCastArgType() {
            // Arrange
            CsvParser testInfo = CreateCsvParser()
                .PropArg("ValueInt2", 4);

            // Act
            TestInfo[] testInfoItems = LoadParse_TestInfo(testInfo);
        }

        [TestMethod]
        public void PropArg_Single() {
            // Arrange
            int valueStart=10, inc=20;
            CsvParser testInfo = CreateCsvParser()
                .PropArg("ValueInt2", 1);

            // Act
            TestInfo[] testInfoItems = LoadParse_TestInfo(testInfo);

            // Assert
            AssertFullFilteredSample<int>(testInfoItems, typeof(PropertyInfo), "ValueInt2", valueStart, x => x+inc);
        }

        [TestMethod]
        public void PropArg_Multiple() {
            // Arrange
            int valueStart=10, inc=20;
            CsvParser testInfo = CreateCsvParser()
                .PropArg("ValueInt2", 1)
                .PropArg("ValueDouble", 3);

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
        [ExpectedException(typeof(FieldNotFoundCsvException))]
        public void FieldArg_WithIncorrectArgName() {
            // Arrange
            CsvParser testInfo = CreateCsvParser()
                .FieldArg("INVALID", 1);

            // Act
            TestInfo[] testInfoItems = LoadParse_TestInfo(testInfo);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastCsvException))]
        public void FieldArg_WithInvalidCastArgType() {
            // Arrange
            CsvParser testInfo = CreateCsvParser("valueInt1", 1)
                .FieldArg("fieldValueInt", 4);

            // Act
            TestInfo[] testInfoItems = LoadParse_TestInfo(testInfo);
        }

        [TestMethod]
        public void FieldArg_Single() {
            // Arrange
            int valueStart=10, inc=20;
            CsvParser testInfo = CreateCsvParser()
                .FieldArg("fieldValueInt", 1);

            // Act
            TestInfo[] testInfoItems = LoadParse_TestInfo(testInfo);

            // Assert
            AssertFullFilteredSample<int>(testInfoItems, typeof(FieldInfo), "fieldValueInt", valueStart, x => x+inc);
        }

        [TestMethod]
        public void FieldArg_Multiple() {
            // Arrange
            int valueStart=10, inc=20;
            CsvParser testInfo = CreateCsvParser()
                .FieldArg("fieldValueInt", 1)
                .FieldArg("fieldValueDouble", 3);

            // Act
            TestInfo[] testInfoItems = LoadParse_TestInfo(testInfo);

            // Assert
            AssertFullFilteredSample<int>(testInfoItems, typeof(FieldInfo), "fieldValueInt", valueStart, x => x+inc);
            AssertFullFilteredSample<double>(testInfoItems, typeof(FieldInfo), "fieldValueDouble", valueStart+0.01, x => Math.Round(x+=inc, 2));
        }
    }        
}
