using Microsoft.VisualStudio.TestTools.UnitTesting;
using Request;

namespace Mocky.Test.Delegates {

    [TestClass]
    public class TestMockDelegatesCalcReq {
        
        [TestMethod]
        public void TestMockerCalculator_WhenAdd() {
            Mocker mock = new Mocker(typeof(ICalculator));
            mock.When("Add").Then<int, int, int>((a, b) => a + b); 
            
            Assert.AreEqual(mock.Invoke("Add", 10, 6), 16);
            Assert.AreNotEqual(mock.Invoke("Add", 10, 6), 0);
        }
        
        [TestMethod]
        public void TestMockerRequest_WhenGetBody_Then() {
            Mocker mock = new Mocker(typeof(IRequest));
            mock.When("GetBody").Then<string, string>(url => "URL: " + url);

            Assert.AreEqual(mock.Invoke("GetBody", "https://worldofwarcraft.com"), "URL: https://worldofwarcraft.com");
            Assert.AreNotEqual(mock.Invoke("GetBody", "https://worldofwarcraft.com"), "");
        }
    }
}
