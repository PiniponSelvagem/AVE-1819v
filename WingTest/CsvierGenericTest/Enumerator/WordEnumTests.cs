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
            bool isMoved = wordEnumerator.MoveNext();
            
            // Assert
            Assert.IsFalse(isMoved);
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
        public void Word_SingleWord_StartingWith_SomeSeparator() {
            // Arrange
            WordEnumerable wordEnumerable = new WordEnumerable(" Thrall", ' ');
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
        public void Word_SingleWord_EndingWith_SomeSeparator() {
            // Arrange
            WordEnumerable wordEnumerable = new WordEnumerable("Thrall ", ' ');
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
        public void Word_SeparatedBy_SomeSeparator() {
            // Arrange
            string wowBFA = "Thrall is coming home boys!";
            char separator = ' ';
            string[] wowBFA_split = wowBFA.Split(separator);
            WordEnumerable wordEnumerable = new WordEnumerable(wowBFA, separator);
            IEnumerator<string> wordEnumerator = wordEnumerable.GetEnumerator();

            // Act
            // Assert
            for (int i=0; i<5; ++i) {
                Assert.IsTrue(wordEnumerator.MoveNext());
                Assert.AreEqual(wowBFA_split[i], wordEnumerator.Current);
            }
            Assert.IsFalse(wordEnumerator.MoveNext());
        }

        [TestMethod]
        public void Word_SeparatedBy_SomeSeparator_MultipleTimes() {
            // Arrange
            string wowBFA = "FOR THE  HORDE   !";
            char separator = ' ';
            string _for = "FOR", _the = "THE", _horde = "HORDE", _exclamation = "!";
            WordEnumerable wordEnumerable = new WordEnumerable(wowBFA, separator);
            IEnumerator<string> wordEnumerator = wordEnumerable.GetEnumerator();

            // Act
            // Assert
            Assert.IsTrue(wordEnumerator.MoveNext());
            Assert.AreEqual(_for, wordEnumerator.Current);
            Assert.IsTrue(wordEnumerator.MoveNext());
            Assert.AreEqual(_the, wordEnumerator.Current);
            Assert.IsTrue(wordEnumerator.MoveNext());
            Assert.AreEqual(_horde, wordEnumerator.Current);
            Assert.IsTrue(wordEnumerator.MoveNext());
            Assert.AreEqual(_exclamation, wordEnumerator.Current);
            Assert.IsFalse(wordEnumerator.MoveNext());
        }
    }
}
