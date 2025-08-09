using ComputedTomographyScannerSim.CommandHandlers;
using ComputedTomographyScannerSim.Config;
using MedicalDeviceInterface.Config;
using MedicalDeviceInterface.DTOs;
using MedicalDeviceInterface.Interfaces;
using System.IO.Ports;
using System.Reflection;

class Program
{
    static void Main()
    {

        var config = Config.Instance;

        // Define path to the DLL with the simulated device logic
        string dllPath = config.dllPath;

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

        var deviceConfig = new DeviceConfig
        {
            Id = config.Device.Id,
            Name = config.Device.Name,
            Version = config.Device.Version
        };
        var simulationConfig = new SimulationConfig
        {
            InitialXrayTubeTemperature = config.Simulation.InitialXrayTubeTemperature,
            InitialGantryTemperature = config.Simulation.InitialGantryTemperature,
            InitialRpm = config.Simulation.InitialRpm,
            TemperatureMin = config.Simulation.TemperatureMin,
            TemperatureMax = config.Simulation.TemperatureMax,
            OverheatThreshold = config.Simulation.OverheatThreshold,
            RpmMin = config.Simulation.RpmMin,
            RpmMax = config.Simulation.RpmMax,
            ErrorProbability = config.Simulation.ErrorProbability,
            UpdateIntervalMs = config.Simulation.UpdateIntervalMs
        };

        // Device mit Config-Parametern erstellen
        var device = Activator.CreateInstance(deviceType, deviceConfig, simulationConfig) as IMedicalDevice;

        if (device == null)
        {
            Console.WriteLine("❌ Failed to create device instance.");
            return;
        }

        // Initialize and open the serial port => simulating device communication
        var port = new SerialPort(config.Communication.ComPort, config.Communication.BaudRate)
        {
            NewLine = "\n",
            ReadTimeout = SerialPort.InfiniteTimeout,
            WriteTimeout = config.Communication.WriteTimeout
        };

        port.Open();
        Console.WriteLine($"📟 Simulated device is listening on {config.Communication.ComPort}...");
        var handlers = new List<ICommandHandler> {
                new GetStatusHandler(),
                new GetVersionHandler(),
                new GetTemperatureHandler(),
                new GetRpmHandler(),
                new CalibrateHandler(),
                new ResetHandler(),
                new RepairHandler(),
                new PowerOffHandler(),
                new PowerOnHandler(),
                new DebugHandler()
            };

        while (true)
        {
            var cmd = port.ReadLine().Trim();
            var handler = handlers.FirstOrDefault(h => h.CanHandle(cmd));
            string response = handler != null ? handler.Handle(device, cmd) : "ERR:UNKNOWN";
            port.WriteLine(response);
        }
    }
}
