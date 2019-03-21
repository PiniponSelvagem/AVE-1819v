using Csvier.Attributes;
using System;

namespace Csvier.Test {
    public class TestInfo {

        [Csv("FieldArg", 1)]
        public int fieldValueInt;

        [Csv("FieldArg", 3)]
        public double fieldValueDouble;
        
        public string fieldValueString;



        [Csv("CtorArg", 0)]
        public DateTime ValueDate { get; }

        [Csv("CtorArg", 1)]
        public int ValueInt1 { get; }

        [Csv("PropArg", 1)]
        public int ValueInt2 { get; set; }
        
        public int ValueInt3 { get; set; }

        [Csv("PropArg", 3)]
        public double ValueDouble { get; set; }

        [Csv("PropArg", 4)]
        public String ValueString { get; set; }



        public TestInfo() {
        }

        public TestInfo(DateTime valueDate) {
            this.ValueDate = valueDate;
        }

        public TestInfo(int valueInt1) {
            this.ValueInt1 = valueInt1;
        }

        public TestInfo(DateTime valueDate, int valueInt1) {
            this.ValueDate = valueDate;
            this.ValueInt1 = valueInt1;
        }
    }
}
