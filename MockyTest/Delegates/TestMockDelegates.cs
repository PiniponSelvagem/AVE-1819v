using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mocky.Test.Delegates {

    [TestClass]
    public class TestMockDelegates {

        readonly Mocker mockTest;

        public TestMockDelegates() {
            mockTest = new Mocker(typeof(ITest));
            mockTest.When("Void").Then(() => {/* do nothing */});
            mockTest.When("StringParam").Then<string, string>(str => "str="+str);
            mockTest.When("IntTwoParam").Then<int, int, int>((a, b) => a+b);
        }

        [TestMethod]
        public void Method_Void_Validate() {
            Assert.AreEqual(mockTest.Invoke("Void"), null);
        }

        [TestMethod]
        public void Method_StringParam_Validate() {
            string leatherBelt = "OH DUDE! 4 STRENGTH 4 STAMINA LEATHER BELT?";
            Assert.AreEqual(mockTest.Invoke("StringParam", leatherBelt), "str="+leatherBelt);
        }

        [TestMethod]
        public void Method_IntTwoParam_Validate() {
            Assert.AreEqual(mockTest.Invoke("IntTwoParam", 4, 8), 12);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldFail_StringParam_Overload() {
            mockTest.When("StringParam").Then<string, string>(str => str+" AHHH! LEVEL 18? AUH UUGH");
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void ShouldFail_WhenInvoking_IntOneParam() {
            mockTest.Invoke("IntOneParam", 666);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ShouldFail_WhenInvoking_SomeMethod_But_WasImplemented_Using_WithReturn() {
            mockTest.When("SomeMethod").With(1).Return(666);
            mockTest.Invoke("SomeMethod", 1);
        }
    }
}
