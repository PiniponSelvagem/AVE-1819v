using Csvier;
using Request;
using System;

namespace BankOfCanada {

    /*
     * Bank Of Canada - Valet API
     * https://www.bankofcanada.ca/valet/docs
     *
     */


    public class ValetApi : IDisposable {
        const string VALET_HOST = "https://www.bankofcanada.ca/valet/";
        const string LIST_GROUPS = VALET_HOST + "lists/groups/csv";
        const string GROUP = VALET_HOST + "groups/{0}/csv";
        
        readonly IRequest req;

        public ValetApi() : this(new HttpRequest()) {
        }

        public ValetApi(IRequest req) {
            this.req = req;
        }

        public void Dispose() {
            req.Dispose();
        }

        public ListGroupsInfo[] ListGroups() {
            string textData = req.GetBody(LIST_GROUPS);

            CsvParser listGroups = new CsvParser(typeof(ListGroupsInfo))
                .CtorArg("series", 0)
                .CtorArg("label", 1)
                .CtorArg("link", 2);

            ListGroupsInfo[] items = listGroups
                .Load(textData)
                .RemoveEmpties()
                .RemoveWith("TERMS AND CONDITIONS")
                .Remove(1)
                .RemoveWith("GROUPS")
                .Remove(1)
                .Parse<ListGroupsInfo>();

            return items;
        }

        public GroupSeriesInfo[] Group(string name) {
            string path = String.Format(GROUP, name);
            string textData = req.GetBody(path);

            CsvParser groupSeries = new CsvParser(typeof(GroupSeriesInfo))
                .CtorArg("series", 0)
                .CtorArg("label", 1)
                .CtorArg("link", 2);

            GroupSeriesInfo[] items = groupSeries
                .Load(textData)
                .RemoveEmpties()
                .RemoveWith("TERMS AND CONDITIONS")
                .Remove(1)
                .RemoveWith("GROUP DETAILS")
                .Remove(2)
                .RemoveWith("GROUP SERIES")
                .Remove(1)
                .Parse<GroupSeriesInfo>();

            return items;
        }
    }
}
