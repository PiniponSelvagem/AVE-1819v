using System;

namespace CsvierGeneric.Exceptions {
    public class ConstructorNotFoundCsvException : Exception {
    }

    public class PropertyNotFoundCsvException : Exception {
        public PropertyNotFoundCsvException(string typeName, string value) : 
            base(String.Format(
                "Unable to find property: {0} in type: {1}.",
                value, typeName)) { }
    }

    public class FieldNotFoundCsvException : Exception {
        public FieldNotFoundCsvException(string typeName, string value) :
            base(String.Format(
                "Unable to find field: {0} in type: {1}.",
                value, typeName)) { }
    }

    public class InvalidCastCsvException : Exception {
        public InvalidCastCsvException(string typeName, string value) :
            base(String.Format(
                "Unable to cast value: {0} to type: {1}.",
                value, typeName)) { }
    }
}
