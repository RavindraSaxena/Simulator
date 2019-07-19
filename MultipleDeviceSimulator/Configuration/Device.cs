using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleDeviceSimulator.Configuration
{
    public class SimulatedDevice
    {
        public string DeviceName { get; set; }
        public int MinTemp { get; set; }
        public int MinHumidity { get; set; }
        public int Frequency { get; set; }

        public int DeviceMinTempRange { get; set; }
        public int DeviceMaxTempRange { get; set; }
        public int DeviceMinHumidityRange { get; set; }
        public int DeviceMaxHumidityRange { get; set; }
    }
}
