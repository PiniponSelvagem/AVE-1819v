using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Csvier {
    public class CsvParser {

        private ConstructorInfo[] availableCtors;
        //private ParameterInfo[][] availableParams;

        private List<string> argsList = new List<string>();
        private List<int> colsList = new List<int>();

        private ConstructorInfo selectedCtor;
        
        private char separator;

        public CsvParser(Type klass, char separator) {
            this.separator = separator;

            availableCtors = klass.GetConstructors();
            /*
            availableParams = new ParameterInfo[klass.GetConstructors().Length][];
            for (int i=0; i<availableCtors.Length; ++i) {
                for (int j=0; j<availableParams.Length; ++j) {
                    availableParams[i] = availableCtors[i].GetParameters();
                }
            }
            */

            Console.WriteLine();
        }

        public CsvParser(Type klass) : this(klass, ',') {
        }

        public CsvParser CtorArg(string arg, int col) {
            argsList.Add(arg);
            colsList.Add(col);

            int argsSize = argsList.Count();
            ConstructorInfo cInfo;

            for (int i=0; i<availableCtors.Length; ++i) {
                ParameterInfo[] pInfo = availableCtors[i].GetParameters();
                if (pInfo.Length == argsSize) {
                    for (int j=0; j<pInfo.Length; ++j) {
                        if (!argsList[j].Equals(pInfo[j].Name)) {
                            break;
                        }
                        if (j==pInfo.Length-1) {
                            selectedCtor = availableCtors[i];
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
            string[] arr = src.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            return this;
        }

        public CsvParser Remove(int count) {
            return this;
        }

        public CsvParser RemoveEmpties() {
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
            throw new NotImplementedException();
        }

    }
}
