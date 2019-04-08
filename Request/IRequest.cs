using System;

namespace Request {
    public interface IRequest : IDisposable {
        string GetBody(string url);
    }
}
