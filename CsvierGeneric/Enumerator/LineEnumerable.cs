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
    }
}
