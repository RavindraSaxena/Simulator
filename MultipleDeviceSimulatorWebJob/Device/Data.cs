using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleDeviceSimulator.Device
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
    }
}
