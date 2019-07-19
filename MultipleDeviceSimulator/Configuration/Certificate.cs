using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleDeviceSimulator.Configuration
{
    public class Certificate
    {
        public string IotHub { get; set; }
        public string IotDeviceName { get; set; }
        public bool UseCertificate { get; set; }
        public string CertificatePath  { get; set; }
        public string CertificateName { get; set; }
    }
}
