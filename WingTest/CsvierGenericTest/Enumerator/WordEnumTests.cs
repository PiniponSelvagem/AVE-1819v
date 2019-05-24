using System;
using System.Collections.Generic;
using CsvierGeneric.Enumerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvierGeneric.Test {

    [TestClass]
    public class WordEnumTests {

        /* -----------------------------------
         * -------------- WORD ---------------
         * ----------------------------------- */

        [TestMethod]
        public void Word_With_null() {
            // Arrange
            WordEnumerable wordEnumerable = new WordEnumerable(null, ' ');
            IEnumerator<string> wordEnumerator = wordEnumerable.GetEnumerator();

            // Act
            bool isMoved = wordEnumerator.MoveNext();

            // Assert
            Assert.IsFalse(isMoved);
            Assert.IsNull(wordEnumerator.Current);
        }

        [TestMethod]
        public void Word_Zero_Length() {
            // Arrange
            WordEnumerable wordEnumerable = new WordEnumerable("", ' ');
            IEnumerator<string> wordEnumerator = wordEnumerable.GetEnumerator();

            // Act
            bool isMoved1 = wordEnumerator.MoveNext();
            string empty  = wordEnumerator.Current;
            bool isMoved2 = wordEnumerator.MoveNext();

            // Assert
            Assert.IsTrue(isMoved1);
            Assert.AreEqual("", empty);
            Assert.IsFalse(isMoved2);
        }

        [TestMethod]
        public void Word_SingleWord() {
            // Arrange
            WordEnumerable wordEnumerable = new WordEnumerable("Thrall", ' ');
            IEnumerator<string> wordEnumerator = wordEnumerable.GetEnumerator();

            // Act
            bool isMoved1 = wordEnumerator.MoveNext();
            string thrall = wordEnumerator.Current;
            bool isMoved2 = wordEnumerator.MoveNext();

            // Assert
            Assert.IsTrue(isMoved1);
            Assert.AreEqual("Thrall", thrall);
            Assert.IsFalse(isMoved2);
        }

        [TestMethod]
        public void Word_SingleWord_StartingWith_SomeSeperator() {
            // Arrange
            WordEnumerable wordEnumerable = new WordEnumerable(" Thrall", ' ');
            IEnumerator<string> wordEnumerator = wordEnumerable.GetEnumerator();

            // Act
            bool isMoved1 = wordEnumerator.MoveNext();
            string empty  = wordEnumerator.Current;
            bool isMoved2 = wordEnumerator.MoveNext();
            string thrall = wordEnumerator.Current;
            bool isMoved3 = wordEnumerator.MoveNext();

            // Assert
            Assert.IsTrue(isMoved1);
            Assert.AreEqual("", empty);
            Assert.IsTrue(isMoved2);
            Assert.AreEqual("Thrall", thrall);
            Assert.IsFalse(isMoved3);
        }

        [TestMethod]
        public void Word_SingleWord_EndingWith_SomeSeperator() {
            // Arrange
            WordEnumerable wordEnumerable = new WordEnumerable("Thrall ", ' ');
            IEnumerator<string> wordEnumerator = wordEnumerable.GetEnumerator();

            // Act
            bool isMoved1 = wordEnumerator.MoveNext();
            string thrall = wordEnumerator.Current;
            bool isMoved2 = wordEnumerator.MoveNext();
            string empty  = wordEnumerator.Current;
            bool isMoved3 = wordEnumerator.MoveNext();

            // Assert
            Assert.IsTrue(isMoved1);
            Assert.AreEqual("Thrall", thrall);
            Assert.IsTrue(isMoved2);
            Assert.AreEqual("", empty);
            Assert.IsFalse(isMoved3);
        }

        [TestMethod]
        public void Word_SeperatedBy_SomeSeperator() {
            // Arrange
            string wowBFA = "Thrall is coming home boys!";
            char seperator = ' ';
            string[] wowBFA_split = wowBFA.Split(seperator);
            WordEnumerable wordEnumerable = new WordEnumerable(wowBFA, seperator);
            IEnumerator<string> wordEnumerator = wordEnumerable.GetEnumerator();

            // Act
            // Assert
            for (int i=0; i<5; ++i) {
                Assert.IsTrue(wordEnumerator.MoveNext());
                Assert.AreEqual(wowBFA_split[i], wordEnumerator.Current);
            }
            Assert.IsFalse(wordEnumerator.MoveNext());
        }
    }
}
