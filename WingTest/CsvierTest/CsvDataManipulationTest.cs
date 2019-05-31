using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Csvier.Test {

    [TestClass]
    public class CsvDataManipulationTest : CsvBase {

        /* -----------------------------------
         * ---------- REMOVE TESTS -----------
         * ----------------------------------- */

        [TestMethod]
        public void RemoveCount() {
            // Arrange
            int valueStart=50, inc=20;
            int count=2;
            CsvParser testInfo = CreateCsvParser("valueInt1", 1);

            // Act
            TestInfo[] testInfoItems = testInfo
                .Load(sample_Filtered_DateIntIntDoubleString)
                .Remove(count)
                .Parse<TestInfo>();

            // Assert
            AssertFullFilteredSample<int>(testInfoItems, typeof(PropertyInfo), "ValueInt1", valueStart, x => x+inc);
        }

        [TestMethod]
        public void RemoveEmpties() {
            // Arrange
            int valueStart=10, inc=20;
            CsvParser testInfo = CreateCsvParser("valueInt1", 1);

            // Act
            TestInfo[] testInfoItems = testInfo
                .Load(sample_EmptyLines_DateIntIntDoubleString)
                .RemoveEmpties()
                .Parse<TestInfo>();

            // Assert
            AssertFullFilteredSample<int>(testInfoItems, typeof(PropertyInfo), "ValueInt1", valueStart, x => x+inc);
        }

        [TestMethod]
        public void RemoveWith() {
            // Arrange
            string toRemove = "#";
            int valueStart=10, inc=20;
            CsvParser testInfo = CreateCsvParser("valueInt1", 1);

            // Act
            TestInfo[] testInfoItems = testInfo
                .Load(sample_ToFilterWith_DateIntIntDoubleString)
                .RemoveWith(toRemove)
                .Parse<TestInfo>();

            // Assert
            AssertFullFilteredSample<int>(testInfoItems, typeof(PropertyInfo), "ValueInt1", valueStart, x => x+inc);
        }

        [TestMethod]
        public void RemoveEvenIndexes() {
            // Arrange
            int valueStart=30, inc=40;
            CsvParser testInfo = CreateCsvParser("valueInt1", 1);

            // Act
            TestInfo[] testInfoItems = testInfo
                .Load(sample_Filtered_DateIntIntDoubleString)
                .RemoveEvenIndexes()
                .Parse<TestInfo>();

            // Assert
            AssertFullFilteredSample<int>(testInfoItems, typeof(PropertyInfo), "ValueInt1", valueStart, x => x+inc);
        }

        [TestMethod]
        public void RemoveOddIndexes() {
            // Arrange
            int valueStart=10, inc=40;
            CsvParser testInfo = CreateCsvParser("valueInt1", 1);

            // Act
            TestInfo[] testInfoItems = testInfo
                .Load(sample_Filtered_DateIntIntDoubleString)
                .RemoveOddIndexes()
                .Parse<TestInfo>();

            // Assert
            AssertFullFilteredSample<int>(testInfoItems, typeof(PropertyInfo), "ValueInt1", valueStart, x => x+inc);
        }
    }
}
