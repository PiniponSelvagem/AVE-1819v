using System;
using System.Collections.Generic;
using System.Reflection;

namespace Csvier {
    class KlassInfo {
        public struct CInfo {
            public ConstructorInfo CtorInfo { get; private set; }
            public ParameterInfo[] ParamInfos { get; private set; }

            public CInfo(ConstructorInfo ctorInfo, ParameterInfo[] paramInfos) {
                this.CtorInfo = ctorInfo;
                this.ParamInfos = paramInfos;
            }
        }

        public Type Type { get; private set; }
        public CInfo[] CInfos { get; set; }
        private Dictionary<string, PropertyInfo> PropInfos { get; set; }
        public int PropInfosLength { get; private set; }
        private Dictionary<string, FieldInfo> FieldInfos { get; set; }
        public int FieldInfosLength { get; private set; }

        public KlassInfo(Type type) {
            this.Type = type;
            FillAllInfo();
        }

        private void FillAllInfo() {
            ConstructorInfo[] ctorInfos = Type.GetConstructors();
            CInfos = new CInfo[ctorInfos.Length];

            for (int i = 0; i<ctorInfos.Length; ++i) {
                ParameterInfo[] pInfos = ctorInfos[i].GetParameters();
                CInfos[i] = new CInfo(ctorInfos[i], pInfos);
            }

            PropInfos  = PropInfosToDictionary(Type.GetProperties());
            FieldInfos = FieldInfosToDictionary(Type.GetFields());
        }
        
        public int GetConstructorsLength { get { return CInfos.Length; } }
        public ConstructorInfo GetConstructor(int i) { return CInfos[i].CtorInfo; }

        public int GetParametersLengthForCtor(int i) { return CInfos[i].ParamInfos.Length; }
        public ParameterInfo[] GetPamatersForCtor(int i) { return CInfos[i].ParamInfos;  }

        public PropertyInfo GetProperty(string name) {
            PropInfos.TryGetValue(name, out PropertyInfo value);
            return value;
        }

        public FieldInfo GetField(string name) {
            FieldInfos.TryGetValue(name, out FieldInfo value);
            return value;
        }



        /*
        private Dictionary<string, T> InfosToDictionary<T>(Type type, T[] ts) {
            Dictionary<string, T> dict = new Dictionary<string, T>();
            foreach (T t in ts) {
                dict.Add(t.Name, t);
            }
            return dict;
        }
        */

        private Dictionary<string, PropertyInfo> PropInfosToDictionary(PropertyInfo[] props) {
            Dictionary<string, PropertyInfo> dict = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo p in props) {
                dict.Add(p.Name, p);
                ++PropInfosLength;
            }
            return dict;
        }

        private Dictionary<string, FieldInfo> FieldInfosToDictionary(FieldInfo[] fields) {
            Dictionary<string, FieldInfo> dict = new Dictionary<string, FieldInfo>();
            foreach (FieldInfo f in fields) {
                dict.Add(f.Name, f);
                ++FieldInfosLength;
            }
            return dict;
        }
    }
}
