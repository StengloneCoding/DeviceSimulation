
using MedicalDeviceInterface.Interfaces;
using System.Text.Json;

namespace ComputedTomographyScannerSim.CommandHandlers;

public class DebugHandler : ICommandHandler
{
    public bool CanHandle(string command) => command == "DEBUG";
    public string Handle(IMedicalDevice device, string command)
    {
        var result = device.Debug();
        return JsonSerializer.Serialize(result);
    }
}