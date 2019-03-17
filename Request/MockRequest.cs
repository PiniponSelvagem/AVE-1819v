using System;
using System.IO;
using System.Text;

namespace Request {
    public class MockRequest : IRequest, IDisposable {
        private readonly string CURRENT_DIR = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

        public void Dispose() {
        }

        public string GetBody(string url) {
            string filePath = Path.Combine(CURRENT_DIR, url);
            string data;
            using (StreamReader streamReader = new StreamReader(filePath, Encoding.UTF8)) {
                data = streamReader.ReadToEnd();
            }
            return data;
        }
    }
}
