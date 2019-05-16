
namespace CsvierGeneric.ArgsManager {
    interface IArgsManager {
        void SetArg(string arg, int col);
        void SetValues(string[] line, object instance);
    }
}
