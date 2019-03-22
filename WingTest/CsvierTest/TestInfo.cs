using Csvier.Attributes;
using System;

namespace Csvier.Test {
    public class TestInfo {

        [Csv("fieldValueInt", 1)]
        public int fieldValueInt;

        [Csv("fieldValueDouble", 3)]
        public double fieldValueDouble;
        
        public string fieldValueString;



        [Csv("ValueDate", 0)]
        public DateTime ValueDate { get; }

        [Csv("ValueInt1", 1)]
        public int ValueInt1 { get; }

        [Csv("ValueInt2", 1)]
        public int ValueInt2 { get; set; }
        
        public int ValueInt3 { get; set; }

        [Csv("ValueDouble", 3)]
        public double ValueDouble { get; set; }

        [Csv("ValueString", 4)]
        public String ValueString { get; set; }



        public TestInfo() {
        }

        [Csv("valueDate", 0)]
        public TestInfo(DateTime valueDate) {
            this.ValueDate = valueDate;
        }

        [Csv("valueInt1", 1)]
        public TestInfo(int valueInt1) {
            this.ValueInt1 = valueInt1;
        }

        [Csv("valueDate", 0)]
        [Csv("valueInt1", 1)]
        public TestInfo(DateTime valueDate, int valueInt1) {
            this.ValueDate = valueDate;
            this.ValueInt1 = valueInt1;
        }
    }
}
