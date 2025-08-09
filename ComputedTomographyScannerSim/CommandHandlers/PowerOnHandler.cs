
using MedicalDeviceInterface.Interfaces;
using System.Text.Json;

namespace ComputedTomographyScannerSim.CommandHandlers;

public class PowerOnHandler : ICommandHandler
{
    public bool CanHandle(string command) => command == "POWER_ON";
    public string Handle(IMedicalDevice device, string command)
    {
        var result = device.PowerOn();
        return JsonSerializer.Serialize(result);
    }
}