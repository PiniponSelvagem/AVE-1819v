using CsvierGeneric.ArgsManager;
using CsvierGeneric.Exceptions;
using System;
using System.Reflection;

namespace CsvierGeneric.ArgsContainers {
    class PropArgs : BaseArgsManager {
        
        public PropArgs(KlassInfo klassInfo) : base(klassInfo) { }
        
        public override void SetValues(string[] line, object instance) {
            for (int i = 0; i<argsList.Count; ++i) {
                PropertyInfo prop = klassInfo.GetProperty(argsList[i].arg);
                if (prop==null) {
                    throw new PropertyNotFoundCsvException(klassInfo.Type.Name, argsList[i].arg);
                }
                string value = line[argsList[i].col];
                object obj = TryParseValue(value, prop.PropertyType);
                prop.SetValue(instance, obj);
            }
        }
    }
}
