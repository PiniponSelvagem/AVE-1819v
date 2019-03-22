using Csvier.Attributes;
using System;

namespace BankOfCanada {
    public class ListGroupsInfo {
        [Csv("Series", 0)]
        public String Series { get; set; }

        [Csv("Label", 1)]
        public String Label { get; set; }

        [Csv("Link", 2)]
        public String Link { get; set; }


        public ListGroupsInfo() {
        }

        [Csv("series", 0)]
        [Csv("label", 1)]
        public ListGroupsInfo(string series, string label) {
            Series = series;
            Label = label;
        }

        [Csv("series", 0)]
        [Csv("label", 1)]
        [Csv("link", 2)]
        public ListGroupsInfo(string series, string label, string link) {
            Series = series;
            Label = label;
            Link = link;
        }

        public override String ToString() {
            return "ListGroupsInfo{" +
                "series=" + Series +
                " | label=" + Label +
                " | link='" + Link + '\'' +
                '}';
        }
    }
}
