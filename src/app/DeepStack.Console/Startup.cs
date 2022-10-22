using CommandLine;
using CommandLine.Text;
using DeepStack.Console.Hosts;
using DeepStack.Console.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepStack.Console
{
    /// <summary>
    /// handles all the dependency injection when starting the application
    /// </summary>
    internal class Startup
    {

        public string[] CommandLineArgs { get; private set; }

        public Startup()
        { }


        public IHost Build(string[] commandLineArgs)
        {
            this.CommandLineArgs = commandLineArgs;


            var host = Microsoft.Extensions.Hosting.Host
                .CreateDefaultBuilder(commandLineArgs)
                //.UseWindowsService() // required to run as a Windows Service, otherwise the service will not start
                .ConfigureServices((hostContext, services) =>
                {
                    
                    //TODO: remove injected logging services since we are manually specifying them
                    services.RemoveAll<ILoggerFactory>();
                    services.RemoveAll<ILoggerProvider>();

                    //services.Where(svc=>svc.ServiceType )



                    string serviceName = hostContext.HostingEnvironment.ApplicationName;

                    ConfigureServices(hostContext, services);

                    // run this last so that we can handle any manually injected services by the host application.  Especially useful for testing.
                    //afterConfigureServices?.Invoke(services);

                    //services.AddHostedService<TestHost>();
                }).Build();

            return host;
        }

      
        public void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            ProcessCommandLineArgs(this.CommandLineArgs, services);


            services.AddHttpClient();

            

        }

        private static void ProcessCommandLineArgs(string[] args, IServiceCollection services)
        {
            var parser = new Parser(settings =>
            {
                settings.HelpWriter = System.Console.Error;
                settings.CaseSensitive = false;
                settings.CaseInsensitiveEnumValues = true;
            });


            var result = parser.ParseArguments<
                VisionOptions>(args);

            result.WithParsed<VisionOptions>(ops =>
            {

                services.AddSingleton<VisionOptions>(ops);

                services.AddHostedService<VisionHost>();

            })
             .WithNotParsed(errors =>
             {

                 var helpText = HelpText.AutoBuild(result,
                            h => HelpText.DefaultParsingErrorsHandler(result, h),
                            e => e);
                 System.Console.WriteLine(helpText);

                 //var sentenceBuilder = SentenceBuilder.Create();
                 //foreach (var error in errors)
                 //{
                 //    Console.Error.WriteLine(sentenceBuilder.FormatError(error));
                 //}
             });
        }


    }
}
