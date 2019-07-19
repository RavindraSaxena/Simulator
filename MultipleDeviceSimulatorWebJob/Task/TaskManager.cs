using MultipleDeviceSimulator.Configuration;
using MultipleDeviceSimulator.Device;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultipleDeviceSimulator
{
    public static class TaskManager
    {
        public static void InitializeTasksAndRun(Configurations configuration)
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            

            Task[] taskArray = new Task[configuration.DeviceConfiguration.SimulatedDevices.Length];
            int count = 0;
            foreach (var device in configuration.DeviceConfiguration.SimulatedDevices)
            {
                taskArray[count++] = Task.Factory.StartNew(() => DeviceManager.RunDevice(device, token));
            }

            if (string.Compare(configuration.Environment, "local", true) == 0)
            {
                Console.WriteLine("Press c to cancel the operation.");
                char ch = Console.ReadKey().KeyChar;
                if (ch == 'c' || ch == 'C')
                {
                    tokenSource.Cancel();
                    Console.WriteLine("\nTask cancellation requested.");
                }
            }

            try
            {
                Task.WaitAll(taskArray);
            }
            catch (AggregateException e)
            {
                Console.WriteLine("\nAggregateException thrown with the following inner exceptions:");
                // Display information about each exception. 
                foreach (var v in e.InnerExceptions)
                {
                    if (v is TaskCanceledException)
                        Console.WriteLine("   TaskCanceledException: Task {0}",
                                          ((TaskCanceledException)v).Task.Id);
                    else
                        Console.WriteLine("   Exception: {0}", v.GetType().Name);
                }
                Console.WriteLine();
            }
            finally
            {
                tokenSource.Dispose();
            }
        }
    }
}
