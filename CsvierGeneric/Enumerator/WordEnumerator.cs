﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace CsvierGeneric.Enumerator {
    public class WordEnumerator : IEnumerator<string> {
        private CharEnumerator charEnum;
        private string src;
        private char separator;

        private string currStr;
        private bool isEOF = false;
        private int lastIndex = 0;

        public WordEnumerator(string src, char separator) {
            this.src = src;
            this.separator = separator;
            charEnum = src.GetEnumerator();
        }
        
        public string Current => currStr;

        object IEnumerator.Current => Current;

        //TODO: TESTS
        //TODO: TESTS
        //TODO: TESTS
        //TODO: TESTS
        //TODO: TESTS

        public bool MoveNext() {
            int count = 0;
            int length = 0;
            bool ignore = false;
            if (isEOF) return false;
            bool foundNR = false;
            do {
                if (lastIndex+count>=src.Length) {
                    currStr = src.Substring(lastIndex, length);
                    isEOF = true;
                    return true;
                }
                foundNR = src[lastIndex + count] == separator;
                if (!ignore && foundNR) {
                    currStr = src.Substring(lastIndex, length);
                    ignore = true;
                }
                if (ignore && !foundNR) {
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
