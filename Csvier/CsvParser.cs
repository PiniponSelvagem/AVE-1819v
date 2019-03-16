using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Csvier {
    public class CsvParser {

        private struct ValueCol {
            public string value;
            public int col;

            public ValueCol(string value, int col) {
                this.value = value;
                this.col = col;
            }
        }

        private List<ValueCol> argsList = new List<ValueCol>();
        private ConstructorInfo[] availableCtors;
        private ConstructorInfo selectedCtor;

        private Type klass;
        private char separator;

        private string[] textData;

        public CsvParser(Type klass, char separator) {
            this.klass = klass;
            this.separator = separator;

            availableCtors = klass.GetConstructors();
        }

        public CsvParser(Type klass) : this(klass, ',') {
        }

        public CsvParser CtorArg(string arg, int col) {
            argsList.Add(new ValueCol(arg, col));

            int argsSize = argsList.Count();

            for (int i=0; i<availableCtors.Length; ++i) {
                ParameterInfo[] pInfo = availableCtors[i].GetParameters();
                if (pInfo.Length == argsSize) {
                    for (int j=0; j<pInfo.Length; ++j) {
                        if (!argsList[j].value.Equals(pInfo[j].Name)) {
                            break;
                        }
                        if (j==pInfo.Length-1) {
                            selectedCtor = availableCtors[i];
                            break;
                        }
                    }
                }
            }

            return this;
        }

        public CsvParser PropArg(string arg, int col) {
            return this;
        }

        public CsvParser FieldArg(string arg, int col) {
            return this;
        }

        public CsvParser Load(String src) {
            textData = src.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            return this;
        }

        public CsvParser Remove(int count) {
            int size = textData.Length-count;
            string[] dest = new string[size];
            Array.Copy(textData, count, dest, 0, size);
            textData = dest;
            return this;
        }

        public CsvParser RemoveEmpties() {
            int deleted = 0;
            string[] dest = new string[textData.Length];
            for (int i=0, j=0; i<textData.Length; ++i) {
                if (textData[i].Length!=0) {
                    dest[j] = textData[i];
                    ++j;
                }
                else {
                    ++deleted;
                }
            }
            string[] ret = new string[dest.Length-deleted];
            Array.Copy(dest, 0, ret, 0, dest.Length-deleted);
            textData = ret;
            return this;
        }
        
        public CsvParser RemoveWith(string word) {
            return this;
        }

        public CsvParser RemoveEvenIndexes() {
            return this;
        }
        public CsvParser RemoveOddIndexes() {
            return this;
        }

        public object[] Parse() {
            object[] ret = new object[textData.Length];

            for (int i = 0; i<ret.Length; ++i) {
                string[] line = textData[i].Split(separator);
                object[] args = new object[selectedCtor.GetParameters().Length];

                for (int j = 0; j<args.Length; ++j) {
                    ParameterInfo[] pInfo = selectedCtor.GetParameters();
                    Type type = pInfo[j].ParameterType;
                    //args[j] = TryParseValue(line[argsList[j].col], type);
                    args[j] = Convert.ChangeType(line[argsList[j].col], type);
                }

                ret[i] = Activator.CreateInstance(klass, args);
            }

            return ret;
        }


        // TODO: Ask prof if Convert.ChangeType is allowed... or if theres a better way.
        /*
        private object TryParseValue(string str, Type type) {
            object obj = null;

            if (typeof(int).Equals(type)) {
                obj = Int32.Parse(str);
            }
            else if (typeof(DateTime).Equals(type)) {
                obj = DateTime.Parse(str);
            }
            else {
                obj = str;
            }

            return obj;
        }
        */

    }
}
