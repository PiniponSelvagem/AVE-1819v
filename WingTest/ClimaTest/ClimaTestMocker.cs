using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocky;
using System;
using System.Collections.Generic;

namespace Clima.Test {

    [TestClass]
    public class ClimaTestMocker {

        [TestMethod]
        public void TestLoadSearchOporto() {
            Mocker mocker = new Mocker(typeof(IWeatherWebApi));
            mocker
                  .When("Search")
                  .With("oporto")
                  .Return(new LocationInfo[] {
                        null,
                        null,
                        null,
                        null,
                        null,
                        new LocationInfo("Cuba", "", 0, 0)
                  }
            );

            int length = -1;
            string name = null;

            try {
                using (IWeatherWebApi api = (IWeatherWebApi)mocker.Create()) {
                    LocationInfo[] locals = api.Search("oporto");
                    length = 6;
                    name = locals[5].Country;
                }
            }
            catch(NotImplementedException e) {
                Assert.AreEqual("Dispose", e.TargetSite.Name);
            }
            Assert.AreEqual(6, length);
            Assert.AreEqual("Cuba", name);
        }

        [TestMethod]
        public void TestLoadPastWeatherOnJanuaryAndCheckMaximumTempC() {
            Mocker mocker = new Mocker(typeof(IWeatherWebApi));
            mocker
                  .When("PastWeather")
                  .With(37.017, -7.933, DateTime.Parse("2019-01-01"), DateTime.Parse("2019-01-30"))
                  .Return(new WeatherInfo[] {
                        new WeatherInfo(DateTime.Parse("2019-01-01"), 17),
                        new WeatherInfo(DateTime.Parse("2019-01-02"), 18),
                        new WeatherInfo(DateTime.Parse("2019-01-03"), 16),
                        new WeatherInfo(DateTime.Parse("2019-01-04"), 16),
                        new WeatherInfo(DateTime.Parse("2019-01-05"), 16),
                        new WeatherInfo(DateTime.Parse("2019-01-06"), 17),
                        new WeatherInfo(DateTime.Parse("2019-01-07"), 16),
                        new WeatherInfo(DateTime.Parse("2019-01-08"), 16),
                        new WeatherInfo(DateTime.Parse("2019-01-09"), 17),
                        new WeatherInfo(DateTime.Parse("2019-01-10"), 16),
                        new WeatherInfo(DateTime.Parse("2019-01-11"), 14),
                        new WeatherInfo(DateTime.Parse("2019-01-12"), 14),
                        new WeatherInfo(DateTime.Parse("2019-01-13"), 16),
                        new WeatherInfo(DateTime.Parse("2019-01-14"), 16),
                        new WeatherInfo(DateTime.Parse("2019-01-15"), 15),
                        new WeatherInfo(DateTime.Parse("2019-01-16"), 15),
                        new WeatherInfo(DateTime.Parse("2019-01-17"), 14),
                        new WeatherInfo(DateTime.Parse("2019-01-18"), 15),
                        new WeatherInfo(DateTime.Parse("2019-01-19"), 16),
                        new WeatherInfo(DateTime.Parse("2019-01-20"), 16),
                        new WeatherInfo(DateTime.Parse("2019-01-21"), 15),
                        new WeatherInfo(DateTime.Parse("2019-01-22"), 14),
                        new WeatherInfo(DateTime.Parse("2019-01-23"), 16),
                        new WeatherInfo(DateTime.Parse("2019-01-24"), 19),
                        new WeatherInfo(DateTime.Parse("2019-01-25"), 19),
                        new WeatherInfo(DateTime.Parse("2019-01-26"), 18),
                        new WeatherInfo(DateTime.Parse("2019-01-27"), 16),
                        new WeatherInfo(DateTime.Parse("2019-01-28"), 17),
                        new WeatherInfo(DateTime.Parse("2019-01-29"), 16),
                        new WeatherInfo(DateTime.Parse("2019-01-30"), 16)
                  }
            );

            int max = int.MinValue;

            try {
                using (IWeatherWebApi api = (IWeatherWebApi)mocker.Create()) {
                    IEnumerable<WeatherInfo> infos = api.PastWeather(37.017, -7.933, DateTime.Parse("2019-01-01"), DateTime.Parse("2019-01-30"));
                    foreach (WeatherInfo wi in infos) {
                        if (wi.TempC > max) max = wi.TempC;
                    }
                }
            }
            catch(NotImplementedException e) {
                Assert.AreEqual("Dispose", e.TargetSite.Name);
            }
            Assert.AreEqual(19, max);
        }
    }
}
