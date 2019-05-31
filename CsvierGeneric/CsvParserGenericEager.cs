using CsvierGeneric.Enumerator;
using System;
using System.Collections.Generic;

namespace CsvierGeneric {
    public class CsvParserGenericEager<T> : BaseCsvParserGeneric<T> {

        private const char DEFAULT_SEPARATOR = ',';
        private readonly char separator;

        private string[] textData;
        
        public CsvParserGenericEager(Type type, char separator) : base(type) {
            this.separator = separator;
        }

        public CsvParserGenericEager(Type type) : base(type) {
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
            T[] ret = new T[textData.Length]; // place to store the instances

            for (int i = 0; i<ret.Length; ++i) {
                string[] line = textData[i].Split(separator);

                ctorArgs.SetValues(line, null);
                ret[i] = ctorArgs.CreateInstance<T>();
                propArgs.SetValues(line, ret[i]);
                fieldArgs.SetValues(line, ret[i]);
            }

            return ret;
        }

        public override T[] Parse(Func<string, T> parser) {
            T[] ret = new T[textData.Length]; // place to store the instances

            // This string.Join is just to not have duplicate textData (1 splitted, and the other the original string)
            LineEnumerable lineEnumerable = new LineEnumerable(string.Join("\n", textData));
            IEnumerator<string> lineEnumerator = lineEnumerable.GetEnumerator();

            int i = 0;
            while (lineEnumerator.MoveNext()) {
                ret[i++] = parser.Invoke(lineEnumerator.Current);
            }

            return ret;
        }

        public override IEnumerable<T> ToEnumerable(Func<string, T> parser) {
            throw new NotImplementedException();
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
