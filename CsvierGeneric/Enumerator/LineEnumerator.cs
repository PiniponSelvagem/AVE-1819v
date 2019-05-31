using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CsvierGeneric.Enumerator {
    public class LineEnumerator : IEnumerator<string> {

        private CharEnumerator charEnum;
        private string src;

        private string currStr;
        private int index = -1;
        private bool lastLineEndWithSlashR;

        internal int    skipNLines;
        internal bool   skipEmpties;
        internal string skipStartingWith;
        internal bool   skipEvenIndexes;
        internal bool   skipOddIndexes;

        public LineEnumerator(string src) {
            this.src = src;
            if (src != null) {
                charEnum = src.GetEnumerator();
            }
        }
        
        public string Current => currStr;

        object IEnumerator.Current => Current;
        
        public bool MoveNext() {
            if (src == null || index >= src.Length) {
                return false;
            }

            StringBuilder strBuilder = new StringBuilder();
            char curr;

            if (lastLineEndWithSlashR) {
                lastLineEndWithSlashR = false;
                if (charEnum.MoveNext()) {
                    curr = charEnum.Current;
                    if (curr!='\n') {
                        strBuilder.Append(curr);
                    }
                }
                else {
                    currStr = null;
                    return false;
                }
            }

            if (skipEmpties) {
                SkipEmpties();
            }
            
            while (charEnum.MoveNext()) {
                curr = charEnum.Current;
                ++index;
                if (index < src.Length) {
                    lastLineEndWithSlashR = (curr=='\r');
                    if (curr != '\r' && curr != '\n') {
                        strBuilder.Append(curr);
                    }
                    else if (index != 0) {
                        currStr = strBuilder.ToString();
                        return true;
                    }
                }
            }

            if (strBuilder.Length != 0) {
                currStr = strBuilder.ToString();
                return true;
            }

            currStr = null;
            return false;
        }

        public void Reset() {
            if (src != null) {
                currStr = null;
                index = -1;
                charEnum.Reset();
            }
        }

        public void Dispose() {
            charEnum.Dispose();
        }


        private void SkipEmpties() {
            while (index+1 < src.Length && (src[index+1]=='\r' || src[index+1]=='\n')) {
                ++index;
                if (!charEnum.MoveNext())
                    break;
            }
        }
    }
}
