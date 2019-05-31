using CsvierGeneric.Enumerator;
using System;
using System.Collections.Generic;

namespace CsvierGeneric {
    public class CsvParserGenericLazy<T> : BaseCsvParserGeneric<T> {

        private const char DEFAULT_SEPARATOR = ',';
        private readonly char separator;
        
        private LineEnumerable lineEnum;

        public CsvParserGenericLazy(Type type, char separator) : base(type) {
            this.separator = separator;
        }

        public CsvParserGenericLazy(Type type) : base(type) {
            this.separator = DEFAULT_SEPARATOR;
        }

        public override BaseCsvParserGeneric<T> Load(string src) {
            lineEnum = new LineEnumerable(src);
            return this;
        }

        public override BaseCsvParserGeneric<T> Remove(int count) {
            lineEnum.RemoveNLines(count);
            return this;
        }

        public override BaseCsvParserGeneric<T> RemoveEmpties() {
            lineEnum.RemoveEmpties();
            return this;
        }

        public override BaseCsvParserGeneric<T> RemoveWith(string word) {
            lineEnum.RemoveStartWith(word);
            return this;
        }

        public override BaseCsvParserGeneric<T> RemoveEvenIndexes() {
            lineEnum.RemoveEvenIndexes();
            return this;
        }

        public override BaseCsvParserGeneric<T> RemoveOddIndexes() {
            lineEnum.RemoveOddIndexes();
            return this;
        }

        public override T[] Parse() {
            throw new NotImplementedException();
        }

        public override T[] Parse(Func<string, T> parser) {
            throw new NotImplementedException();
        }

        public override IEnumerable<T> ToEnumerable(Func<string, T> parser) {
            IEnumerator<string> lineEnumerator = lineEnum.GetEnumerator();
            
            while (lineEnumerator.MoveNext()) {
                yield return parser.Invoke(lineEnumerator.Current);
            }
        }
    }
}
