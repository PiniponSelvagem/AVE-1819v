using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Request;

namespace Mocky.Test {

    [TestClass]
    public class TestMockerForPartialRequest {
        readonly IRequest req;

        public TestMockerForPartialRequest() {
            Mocker mock = new Mocker(typeof(IRequest));
            mock.When("GetBody").With("testURL1").Return("returned body of 1");
            mock.When("GetBody").With("testURL2").Return("returned body of 2");
            req = (IRequest)mock.Create();
        }
        
        [TestMethod]
        public void TestRequestSuccessfully() {
            Assert.AreEqual(req.GetBody("testURL1"), "returned body of 1");
            Assert.AreEqual(req.GetBody("invalid"), null); // Returns null because that behavior was not defined for GetBody
            Assert.AreEqual(req.GetBody("testURL2"), "returned body of 2");
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void TestRequestFailing() {
            req.Dispose(); // NotImplementedException
        }
    }
}