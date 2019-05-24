using System;
using System.Collections.Generic;
using CsvierGeneric.Enumerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvierGeneric.Test {

    [TestClass]
    public class LineEnumTests {

        /* -----------------------------------
         * -------------- LINE ---------------
         * ----------------------------------- */

        [TestMethod]
        public void Line_With_null() {
            // Arrange
            LineEnumerable lineEnumerable = new LineEnumerable(null);
            IEnumerator<string> lineEnumerator = lineEnumerable.GetEnumerator();

            // Act
            bool isMoved = lineEnumerator.MoveNext();

            // Assert
            Assert.IsFalse(isMoved);
            Assert.IsNull(lineEnumerator.Current);
        }

        [TestMethod]
        public void Line_Zero_Length() {
            // Arrange
            LineEnumerable lineEnumerable = new LineEnumerable("");
            IEnumerator<string> lineEnumerator = lineEnumerable.GetEnumerator();

            // Act
            bool isMoved1 = lineEnumerator.MoveNext();
            string empty  = lineEnumerator.Current;
            bool isMoved2 = lineEnumerator.MoveNext();

            // Assert
            Assert.IsTrue(isMoved1);
            Assert.AreEqual("",  empty);
            Assert.IsFalse(isMoved2);
        }

        [TestMethod]
        public void Line_StartingWith_N() {
            // Arrange
            LineEnumerable lineEnumerable = new LineEnumerable("\nThrall");
            IEnumerator<string> lineEnumerator = lineEnumerable.GetEnumerator();

            // Act
            bool isMoved1 = lineEnumerator.MoveNext();
            string empty  = lineEnumerator.Current;
            bool isMoved2 = lineEnumerator.MoveNext();
            string thrall = lineEnumerator.Current;
            bool isMoved3 = lineEnumerator.MoveNext();

            // Assert
            Assert_EmptyThrall(isMoved1, empty, isMoved2, thrall, isMoved3);
        }

        [TestMethod]
        public void Line_StartingWith_R() {
            // Arrange
            LineEnumerable lineEnumerable = new LineEnumerable("\rThrall");
            IEnumerator<string> lineEnumerator = lineEnumerable.GetEnumerator();

            // Act
            bool isMoved1 = lineEnumerator.MoveNext();
            string empty  = lineEnumerator.Current;
            bool isMoved2 = lineEnumerator.MoveNext();
            string thrall = lineEnumerator.Current;
            bool isMoved3 = lineEnumerator.MoveNext();

            // Assert
            Assert_EmptyThrall(isMoved1, empty, isMoved2, thrall, isMoved3);
        }

        [TestMethod]
        public void Line_StartingWith_RN() {
            // Arrange
            LineEnumerable lineEnumerable = new LineEnumerable("\r\nThrall");
            IEnumerator<string> lineEnumerator = lineEnumerable.GetEnumerator();

            // Act
            bool isMoved1 = lineEnumerator.MoveNext();
            string empty  = lineEnumerator.Current;
            bool isMoved2 = lineEnumerator.MoveNext();
            string thrall = lineEnumerator.Current;
            bool isMoved3 = lineEnumerator.MoveNext();

            // Assert
            Assert_EmptyThrall(isMoved1, empty, isMoved2, thrall, isMoved3);
        }

        [TestMethod]
        public void Line_EndingWith_N() {
            // Arrange
            LineEnumerable lineEnumerable = new LineEnumerable("Thrall\n");
            IEnumerator<string> lineEnumerator = lineEnumerable.GetEnumerator();

            // Act
            bool isMoved1 = lineEnumerator.MoveNext();
            string thrall = lineEnumerator.Current;
            bool isMoved2 = lineEnumerator.MoveNext();
            string empty  = lineEnumerator.Current;
            bool isMoved3 = lineEnumerator.MoveNext();

            // Assert
            Assert_ThrallEmpty(isMoved1, thrall, isMoved2, empty, isMoved3);
        }

        [TestMethod]
        public void Line_EndingWith_R() {
            // Arrange
            LineEnumerable lineEnumerable = new LineEnumerable("Thrall\r");
            IEnumerator<string> lineEnumerator = lineEnumerable.GetEnumerator();

            // Act
            bool isMoved1 = lineEnumerator.MoveNext();
            string thrall = lineEnumerator.Current;
            bool isMoved2 = lineEnumerator.MoveNext();
            string empty  = lineEnumerator.Current;
            bool isMoved3 = lineEnumerator.MoveNext();

            // Assert
            Assert_ThrallEmpty(isMoved1, thrall, isMoved2, empty, isMoved3);
        }

        [TestMethod]
        public void Line_EndingWith_RN() {
            // Arrange
            LineEnumerable lineEnumerable = new LineEnumerable("Thrall\r\n");
            IEnumerator<string> lineEnumerator = lineEnumerable.GetEnumerator();

            // Act
            bool isMoved1 = lineEnumerator.MoveNext();
            string thrall = lineEnumerator.Current;
            bool isMoved2 = lineEnumerator.MoveNext();
            string empty  = lineEnumerator.Current;
            bool isMoved3 = lineEnumerator.MoveNext();

            // Assert
            Assert_ThrallEmpty(isMoved1, thrall, isMoved2, empty, isMoved3);
        }

        [TestMethod]
        public void Line_SingleLine_StartingWith_N() {
            Assert.Fail();
        }

        [TestMethod]
        public void Line_SingleLine_StartingWith_R() {
            Assert.Fail();
        }

        [TestMethod]
        public void Line_SingleLine_StartingWith_RN() {
            Assert.Fail();
        }

        [TestMethod]
        public void Line_SingleLine_EndingWith_N() {
            Assert.Fail();
        }

        [TestMethod]
        public void Line_SingleLine_EndingWith_R() {
            Assert.Fail();
        }

        [TestMethod]
        public void Line_SingleLine_EndingWith_RN() {
            Assert.Fail();
        }

        [TestMethod]
        public void Line_SingleLine_WithoutEnding() {
            Assert.Fail();
        }
        


        /* -----------------------------------
         * ---------- AUX METHODS ------------
         * ----------------------------------- */

        private static void Assert_EmptyThrall(bool moved1, string empty, bool moved2, string thrall, bool moved3) {
            Assert.IsTrue(moved1);
            Assert.AreEqual("", empty);
            Assert.IsTrue(moved2);
            Assert.AreEqual("Thrall", thrall);
            Assert.IsFalse(moved3);
        }

        private static void Assert_ThrallEmpty(bool moved1, string thrall, bool moved2, string empty, bool moved3) {
            Assert.IsTrue(moved1);
            Assert.AreEqual("Thrall", thrall);
            Assert.IsTrue(moved2);
            Assert.AreEqual("", empty);
            Assert.IsFalse(moved3);
        }
    }
}
