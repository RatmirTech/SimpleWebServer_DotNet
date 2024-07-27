using System.Net;

namespace SimpleWebServer.Core.Abstractions
{
    public interface IHttpServer
    {
        void Use(Func<HttpListenerContext, Func<Task>, Task> middleware);

        Task Start();

        Task ExecuteMiddleware(HttpListenerContext context, int middlewareIndex);

        void Stop();
    }
}
