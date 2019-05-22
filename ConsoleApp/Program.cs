using Clima;
using Csvier;
using CsvierGeneric.Enumerator;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ConsoleApp {
    class Program {
        static void Main(string[] args) {
            //CtorArgsTest();
            //PastWeather();

            //TestLineEnumerator();
            TestWordEnumerator();

            Console.ReadLine();
        }

        static void TestWordEnumerator() {
            string sample = "This is just a simple sample.";
            WordEnumerable wordEnumerable = new WordEnumerable(sample);
            IEnumerator<string> wordEnumerator = wordEnumerable.GetEnumerator();
            int i = 1;
            while (wordEnumerator.MoveNext()) {
                Console.WriteLine("LINE {0}: {1}", i++, wordEnumerator.Current);
            }
        }

        static void TestLineEnumerator() {
            string sample2 =
@"2019-01-01,24,17,63,6,10,74,ENE,116,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0002_sunny_intervals.png,Partly cloudy,0.0,59,10,1031,43,14,57,6,43,13,56,11,17,13,56
2019-01-02,24,18,64,6,9,179,S,116,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0002_sunny_intervals.png,Partly cloudy,0.0,57,10,1030,15,14,57,6,42,13,56,11,17,13,56
2019-01-03,24,16,60,7,11,89,E,113,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0001_sunny.png,Sunny,0.0,67,10,1026,3,13,55,7,45,12,54,11,18,12,54
#absda

2019-01-04,24,16,60,9,15,78,ENE,116,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0002_sunny_intervals.png,Partly cloudy,0.1,73,10,1028,27,14,57,9,48,13,55,14,23,13,55
";
            string sample =
@"line number 1
line number 2
line number 3
line number 4
line number 5";
            LineEnumerable lineEnumerable = new LineEnumerable(sample);
            IEnumerator<string> lineEnumerator = lineEnumerable.GetEnumerator();
            int i = 1;            
            while(lineEnumerator.MoveNext()) { 
                Console.WriteLine("LINE {0}: {1}", i++, lineEnumerator.Current);
            }
        }


        static void CtorArgsTest() {
            object w = null;

            Type t = typeof(WeatherInfo);
            ConstructorInfo[] c = t.GetConstructors();
            for (int i=0; i<c.Length; ++i) {
                ParameterInfo[] p = c[i].GetParameters();
                for (int x=0; x<p.Length; ++x) {
                    Console.WriteLine(c[i] + "-" + p[x].Name);
                    if (p.Length == 2) {
                        object[] args = { DateTime.Parse("2019-01-02"), 20 };
                        //w = new WeatherInfo(DateTime.Parse("2019-01-02"), 20);
                        w = Activator.CreateInstance(t, args);
                    }
                }
            }

            Console.WriteLine(w);
            
            CsvParser pastWeather = new CsvParser(typeof(WeatherInfo))
                .CtorArg("date", 0)
                .CtorArg("tempC", 2);

            Console.WriteLine(pastWeather);


            //CsvParser pastWeather = new CsvParser(typeof(WeatherInfo)).AutoCreate();
            CsvParser test = new CsvParser(typeof(WeatherInfo))
                .CsvParserAutoCreate();            
        }

        static void PastWeather() {
            string sampleWeatherInLisbonFiltered =
@"2019-01-01,24,17,63,6,10,74,ENE,116,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0002_sunny_intervals.png,Partly cloudy,0.0,59,10,1031,43,14,57,6,43,13,56,11,17,13,56
2019-01-02,24,18,64,6,9,179,S,116,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0002_sunny_intervals.png,Partly cloudy,0.0,57,10,1030,15,14,57,6,42,13,56,11,17,13,56
2019-01-03,24,16,60,7,11,89,E,113,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0001_sunny.png,Sunny,0.0,67,10,1026,3,13,55,7,45,12,54,11,18,12,54
#absda

2019-01-04,24,16,60,9,15,78,ENE,116,http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0002_sunny_intervals.png,Partly cloudy,0.1,73,10,1028,27,14,57,9,48,13,55,14,23,13,55
";

            CsvParser pastWeather = new CsvParser(typeof(WeatherInfo))
                .CtorArg("date", 0)
                .CtorArg("tempC", 2)
                .PropArg("PrecipMM", 11)
                .PropArg("Desc", 10);
            
            WeatherInfo[] items = pastWeather
                .Load(sampleWeatherInLisbonFiltered)
                .RemoveEmpties()
                .RemoveWith("#")
                //.RemoveEvenIndexes()
                .Parse<WeatherInfo>();

            PrintAll(items);


            Console.WriteLine("-----------------------------------------------------------------------------------");


            CsvParser test = new CsvParser(typeof(WeatherInfo))
                .CsvParserAutoCreate();

            WeatherInfo[] items2 = test
                .Load(sampleWeatherInLisbonFiltered)
                .RemoveEmpties()
                .RemoveWith("#")
                //.RemoveEvenIndexes()
                .Parse<WeatherInfo>();

            PrintAll(items2);
        }

        static void PrintAll(object[] items) {
            for (int i=0; i<items.Length; ++i) {
                Console.WriteLine((WeatherInfo) items[i]);
            }
        }
    }
}
