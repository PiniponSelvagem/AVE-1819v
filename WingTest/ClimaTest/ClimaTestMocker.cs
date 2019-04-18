﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Request.Test;
using Mocky;

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

            IWeatherWebApi api = (IWeatherWebApi)mocker.Create();
            LocationInfo[] locals = api.Search("oporto");
            Assert.AreEqual(6, locals.Length);
            Assert.AreEqual("Cuba", locals[5].Country);
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
            IWeatherWebApi api = (IWeatherWebApi)mocker.Create();

            IEnumerable<WeatherInfo> infos = api.PastWeather(37.017, -7.933, DateTime.Parse("2019-01-01"), DateTime.Parse("2019-01-30"));
            int max = int.MinValue;
            foreach (WeatherInfo wi in infos) {
                if (wi.TempC > max) max = wi.TempC;
            }
            Assert.AreEqual(19, max);
        }
    }
}