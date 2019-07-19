using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleDeviceSimulator.Logging
{
    public class Logger
    {

        private string LogErrorFileName;
        private string LogInfoFileName;
        public Logger()
        {
            LogInfoFileName = $"LogInfo-{DateTime.Now.ToString("yyyy-MM-dd-HH-mm")}.txt";
            LogErrorFileName = $"LogInfo-{DateTime.Now.ToString("yyyy-MM-dd-HH-mm")}.txt";
        }

        public static void Info(string LogInfoFileName,string line)
        {
            Console.WriteLine(line);

            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(LogInfoFileName, true))
            {
                file.WriteLine(line);
            }
        }

        public static void Error(string LogErrorFileName,string line)
        {
            Console.WriteLine(line);

            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(LogErrorFileName, true))
            {
                file.WriteLine(line);
            }
        }

    }
}
