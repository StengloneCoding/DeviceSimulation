using System.IO.Ports;
using System.Reflection;
using MedicalDeviceInterface;

class Program
{
    static void Main()
    {
        // Define path to the DLL with the simulated device logic
        string dllPath = @"C:\PrivateRepo\DeviceSimulation\MedicalDeviceLib\bin\Debug\net9.0\MedicalDeviceLib.dll";

        if (!File.Exists(dllPath))
        {
            Console.WriteLine($"❌ DLL not found: {dllPath}");
            throw new FileNotFoundException("The specified device DLL could not be found.", dllPath);
        }

        // Load the assembly (DLL) 
        var asm = Assembly.LoadFrom(dllPath);

        // Find any class implementing IMedicalDevice
        var deviceType = asm.GetTypes()
               .FirstOrDefault(t => typeof(IMedicalDevice).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
        if (deviceType == null)
        {
            Console.WriteLine("❌ No IMedicalDevice implementation found in plugin.");
            return;
        }

        // Create an instance and cast to known interface
        var device = Activator.CreateInstance(deviceType) as IMedicalDevice;

        if (device == null)
        {
            Console.WriteLine("❌ Failed to create device instance.");
            return;
        }

        // Initialize and open the serial port => simulating device communication
        var port = new SerialPort("COM11", 9600)
        {
            NewLine = "\n",
            ReadTimeout = 5000,
            WriteTimeout = 1000
        };

        port.Open();
        Console.WriteLine("📟 Simulated device is listening on COM11...");

        // Main loop: listen for commands and respond appropriately
        while (true)
        {
            try
            {
                // Read incoming command
                var cmd = port.ReadLine().Trim();

                // Select response based on command
                string response = cmd switch
                {
                    "GET_TEMP" => device.GetTemperature(),
                    "CALIBRATE" => device.Calibrate(),
                    "GET_VERSION" => device.GetVersion(),   
                    "GET_STATUS" => device.GetStatus(),
                    "RESET" => device.Reset(),
                    _ => "ERR:UNKNOWN"
                };

                // Send response over serial port
                port.WriteLine(response);
                Console.WriteLine($"📥 Received: {cmd} → 📤 Sent: {response}");
            }
            catch (TimeoutException)
            {
                // Ignore timeout – allows loop to remain responsive
            }
        }
    }
}