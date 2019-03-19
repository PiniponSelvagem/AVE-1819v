using Csvier;
using Request;
using System;
using System.Globalization;

namespace Clima {
    public class WeatherWebApi : IDisposable {
        const string WEATHER_KEY = "88bcf72882994515b56161022192302";
        const string WEATHER_HOST = "http://api.worldweatheronline.com/premium/v1/";
        const string PATH_WEATHER = WEATHER_HOST + "past-weather.ashx?q={0},{1}&date={2}&enddate={3}&tp=24&format=csv&key=" + WEATHER_KEY;
        const string SEARCH = WEATHER_HOST + "search.ashx?query={0}&format=tab&key=" + WEATHER_KEY;
        const string DATE_FORMAT = "yyyy-mm-dd";        

        readonly CsvParser pastWeather;
        readonly CsvParser locations;
        readonly IRequest req;

        public WeatherWebApi() : this(new HttpRequest()) {
        }

        public WeatherWebApi(IRequest req) {
            this.req = req;
        }

        public void Dispose() {
            req.Dispose();
        }

        public WeatherInfo[] PastWeather(double lat, double log, DateTime from, DateTime to) {
            string latStr = lat.ToString("0.000", CultureInfo.InvariantCulture);
            string logStr = log.ToString("0.000", CultureInfo.InvariantCulture);

            string path = String.Format(PATH_WEATHER, lat, log, from.ToString(DATE_FORMAT), to.ToString(DATE_FORMAT));
            string textData = req.GetBody(path);

            CsvParser pastWeather = new CsvParser(typeof(WeatherInfo))
                .CtorArg("date", 0)
                .CtorArg("tempC", 2)
                .PropArg("PrecipMM", 11)
                .PropArg("Desc", 10);

            WeatherInfo[] items = pastWeather
                .Load(textData)
                .RemoveEmpties()
                .RemoveWith("#")
                .Remove(1)
                .RemoveEvenIndexes()
                .RemoveEmpties()
                .Parse<WeatherInfo>();

            return items;
        }

        public LocationInfo[] Search(string query) {
            string path = String.Format(SEARCH, query);
            string textData = req.GetBody(path);

            CsvParser searchWeather = new CsvParser(typeof(LocationInfo), '\t')
                .CtorArg("country", 1)
                .CtorArg("region", 2)
                .CtorArg("latitude", 3)
                .CtorArg("longitude", 4);

            LocationInfo[] items = searchWeather
                .Load(textData)
                .RemoveEmpties()
                .RemoveWith("#")
                .RemoveEmpties()
                .Parse<LocationInfo>();

            return items;
        }
    }
}
