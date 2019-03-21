using Csvier.Attributes;
using System;

namespace Clima {
    public class WeatherInfo {
        [Csv("CtorArg", 0)]
        public DateTime Date { get; }

        [Csv("CtorArg", 1)]
        public int TempC { get; }

        [Csv("PropArg", 11)]
        public double PrecipMM { get; set; }

        [Csv("PropArg", 10)]
        public String Desc { get; set; }

        public WeatherInfo() {
        }

        public WeatherInfo(DateTime date) {
            this.Date = date;
        }

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
