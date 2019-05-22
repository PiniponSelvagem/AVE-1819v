using System;
using System.Collections;
using System.Collections.Generic;

namespace CsvierGeneric.Enumerator {
    public class LineEnumerator : IEnumerator<string> {
        private CharEnumerator charEnum;
        private string src;
        private string currStr;
        private bool isEOF = false;
        private int lastIndex = 0;

        public LineEnumerator(string src) {
            this.src = src;
            charEnum = src.GetEnumerator();
        }

        
        public string Current => currStr;

        object IEnumerator.Current => Current;

        public bool MoveNext() {
            int count = 0;
            int length = 0;
            bool ignore = false;
            if (isEOF) return false;
            do {
                if (lastIndex+count>=src.Length) {
                    currStr = src.Substring(lastIndex, length);
                    isEOF = true;
                    return true;
                }
                if (!ignore && (src[lastIndex + count] == '\n' || src[lastIndex + count] == '\r')) { // "\r\n", "\r", "\n"
                    currStr = src.Substring(lastIndex, length);
                    ignore = true;
                }
                if (ignore && !(src[lastIndex + count] == '\n' || src[lastIndex + count] == '\r')) {
                    lastIndex += count;
                    return true;
                }
                ++length;
                ++count;
            } while (charEnum.MoveNext());

            return false;
        }

        public void Reset() {
            currStr = null;
            lastIndex = 0;
            isEOF = false;
            charEnum.Reset();
        }

        public void Dispose() {
            charEnum.Dispose();
        }
    }
}
