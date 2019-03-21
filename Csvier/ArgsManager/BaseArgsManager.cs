using Csvier.Exceptions;
using System;
using System.Collections.Generic;

namespace Csvier.ArgsManager {
    abstract class BaseArgsManager : IArgsManager {

        protected readonly KlassInfo klassInfo;
        protected List<ArgCol> argsList  = new List<ArgCol>();

        protected struct ArgCol {
            public string arg;
            public int col;

            public ArgCol(string arg, int col) {
                this.arg = arg;
                this.col = col;
            }
        }

        
        public BaseArgsManager(KlassInfo klassInfo) {
            this.klassInfo = klassInfo;
        }

        public virtual void SetArg(string arg, int col) {
            argsList.Add(new ArgCol(arg, col));
        }

        public abstract void SetValues(string[] line, object ret);


        protected object TryParseValue(string str, Type type) {
            try {
                if (typeof(double).Equals(type)) {
                    str = str.Replace(".", ",");    // this might create problems if PC set to other culture?
                    return Convert.ToDouble(str);
                }
                return Convert.ChangeType(str, type);
            }
            catch (FormatException) {
                throw new InvalidCastCsvException(type.Name, str);
            }
        }
    }
}
