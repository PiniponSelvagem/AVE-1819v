using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace Csvier.Test {
    public class CsvBase : ICsv {

        public CsvParser CreateCsvParser() {
            return new CsvParser(typeof(TestInfo));
        }

        public CsvParser CreateCsvParser(string argName, int argCol) {
            return new CsvParser(typeof(TestInfo)).CtorArg(argName, argCol);
        }

        public CsvParser CreateCsvParser(string[] argName, int[] argCol) {
            if (argName.Length!=argCol.Length) {
                string message = String.Format("The Lenght of both arrays must be equal. string[].Lenght={0} != int[].Lenght={1}", argName.Length, argCol.Length);
                throw new ArgumentOutOfRangeException(message);
            }
            CsvParser csvParser = new CsvParser(typeof(TestInfo));
            for (int i = 0; i<argName.Length; ++i) {
                csvParser.CtorArg(argName[i], argCol[i]);
            }
            return csvParser;
        }



        protected TestInfo[] LoadParse_TestInfo(CsvParser testInfo) {
            return testInfo
                .Load(sample_Filtered_DateIntIntDoubleString)
                .Parse<TestInfo>();
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
        protected void AssertFullFilteredSample<T>(object[] testInfoItems, Type type, string name, T value, Func<T, T> func) {
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

        protected readonly string sample_Filtered_DateIntIntDoubleString =
@"2000-01-01,10,21,10.01,lineNumber0
2000-01-02,30,41,30.01,lineNumber1
2000-01-03,50,61,50.01,lineNumber2
2000-01-04,70,81,70.01,lineNumber3
2000-01-05,90,101,90.01,lineNumber4
2000-01-06,110,121,110.01,lineNumber5";

        protected readonly string sample_EmptyLines_DateIntIntDoubleString =
@"
2000-01-01,10,21,10.01,lineNumber0


2000-01-02,30,41,30.01,lineNumber1

2000-01-03,50,61,50.01,lineNumber2
2000-01-04,70,81,70.01,lineNumber3

2000-01-05,90,101,90.01,lineNumber4
2000-01-06,110,121,110.01,lineNumber5
";

        protected readonly string sample_ToFilterWith_DateIntIntDoubleString =
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
