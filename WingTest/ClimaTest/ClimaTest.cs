using Microsoft.VisualStudio.TestTools.UnitTesting;
using Request.Test;
using System;
using System.Collections.Generic;

namespace Clima.Test {

    [TestClass]
    public class ClimaTest {

        /*
         * For some reason WebClient returns only part of the HTML
         * Don't know if its my VisualStudio / .NET Framework that's
         * bugged, but using MockRequest to read data from file
         * works fine as expected.
         * 
         */

        [TestMethod]
        public void TestLoadSearchOporto() {
            using (WeatherWebApi api = new WeatherWebApi(new MockRequest())) {
                LocationInfo[] locals = api.Search("oporto");
                Assert.AreEqual(6, locals.Length);
                Assert.AreEqual("Cuba", locals[5].Country);
            }
        }

        [TestMethod]
        public void TestLoadPastWeatherOnJanuaryAndCheckMaximumTempC() {
            using (WeatherWebApi api = new WeatherWebApi(new MockRequest())) {
                IEnumerable<WeatherInfo> infos = api.PastWeather(37.017, -7.933, DateTime.Parse("2019-01-01"), DateTime.Parse("2019-01-30"));
                int max = int.MinValue;
                foreach (WeatherInfo wi in infos) {
                    if (wi.TempC > max) max = wi.TempC;
                }
                Assert.AreEqual(19, max);
                //Console.WriteLine(String.Join("\n", infos));
            }
        }
    }
}
