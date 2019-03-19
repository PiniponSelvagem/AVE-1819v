using System;
using System.IO;
using System.Text;

namespace Request.Test {
    public class MockRequest : IRequest, IDisposable {
        private readonly string RESOURCES_DIR = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "Resources");

        public void Dispose() {
        }

        public string GetBody(string url) {
            string fileName = ConvertUrlToFilename(url);
            string filePath = Path.Combine(RESOURCES_DIR, fileName);
            string data;
            using (StreamReader streamReader = new StreamReader(filePath, Encoding.UTF8)) {
                data = streamReader.ReadToEnd();
            }
            return data;
        }

        private string ConvertUrlToFilename(string url) {
            String[] parts = url.Split('/');
            if (parts.Length>1) {
                return parts[parts.Length-1]
                    .Replace('?', '-')
                    .Replace('&', '-')
                    .Replace('=', '-')
                    .Replace(',', '-')
                    .Substring(0, 68);
            }
            return url;
        }
    }
}
