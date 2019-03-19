using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wing.Test.CsvierTest;
using Csvier.Exceptions;

namespace Csvier.Test {

    [TestClass]
    public class CsvParserTest {

        [TestMethod]
        [ExpectedException(typeof(ConstructorNotFoundCsvException))]
        public void LoadParse_WithIncorrectCtorArgName() {
            // Arrange
            CsvParser testInfo = CreateCsvParser("INVALID", 1);

            // Act
            TestInfo[] testInfoItems = testInfo
                .Load(sample_Filtered_DateIntIntDoubleString)
                .Parse<TestInfo>();

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastCsvException))]
        public void LoadParse_WithInvalidCastForCtorArgType() {
            // Arrange
            CsvParser testInfo = CreateCsvParser("valueInt1", 4);

            // Act
            TestInfo[] testInfoItems = testInfo
                .Load(sample_Filtered_DateIntIntDoubleString)
                .Parse<TestInfo>();

            // Assert
        }

        [TestMethod]
        public void LoadParse_Single() {
            // Arrange
            int valueStart=10, inc=20;
            CsvParser testInfo = CreateCsvParser("valueInt1", 1);

            // Act
            TestInfo[] testInfoItems = testInfo
                .Load(sample_Filtered_DateIntIntDoubleString)
                .Parse<TestInfo>();

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
            TestInfo[] testInfoItems = testInfo
                .Load(sample_Filtered_DateIntIntDoubleString)
                .Parse<TestInfo>();

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
            CsvParser testInfo = CreateCsvParser("valueInt1", 1);

            // Act
            TestInfo[] testInfoItems = testInfo
                .Load(sample_Filtered_DateIntIntDoubleString)
                .PropArg("INVALID", 1)
                .Parse<TestInfo>();

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastCsvException))]
        public void PropArg_WithInvalidCastArgType() {
            // Arrange
            CsvParser testInfo = CreateCsvParser("valueInt1", 1);

            // Act
            TestInfo[] testInfoItems = testInfo
                .Load(sample_Filtered_DateIntIntDoubleString)
                .PropArg("ValueInt2", 4)
                .Parse<TestInfo>();

            // Assert
        }

        [TestMethod]
        public void PropArg_Single() {
            // Arrange
            int valueStart=10, inc=20;
            CsvParser testInfo = CreateCsvParser();

            // Act
            TestInfo[] testInfoItems = testInfo
                .Load(sample_Filtered_DateIntIntDoubleString)
                .PropArg("ValueInt2", 1)
                .Parse<TestInfo>();

            // Assert
            AssertFullFilteredSample<int>(testInfoItems, typeof(PropertyInfo), "ValueInt2", valueStart, x => x+inc);
        }

        [TestMethod]
        public void PropArg_Multiple() {
            // Arrange
            int valueStart=10, inc=20;
            CsvParser testInfo = CreateCsvParser();

            // Act
            TestInfo[] testInfoItems = testInfo
                .Load(sample_Filtered_DateIntIntDoubleString)
                .PropArg("ValueInt2", 1)
                .PropArg("ValueDouble", 3)
                .Parse<TestInfo>();

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
            CsvParser testInfo = CreateCsvParser("valueInt1", 1);

            // Act
            TestInfo[] testInfoItems = testInfo
                .Load(sample_Filtered_DateIntIntDoubleString)
                .FieldArg("INVALID", 1)
                .Parse<TestInfo>();

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastCsvException))]
        public void FieldArg_WithInvalidCastArgType() {
            // Arrange
            CsvParser testInfo = CreateCsvParser("valueInt1", 1);

            // Act
            TestInfo[] testInfoItems = testInfo
                .Load(sample_Filtered_DateIntIntDoubleString)
                .FieldArg("fieldValueInt", 4)
                .Parse<TestInfo>();

            // Assert
        }

        [TestMethod]
        public void FieldArg_Single() {
            // Arrange
            int valueStart=10, inc=20;
            CsvParser testInfo = CreateCsvParser();

            // Act
            TestInfo[] testInfoItems = testInfo
                .Load(sample_Filtered_DateIntIntDoubleString)
                .FieldArg("fieldValueInt", 1)
                .Parse<TestInfo>();

            // Assert
            AssertFullFilteredSample<int>(testInfoItems, typeof(FieldInfo), "fieldValueInt", valueStart, x => x+inc);
        }

        [TestMethod]
        public void FieldArg_Multiple() {
            // Arrange
            int valueStart=10, inc=20;
            CsvParser testInfo = CreateCsvParser();

            // Act
            TestInfo[] testInfoItems = testInfo
                .Load(sample_Filtered_DateIntIntDoubleString)
                .FieldArg("fieldValueInt", 1)
                .FieldArg("fieldValueDouble", 3)
                .Parse<TestInfo>();

            // Assert
            AssertFullFilteredSample<int>(testInfoItems, typeof(FieldInfo), "fieldValueInt", valueStart, x => x+inc);
            AssertFullFilteredSample<double>(testInfoItems, typeof(FieldInfo), "fieldValueDouble", valueStart+0.01, x => Math.Round(x+=inc, 2));
        }



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



        /* -----------------------------------
         * -------- AUXILIARY METHODS --------
         * ----------------------------------- */
        
        private CsvParser CreateCsvParser() {
            return new CsvParser(typeof(TestInfo));
        }

        private CsvParser CreateCsvParser(string argName, int argCol) {
            return new CsvParser(typeof(TestInfo)).CtorArg(argName, argCol);
        }

        private CsvParser CreateCsvParser(string[] argName, int[] argCol) {
            if (argName.Length!=argCol.Length) {
                string message = String.Format("The Lenght of both arrays must be equal. string[].Lenght={0} != int[].Lenght={1}", argName.Length, argCol.Length);
                throw new ArgumentOutOfRangeException(message);
            }
            CsvParser csvParser = new CsvParser(typeof(TestInfo));
            for (int i=0; i<argName.Length; ++i) {
                csvParser.CtorArg(argName[i], argCol[i]);
            }
            return csvParser;
        }
        
        
        /// <summary>
        /// Asserts if all items of the testinfoItems[] follow the func condition.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when parameter type is not typeof [ PropertyInfo / FieldInfo ].
        /// </exception>
        /// <param name="testInfoItems">Objects to be tested.</param>
        /// <param name="type">Type of the value to be tested (Propertyinfo or FieldInfo).</param>
        /// <param name="name">Name of the type.</param>
        /// <param name="value">Start value.</param>
        /// <param name="func">Function that will affect value after each assertion to prepare for the next one.</param>
        private void AssertFullFilteredSample<T>(object[] testInfoItems, Type type, string name, T value, Func<T, T> func) {
            foreach (TestInfo tItem in testInfoItems) {
                T actual = default(T);
                if (type == typeof(PropertyInfo)) {
                    actual = (T)tItem.GetType().GetProperty(name).GetValue(tItem);
                }
                else if (type == typeof(FieldInfo)) {
                    actual = (T)tItem.GetType().GetField(name).GetValue(tItem);
                }
                else {
                    throw new InvalidOperationException();
                }
                Assert.AreEqual(actual, value);
                value = func(value);
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
