using System;
using System.Reflection;

namespace Csvier.Attributes {

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class CsvAttribute : Attribute {
        public MethodInfo Method { get; private set; }

        public string MethodName { get; private set; }
        public int Column { get; private set; }

        public CsvAttribute(string methodName, int column) {
            //Method = this.GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
            this.MethodName = methodName;
            this.Column = column;
        }

        /*
        public bool Set(int column) {
            return (bool)Method.Invoke(null, new object[] { column });
        }
        */
    }
}
