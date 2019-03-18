using System;

namespace Wing.Test.CsvierTest {
    class TestInfo {
        public int fieldValueInt;
        public double fieldValueDouble;
        public string fieldValueString;

        public DateTime ValueDate { get; }
        public int ValueInt1 { get; }
        public int ValueInt2 { get; set; }
        public int ValueInt3 { get; set; }
        public double ValueDouble { get; set; }
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
