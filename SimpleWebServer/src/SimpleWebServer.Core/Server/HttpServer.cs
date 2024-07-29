using SimpleWebServer.Core.Abstractions;
using System.Net;

namespace SimpleWebServer.Core.Server
{
    public class HttpServer : IHttpServer
    {
        private readonly HttpListener _listener = new();

        private readonly List<Func<HttpListenerContext, Func<Task>, Task>> _middlewares = new();

        public HttpServer(string?[] prefixes)
        {
            /* example of full URI predixes: http://www.contoso.com:8080/customerData/ */
            /* Префиксы должны заканчиваться косой чертой("/") */
            foreach (var prefix in prefixes)
            {
                if (!string.IsNullOrEmpty(prefix))
                    _listener.Prefixes.Add(prefix);
            }
        }

        public void Use(Func<HttpListenerContext, Func<Task>, Task> middleware)
        {
            _middlewares.Add(middleware);
        }

        public async Task Start()
        {
            if (!_listener.IsListening)
            {
                _listener.Start();
                while (true)
                {
                    var context = await _listener.GetContextAsync();
                    await ExecuteMiddleware(context, 0);
                }
            }
        }

        public Task ExecuteMiddleware(HttpListenerContext context, int middlewareIndex)
        {
            if (middlewareIndex < _middlewares.Count)
            {
                var middleware = _middlewares[middlewareIndex];
                return middleware(context, () => ExecuteMiddleware(context, middlewareIndex + 1));
            }

            middlewareIndex = 0;
            return Task.CompletedTask;
        }

        public void Stop()
        {
            if (_listener != null && _listener.IsListening)
            {
                /* not close or abort */
                _listener.Stop();
            }
        }
    }
}
