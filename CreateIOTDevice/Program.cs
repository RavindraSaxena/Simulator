////The MIT License(MIT)
////Copyright(c) 2016 BardaanA

////Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

////The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

////THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Threading.Tasks;

// These are the Microsoft's recommended libraries to access
// and make changes to Azure IoT Hub's Registry
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;

// This namespace which also dictates the name of the Assembly can be anything you desire.

namespace CreateIOTDevice
{
    class Program
    {
        // RegistryManager object which is going to do most of the work for our application
        static RegistryManager registryManager;
        // This here is the Connection String that you copied from the IoT Hub Shared Access Policies > iothubowner > Shared access keys; remember??
        static string connectionString = "HostName=asp-iothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=LzSgph+uPKprlDZRg4sQhAXWzreU2M18wyvCT/4tmsc=";
        // Keeps track of the registry access status
        static string registryAccessStatus = "";

        static void Main(string[] args)
        {
            try
            {
                // Let's try and create a Registry Manager using our connection string, shall we?
                registryManager = RegistryManager.CreateFromConnectionString(connectionString);
                registryAccessStatus = "Successfuly connected to the IoT Hub registry"; // Yay!
            }
            catch (Exception ex)
            {
                Console.WriteLine("Registry access failed!  {0}", ex.Message);  // Bummer!!
            }
            // Check if RegistryManager was created successfully
            if (registryManager != null)
            {
                Console.WriteLine("*****************************************************");
                Console.WriteLine("===== Welcome to the Azure IoT Registry Manager =====");
                Console.WriteLine();
                Console.WriteLine("++ {0} ++", registryAccessStatus);
                Console.WriteLine();
                int menuSelection = 0;
                while (menuSelection != 3)  // Loop to keep you going...
                {
                    Console.WriteLine("  1) Add device into registry");
                    Console.WriteLine("  2) Remove device from the registry");
                    Console.WriteLine("  3) Close this application");
                    Console.WriteLine("------------------------------------");
                    Console.Write("Enter your selection: ");
                    menuSelection = int.Parse(Console.ReadLine());
                    Console.WriteLine();
                    switch (menuSelection)
                    {
                        case 1:
                            Console.Write("Enter device name that you want to register: ");
                            string deviceName = Console.ReadLine();
                            Console.WriteLine();
                            if (deviceName.Length > 0 && !deviceName.Contains(" "))  // Weak validation :)
                            {
                                // Calling method that actually adds the device into the registry
                                AddDevice(deviceName).Wait();
                            }
                            else
                            {
                                Console.WriteLine("---");
                                Console.WriteLine("Enter valid name!");
                                Console.WriteLine("---");
                            }
                            break;
                        case 2:
                            Console.Write("Enter name of the device to be removed: ");
                            string deviceRemoveName = Console.ReadLine();
                            Console.WriteLine();
                            if (deviceRemoveName.Length > 0 && !deviceRemoveName.Contains(" "))  // Weak validation :(
                            {
                                // Calling method that actually removes the device from the registry
                                RemoveDevice(deviceRemoveName).Wait();
                            }
                            else
                            {
                                Console.WriteLine("---");
                                Console.WriteLine("Enter valid name!");
                                Console.WriteLine("---");
                            }
                            break;
                        case 3:
                            // Breaks out of the loop
                            break;
                        default:
                            Console.WriteLine("---");
                            Console.WriteLine("Choose valid entry!");
                            Console.WriteLine("---");
                            break;
                    }
                }
                // Closes the RegistryManager access right before exiting the application
                registryManager.CloseAsync().Wait();
            }
        }

        // Method used to add device into the Registry, takes in a string as a parameter
        private static async Task AddDevice(string deviceId)
        {
            // A Device object
            Device device;
            try
            {
                // Lets try and create a Device into the Device Registry
                Device newdevice = new Device(deviceId);
                newdevice.Authentication = new AuthenticationMechanism() { Type=AuthenticationType.CertificateAuthority};
                device = await registryManager.AddDeviceAsync(newdevice);
                if (device != null)
                {
                    Console.WriteLine("Device: {0} added successfully!", deviceId); // Hooray!
                }
            }
            catch (DeviceAlreadyExistsException)  // What?
            {
                Console.WriteLine("---");
                Console.WriteLine("This device has already been registered...");// When did I do that??
                Console.WriteLine("---");
                device = await registryManager.GetDeviceAsync(deviceId);
            }
            Console.WriteLine();
            Console.WriteLine("Generated device key: {0}", device.Authentication.SymmetricKey.PrimaryKey);  // Now you're talking!
            Console.WriteLine();
        }

        // Method used to remove a device from the Device Registry, takes a string as a parameter
        private static async Task RemoveDevice(string deviceId)
        {
            try
            {
                // Lets try and get rid of the Device from our registry, using the device id.
                await registryManager.RemoveDeviceAsync(deviceId);
                Console.WriteLine("Device: {0} removed successfully!", deviceId);  // Yup!
            }
            catch (DeviceNotFoundException)
            {
                Console.WriteLine("---");
                Console.WriteLine("This device has not been registered into this registry!");  // Are you sure??
                Console.WriteLine("---");
            }
        }
    }
}
