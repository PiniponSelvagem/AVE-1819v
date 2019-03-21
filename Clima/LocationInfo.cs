using Csvier.Attributes;
using System;

namespace Clima {
    public class LocationInfo {

        [Csv("CtorArg", 1)]
        public String Country { get; set; }

        [Csv("CtorArg", 2)]
        public String Region { get; set; }

        [Csv("CtorArg", 3)]
        public double Latitude { get; set; }

        [Csv("CtorArg", 4)]
        public double Longitude { get; set; }

        public LocationInfo() {
        }

        public LocationInfo(string country, string region, double latitude, double longitude) {
            Country = country;
            Region = region;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
