using Csvier.Exceptions;
using System;
using System.Reflection;

namespace Csvier.ArgsManager.ArgsContainers {
    class CtorArgs : BaseArgsManager {

        private int selectedCtorIndex;
        private object[] currArgsValues;

        public CtorArgs(KlassInfo klassInfo) : base(klassInfo) { }
        
        public override void SetArg(string arg, int col) {
            argsList.Add(new ArgCol(arg, col));
            selectedCtorIndex = SearchConstructor();
        }

        public override void SetValues(string[] line, object instance) {
            if (selectedCtorIndex==-1) {
                throw new ConstructorNotFoundCsvException();
            }

            int ctorParamsLength = klassInfo.GetPamatersForCtor(selectedCtorIndex).Length;
            currArgsValues = new object[ctorParamsLength];

            for (int j = 0; j<currArgsValues.Length; ++j) {
                ParameterInfo[] pInfo = klassInfo.GetPamatersForCtor(selectedCtorIndex);
                string value = line[argsList[j].col];
                currArgsValues[j] = TryParseValue(value, pInfo[j].ParameterType);
            }
        }

        public T CreateInstance<T>() {
            T t = (T) Activator.CreateInstance(klassInfo.Type, currArgsValues);
            currArgsValues = null;
            return t;
        }





        private int SearchConstructor() {
            int argsSize = argsList.Count;

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
                if (j==pInfos.Length-1 && argsList[j].arg.Equals(pInfos[j].Name)) { // parameters names && number of parameters match, select this ctor
                    return true;
                }
            }
            return false;
        }
    }
}
