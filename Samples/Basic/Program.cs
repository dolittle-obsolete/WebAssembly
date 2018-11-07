using System;
using Dolittle.Hosting;
using WebAssembly;

namespace Basic
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("HELLO WORLD 2");

            var window = (JSObject)WebAssembly.Runtime.GetGlobalObject("window");
            var location = (JSObject)window.GetObjectProperty("location");

            Console.WriteLine("Location : "+location);
            //var hostBuilder = new HostBuilder();
            //hostBuilder.Build();
        }
    }
}
