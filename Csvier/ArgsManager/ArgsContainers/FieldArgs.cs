using Csvier.ArgsManager;
using Csvier.Exceptions;
using System;
using System.Reflection;

namespace Csvier.ArgsContainers {
    class FieldArgs : BaseArgsManager {
        
        public FieldArgs(KlassInfo klassInfo) : base(klassInfo) { }

        public override void SetValues(string[] line, object instance) {
            for (int i = 0; i<argsList.Count; ++i) {
                FieldInfo field = klassInfo.GetField(argsList[i].arg);
                if (field==null) {
                    throw new FieldNotFoundCsvException(klassInfo.Type.Name, argsList[i].arg);
                }
                string value = line[argsList[i].col];
                object obj = TryParseValue(value, field.FieldType);
                field.SetValue(instance, obj);
            }
        }
    }
}
