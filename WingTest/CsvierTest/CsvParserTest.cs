using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wing.Test.CsvierTest;

namespace Csvier.Test {

    [TestClass]
    public class CsvParserTest {

        [TestMethod]
        public void LoadParse() {
            // Arrange
            int valueStart=10, inc=20;
            CsvParser testInfo = CreateCsvParser_valueInt1();

            // Act
            object[] testInfoItems = testInfo
                .Load(sample_Filtered_DateIntIntDoubleString)
                .Parse();

            // Assert
            AssertFullFilteredSample_Int("ValueInt1", valueStart, inc, testInfoItems);
        }



        /* -----------------------------------
         * -------- PROPERTIES TESTS ----------
         * ----------------------------------- */

        [TestMethod]
        public void PropArg_Single() {
            // Arrange
            int valueStart=10, inc=20;
            CsvParser testInfo = CreateCsvParser();

            // Act
            object[] testInfoItems = testInfo
                .Load(sample_Filtered_DateIntIntDoubleString)
                .PropArg("ValueInt2", 1)
                .Parse();

            // Assert
            AssertFullFilteredSample_Int("ValueInt2", valueStart, inc, testInfoItems);
        }

        [TestMethod]
        public void PropArg_Multiple() {
            // Arrange
            int valueStart=10, inc=20;
            CsvParser testInfo = CreateCsvParser();

            // Act
            object[] testInfoItems = testInfo
                .Load(sample_Filtered_DateIntIntDoubleString)
                .PropArg("ValueInt2", 1)
                .PropArg("ValueDouble", 3)
                .Parse();

            // Assert
            AssertFullFilteredSample_Int("ValueInt2", valueStart, inc, testInfoItems);
            AssertFullFilteredSample_Double("ValueDouble", valueStart+0.01, inc, testInfoItems);
        }


        /* -----------------------------------
         * ----------- FIELD TESTS -----------
         * ----------------------------------- */

        [TestMethod]
        public void FieldArg_Single() {
            // Arrange
            int valueStart=10, inc=20;
            CsvParser testInfo = CreateCsvParser();

            // Act
            object[] testInfoItems = testInfo
                .Load(sample_Filtered_DateIntIntDoubleString)
                .FieldArg("fieldValueInt", 1)
                .Parse();

            // Assert
            AssertFullFilteredSample_Int_Field("fieldValueInt", valueStart, inc, testInfoItems);
        }

        [TestMethod]
        public void FieldArg_Multiple() {
            // Arrange
            int valueStart=10, inc=20;
            CsvParser testInfo = CreateCsvParser();

            // Act
            object[] testInfoItems = testInfo
                .Load(sample_Filtered_DateIntIntDoubleString)
                .FieldArg("fieldValueInt", 1)
                .FieldArg("fieldValueDouble", 3)
                .Parse();

            // Assert
            AssertFullFilteredSample_Int_Field("fieldValueInt", valueStart, inc, testInfoItems);
            AssertFullFilteredSample_Double_Field("fieldValueDouble", valueStart+0.01, inc, testInfoItems);
        }



        /* -----------------------------------
         * ---------- REMOVE TESTS -----------
         * ----------------------------------- */

        [TestMethod]
        public void RemoveCount() {
            // Arrange
            int valueStart=50, inc=20;
            int count=2;
            CsvParser testInfo = CreateCsvParser_valueInt1();

            // Act
            object[] testInfoItems = testInfo
                .Load(sample_Filtered_DateIntIntDoubleString)
                .Remove(count)
                .Parse();

            // Assert
            AssertFullFilteredSample_Int("ValueInt1", valueStart, inc, testInfoItems);
        }

        [TestMethod]
        public void RemoveEmpties() {
            // Arrange
            int valueStart=10, inc=20;
            CsvParser testInfo = CreateCsvParser_valueInt1();

            // Act
            object[] testInfoItems = testInfo
                .Load(sample_EmptyLines_DateIntIntDoubleString)
                .RemoveEmpties()
                .Parse();

            // Assert
            AssertFullFilteredSample_Int("ValueInt1", valueStart, inc, testInfoItems);
        }

        [TestMethod]
        public void RemoveWith() {
            // Arrange
            string toRemove = "#";
            int valueStart=10, inc=20;
            CsvParser testInfo = CreateCsvParser_valueInt1();

            // Act
            object[] testInfoItems = testInfo
                .Load(sample_ToFilterWith_DateIntIntDoubleString)
                .RemoveWith(toRemove)
                .Parse();

            // Assert
            AssertFullFilteredSample_Int("ValueInt1", valueStart, inc, testInfoItems);
        }

