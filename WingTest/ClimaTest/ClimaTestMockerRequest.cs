﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocky;
using Request;
using System;
using System.Collections.Generic;

namespace Clima.Test {

    [TestClass]
    public class ClimaTestMockerRequest {

        [TestMethod]
        public void TestLoadSearchOporto() {
            Mocker mocker = new Mocker(typeof(IRequest));
            mocker
                  .When("GetBody")
                  .With("http://api.worldweatheronline.com/premium/v1/search.ashx?query=oporto&format=tab&key=88bcf72882994515b56161022192302")
                  .Return(sample_ForSearchOporto);

            int length = -1;
            string name = null;

            try {
                using (IRequest req = (IRequest)mocker.Create()) {
                    IWeatherWebApi api = new WeatherWebApi(req);
                    LocationInfo[] locals = api.Search("oporto");
                    length = 6;
                    name = locals[5].Country;
                }
            }
            catch (NotImplementedException e) {
                Assert.AreEqual("Dispose", e.TargetSite.Name);
            }
            Assert.AreEqual(6, length);
            Assert.AreEqual("Cuba", name);
        }

        [TestMethod]
        public void TestLoadPastWeatherOnJanuaryAndCheckMaximumTempC() {
            Mocker mocker = new Mocker(typeof(IRequest));
            mocker
                  .When("GetBody")
                  .With("http://api.worldweatheronline.com/premium/v1/past-weather.ashx?q=37,017,-7,933&date=2019-00-01&enddate=2019-00-30&tp=24&format=csv&key=88bcf72882994515b56161022192302")
                  .Return(sample_ForPastWeatherOnJanuaryAndCheckMaximumTempC);

            int max = int.MinValue;

            try {
                using (IRequest req = (IRequest)mocker.Create()) {
                    IWeatherWebApi api = new WeatherWebApi(req);
                    IEnumerable<WeatherInfo> infos = api.PastWeather(37.017, -7.933, DateTime.Parse("2019-01-01"), DateTime.Parse("2019-01-30"));
                    foreach (WeatherInfo wi in infos) {
                        if (wi.TempC > max) max = wi.TempC;
                    }
                }
            }
            catch (NotImplementedException e) {
                Assert.AreEqual("Dispose", e.TargetSite.Name);
            }
            Assert.AreEqual(19, max);
        }




        /* -----------------------------------
        * --- DO NOT IDENT THESE SAMPLES ----
        * ----------------------------------- */

        private readonly string sample_ForSearchOporto =
@"#The Search API
#Data returned is laid out in the following order:-
#AreaName    Country     Region(If available)    Latitude    Longitude   Population(if available)    Weather Forecast URL
#
Oporto	Spain	Galicia	42.383	-7.100	0	http://api-cdn.worldweatheronline.com/v2/weather.aspx?q=42.3833,-7.1
Oporto	Portugal	Porto	41.150	-8.617	0	http://api-cdn.worldweatheronline.com/v2/weather.aspx?q=41.15,-8.6167
Oporto	South Africa	Limpopo	-22.667	29.633	0	http://api-cdn.worldweatheronline.com/v2/weather.aspx?q=-22.6667,29.6333
El Oporto	Mexico	Tamaulipas	23.266	-98.768	0	http://api-cdn.worldweatheronline.com/v2/weather.aspx?q=23.2658,-98.7675
Puerto Oporto	Bolivia	Pando	-9.933	-66.417	0	http://api-cdn.worldweatheronline.com/v2/weather.aspx?q=-9.9333,-66.4167
Oporto	Cuba	Santiago de Cuba	20.233	-76.167	0	http://api-cdn.worldweatheronline.com/v2/weather.aspx?q=20.2333,-76.1667
";

