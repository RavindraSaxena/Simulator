/*
 * How to use task
 * https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/how-to-cancel-a-task-and-its-children
 * https://binary-studio.com/2015/10/23/task-cancellation-in-c-and-things-you-should-know-about-it/
 * 
 * How to use configuration
 * https://garywoodfine.com/configuration-api-net-core-console-application/
 * 
 * How to make exe in core 
 * https://stackoverflow.com/questions/44074121/build-net-core-console-application-to-output-an-exe
 * 
 * Web Jobs
 * https://dotnetcoretutorials.com/2018/10/10/azure-webjobs-in-net-core-part-2/
 * 
 * Log4Net
 * https://www.stewshack.com/aspnet/log4net/
 * 
 */
using log4net;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using MultipleDeviceSimulator.Configuration;
using MultipleDeviceSimulator.Device;
using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace MultipleDeviceSimulator
{
    class Program
    {

        static void Main(string[] args)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            var logger = LogManager.GetLogger(typeof(Program));

            try
            {

                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsetting.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();

                IConfigurationRoot configuration = builder.Build();

                var appConfig = configuration.GetSection("Configurations").Get<Configurations>();

                if (appConfig == null)
                {
                    Console.WriteLine("No application configuration found!...");
                    Environment.Exit(0);
                }

                var titleMessage = $"Sending telemetry in ({appConfig.Environment}) environment";
                Console.Title = titleMessage;
                Console.WriteLine(titleMessage);
                changeUIColor(appConfig.Environment.ToLower());

                if (DeviceManager.InitializeDevice(appConfig))
                {
                    TaskManager.InitializeTasksAndRun(appConfig);
                }

                Console.WriteLine("Press any key to exit the application...");
                Console.ReadLine();
            }
            catch (OperationCanceledException)
            { }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                logger.Error($"Error occured while running simulator (Error: { ex.Message }");
                Console.WriteLine($"Error occured while running simulator (Error: { ex.Message }");
                Console.ReadLine();
            }
            finally {
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        private static void changeUIColor(string environment)
        {
            switch (environment)
            {
                case "local":
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case "qa":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "uat":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
            }

            

        } 
        
    }
}
