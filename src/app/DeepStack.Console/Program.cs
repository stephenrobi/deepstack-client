using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepStack.Console
{
    public static class Program
    {
        async public static Task Main(string[] args)
        {
            var startup = new Startup();

            var host = startup.Build(args);


            // make the process cancellable
            var cancelToken = new CancellationTokenSource();
            System.Console.CancelKeyPress += (sender, e) =>
            {
                cancelToken.Cancel();
            };


            await host.RunAsync(cancelToken.Token);

        }



        


    }
}
