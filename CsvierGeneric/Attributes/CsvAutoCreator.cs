using System;
using System.Reflection;

namespace CsvierGeneric.Attributes {
    public class CsvAutoCreator<T> {

        private CsvParserGeneric<T> csv;
        private Type type;

        public CsvAutoCreator(CsvParserGeneric<T> csv, Type type) {
            this.csv = csv;
            this.type = type;
        }     
        
        public void Set(CsvParserGeneric<T> csv, Type type) {
            SetCtors();
            SetProps();
            SetFields();
        }

        private void SetCtors() {
            ConstructorInfo selectedCtor = null;
            object[] selectedAtts = null;

            ConstructorInfo[] cs = type.GetConstructors();
            for (int i=0, maxCount=-1; i<cs.Length; ++i) {
                object[] atts = cs[i].GetCustomAttributes(typeof(CsvAttribute), false);
                if (atts.Length > maxCount) {
                    selectedCtor = cs[i];
                    selectedAtts = atts;
                    maxCount = atts.Length;
                }
            }

            ParameterInfo[] paramInfos = selectedCtor.GetParameters();

            if (selectedAtts.Length == 0 || paramInfos.Length == 0) {
                return;     // return if the selected ctor dosent have params
            }
            
            for(int i=0; i<selectedAtts.Length; ++i) {
                CsvAttribute csvAtt = (CsvAttribute)selectedAtts[i];
                csv.CtorArg(paramInfos[i].Name, csvAtt.Column);
            }
        }

        private void SetProps() {
            PropertyInfo[] ps = type.GetProperties();
            foreach (PropertyInfo p in ps) {
                object[] atts = p.GetCustomAttributes(typeof(CsvAttribute), false);
                if (atts.Length > 0 && p.GetSetMethod() != null) {
                    CsvAttribute csvAtt = (CsvAttribute)atts[0];    // ONLY SELECT THE FIRST ATTRIBUTE
                    csv.PropArg(p.Name, csvAtt.Column);
                }
            }
        }

        private void SetFields() {
            FieldInfo[] fs = type.GetFields();
            foreach (FieldInfo f in fs) {
                object[] atts = f.GetCustomAttributes(typeof(CsvAttribute), false);
                if (atts.Length > 0) {
                    CsvAttribute csvAtt = (CsvAttribute)atts[0];    // ONLY SELECT THE FIRST ATTRIBUTE
                    csv.FieldArg(f.Name, csvAtt.Column);
                }
            }
        }
    }
}
