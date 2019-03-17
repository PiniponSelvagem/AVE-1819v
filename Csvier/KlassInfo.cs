using System;
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
        private CInfo[] CInfos { get; set; }
        private PropertyInfo[] PropInfos { get; set; }
        private FieldInfo[] FildInfos { get; set; }

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

            PropInfos = Type.GetProperties();
            FildInfos = Type.GetFields();
        }


        public int GetConstructorsLength { get { return CInfos.Length; } }
        public ConstructorInfo GetConstructor(int i) { return CInfos[i].CtorInfo; }

        public int GetParametersLengthForCtor(int i) { return CInfos[i].ParamInfos.Length; }
        public ParameterInfo[] GetPamatersForCtor(int i) { return CInfos[i].ParamInfos;  }
    }
}
