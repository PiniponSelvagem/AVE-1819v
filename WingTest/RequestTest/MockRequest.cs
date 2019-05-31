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
            string formattedUrl = url
                .Replace("http://", "")
                .Replace("https://", "")
                .Replace("www", "")
                .Replace("/", "")
                .Replace("-", "")
                .Replace("?", "")
                .Replace("&", "")
                .Replace("=", "")
                .Replace(",", "")
                .Replace(".", "");

            return formattedUrl?.Substring(0, Math.Min(formattedUrl.Length, 100));
        }
    }
}
