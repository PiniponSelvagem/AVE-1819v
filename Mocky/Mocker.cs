using System;
using System.Collections.Generic;
using System.Linq;

namespace Mocky {
    public class Mocker {

        private readonly Type klass;
        private readonly Generator generator;
        private Dictionary<string, MockMethod> ms;

        public Mocker(Type klass) {
            this.klass = klass;
            this.generator = new Generator(klass);
            this.ms = new Dictionary<string, MockMethod>();
        }

        public MockMethod When(string name) {
            MockMethod m;
            if (!ms.TryGetValue(name, out m)) {
                m = new MockMethod(klass, name);
                ms.Add(name, m);
            }
            return m;
        }

        public object Create() {
            Type t = BuildType();
            return Activator.CreateInstance(t, new object[1] { ms.Values.ToArray() });
        }

        public object Invoke(string methodName, params object[] parms) {
            if (ms.TryGetValue(methodName, out MockMethod mockMethod)) {
                if (mockMethod.Del != null) {
                    return mockMethod.Del.DynamicInvoke(parms);
                }

                throw new NotSupportedException("Method "+methodName+" already has implementation. Did you implemented it with: With(...).Return(...)?");
            }

            throw new NotImplementedException("Method "+methodName+" is not implemented, 'Add one today with Then(...) just for 2.99!'");
        }

        private Type BuildType() {
            return generator.GenWith(ms);
        }
    }
}