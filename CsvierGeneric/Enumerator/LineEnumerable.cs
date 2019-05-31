using System;
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

        public LineEnumerable SkipNLines(int count) {
            lineEnumerator.skipNLines = count;
            return this;
        }

        public LineEnumerable SkipEmpties() {
            lineEnumerator.skipEmpties = true;
            return this;
        }

        public LineEnumerable SkipStartWith(string word) {
            lineEnumerator.skipStartingWith = word;
            return this;
        }

        public LineEnumerable SkipEvenLines() {
            throw new NotSupportedException();
            /*
            //(i => i%2==0);
            lineEnumerator.skipEvenIndexes = true;
            return this;
            */
        }

        public LineEnumerable SkipOddLines() {
            throw new NotSupportedException();
            /*
            //(i => i%2==1);
            lineEnumerator.skipOddIndexes = true;
            return this;
            */
        }
    }
}
