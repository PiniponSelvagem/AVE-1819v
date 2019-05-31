using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CsvierGeneric.Enumerator {
    public class LineEnumerator : IEnumerator<string> {

        private CharEnumerator charEnum;
        private string src;

        private string currStr;
        private int  index = -1;
        private bool lastLineEndWithSlashR;

        internal int    skipNLines;
        internal bool   skipEmpties;
        internal string skipStartingWith;
        //internal bool   skipEvenIndexes;
        //internal bool   skipOddIndexes;

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

            // Check if last line ended with '\r', if this one starts with '\n' then move to next char
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
            
            // If skipEmpties, then check if this line is empty and if true then skip it
            if (skipEmpties) {
                SkipEmpties();
            }

            // If skipNLines, skip lines while skipNLines > 0 
            if (skipNLines > 0) {
                SkipNLines();
            }

            int skipStartingWithIndex = 0, skipStartingWithCount = 0;
            bool shouldSkip = false;
            while (charEnum.MoveNext()) {
                curr = charEnum.Current;
                ++index;
                
                // Check if line starts with skipStartWith, if true mark it to skip
                if (skipStartingWith != null && skipStartingWithIndex < skipStartingWith.Length && skipStartingWith[skipStartingWithIndex++] == curr) {
                    ++skipStartingWithCount;
                    if (skipStartingWithCount == skipStartingWith.Length) {
                        shouldSkip = true;
                    }
                }

                if (index < src.Length) {
                    lastLineEndWithSlashR = (curr=='\r');
                    if (curr != '\r' && curr != '\n') {
                        strBuilder.Append(curr);
                    }
                    else if (shouldSkip) {
                        strBuilder.Clear();
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
                if (!charEnum.MoveNext())
                    break;

                ++index;
            }
        }

        private void SkipNLines() {
            while (index+1 < src.Length && skipNLines > 0) {
                if (!charEnum.MoveNext())
                    break;
                if (src[index+1]=='\r' || src[index+1]=='\n')
                    --skipNLines;

                ++index;
            }

            if (index+1 < src.Length && src[index+1]=='\n') {
                charEnum.MoveNext();
            }
        }
    }
}
