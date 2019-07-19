using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SimulateX509Device
{
    class Program
    {
        private static int MESSAGE_COUNT = 50;
        private const int TEMPERATURE_THRESHOLD = 30;
        private static float temperature;
        private static float humidity;
        private static Random rnd = new Random();


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
        }
        static void Main(string[] args)
        {
            try
            {
                //HCL
                //var cert = new X509Certificate2(@"C:\Certificate\certificatedevice1.pfx", "1234");
                //var auth = new DeviceAuthenticationWithX509Certificate("CERT1", cert);
                //var deviceClient = DeviceClient.Create("asp-iothub.azure-devices.net", auth, TransportType.Mqtt_Tcp_Only);

                //JnJ Hub
                var cert = new X509Certificate2(@"C:\MyProjects\Certificate\JnJVerificationCode\Device3\Device3.pfx");
                var auth = new DeviceAuthenticationWithX509Certificate("Device3", cert);
                var deviceClient = DeviceClient.Create("ASP-BI-Lab-IIOT-IotHub.azure-devices.net", auth, TransportType.Mqtt_Tcp_Only);

                if (deviceClient == null)
                {
                    Console.WriteLine("Failed to create DeviceClient!");
                }
                else
                {
                    Console.WriteLine("Successfully created DeviceClient!");
                    SendEvent(deviceClient).Wait();
                }

                Console.WriteLine("Exiting...\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in sample: {0}", ex.Message);
            }
            Console.ReadLine();
        }

        static async Task SendEvent(DeviceClient deviceClient)
        {
            string dataBuffer;
            Console.WriteLine("Device sending {0} messages to IoTHub...\n", MESSAGE_COUNT);

            for (int count = 0; count < MESSAGE_COUNT; count++)
            {
                temperature = rnd.Next(20, 35);
                humidity = rnd.Next(60, 80);


                //dataBuffer = string.Format("{{\"deviceId\":\"{0}\",\"messageId\":{1},\"temperature\":{2},\"humidity\":{3}}}", deviceId, count, temperature, humidity);
                //Message eventMessage = new Message(Encoding.UTF8.GetBytes(dataBuffer));

                var telemetryDataPoint = new Data()
                {
                    DeviceID="6703",
                    Temp = (decimal)temperature,
                    Humidity = (decimal)humidity,
                    EpochTime = (long)GetEpochNumber(),
                    ChkFlag = 1
                };

                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                Message eventMessage = new Message(Encoding.UTF8.GetBytes(messageString));
                eventMessage.Properties.Add("temperatureAlert", (temperature > TEMPERATURE_THRESHOLD) ? "true" : "false");
                Console.WriteLine("\t{0}> Sending message: {1}, Data: [{2}]", DateTime.Now.ToLocalTime(), count, messageString);

                await deviceClient.SendEventAsync(eventMessage);

                await Task.Delay(1000);
            }
        }

        private static double GetEpochNumber()
        {
            var epoch = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            return epoch;
        }
    }
}
