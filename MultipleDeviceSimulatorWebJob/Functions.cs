using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MultipleDeviceSimulatorWebJob
{
    public class Functions
    {
        [NoAutomaticTrigger]
        public static async Task ProcessMethod(TextWriter log)
        {
            // Do something.      
            log.WriteLine("This ProcessMethod method is executed");
            Console.WriteLine("This ProcessMethod method is executed");

            await Task.Delay(TimeSpan.FromMinutes(1));
        }
    }
}
