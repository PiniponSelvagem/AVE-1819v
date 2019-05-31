using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CsvierGeneric.Enumerator {
    public class WordEnumerator : IEnumerator<string> {

        private CharEnumerator charEnum;
        private string src;
        private char separator;

        private string currStr;
        private int index = -1;

        public WordEnumerator(string src, char separator) {
            this.src = src;
            this.separator = separator;
            if (src != null) {
                charEnum = src.GetEnumerator();
            }
        }
        
        public string Current => currStr;

        object IEnumerator.Current => Current;

        public bool MoveNext() {
            if (src == null || index >= src.Length)
                return false;
            
            AllignIndexForNextIteration();
            StringBuilder strBuilder = new StringBuilder();
            char curr;
            while (charEnum.MoveNext()) {
                curr = charEnum.Current;
                ++index;
                if (index < src.Length) {
                    if (curr != separator) {
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



        private void AllignIndexForNextIteration() {
            while (index+1 < src.Length && src[index+1]==separator) {
                ++index;
                if (!charEnum.MoveNext())
                    break;
            }
        }
    }
}
