using CsvierGeneric.ArgsContainers;
using CsvierGeneric.Attributes;
using CsvierGeneric.Enumerator;
using System;
using System.Collections.Generic;

namespace CsvierGeneric {
    public class CsvParserGeneric<T> {

        public readonly Type type;
        private readonly CtorArgs ctorArgs;
        private readonly PropArgs propArgs;
        private readonly FieldArgs fieldArgs;

        private const char DEFAULT_SEPARATOR = ',';
        private readonly char separator;
        private string[] textData;
        
        public CsvParserGeneric(char separator) {
            this.type = typeof(T);
            this.separator = separator;
            
            KlassInfo klassInfo = new KlassInfo(type);
            ctorArgs  = new CtorArgs(klassInfo);
            propArgs  = new PropArgs(klassInfo);
            fieldArgs = new FieldArgs(klassInfo);
        }

        public CsvParserGeneric(Type klass) : this(DEFAULT_SEPARATOR) {
        }

        public CsvParserGeneric<T> CtorArg(string arg, int col) {
            ctorArgs.SetArg(arg, col);
            return this;
        }

        public CsvParserGeneric<T> PropArg(string arg, int col) {
            propArgs.SetArg(arg, col);
            return this;
        }

        public CsvParserGeneric<T> FieldArg(string arg, int col) {
            fieldArgs.SetArg(arg, col);
            return this;
        }

        public CsvParserGeneric<T> CsvParserAutoCreate() {
            new CsvAutoCreator<T>(this, type).Set(this, type);
            return this;
        }

        public CsvParserGeneric<T> Load(String src) {
            textData = src.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            return this;
        }

        public CsvParserGeneric<T> Remove(int count) {
            int size = textData.Length-count;
            string[] dest = new string[size];
            Array.Copy(textData, count, dest, 0, size);
            textData = dest;
            return this;
        }

        public CsvParserGeneric<T> RemoveEmpties() {
            return Remove(i => textData[i].Length==0);
        }

        public CsvParserGeneric<T> RemoveWith(string word) {
            return Remove(i => textData[i].StartsWith(word));
        }

        public CsvParserGeneric<T> RemoveEvenIndexes() {
            return Remove(i => i%2==0);
        }

        public CsvParserGeneric<T> RemoveOddIndexes() {
            return Remove(i => i%2==1);
        }

        public T[] Parse() {
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

        public T[] Parse(Func<string, T> parser) {
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

        /// <summary>
        /// Developer note:
        ///     The returned IEnumerable is Lazy, the preparation of the "string" (about the "Remove" methods) isnt.
        ///     This is currently a design decision since it was requested to keep the "T[] Parse(Func<string, T> parser)"
        ///     method working.
        ///     "I am aware that it is working this way right now, and i know how to rework it in case it is needed."
        /// </summary>
        public IEnumerable<T> ToEnumerable(Func<string, T> parser) {
            LineEnumerable lineEnumerable = new LineEnumerable(string.Join("\n", textData));
            IEnumerator<string> lineEnumerator = lineEnumerable.GetEnumerator();

            int i = 0;
            while (lineEnumerator.MoveNext()) {
                yield return parser.Invoke(lineEnumerator.Current);
            }
        }



        /* -----------------------------------
         * -------- AUXILIARY METHODS --------
         * ----------------------------------- */

        private CsvParserGeneric<T> Remove(Predicate<int> condition) {
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
