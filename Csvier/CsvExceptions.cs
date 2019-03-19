using System;

namespace Csvier.Exceptions {
    public class ConstructorNotFoundCsvException : Exception {
    }

    public class PropertyNotFoundCsvException : Exception {
        public PropertyNotFoundCsvException(string typeName, string value) : 
            base(String.Format("Unable to find property: {0} for type: {1}", value, typeName)) { }
    }

    public class FieldNotFoundCsvException : Exception {
        public FieldNotFoundCsvException(string typeName, string value) :
            base(String.Format("Unable to find field: {0} for type: {1}", value, typeName)) { }
    }
}
