using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleDeviceSimulator.Configuration
{
    public class DeviceConfiguration
    {
        public string DeviceConnectionString { get; set; }
        public Certificate Certificate { get; set; }
        public SimulatedDevice[] SimulatedDevices { get; set; }
    }
}
