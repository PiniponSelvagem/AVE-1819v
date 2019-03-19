using Csvier.Exceptions;
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
            selectedCtorIndex = SelectConstructor();
            return this;
        }

        public CsvParser PropArg(string arg, int col) {
            propArgsList.Add(new ArgCol(arg, col));
            return this;
        }

        public CsvParser FieldArg(string arg, int col) {
            fieldArgsList.Add(new ArgCol(arg, col));
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

        public T[] Parse<T>() {
            if (selectedCtorIndex==-1) {
                throw new ConstructorNotFoundCsvException();
            }

            T[] ret = new T[textData.Length]; // place to store the instances
            object[] args = new object[klassInfo.GetPamatersForCtor(selectedCtorIndex).Length];

            for (int i = 0; i<ret.Length; ++i) {
                string[] line = textData[i].Split(separator);
                SetValuesForConstructors(line, args);
                ret[i] = (T) Activator.CreateInstance(klassInfo.Type, args);
                SetValuesForProperties(line, ret[i]);
                SetValuesForFields(line, ret[i]);
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

        private int SelectConstructor() {
            int argsSize = ctorArgsList.Count();

            for (int i = 0; i<klassInfo.GetConstructorsLength; ++i) {
                ParameterInfo[] pInfos = klassInfo.GetPamatersForCtor(i);
                if (pInfos.Length == argsSize && CheckIfParamsMatch(i, pInfos)) { // check if currentCtor thats being checked has the same number of parameters in argsList
                    return i;
                }
            }
            return -1;
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
            try {
                if (typeof(double).Equals(type)) {
                    str = str.Replace(".", ",");    // this might create problems if PC set to other culture?
                    return Convert.ToDouble(str);
                }
                return Convert.ChangeType(str, type);
            } catch(FormatException) {
                throw new InvalidCastCsvException(type.Name, str);
            }
        }



        private void SetValuesForConstructors(string[] line, object[] args) {
            for (int j = 0; j<args.Length; ++j) {
                ParameterInfo[] pInfo = klassInfo.GetPamatersForCtor(selectedCtorIndex);
                string value = line[ctorArgsList[j].col];
                args[j] = TryParseValue(value, pInfo[j].ParameterType);
            }
        }

        private void SetValuesForProperties(string[] line, object ret) {
            for (int i = 0; i<propArgsList.Count; ++i) {
                PropertyInfo prop = klassInfo.GetProperty(propArgsList[i].arg);
                if (prop==null) {
                    throw new PropertyNotFoundCsvException(klassInfo.Type.Name, propArgsList[i].arg);
                }
                string value = line[propArgsList[i].col];
                object obj = TryParseValue(value, prop.PropertyType);
                prop.SetValue(ret, obj);
            }
        }

        private void SetValuesForFields(string[] line, object ret) {
            for (int i = 0; i<fieldArgsList.Count; ++i) {
                FieldInfo field = klassInfo.GetField(fieldArgsList[i].arg);
                if (field==null) {
                    throw new FieldNotFoundCsvException(klassInfo.Type.Name, fieldArgsList[i].arg);
                }
                string value = line[fieldArgsList[i].col];
                object obj = TryParseValue(value, field.FieldType);
                field.SetValue(ret, obj);
            }
        }
    }
}
