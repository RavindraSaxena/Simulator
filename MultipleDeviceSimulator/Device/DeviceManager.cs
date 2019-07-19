using Microsoft.Azure.Devices.Client;
using MultipleDeviceSimulator.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultipleDeviceSimulator.Device
{
    
    public static class DeviceManager
    {
        private static DeviceClient deviceClient;

        public static bool InitializeDevice(Configurations configurations)
        {
            var deviceConfiguration = configurations.DeviceConfiguration;

            if (deviceConfiguration.SimulatedDevices != null && deviceConfiguration.SimulatedDevices.Length > 0)
            {
                if (deviceConfiguration.Certificate !=null && deviceConfiguration.Certificate.UseCertificate)
                {
                    var certificatepath = Directory.GetCurrentDirectory();
                    if (!string.IsNullOrEmpty(deviceConfiguration.Certificate.CertificatePath))
                    {
                        certificatepath = deviceConfiguration.Certificate.CertificatePath;
                    }
                    var certificateName = string.Empty;
                    if (!string.IsNullOrEmpty(deviceConfiguration.Certificate.CertificateName))
                    {
                        certificateName = deviceConfiguration.Certificate.CertificateName;
                    }
                    else
                    {
                        throw new Exception("No certificate name is specified.");
                    }

                    var certifcatefullname = Path.Combine(certificatepath, certificateName);
                    if (!File.Exists(certifcatefullname))
                    {
                        throw new Exception($"Certificate file does not exists on path ({certifcatefullname})");
                    }

                    var cert = new X509Certificate2(certifcatefullname);
                    var auth = new DeviceAuthenticationWithX509Certificate(deviceConfiguration.Certificate.IotDeviceName, cert);
                    deviceClient = DeviceClient.Create(deviceConfiguration.Certificate.IotHub, auth, TransportType.Mqtt_Tcp_Only);
                }
                else
                {
                    deviceClient = DeviceClient.CreateFromConnectionString(deviceConfiguration.DeviceConnectionString);
                }
            }
            else
            {
                Console.WriteLine("No simulator devices are configured in appsetting.json file....");
                return false;
            }
            return true;
        }

        public static async void RunDevice(SimulatedDevice device, CancellationToken ct)
        {
            if (ct.IsCancellationRequested == true)
            {
                Console.WriteLine("Device: {0} was cancelled before it got started.",
                                  device.DeviceName);
                ct.ThrowIfCancellationRequested();
            }

            Random rnd = new Random();
            while (!ct.IsCancellationRequested)
            {
                var temperature = device.MinTemp + rnd.Next(device.DeviceMinTempRange, device.DeviceMaxTempRange);
                var humidity = device.MinHumidity + rnd.Next(device.DeviceMinHumidityRange,device.DeviceMaxHumidityRange);

                var telemetryDataPoint = new Data()
                {
                    DeviceID = device.DeviceName,
                    Temp = (decimal)temperature,
                    Humidity = (decimal)humidity,
                    EpochTime = (long)GetEpochNumber(),
                    ChkFlag = 1
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                Message eventMessage = new Message(Encoding.UTF8.GetBytes(messageString));
                Console.WriteLine("{0}> Data: [{1}]", DateTime.UtcNow, messageString);

                await deviceClient.SendEventAsync(eventMessage);


                if (ct.IsCancellationRequested)
                {
                    Console.WriteLine("Device {0} cancelled", device.DeviceName);
                    ct.ThrowIfCancellationRequested();
                }
                await Task.Delay(device.Frequency * 1000);
            }
        }

        private static double GetEpochNumber()
        {
            var epoch = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            return epoch;
        }
    }
}