        [TestMethod]
        public void RemoveEvenIndexes() {
            // Arrange
            int valueStart=30, inc=40;
            CsvParser testInfo = CreateCsvParser_valueInt1();

            // Act
            object[] testInfoItems = testInfo
                .Load(sample_Filtered_DateIntIntDoubleString)
                .RemoveEvenIndexes()
                .Parse();

            // Assert
            AssertFullFilteredSample_Int("ValueInt1", valueStart, inc, testInfoItems);
        }

        [TestMethod]
        public void RemoveOddIndexes() {
            // Arrange
            int valueStart=10, inc=40;
            CsvParser testInfo = CreateCsvParser_valueInt1();

            // Act
            object[] testInfoItems = testInfo
                .Load(sample_Filtered_DateIntIntDoubleString)
                .RemoveOddIndexes()
                .Parse();

            // Assert
            AssertFullFilteredSample_Int("ValueInt1", valueStart, inc, testInfoItems);
        }



        /* -----------------------------------
         * -------- AUXILIARY METHODS --------
         * ----------------------------------- */
        
        private CsvParser CreateCsvParser() {
            return new CsvParser(typeof(TestInfo));
        }

        private CsvParser CreateCsvParser_valueInt1() {
            return new CsvParser(typeof(TestInfo)).CtorArg("valueInt1", 1);
        }
        
        private void AssertFullFilteredSample_Int(string propName, int value, int inc, object[] testInfoItems) {
            foreach (TestInfo tItem in testInfoItems) {
                PropertyInfo prop = tItem.GetType().GetProperty(propName);
                Assert.AreEqual(prop.GetValue(tItem), value);
                value+=inc;
            }
        }

        private void AssertFullFilteredSample_Double(string propName, double value, double inc, object[] testInfoItems) {
            foreach (TestInfo tItem in testInfoItems) {
                PropertyInfo prop = tItem.GetType().GetProperty(propName);
                Assert.AreEqual(prop.GetValue(tItem), value);
                value = Math.Round(value+=inc, 2);
            }
        }

        private void AssertFullFilteredSample_Int_Field(string fieldname, int value, int inc, object[] testInfoItems) {
            foreach (TestInfo tItem in testInfoItems) {
                FieldInfo field = tItem.GetType().GetField(fieldname);
                Assert.AreEqual(field.GetValue(tItem), value);
                value+=inc;
            }
        }

        private void AssertFullFilteredSample_Double_Field(string fieldname, double value, double inc, object[] testInfoItems) {
            foreach (TestInfo tItem in testInfoItems) {
                FieldInfo field = tItem.GetType().GetField(fieldname);
                Assert.AreEqual(field.GetValue(tItem), value);
                value = Math.Round(value+=inc, 2);
            }
        }


        /* -----------------------------------
         * --- DO NOT IDENT THESE SAMPLES ----
         * ----------------------------------- */

        private readonly string sample_Filtered_DateIntIntDoubleString =
@"2000-01-01,10,21,10.01,lineNumber0
2000-01-02,30,41,30.01,lineNumber1
2000-01-03,50,61,50.01,lineNumber2
2000-01-04,70,81,70.01,lineNumber3
2000-01-05,90,101,90.01,lineNumber4
2000-01-06,110,121,110.01,lineNumber5";

        private readonly string sample_EmptyLines_DateIntIntDoubleString =
@"
2000-01-01,10,21,10.01,lineNumber0


2000-01-02,30,41,30.01,lineNumber1

2000-01-03,50,61,50.01,lineNumber2
2000-01-04,70,81,70.01,lineNumber3

2000-01-05,90,101,90.01,lineNumber4
2000-01-06,110,121,110.01,lineNumber5
";

        private readonly string sample_ToFilterWith_DateIntIntDoubleString =
@"#this is some line at start
2000-01-01,10,21,10.01,lineNumber0
#this is some line
#this is someother line
2000-01-02,30,41,30.01,lineNumber1
2000-01-03,50,61,50.01,lineNumber2
#this is some line alone
2000-01-04,70,81,70.01,lineNumber3
2000-01-05,90,101,90.01,lineNumber4
2000-01-06,110,121,110.01,lineNumber5
#this is some line at the end";

    }
}
