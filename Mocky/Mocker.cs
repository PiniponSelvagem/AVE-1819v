using System;
using System.Collections.Generic;
using System.Linq;

namespace Mocky {

    public class Mocker {
        private readonly Type type;
        private readonly Generator generator;
        private Dictionary<string, MockMethod> ms;

        public Mocker(Type type) {
            this.type = type;
            this.generator = new Generator();
            this.ms = new Dictionary<string, MockMethod>();
        }

        public MockMethod When(string name) {
            MockMethod m;
            if (!ms.TryGetValue(name, out m)) {
                m = new MockMethod(type, name);
                ms.Add(name, m);
            }
            return m;
        }

        public object Create() {
            Type t = BuildType();
            return Activator.CreateInstance(t, new object[1] { ms.Values.ToArray() });
        }

        private Type BuildType() {
            return generator.For(type, ms);
        }
    }
}