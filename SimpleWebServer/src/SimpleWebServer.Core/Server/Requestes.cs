using SimpleWebServer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebServer.Core.Server
{
    public class Requestes : IMiddleware
    {
        public async Task InvokeAsync(HttpListenerContext context, Func<Task> next)
        {
            Console.WriteLine($"Request: {context.Request.HttpMethod} {context.Request.Url}");
            await next.Invoke();
        }
    }
}
