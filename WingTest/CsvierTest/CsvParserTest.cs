﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wing.Test.CsvierTest;

namespace Csvier.Test {

    [TestClass]
    public class CsvParserTest {

        [TestMethod]
        public void LoadParse() {
            // Arrange
            int valueStart=10,  inc=20;
            CsvParser testInfo = new CsvParser(typeof(TestInfo))
                .CtorArg("valueInt", 1);

            // Act
            object[] testInfoItems = testInfo
                .Load(sample_Filtered_DateIntIntDoubleString)
                .Parse();

            // Assert
            foreach(TestInfo tItem in testInfoItems) {
                Assert.AreEqual(tItem.ValueInt, valueStart);
                valueStart+=inc;
            }
        }

        [TestMethod]
        public void RemoveCount() {
            // Arrange
            int valueStart=50,  inc=20;
            int count=2;
            CsvParser testInfo = new CsvParser(typeof(TestInfo))
                .CtorArg("valueInt", 1);

            // Act
            object[] testInfoItems = testInfo
                .Load(sample_Filtered_DateIntIntDoubleString)
                .Remove(count)
                .Parse();

            // Assert
            foreach (TestInfo tItem in testInfoItems) {
                Assert.AreEqual(tItem.ValueInt, valueStart);
                valueStart+=inc;
            }
        }

        [TestMethod]
        public void RemoveEmpties() {
            // Arrange
            int valueStart=10,  inc=20;
            CsvParser testInfo = new CsvParser(typeof(TestInfo))
                .CtorArg("valueInt", 1);

            // Act
            object[] testInfoItems = testInfo
                .Load(sample_EmptyLines_DateIntIntDoubleString)
                .RemoveEmpties()
                .Parse();

            // Assert
            foreach (TestInfo tItem in testInfoItems) {
                Assert.AreEqual(tItem.ValueInt, valueStart);
                valueStart+=inc;
            }
        }

        [TestMethod]
        public void RemoveWith() {
            // Arrange
            string toRemove = "#";
            int valueStart=10,  inc=20;
            CsvParser testInfo = new CsvParser(typeof(TestInfo))
                .CtorArg("valueInt", 1);

            // Act
            object[] testInfoItems = testInfo
                .Load(sample_ToFilterWith_DateIntIntDoubleString)
                .RemoveWith(toRemove)
                .Parse();

            // Assert
            foreach (TestInfo tItem in testInfoItems) {
                Assert.AreEqual(tItem.ValueInt, valueStart);
                valueStart+=inc;
            }
        }

        [TestMethod]
        public void RemoveEvenIndexes() {
            // Arrange
            int valueStart=30,  inc=40;
            CsvParser testInfo = new CsvParser(typeof(TestInfo))
                .CtorArg("valueInt", 1);

            // Act
            object[] testInfoItems = testInfo
                .Load(sample_Filtered_DateIntIntDoubleString)
                .RemoveEvenIndexes()
                .Parse();

            // Assert
            foreach (TestInfo tItem in testInfoItems) {
                Assert.AreEqual(tItem.ValueInt, valueStart);
                valueStart+=inc;
            }
        }

        [TestMethod]
        public void RemoveOddIndexes() {
            // Arrange
            int valueStart=10,  inc=40;
            CsvParser testInfo = new CsvParser(typeof(TestInfo))
                .CtorArg("valueInt", 1);

            // Act
            object[] testInfoItems = testInfo
                .Load(sample_Filtered_DateIntIntDoubleString)
                .RemoveOddIndexes()
                .Parse();

            // Assert
            foreach (TestInfo tItem in testInfoItems) {
                Assert.AreEqual(tItem.ValueInt, valueStart);
                valueStart+=inc;
            }
        }



        /* -----------------------------------
         * --- DO NOT IDENT THESE SAMPLES ----
         * ----------------------------------- */

        private readonly string sample_Filtered_DateIntIntDoubleString =
@"2000-01-01,10,21,10.21,lineNumber0
2000-01-02,30,41,30.41,lineNumber1
2000-01-03,50,61,50.61,lineNumber2
2000-01-04,70,81,70.81,lineNumber3
2000-01-05,90,101,70.81,lineNumber4
2000-01-06,110,121,70.81,lineNumber5";

        private readonly string sample_EmptyLines_DateIntIntDoubleString =
@"
2000-01-01,10,21,10.21,lineNumber0


2000-01-02,30,41,30.41,lineNumber1

2000-01-03,50,61,50.61,lineNumber2
2000-01-04,70,81,70.81,lineNumber3

2000-01-05,90,101,70.81,lineNumber4
2000-01-06,110,121,70.81,lineNumber5
";

        private readonly string sample_ToFilterWith_DateIntIntDoubleString =
@"#this is some line at start
2000-01-01,10,21,10.21,lineNumber0
#this is some line
#this is someother line
2000-01-02,30,41,30.41,lineNumber1
2000-01-03,50,61,50.61,lineNumber2
#this is some line alone
2000-01-04,70,81,70.81,lineNumber3
2000-01-05,90,101,70.81,lineNumber4
2000-01-06,110,121,70.81,lineNumber5
#this is some line at the end";

    }
}
