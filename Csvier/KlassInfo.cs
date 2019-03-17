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

        public Type type { get; private set; }
        private CInfo[] CInfos { get; set; }

        public KlassInfo(Type type) {
            this.type = type;
            FillInfo(type);
        }

        private void FillInfo(Type type) {
            ConstructorInfo[] csInfo = type.GetConstructors();
            CInfos = new CInfo[csInfo.Length];

            for (int i = 0; i<csInfo.Length; ++i) {
                ParameterInfo[] pInfos = csInfo[i].GetParameters();
                CInfos[i] = new CInfo(csInfo[i], pInfos);
            }
        }

        public int GetConstructorsLength { get { return CInfos.Length; } }
        public ConstructorInfo GetConstructor(int i) { return CInfos[i].CtorInfo; }

        public int GetParametersLengthForCtor(int i) { return CInfos[i].ParamInfos.Length; }
        public ParameterInfo[] GetPamatersForCtor(int i) { return CInfos[i].ParamInfos;  }
    }
}
