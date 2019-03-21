using System;
using System.Reflection;

namespace Csvier.Attributes {
    public class CsvAutoCreator {

        /*
        // To save in case it was already created
        private static CsvAttribute att;
        private static bool Created { get; set; }
        */

        public static int Set(CsvParser csv, Type type) { 
            //if (!Created) {


            CsvAttribute att;

            PropertyInfo[] ps = type.GetProperties();
            foreach (PropertyInfo p in ps) {
                att = (CsvAttribute)p.GetCustomAttribute(typeof(CsvAttribute), false);
                if (att != null) {
                    string attMethodName = att.MethodName;
                    MethodInfo m = csv.GetType().GetMethod(attMethodName);
                    string pName = p.Name;
                    char c = attMethodName[0];
                    if (c=='C') {
                        char[] arr = pName.ToCharArray();
                        arr[0] = Char.ToLowerInvariant(arr[0]);
                        pName = new String(arr);
                    }
                    m.Invoke(csv, new object[] { pName, att.Column });
                }
            }

            FieldInfo[] fs = type.GetFields();
            foreach (FieldInfo f in fs) {
                att = (CsvAttribute)f.GetCustomAttribute(typeof(CsvAttribute), false);
                if (att != null) {
                    MethodInfo m = csv.GetType().GetMethod(att.MethodName);
                    m.Invoke(csv, new object[] { f.Name, att.Column });
                }
            }

                /*
                PropertyInfo temp = type.GetProperty("TempC");
                CsvAttribute att = (CsvAttribute)temp.GetCustomAttribute(typeof(CsvAttribute), false);

                csv.CtorArg("Date", 0);
                csv.CtorArg("TempC", att.Column);
                */



                // Change Created
                //Created = true;
                //}
                int result = 1;
            //if (att != null) {
                /*
                result = att.IsValidNumber(studentNum);
                if (result) {
                    student.Nr = studentNum;
                }
                */
            //}
            return result;
        }
    }
}
