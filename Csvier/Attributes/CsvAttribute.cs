using System;
using System.Reflection;

namespace Csvier.Attributes {

    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class CsvAttribute : Attribute {

        public string Name { get; private set; }
        public int Column { get; private set; }

        public CsvAttribute(string name, int column) {
            //Method = this.GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
            this.Name = name;
            this.Column = column;
        }

        /*
        public bool Set(int column) {
            return (bool)Method.Invoke(null, new object[] { column });
        }
        */
    }
}
