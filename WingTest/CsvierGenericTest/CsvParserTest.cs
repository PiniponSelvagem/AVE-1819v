using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Clima;
using CsvierGeneric.Enumerator;
using System.Collections.Generic;

namespace CsvierGeneric.Test {

    [TestClass]
    public class CsvParserTest {

        [TestMethod]
        public void Eager_Parse_NoParameters() {
            // Arrange
            CsvParserGeneric<WeatherInfo> pastWeather = new CsvParserGeneric<WeatherInfo>(typeof(WeatherInfo))
                            .CtorArg("date", 0)
                            .CtorArg("tempC", 2)
                            .PropArg("Desc", 10)
                            .PropArg("PrecipMM", 11);

            // Act
            object[] items = pastWeather
                            .Load(sampleWeatherInLisbonFiltered)
                            .RemoveEmpties()
                            .Parse();

            // Assert
            AssertAllItems((IEnumerable<WeatherInfo>)items);
        }

        [TestMethod]
        public void Eager_Parse_WithParam_Func() {
            // Arrange
            CsvParserGeneric<WeatherInfo> pastWeather = new CsvParserGeneric<WeatherInfo>(typeof(WeatherInfo));
                            //.CtorArg("date", 0)
                            //.CtorArg("tempC", 2)
                            //.PropArg("Desc", 10)
                            //.PropArg("PrecipMM", 11);

            // Act
            object[] items = pastWeather
                            .Load(sampleWeatherInLisbonFiltered)
                            .RemoveEmpties()
                            .Parse(s => {
                                    WordEnumerable wordEnumerable = new WordEnumerable(s, ',');
                                    IEnumerator<string> wordEnumerator = wordEnumerable.GetEnumerator();
                                    wordEnumerator.MoveNext(); //go to: 0
                                    DateTime date = DateTime.Parse(wordEnumerator.Current);
                                    
                                    wordEnumerator.MoveNext(); //go to: 1
                                    wordEnumerator.MoveNext(); //go to: 2
                                    Int32 tempC = Int32.Parse(wordEnumerator.Current);

                                    for (int i = 2; i<10; ++i) {
                                        wordEnumerator.MoveNext(); //go to: 10
                                    }
                                    string desc = wordEnumerator.Current;

                                    wordEnumerator.MoveNext(); //go to: 11
                                    double precipMM = Double.Parse(wordEnumerator.Current.Replace(".", ",")); //Replacing '.' with ',' due to current set culture (migh crash on other culture)

                                    WeatherInfo w = new WeatherInfo(date, tempC);
                                    w.Desc = desc;
                                    w.PrecipMM = precipMM;
                                    return w;
                                }
                            );

            // Assert
            AssertAllItems((IEnumerable<WeatherInfo>)items);
        }

        [TestMethod]
        public void Eager_Assert__Parse_WithParam_Func() {
            // Arrange
            int count = 0;
            CsvParserGeneric<WeatherInfo> pastWeather = new CsvParserGeneric<WeatherInfo>(typeof(WeatherInfo));

            // Act
            object[] items = pastWeather
                            .Load(sampleWeatherInLisbonFiltered)
                            .RemoveEmpties()
                            .Parse(s => {
                                    ++count;          
                                    WeatherInfo w = new WeatherInfo();
                                    return w;
                                }
                            );

            // Assert
            Assert.AreEqual(count, 4);
        }

        [TestMethod]
        public void Lazy_Assert__Parse_WithParam_Func() {
            // Arrange
            int count = 0;
            IEnumerator<WeatherInfo> enumWI;
            CsvParserGeneric<WeatherInfo> pastWeather = new CsvParserGeneric<WeatherInfo>(typeof(WeatherInfo));

            // Act
            IEnumerable<WeatherInfo> items = pastWeather
                            .Load(sampleWeatherInLisbonFiltered)
                            .RemoveEmpties()
                            .ToEnumerable(s => {
                                    ++count;
                                    WeatherInfo w = new WeatherInfo();
                                    return w;
                                }
                            );
            enumWI = items.GetEnumerator();

            // Assert
            for (int i=0; i<4; ++i) {
                Assert.AreEqual(count, i);
                Assert.IsTrue(enumWI.MoveNext());
            }
            Assert.IsFalse(enumWI.MoveNext());
        }

        [TestMethod]
        public void Lazy_Parse_WithParam_Func() {
            // Arrange
            CsvParserGeneric<WeatherInfo> pastWeather = new CsvParserGeneric<WeatherInfo>(typeof(WeatherInfo));
                            //.CtorArg("date", 0)
                            //.CtorArg("tempC", 2)
                            //.PropArg("Desc", 10)
                            //.PropArg("PrecipMM", 11);

            // Act
            IEnumerable<WeatherInfo> items = pastWeather
                            .Load(sampleWeatherInLisbonFiltered)
                            .RemoveEmpties()
                            .ToEnumerable(s => {
                                    WordEnumerable wordEnumerable = new WordEnumerable(s, ',');
                                    IEnumerator<string> wordEnumerator = wordEnumerable.GetEnumerator();
                                    wordEnumerator.MoveNext(); //go to: 0
                                    DateTime date = DateTime.Parse(wordEnumerator.Current);

                                    wordEnumerator.MoveNext(); //go to: 1
                                    wordEnumerator.MoveNext(); //go to: 2
                                    Int32 tempC = Int32.Parse(wordEnumerator.Current);

                                    for (int i = 2; i<10; ++i) {
                                        wordEnumerator.MoveNext(); //go to: 10
                                    }
                                    string desc = wordEnumerator.Current;

                                    wordEnumerator.MoveNext(); //go to: 11
                                    double precipMM = Double.Parse(wordEnumerator.Current.Replace(".", ",")); //Replacing '.' with ',' due to current set culture (migh crash on other culture)

                                    WeatherInfo w = new WeatherInfo(date, tempC);
                                    w.Desc = desc;
                                    w.PrecipMM = precipMM;
                                    return w;
                                }
                            );

            // Assert
            AssertAllItems(items);
        }



        /* -----------------------------------
         * ---------- AUX METHODS ------------
         * ----------------------------------- */

        private static void AssertAllItems(IEnumerable<WeatherInfo> items) {
            foreach (object item in items) {
                WeatherInfo wi = (WeatherInfo) item;
                Assert.IsNotNull(wi.Date);
                Assert.IsTrue(wi.TempC > 0);
                Assert.IsNotNull(wi.Desc);
                Assert.IsTrue(wi.PrecipMM > 0);
            }
        }

        string sampleWeatherInLisbonFiltered =
@"2019-01-01,24,17,63,6,10,74,ENE,116,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0002_sunny_intervals.png,Partly cloudy,0.2,59,10,1031,43,14,57,6,43,13,56,11,17,13,56
2019-01-02,24,18,64,6,9,179,S,116,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0002_sunny_intervals.png,Partly cloudy,0.3,57,10,1030,15,14,57,6,42,13,56,11,17,13,56
2019-01-03,24,16,60,7,11,89,E,113,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0001_sunny.png,Sunny,0.2,67,10,1026,3,13,55,7,45,12,54,11,18,12,54
2019-01-04,24,16,60,9,15,78,ENE,116,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0002_sunny_intervals.png,Partly cloudy,0.1,73,10,1028,27,14,57,9,48,13,55,14,23,13,55
";
    }
}
