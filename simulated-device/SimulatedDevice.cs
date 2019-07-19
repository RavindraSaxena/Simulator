// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// This application uses the Azure IoT Hub device SDK for .NET
// For samples see: https://github.com/Azure/azure-iot-sdk-csharp/tree/master/iothub/device/samples

using System;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace simulated_device
{
    public class Data
    { 

        [JsonProperty(PropertyName = "DeviceID")]
        public string DeviceID { get; set; }

        [JsonProperty(PropertyName = "Temp")]
        public decimal Temp { get; set; }

        [JsonProperty(PropertyName = "Humidity")]
        public decimal Humidity { get; set; }

        [JsonProperty(PropertyName = "EpochTime")]
        public long EpochTime { get; set; }

        [JsonProperty(PropertyName = "ChkFlag")]
        public int ChkFlag { get; set; }

        //[JsonProperty(PropertyName = "EpochNo")]
        //public long EpochNo { get; set; }
    }

    class SimulatedDevice
    {
        private static DeviceClient s_deviceClient;
        private static DeviceClient s_deviceClient2;

        private static string LogFileName;
        // The device connection string to authenticate the device with your IoT hub.
        // Using the Azure CLI:
        // az iot hub device-identity show-connection-string --hub-name {YourIoTHubName} --device-id MyDotnetDevice --output table
        //private readonly static string s_connectionString2 = "HostName=asp-iothub.azure-devices.net;DeviceId=Chamber5;SharedAccessKey=fvtRmFvQXVeGOF7Qa5/d0ApLSLSap97F1sZBEuWU3bw=";
        private readonly static string s_connectionString = "HostName=asp-iothub.azure-devices.net;DeviceId=S6204;SharedAccessKey=Bs03Ucv3X8cmxrDlJTz6zd1TQdhWKagkoBljMLUnOjU=";

        //JnJ Device
        //private readonly static string s_connectionString= "HostName=ASP-BI-Lab-IIOT-IotHub.azure-devices.net;DeviceId=Chamber5;SharedAccessKey=q8+OE39bv0MCJQ5FBX+8K+x4mqIDwmPRI28JyPUtjjo=";

        // Async method to send simulated telemetry
        private static async void SendDeviceToCloudMessagesAsync()
        {
            // Initial telemetry values
            
            Random rand = new Random();

            int count = 0;

            while (true)
            {
                count++;
                double minTemperature = 80;
                double minHumidity = 80;

                double currentTemperature = minTemperature + rand.NextDouble() * 15;
                double currentHumidity = minHumidity + rand.NextDouble() * 20;


                var telemetryDataPoint = new Data() {
                    DeviceID= "S6204",
                    Temp =(decimal)currentTemperature,
                    Humidity = (decimal)currentHumidity,
                    EpochTime = (long)GetEpochNumber(),
                    ChkFlag=1
                    ///EpochNo= (long)GetEpochNumber()
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.UTF8.GetBytes(messageString));

                //Console.WriteLine("{0} - {2} > Sending message: {1}", DateTime.Now, messageString,"Chamber5");
                Log(string.Format("{0} - {2} > Sending message: {1}", DateTime.Now, messageString, "S6204"));

                // Send the tlemetry message
                await s_deviceClient.SendEventAsync(message);

                ////if (count>25 && count<=35)
                //    minTemperature = 40;
                ////else
                ////    minTemperature = 10;

                //currentTemperature = minTemperature + rand.NextDouble() * 10;
                //currentHumidity = minHumidity + rand.NextDouble() * 20;

                //telemetryDataPoint = new Data()
                //{
                //    Temp = (decimal)currentTemperature,
                //    Humidity = (decimal)currentHumidity,
                //    EpochTime = (long)GetEpochNumber(),
                //    ChkFlag = 1
                //    ///EpochNo= (long)GetEpochNumber()
                //};
                //messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                //message = new Message(Encoding.UTF8.GetBytes(messageString));

                //await s_deviceClient2.SendEventAsync(message);

                //Console.WriteLine("{0} - {2} > Sending message: {1}", DateTime.Now, messageString, "Chamber5");

                await Task.Delay(5000);

                
            }
        }

        private static void Log(string line)
        {
            Console.WriteLine(line);

            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(LogFileName, true))
            {
                file.WriteLine(line);
            }
        }
        

private static double GetEpochNumber()
        {
            var epoch = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            return epoch;
        }
        private static void Main(string[] args)
        {

            LogFileName = $"Log-{DateTime.Now.ToString("yyyy-MM-dd-HH-mm")}.txt";

            Console.WriteLine("IoT Hub Quickstarts #1 - Simulated device. Ctrl-C to exit.\n");

            // Connect to the IoT hub using the MQTT protocol
            s_deviceClient = DeviceClient.CreateFromConnectionString(s_connectionString, TransportType.Mqtt);
            //s_deviceClient2 = DeviceClient.CreateFromConnectionString(s_connectionString2, TransportType.Mqtt);

            SendDeviceToCloudMessagesAsync();
            Console.ReadLine();
        }
    }
}
