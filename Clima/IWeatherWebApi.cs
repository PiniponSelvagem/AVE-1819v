using Csvier;
using Request;
using System;
using System.Globalization;

namespace Clima {
    public interface IWeatherWebApi : IDisposable {

        WeatherInfo[] PastWeather(double lat, double log, DateTime from, DateTime to);

        LocationInfo[] Search(string query);
    }
}
