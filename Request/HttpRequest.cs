﻿using System;
using System.Net;

namespace Request {
    public class HttpRequest : IRequest, IDisposable {

        private readonly WebClient client = new WebClient();

        public void Dispose() {
            client.Dispose();
        }

        public string GetBody(string url) {
            return client.DownloadString(url);
        }
    }
}
