using CsvierGeneric.ArgsContainers.ArgsContainers;
using CsvierGeneric.Attributes;
using CsvierGeneric.Enumerator;
using System;
using System.Collections.Generic;

namespace CsvierGeneric {
    public abstract class BaseCsvParserGeneric<T> {

        public readonly Type type;
        protected readonly CtorArgs ctorArgs;
        protected readonly PropArgs propArgs;
        protected readonly FieldArgs fieldArgs;
        
        
        public BaseCsvParserGeneric() {
            this.type = typeof(T);
            
            KlassInfo klassInfo = new KlassInfo(type);
            ctorArgs  = new CtorArgs(klassInfo);
            propArgs  = new PropArgs(klassInfo);
            fieldArgs = new FieldArgs(klassInfo);
        }

        public BaseCsvParserGeneric(Type klass) : this() {
        }

        public BaseCsvParserGeneric<T> CtorArg(string arg, int col) {
            ctorArgs.SetArg(arg, col);
            return this;
        }

        public BaseCsvParserGeneric<T> PropArg(string arg, int col) {
            propArgs.SetArg(arg, col);
            return this;
        }

        public BaseCsvParserGeneric<T> FieldArg(string arg, int col) {
            fieldArgs.SetArg(arg, col);
            return this;
        }

        public BaseCsvParserGeneric<T> CsvParserAutoCreate() {
            new CsvAutoCreator<T>(this, type).Set(this, type);
            return this;
        }

        public abstract BaseCsvParserGeneric<T> Load(String src);

        public abstract BaseCsvParserGeneric<T> Remove(int count);
        public abstract BaseCsvParserGeneric<T> RemoveEmpties();
        public abstract BaseCsvParserGeneric<T> RemoveWith(string word);
        public abstract BaseCsvParserGeneric<T> RemoveEvenIndexes();
        public abstract BaseCsvParserGeneric<T> RemoveOddIndexes();

        public abstract T[] Parse();
        public abstract T[] Parse(Func<string, T> parser);
        public abstract IEnumerable<T> ToEnumerable(Func<string, T> parser);
    }
}
