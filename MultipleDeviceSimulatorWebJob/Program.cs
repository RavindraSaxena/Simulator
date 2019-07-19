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
 */

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using MultipleDeviceSimulator;
using MultipleDeviceSimulator.Configuration;
using MultipleDeviceSimulator.Device;
using System;
using System.IO;


namespace MultipleDeviceSimulatorWebJob
{
    class Program
    {

        static void Main(string[] args)
        {
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

                Console.WriteLine($"Simulator is running on ({appConfig.Environment}) environment.");

                
                var watcher = new WebJobsShutdownWatcher();

                if (DeviceManager.InitializeDevice(appConfig))
                {
                    TaskManager.InitializeTasksAndRun(appConfig);
                }

                var host = new JobHost(configuration,);
                host.CallAsync(typeof(Functions).GetMethod("ProcessMethod"));
                host.RunAndBlock();
            }
            catch (OperationCanceledException)
            { }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured while running simulator (Error: { ex.Message }");
            }
        }


    }
}
