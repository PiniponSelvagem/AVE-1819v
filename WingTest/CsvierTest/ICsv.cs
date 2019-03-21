
namespace Csvier.Test {
    interface ICsv {
        CsvParser CreateCsvParser();
        CsvParser CreateCsvParser(string argName, int argCol);
        CsvParser CreateCsvParser(string[] argName, int[] argCol);
    }
}
