using System;

namespace Wing.Test.CsvierTest {
    class TestInfo {
        public DateTime ValueDate { get; }
        public int ValueInt { get; }
        public double ValueDouble { get; set; }
        public String ValueString { get; set; }

        public TestInfo() {
        }

        public TestInfo(DateTime valueDate) {
            this.ValueDate = valueDate;
        }

        public TestInfo(int valueInt) {
            this.ValueInt = valueInt;
        }

        public TestInfo(DateTime valueDate, int valueInt) {
            this.ValueDate = valueDate;
            this.ValueInt = valueInt;
        }
    }
}
