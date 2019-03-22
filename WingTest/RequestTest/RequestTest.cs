using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Request.Test {

    [TestClass]
    public class RequestTest {

        [TestMethod]
        public void HttpRequest() {
            using (HttpRequest request = new HttpRequest()) {
                string body = request.GetBody("http://google.com");
                Assert.IsTrue(body.StartsWith("<!doctype html>"));
            }
        }

        [TestMethod]
        public void MockRequest() {
            using (MockRequest request = new MockRequest()) {
                string body = request.GetBody("Test_MockRequest");
                Assert.IsTrue(body.StartsWith("ABC"));
                Assert.IsTrue(body.EndsWith("GHI"));
            }
        }
    }
}
