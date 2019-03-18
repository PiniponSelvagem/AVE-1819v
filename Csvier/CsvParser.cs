using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Csvier {
    public class CsvParser {

        private struct ArgCol {
            public string arg;
            public int col;

            public ArgCol(string arg, int col) {
                this.arg = arg;
                this.col = col;
            }
        }

        private KlassInfo klassInfo;
        private List<ArgCol> ctorArgsList  = new List<ArgCol>();
        private List<ArgCol> propArgsList  = new List<ArgCol>();
        private List<ArgCol> fieldArgsList = new List<ArgCol>();
        private int selectedCtorIndex;
        private int propArgsListLength = 0;
        private int fieldArgsListLength = 0;

        private readonly char separator;
        private string[] textData;

        public CsvParser(Type klass, char separator) {
            this.separator = separator;
            klassInfo = new KlassInfo(klass);
        }

        public CsvParser(Type klass) : this(klass, ',') {
        }

        public CsvParser CtorArg(string arg, int col) {
            ctorArgsList.Add(new ArgCol(arg, col));
            SelectConstructor();
            return this;
        }

        public CsvParser PropArg(string arg, int col) {
            propArgsList.Add(new ArgCol(arg, col));
            ++propArgsListLength;
            return this;
        }

        public CsvParser FieldArg(string arg, int col) {
            fieldArgsList.Add(new ArgCol(arg, col));
            ++fieldArgsListLength;
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
            object[] args = new object[klassInfo.GetPamatersForCtor(selectedCtorIndex).Length];

            for (int i = 0; i<ret.Length; ++i) {
                string[] line = textData[i].Split(separator);

                for (int j = 0; j<args.Length; ++j) {
                    ParameterInfo[] pInfo = klassInfo.GetPamatersForCtor(selectedCtorIndex);
                    Type type = pInfo[j].ParameterType;
                    args[j] = TryParseValue(line[ctorArgsList[j].col], type);
                }

                ret[i] = Activator.CreateInstance(klassInfo.Type, args);

                //TODO: extract method                
                for (int pIndx=0; pIndx<propArgsListLength; ++pIndx) {
                    PropertyInfo prop = klassInfo.GetProperty(propArgsList[pIndx].arg);
                    Type type = prop.PropertyType;
                    object obj = TryParseValue(line[propArgsList[pIndx].col], type);
                    prop.SetValue(ret[i], obj);
                }

                //TODO: fields
                
                for (int fIndx = 0; fIndx<fieldArgsListLength; ++fIndx) {
                    FieldInfo field = klassInfo.GetField(fieldArgsList[fIndx].arg);
                    Type type = field.FieldType;
                    object obj = TryParseValue(line[fieldArgsList[fIndx].col], type);
                    field.SetValue(ret[i], obj);
                }
                
                //SetValueForArg
            }

            return ret;
        }


        /*
        private void SetValueForArg<T>(Type type, T t, string[] line, object ret) {
            for (int i = 0; i<fieldArgsListLength; ++i) {
                T t = klassInfo.GetField(fieldArgsList[i].arg);
                object obj = TryParseValue(line[fieldArgsList[i].col], type);
                t.SetValue(ret, obj);
            }
        }
        */










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

        private void SelectConstructor() {
            int argsSize = ctorArgsList.Count();

            for (int i = 0; i<klassInfo.GetConstructorsLength; ++i) {
                ParameterInfo[] pInfos = klassInfo.GetPamatersForCtor(i);
                if (pInfos.Length == argsSize && CheckIfParamsMatch(i, pInfos)) { // check if currentCtor thats being checked has the same number of parameters in argsList
                    selectedCtorIndex = i;
                    break; // Constructor found, can leave now
                }
            }
        }

        private bool CheckIfParamsMatch(int i, ParameterInfo[] pInfos) {
            for (int j = 0; j<pInfos.Length; ++j) {
                if (j==pInfos.Length-1 && ctorArgsList[j].arg.Equals(pInfos[j].Name)) { // parameters names && number of parameters match, select this ctor
                    return true;
                }
            }
            return false;
        }

        private object TryParseValue(string str, Type type) {
            if (typeof(double).Equals(type)) {
                str = str.Replace(".", ",");    // this might create problems if PC set to other culture?
                return Convert.ToDouble(str);
            }
            return Convert.ChangeType(str, type);
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
