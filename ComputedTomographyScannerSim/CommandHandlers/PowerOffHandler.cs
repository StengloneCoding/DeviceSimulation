
using MedicalDeviceInterface.Interfaces;
using System.Text.Json;

namespace ComputedTomographyScannerSim.CommandHandlers;

public class PowerOffHandler : ICommandHandler
{
    public bool CanHandle(string command) => command == "POWER_OFF";
    public string Handle(IMedicalDevice device, string command)
    {
        var result = device.PowerOff();
        return JsonSerializer.Serialize(result);
    }
}