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
    }
}
