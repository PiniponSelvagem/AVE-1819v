using Csvier.Attributes;
using System;

namespace Clima {
    public class WeatherInfo {
        [Csv("Date", 0)]
        public DateTime Date { get; }

        [Csv("TempC", 2)]
        public int TempC { get; }

        [Csv("PrecipMM", 11)]
        public double PrecipMM { get; set; }

        [Csv("Desc", 10)]
        public String Desc { get; set; }



        public WeatherInfo() {
        }

        [Csv("date", 0)]
        public WeatherInfo(DateTime date) {
            this.Date = date;
        }

        [Csv("date", 0)]
        [Csv("tempC", 2)]
        public WeatherInfo(DateTime date, int tempC) {
            this.Date = date;
            this.TempC = tempC;
        }

        public override String ToString() {
            return "WeatherInfo{" +
                "date=" + Date +
                " | tempC=" + TempC +
                " | precipMM=" + PrecipMM +
                " | desc='" + Desc + '\'' +
                '}';
        }
    }
}
