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
        private KlassInfo klassInfo;
        private ConstructorInfo selectedCtor;

        private readonly char separator;
        private string[] textData;

        public CsvParser(Type klass, char separator) {
            this.separator = separator;
            klassInfo = new KlassInfo(klass);
        }

        public CsvParser(Type klass) : this(klass, ',') {
        }

        public CsvParser CtorArg(string arg, int col) {
            argsList.Add(new ValueCol(arg, col));
            int argsSize = argsList.Count();

            for (int i=0; i<klassInfo.GetConstructorsLength; ++i) {
                ParameterInfo[] pInfo = klassInfo.GetPamatersForCtor(i);
                if (pInfo.Length == argsSize) {         // check if currentCtor thats being checked has the same number of parameters in argsList
                    for (int j=0; j<pInfo.Length; ++j) {
                        if (argsList[j].value.Equals(pInfo[j].Name) && j==pInfo.Length-1) { // parameters names && number of parameters match, select this ctor
                            selectedCtor = klassInfo.GetConstructor(i);
                            goto Return;
                        }
                    }
                }
            }
            
            Return:
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
            return Remove(i => textData[i].Length==0);
        }
        
        public CsvParser RemoveWith(string word) {
            return Remove(i => textData[i].StartsWith(word));
        }

        public CsvParser RemoveEvenIndexes() {
            return Remove(i => i%2==0);
        }
        
        public CsvParser RemoveOddIndexes() {
            return Remove(i => i%2==1);
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

                ret[i] = Activator.CreateInstance(klassInfo.type, args);
            }

            return ret;
        }




        /* -----------------------------------
         * -------- AUXILIARY METHODS --------
         * ----------------------------------- */

        private CsvParser Remove(Predicate<int> condition) {
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
