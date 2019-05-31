using System.Collections;
using System.Collections.Generic;

namespace CsvierGeneric.Enumerator {

    public class LineEnumerable : IEnumerable<string> {

        private LineEnumerator lineEnumerator;

        public LineEnumerable(string str) {
            lineEnumerator = new LineEnumerator(str);
        }

        public IEnumerator<string> GetEnumerator() {
            return lineEnumerator;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void RemoveNLines(int count) {
            lineEnumerator.removeNLines = count;
        }

        public void RemoveEmpties() {
            lineEnumerator.removeEmpties = true;
        }

        public void RemoveStartWith(string word) {
            lineEnumerator.removeStartingWith = word;
        }

        public void RemoveEvenIndexes() {
            //(i => i%2==0);
            lineEnumerator.removeEvenIndexes = true;
        }

        public void RemoveOddIndexes() {
            //(i => i%2==1);
            lineEnumerator.removeOddIndexes = true;
        }
    }
}
