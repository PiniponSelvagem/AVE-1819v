using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace CsvierGeneric.Enumerator.Test {

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
            bool isMoved = lineEnumerator.MoveNext();

            // Assert
            Assert.IsFalse(isMoved);
        }

        [TestMethod]
        public void Line_StartingWith_N() {
            // Arrange
            LineEnumerable lineEnumerable = new LineEnumerable("\nThrall");
            IEnumerator<string> lineEnumerator = lineEnumerable.GetEnumerator();

            // Act
            bool isMoved1 = lineEnumerator.MoveNext();
            string thrall  = lineEnumerator.Current;
            bool isMoved2 = lineEnumerator.MoveNext();

            // Assert
            Assert_Thrall(isMoved1, thrall, isMoved2);
        }

        [TestMethod]
        public void Line_StartingWith_R() {
            // Arrange
            LineEnumerable lineEnumerable = new LineEnumerable("\rThrall");
            IEnumerator<string> lineEnumerator = lineEnumerable.GetEnumerator();

            // Act
            bool isMoved1 = lineEnumerator.MoveNext();
            string thrall = lineEnumerator.Current;
            bool isMoved2 = lineEnumerator.MoveNext();

            // Assert
            Assert_Thrall(isMoved1, thrall, isMoved2);
        }

        [TestMethod]
        public void Line_StartingWith_RN() {
            // Arrange
            LineEnumerable lineEnumerable = new LineEnumerable("\r\nThrall");
            IEnumerator<string> lineEnumerator = lineEnumerable.GetEnumerator();

            // Act
            bool isMoved1 = lineEnumerator.MoveNext();
            string thrall  = lineEnumerator.Current;
            bool isMoved2 = lineEnumerator.MoveNext();

            // Assert
            Assert_Thrall(isMoved1, thrall, isMoved2);
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

            // Assert
            Assert_Thrall(isMoved1, thrall, isMoved2);
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

            // Assert
            Assert_Thrall(isMoved1, thrall, isMoved2);
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

            // Assert
            Assert_Thrall(isMoved1, thrall, isMoved2);
        }

        [TestMethod]
        public void Line_SingleLine_StartingWith_N() {
            // Arrange
            string wowBFA = "\nThrall is coming home boys!";
            LineEnumerable lineEnumerable = new LineEnumerable(wowBFA);
            IEnumerator<string> lineEnumerator = lineEnumerable.GetEnumerator();

            // Act
            // Assert
            Assert.IsTrue(lineEnumerator.MoveNext());
            Assert.AreEqual(wowBFA.Substring(1, 27), lineEnumerator.Current);
            Assert.IsFalse(lineEnumerator.MoveNext());
        }

        [TestMethod]
        public void Line_SingleLine_StartingWith_R() {
            // Arrange
            string wowBFA = "\rThrall is coming home boys!";
            LineEnumerable lineEnumerable = new LineEnumerable(wowBFA);
            IEnumerator<string> lineEnumerator = lineEnumerable.GetEnumerator();

            // Act
            // Assert
            Assert.IsTrue(lineEnumerator.MoveNext());
            Assert.AreEqual(wowBFA.Substring(1, 27), lineEnumerator.Current);
            Assert.IsFalse(lineEnumerator.MoveNext());
        }

        [TestMethod]
        public void Line_SingleLine_StartingWith_RN() {
            // Arrange
            string wowBFA = "\r\nThrall is coming home boys!";
            LineEnumerable lineEnumerable = new LineEnumerable(wowBFA);
            IEnumerator<string> lineEnumerator = lineEnumerable.GetEnumerator();

            // Act
            // Assert
            Assert.IsTrue(lineEnumerator.MoveNext());
            Assert.AreEqual(wowBFA.Substring(2, 27), lineEnumerator.Current);
            Assert.IsFalse(lineEnumerator.MoveNext());
        }

        [TestMethod]
        public void Line_SingleLine_EndingWith_N() {
            // Arrange
            string wowBFA = "Thrall is coming home boys!\n";
            LineEnumerable lineEnumerable = new LineEnumerable(wowBFA);
            IEnumerator<string> lineEnumerator = lineEnumerable.GetEnumerator();

            // Act
            // Assert
            Assert.IsTrue(lineEnumerator.MoveNext());
            Assert.AreEqual(wowBFA.Substring(0, 27), lineEnumerator.Current);
            Assert.IsFalse(lineEnumerator.MoveNext());
        }

        [TestMethod]
        public void Line_SingleLine_EndingWith_R() {
            // Arrange
            string wowBFA = "Thrall is coming home boys!\r";
            LineEnumerable lineEnumerable = new LineEnumerable(wowBFA);
            IEnumerator<string> lineEnumerator = lineEnumerable.GetEnumerator();

            // Act
            // Assert
            Assert.IsTrue(lineEnumerator.MoveNext());
            Assert.AreEqual(wowBFA.Substring(0, 27), lineEnumerator.Current);
            Assert.IsFalse(lineEnumerator.MoveNext());
        }

        [TestMethod]
        public void Line_SingleLine_EndingWith_RN() {
            // Arrange
            string wowBFA = "Thrall is coming home boys!\r\n";
            LineEnumerable lineEnumerable = new LineEnumerable(wowBFA);
            IEnumerator<string> lineEnumerator = lineEnumerable.GetEnumerator();

            // Act
            // Assert
            Assert.IsTrue(lineEnumerator.MoveNext());
            Assert.AreEqual(wowBFA.Substring(0, 27), lineEnumerator.Current);
            Assert.IsFalse(lineEnumerator.MoveNext());
        }

        [TestMethod]
        public void Line_SingleLine_WithoutEnding() {
            // Arrange
            string wowBFA = "Thrall is coming home boys!";
            LineEnumerable lineEnumerable = new LineEnumerable(wowBFA);
            IEnumerator<string> lineEnumerator = lineEnumerable.GetEnumerator();

            // Act
            // Assert
            Assert.IsTrue(lineEnumerator.MoveNext());
            Assert.AreEqual(wowBFA, lineEnumerator.Current);
            Assert.IsFalse(lineEnumerator.MoveNext());
        }

        [TestMethod]
        public void Line_MultiLine_With_N() {
            // Arrange
            string wowBFA = "Thrall is coming home boys!\nFOR THE HORDE!!!";
            string[] wowBFA_split = wowBFA.Split('\n');
            LineEnumerable lineEnumerable = new LineEnumerable(wowBFA);
            IEnumerator<string> lineEnumerator = lineEnumerable.GetEnumerator();

            // Act
            // Assert
            for (int i = 0; i<2; ++i) {
                Assert.IsTrue(lineEnumerator.MoveNext());
                Assert.AreEqual(wowBFA_split[i], lineEnumerator.Current);
            }
            Assert.IsFalse(lineEnumerator.MoveNext());
        }

        [TestMethod]
        public void Line_MultiLine_With_R() {
            // Arrange
            string wowBFA = "Thrall is coming home boys!\rFOR THE HORDE!!!";
            string[] wowBFA_split = wowBFA.Split('\r');
            LineEnumerable lineEnumerable = new LineEnumerable(wowBFA);
            IEnumerator<string> lineEnumerator = lineEnumerable.GetEnumerator();

            // Act
            // Assert
            for (int i = 0; i<2; ++i) {
                Assert.IsTrue(lineEnumerator.MoveNext());
                Assert.AreEqual(wowBFA_split[i], lineEnumerator.Current);
            }
            Assert.IsFalse(lineEnumerator.MoveNext());
        }

        [TestMethod]
        public void Line_MultiLine_With_RN() {
            // Arrange
            string wowBFA = "Thrall is coming home boys!\r\nFOR THE HORDE!!!";
            string line1 = wowBFA.Split('\r')[0];
            string line2 = wowBFA.Split('\n')[1];
            LineEnumerable lineEnumerable = new LineEnumerable(wowBFA);
            IEnumerator<string> lineEnumerator = lineEnumerable.GetEnumerator();

            // Act
            // Assert
            Assert.IsTrue(lineEnumerator.MoveNext());
            Assert.AreEqual(line1, lineEnumerator.Current);
            Assert.IsTrue(lineEnumerator.MoveNext());
            Assert.AreEqual(line2, lineEnumerator.Current);
            Assert.IsFalse(lineEnumerator.MoveNext());
        }

        [TestMethod]
        public void Line_MultiLine_SkipEmpty() {
            // Arrange
            string wowBFA = "Thrall is coming home boys!\r\n\r\nFOR THE HORDE!!!";
            string line1 = wowBFA.Split('\r')[0];
            string line2 = wowBFA.Split('\n')[2];
            LineEnumerable lineEnumerable = new LineEnumerable(wowBFA);
            IEnumerator<string> lineEnumerator = lineEnumerable.GetEnumerator();

            // Act
            // Assert
            Assert.IsTrue(lineEnumerator.MoveNext());
            Assert.AreEqual(line1, lineEnumerator.Current);
            Assert.IsTrue(lineEnumerator.MoveNext());
            Assert.AreEqual(line2, lineEnumerator.Current);
            Assert.IsFalse(lineEnumerator.MoveNext());
        }



        /* -----------------------------------
         * ---------- AUX METHODS ------------
         * ----------------------------------- */

        private static void Assert_Thrall(bool moved1, string thrall, bool moved2) {
            Assert.IsTrue(moved1);
            Assert.AreEqual("Thrall", thrall);
            Assert.IsFalse(moved2);
        }
    }
}
