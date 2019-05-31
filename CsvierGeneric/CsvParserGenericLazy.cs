using CsvierGeneric.Enumerator;
using System;
using System.Collections.Generic;

namespace CsvierGeneric {
    public class CsvParserGenericLazy<T> : BaseCsvParserGeneric<T> {

        private const char DEFAULT_SEPARATOR = ',';
        private readonly char separator;

        private string[] textData;

        public CsvParserGenericLazy(Type type, char separator) : base(type) {
            this.separator = separator;
        }

        public CsvParserGenericLazy(Type type) : base(type) {
            this.separator = DEFAULT_SEPARATOR;
        }

        public override BaseCsvParserGeneric<T> Load(string src) {
            textData = src.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            return this;
        }

        public override BaseCsvParserGeneric<T> Remove(int count) {
            int size = textData.Length-count;
            string[] dest = new string[size];
            Array.Copy(textData, count, dest, 0, size);
            textData = dest;
            return this;
        }

        public override BaseCsvParserGeneric<T> RemoveEmpties() {
            return Remove(i => textData[i].Length==0);
        }

        public override BaseCsvParserGeneric<T> RemoveWith(string word) {
            return Remove(i => textData[i].StartsWith(word));
        }

        public override BaseCsvParserGeneric<T> RemoveEvenIndexes() {
            return Remove(i => i%2==0);
        }

        public override BaseCsvParserGeneric<T> RemoveOddIndexes() {
            return Remove(i => i%2==1);
        }

        public override T[] Parse() {
            throw new NotImplementedException();
        }

        public override T[] Parse(Func<string, T> parser) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Developer note:
        ///     The returned IEnumerable is Lazy, the preparation of the "string" (about the "Remove" methods) isnt.
        ///     This is currently a design decision since it was requested to keep the "T[] Parse(Func<string, T> parser)"
        ///     method working.
        ///     "I am aware that it is working this way right now, and i know how to rework it in case it is needed."
        /// </summary>
        public override IEnumerable<T> ToEnumerable(Func<string, T> parser) {
            LineEnumerable lineEnumerable = new LineEnumerable(string.Join("\n", textData));
            IEnumerator<string> lineEnumerator = lineEnumerable.GetEnumerator();
            
            while (lineEnumerator.MoveNext()) {
                yield return parser.Invoke(lineEnumerator.Current);
            }
        }



        /* -----------------------------------
         * -------- AUXILIARY METHODS --------
         * ----------------------------------- */

        private BaseCsvParserGeneric<T> Remove(Predicate<int> condition) {
            int deleted = 0;
            string[] dest = new string[textData.Length];
            for (int i = 0, j = 0; i<textData.Length; ++i) {
                if (condition(i)) {
                    ++deleted;
                }
                else {
                    dest[j] = textData[i];
                    ++j;
                }
            }
            if (deleted!=0) {
                string[] ret = new string[dest.Length-deleted];
                Array.Copy(dest, 0, ret, 0, dest.Length-deleted);
                textData = ret;
            }
            return this;
        }
    }
}
