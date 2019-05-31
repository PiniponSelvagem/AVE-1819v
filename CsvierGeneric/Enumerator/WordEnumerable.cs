using System.Collections;
using System.Collections.Generic;

namespace CsvierGeneric.Enumerator {
    public class WordEnumerable : IEnumerable<string> {

        private WordEnumerator wordEnumerator;

        public WordEnumerable(string str, char separator) {
            wordEnumerator = new WordEnumerator(str, separator);
        }

        public IEnumerator<string> GetEnumerator() {
            return wordEnumerator;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
