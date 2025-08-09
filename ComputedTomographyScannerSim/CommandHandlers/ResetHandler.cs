
using MedicalDeviceInterface.Interfaces;
using System.Text.Json;

namespace ComputedTomographyScannerSim.CommandHandlers;

public class ResetHandler : ICommandHandler
{
    public bool CanHandle(string command) => command == "RESET";
    public string Handle(IMedicalDevice device, string command)
    {
        var result = device.Reset();
        return JsonSerializer.Serialize(result);
    }
}