        private readonly string sample_ForPastWeatherOnJanuaryAndCheckMaximumTempC =
@"#The CSV format is in following way:-
#The day information is available in following format:-
#date,maxtempC,maxtempF,mintempC,mintempF,sunrise,sunset,moonrise,moonset,moon_phase,moon_illumination
#
#Hourly information follows below the day in the following way:-
#date,time,tempC,tempF,windspeedMiles,windspeedKmph,winddirdegree,winddir16point,weatherCode,weatherIconUrl,weatherDesc,precipMM,humidity,visibilityKm,pressureMB,cloudcover,HeatIndexC,HeatIndexF,DewPointC,DewPointF,WindChillC,WindChillF,WindGustMiles,WindGustKmph,FeelsLikeC,FeelsLikeF
#
Not Available
2019-01-01,17,63,12,54,07:45 AM,05:26 PM,03:18 AM,02:23 PM,Waning Crescent,34
2019-01-01,24,17,63,6,10,74,ENE,116,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0002_sunny_intervals.png,Partly cloudy,0.0,59,10,1031,43,14,57,6,43,13,56,11,17,13,56
2019-01-02,18,64,12,53,07:45 AM,05:27 PM,04:19 AM,02:59 PM,Waning Crescent,28
2019-01-02,24,18,64,6,9,179,S,116,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0002_sunny_intervals.png,Partly cloudy,0.0,57,10,1030,15,14,57,6,42,13,56,11,17,13,56
2019-01-03,16,60,11,52,07:46 AM,05:27 PM,05:19 AM,03:39 PM,Waning Crescent,21
2019-01-03,24,16,60,7,11,89,E,113,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0001_sunny.png,Sunny,0.0,67,10,1026,3,13,55,7,45,12,54,11,18,12,54
2019-01-04,16,60,13,55,07:46 AM,05:28 PM,06:16 AM,04:22 PM,Waning Crescent,14
2019-01-04,24,16,60,9,15,78,ENE,116,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0002_sunny_intervals.png,Partly cloudy,0.1,73,10,1028,27,14,57,9,48,13,55,14,23,13,55
2019-01-05,16,61,11,52,07:46 AM,05:29 PM,07:10 AM,05:10 PM,Waning Crescent,7
2019-01-05,24,16,61,8,13,70,ENE,113,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0001_sunny.png,Sunny,0.0,64,10,1032,0,13,55,6,43,12,53,13,21,12,53
2019-01-06,17,63,11,51,07:46 AM,05:30 PM,07:59 AM,06:01 PM,New Moon,0
2019-01-06,24,17,63,5,9,83,E,113,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0001_sunny.png,Sunny,0.0,52,10,1030,0,13,56,3,38,12,54,10,17,12,54
2019-01-07,16,61,10,49,07:46 AM,05:31 PM,08:44 AM,06:55 PM,Waxing Crescent,0
2019-01-07,24,16,61,5,8,83,E,113,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0001_sunny.png,Sunny,0.0,51,10,1028,0,12,54,2,36,12,53,9,15,12,53
2019-01-08,16,61,12,53,07:46 AM,05:32 PM,09:24 AM,07:50 PM,Waxing Crescent,7
2019-01-08,24,16,61,7,10,82,E,113,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0001_sunny.png,Sunny,0.0,50,10,1027,0,13,56,3,37,12,54,11,18,12,54
2019-01-09,17,62,11,52,07:46 AM,05:33 PM,10:00 AM,08:45 PM,Waxing Crescent,14
2019-01-09,24,17,62,6,9,106,ESE,116,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0002_sunny_intervals.png,Partly cloudy,0.0,51,10,1023,2,13,56,3,38,12,54,10,17,12,54
2019-01-10,16,60,11,52,07:46 AM,05:34 PM,10:32 AM,09:42 PM,Waxing Crescent,21
2019-01-10,24,16,60,10,16,81,E,116,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0002_sunny_intervals.png,Partly cloudy,0.0,60,10,1018,1,13,56,6,42,12,53,16,26,12,53
2019-01-11,14,58,11,51,07:45 AM,05:35 PM,11:02 AM,10:37 PM,First Quarter,28
2019-01-11,24,14,58,17,28,66,ENE,113,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0001_sunny.png,Sunny,0.0,44,10,1024,0,12,54,0,32,9,49,25,40,9,49
2019-01-12,14,57,9,49,07:45 AM,05:36 PM,11:31 AM,11:34 PM,First Quarter,34
2019-01-12,24,14,57,12,20,67,ENE,113,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0001_sunny.png,Sunny,0.0,52,10,1027,0,11,52,2,35,9,48,18,30,9,48
2019-01-13,16,60,10,50,07:45 AM,05:36 PM,11:59 AM,No moonset,First Quarter,41
2019-01-13,24,16,60,8,13,68,ENE,113,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0001_sunny.png,Sunny,0.0,52,10,1027,0,12,54,3,37,11,52,13,21,11,52
2019-01-14,16,60,10,50,07:45 AM,05:37 PM,12:28 PM,12:31 AM,First Quarter,48
2019-01-14,24,16,60,5,8,149,SSE,113,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0001_sunny.png,Sunny,0.0,54,10,1024,0,12,54,3,38,12,53,9,14,12,53
2019-01-15,15,59,10,51,07:44 AM,05:38 PM,01:00 PM,01:31 AM,Waxing Gibbous,55
2019-01-15,24,15,59,5,8,149,SSE,116,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0002_sunny_intervals.png,Partly cloudy,0.0,64,10,1021,4,12,54,6,42,12,53,8,13,12,53
2019-01-16,15,58,10,51,07:44 AM,05:40 PM,01:36 PM,02:33 AM,Waxing Gibbous,62
2019-01-16,24,15,58,4,7,130,SE,116,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0002_sunny_intervals.png,Partly cloudy,0.1,76,10,1019,33,12,54,8,47,12,54,7,11,12,54
2019-01-17,14,58,11,52,07:44 AM,05:41 PM,02:16 PM,03:38 AM,Waxing Gibbous,69
2019-01-17,24,14,58,8,13,121,ESE,119,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0003_white_cloud.png,Cloudy,0.0,75,10,1019,39,12,54,8,47,11,52,12,19,11,52
2019-01-18,15,59,9,49,07:43 AM,05:42 PM,03:04 PM,04:44 AM,Full Moon,98
2019-01-18,24,15,59,9,15,332,NNW,116,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0002_sunny_intervals.png,Partly cloudy,0.0,64,10,1021,23,12,53,5,41,10,50,14,22,10,50
2019-01-19,16,61,13,56,07:43 AM,05:43 PM,04:01 PM,05:51 AM,Full Moon,98
2019-01-19,24,16,61,14,22,292,WNW,176,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0009_light_rain_showers.png,Patchy rain possible,6.2,83,8,1019,83,14,58,12,53,13,56,20,32,13,56
2019-01-20,16,60,12,54,07:43 AM,05:44 PM,05:05 PM,06:55 AM,Full Moon,98
2019-01-20,24,16,60,18,28,332,NNW,113,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0001_sunny.png,Sunny,0.1,71,10,1018,11,14,57,8,47,12,53,25,40,12,53
2019-01-21,15,60,10,49,07:42 AM,05:45 PM,06:16 PM,07:53 AM,Full Moon,98
2019-01-21,24,15,60,11,18,238,WSW,116,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0002_sunny_intervals.png,Partly cloudy,0.0,67,10,1022,2,12,53,6,42,10,50,17,27,10,50
2019-01-22,14,58,9,48,07:42 AM,05:46 PM,07:30 PM,08:44 AM,Waning Gibbous,97
2019-01-22,24,14,58,14,22,322,NW,119,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0003_white_cloud.png,Cloudy,0.1,71,10,1023,56,12,53,7,44,9,49,19,31,9,49
2019-01-23,16,61,12,53,07:41 AM,05:47 PM,08:42 PM,09:30 AM,Waning Gibbous,90
2019-01-23,24,16,61,17,27,311,NW,116,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0002_sunny_intervals.png,Partly cloudy,0.0,73,10,1021,8,13,56,8,47,12,53,24,39,12,53
2019-01-24,19,66,14,58,07:40 AM,05:48 PM,09:53 PM,10:09 AM,Waning Gibbous,83
2019-01-24,24,19,66,13,21,339,NNW,116,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0002_sunny_intervals.png,Partly cloudy,0.0,78,10,1016,16,16,60,12,53,14,58,21,33,14,58
2019-01-25,19,65,13,55,07:40 AM,05:49 PM,11:01 PM,10:44 AM,Waning Gibbous,76
2019-01-25,24,19,65,9,14,64,ENE,113,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0001_sunny.png,Sunny,0.0,70,10,1018,1,15,59,9,49,14,57,16,25,14,57
2019-01-26,18,64,13,56,07:39 AM,05:50 PM,No moonrise,11:18 AM,Last Quarter,69
2019-01-26,24,18,64,6,10,114,ESE,116,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0002_sunny_intervals.png,Partly cloudy,0.0,69,10,1020,2,15,59,9,49,14,58,12,19,14,58
2019-01-27,16,60,12,53,07:39 AM,05:51 PM,12:07 AM,11:51 AM,Last Quarter,62
2019-01-27,24,16,60,12,19,335,NNW,116,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0002_sunny_intervals.png,Partly cloudy,0.0,71,10,1021,10,13,56,8,46,12,53,17,28,12,53
2019-01-28,17,62,11,52,07:38 AM,05:52 PM,01:11 AM,12:25 PM,Last Quarter,55
2019-01-28,24,17,62,14,23,336,NNW,116,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0002_sunny_intervals.png,Partly cloudy,0.0,70,10,1025,6,13,55,8,46,11,52,21,33,11,52
2019-01-29,16,60,12,54,07:37 AM,05:53 PM,02:13 AM,01:00 PM,Waning Crescent,48
2019-01-29,24,16,60,11,18,300,WNW,176,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0009_light_rain_showers.png,Patchy rain possible,0.1,78,10,1023,39,13,56,10,49,12,54,16,26,12,54
2019-01-30,16,60,14,56,07:36 AM,05:54 PM,03:13 AM,01:39 PM,Waning Crescent,41
2019-01-30,24,16,60,14,23,278,W,266,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0017_cloudy_with_light_rain.png,Light drizzle,0.5,84,8,1018,74,14,57,11,53,13,55,20,33,13,55
";
    }
}
