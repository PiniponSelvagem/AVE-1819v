using CsvierGeneric.ArgsContainers;
using CsvierGeneric.Attributes;
using System;

namespace CsvierGeneric {
    public class CsvParserGeneric {

        public readonly Type type;
        private readonly CtorArgs ctorArgs;
        private readonly PropArgs propArgs;
        private readonly FieldArgs fieldArgs;

        private const char DEFAULT_SEPARATOR = ',';
        private readonly char separator;
        private string[] textData;

        public CsvParserGeneric(Type klass, char separator) {
            this.type = klass;
            this.separator = separator;

            KlassInfo klassInfo = new KlassInfo(klass);
            ctorArgs  = new CtorArgs(klassInfo);
            propArgs  = new PropArgs(klassInfo);
            fieldArgs = new FieldArgs(klassInfo);
        }

        public CsvParserGeneric(Type klass) : this(klass, DEFAULT_SEPARATOR) {
        }

        public CsvParserGeneric CtorArg(string arg, int col) {
            ctorArgs.SetArg(arg, col);
            return this;
        }

        public CsvParserGeneric PropArg(string arg, int col) {
            propArgs.SetArg(arg, col);
            return this;
        }

        public CsvParserGeneric FieldArg(string arg, int col) {
            fieldArgs.SetArg(arg, col);
            return this;
        }

        public CsvParserGeneric CsvParserAutoCreate() {
            new CsvAutoCreator(this, type).Set(this, type);
            return this;
        }

        public CsvParserGeneric Load(String src) {
            textData = src.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            return this;
        }

        public CsvParserGeneric Remove(int count) {
            int size = textData.Length-count;
            string[] dest = new string[size];
            Array.Copy(textData, count, dest, 0, size);
            textData = dest;
            return this;
        }

        public CsvParserGeneric RemoveEmpties() {
            return Remove(i => textData[i].Length==0);
        }

        public CsvParserGeneric RemoveWith(string word) {
            return Remove(i => textData[i].StartsWith(word));
        }

        public CsvParserGeneric RemoveEvenIndexes() {
            return Remove(i => i%2==0);
        }

        public CsvParserGeneric RemoveOddIndexes() {
            return Remove(i => i%2==1);
        }

        public T[] Parse<T>() {
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

        

        /* -----------------------------------
         * -------- AUXILIARY METHODS --------
         * ----------------------------------- */

        private CsvParserGeneric Remove(Predicate<int> condition) {
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
