using SimpleWebServer.Core.Server;

class Progrma
{
    static async Task Main(string[] args)
    {
        var server = new HttpServer(new[] { "http://localhost:8080/" });

        // Подключаем middleware
        server.Use(async (context, next) =>
        {
            var loggingMiddleware = new Requestes();
            await loggingMiddleware.InvokeAsync(context, next);
        });

        await server.Start();
    }
}