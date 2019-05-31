using Microsoft.VisualStudio.TestTools.UnitTesting;
using Request.Test;
using System.Collections.Generic;

namespace BankOfCanada.Test {

    [TestClass]
    public class ValetTest {

        [TestMethod]
        public void TestLoadListGroups() {
            using (ValetApi api = new ValetApi(new MockRequest())) {
                ListGroupsInfo[] listGroups = api.ListGroups();
                Assert.AreEqual(326, listGroups.Length);
                Assert.AreEqual("FX_RATES_DAILY", listGroups[9].Series);
            }
        }

        [TestMethod]
        public void TestLoadGroup__FX_RATES_DAILY() {
            using (ValetApi api = new ValetApi(new MockRequest())) {
                IEnumerable<GroupSeriesInfo> groupSeries = api.Group("FX_RATES_DAILY");
                int count = 0;
                foreach (GroupSeriesInfo gsi in groupSeries) {
                    Assert.IsTrue(gsi.Series.Length == 8);
                    Assert.IsTrue(gsi.Label.EndsWith("CAD"));
                    ++count;
                }
                Assert.AreEqual(26, count);
            }
        }
    }
}
