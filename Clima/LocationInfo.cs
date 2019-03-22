using Csvier.Attributes;
using System;

namespace Clima {
    public class LocationInfo {

        [Csv("Country", 1)]
        public String Country { get; set; }

        [Csv("Region", 2)]
        public String Region { get; set; }

        [Csv("Latitude", 3)]
        public double Latitude { get; set; }

        [Csv("Longitude", 4)]
        public double Longitude { get; set; }



        public LocationInfo() {
        }

        [Csv("country", 1)]
        [Csv("region", 2)]
        [Csv("latitude", 3)]
        [Csv("longitude", 4)]
        public LocationInfo(string country, string region, double latitude, double longitude) {
            Country = country;
            Region = region;
            Latitude = latitude;
            Longitude = longitude;
        }

        public override String ToString() {
            return "LocationInfo{" +
                "country=" + Country +
                " | region=" + Region +
                " | latitude=" + Latitude +
                " | longitude='" + Longitude + '\'' +
                '}';
        }
    }
}
