using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebServer.Core.Server
{
    public class HTTPRequests
    {
        public static void GetResponseByMethod(HttpListenerContext context)
        {
            switch (context.Request.HttpMethod)
            {
                case "GET":
                    GET(context);
                    break;

                case "POST":
                    POST();
                    break;

                case "PUT":
                    PUT();
                    break;

                case "DELETE":
                    DELETE();
                    break;

                default:
                    break;
            }
        }
        public static void GET(HttpListenerContext context)
        {
            string responseString = "Z sosal GET";
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);

            HttpListenerResponse response = context.Response;
            response.ContentLength64 = buffer.Length;
            response.ContentType = "text/plain";
            response.StatusCode = (int)HttpStatusCode.OK;

            using (var output = response.OutputStream)
            {
                output.Write(buffer, 0, buffer.Length);
            }
        }

        public static void POST() 
        {
            Console.WriteLine("Вы ввели POST");
        }

        public static void PUT() 
        {
            Console.WriteLine("Вы ввели PUT");
        }

        public static void DELETE() 
        {
            Console.WriteLine("Вы ввели DELETE");
        }
    }
}
