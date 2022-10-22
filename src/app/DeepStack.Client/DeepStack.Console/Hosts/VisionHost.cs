using DeepStack.Client;
using DeepStack.Console.Options;
using ExifLibrary;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepStack.Console.Hosts
{
    internal class VisionHost
        : IHostedService
    {

        private readonly IHostApplicationLifetime _lifetime;
        private readonly HttpClient _httpClient;

        public VisionHost(IHostApplicationLifetime lifetime,
            HttpClient httpClient,
            VisionOptions options)
        {
            _lifetime = lifetime;
            _httpClient = httpClient;
            this.Options = options;
        }

        public VisionOptions Options { get; private set; }

        async public Task StartAsync(CancellationToken cancellationToken)
        {
            var origConsoleForegroundColor = System.Console.ForegroundColor;

            if (!string.IsNullOrEmpty(this.Options.File))
            {
                if(!File.Exists(this.Options.File))
                {
                    throw new FileNotFoundException("Could not find image file.", this.Options.File);
                }


                var client = new DeepStackClient(_httpClient, this.Options.BaseUrl);



                await RunForFile(client, this.Options.File, cancellationToken);

            }
            else if (!string.IsNullOrEmpty(this.Options.Directory))
            {



            }



            //System.Console.ResetColor(); // this is not working for some reason, so manually reset it in next line
            System.Console.ForegroundColor = origConsoleForegroundColor;


            _lifetime.StopApplication();

        }


        async private Task RunForFile(DeepStackClient client, string filepath, CancellationToken cancellationToken)
        {

            var sw = new Stopwatch();
            sw.Start();
            var response = await client.DetectObjects(this.Options.File, cancellationToken);
            sw.Stop();


          

            if (this.Options.UpdateExifData && response.Success)
            {
                // updates image EXIF data with the results

                var file = ImageFile.FromFile(filepath);
                file.Properties.Set(ExifTag.WindowsTitle, response.ToString());
                file.Properties.Set(ExifTag.WindowsSubject, response.ToString());
                file.Save(filepath);
            }


            if (client.LastResponseHeaders.Contains("X-Upstream"))
            {
                var upstream = client.LastResponseHeaders.GetValues("X-Upstream");
            }

            if (response.Success)
            {
                System.Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                // no objects detected
                System.Console.ForegroundColor = ConsoleColor.Yellow;
            }


            System.Console.Write(this.Options.File + " -> ");
            System.Console.WriteLine(response.ToString() + $":  {sw.ElapsedMilliseconds} ms");

        }




        async public Task StopAsync(CancellationToken cancellationToken)
        {
            

        }



    }
}
