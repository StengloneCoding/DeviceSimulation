
using MedicalDeviceInterface.Interfaces;
using System.Text.Json;

namespace ComputedTomographyScannerSim.CommandHandlers;

public class RepairHandler : ICommandHandler
{
    public bool CanHandle(string command) => command == "REPAIR";
    public string Handle(IMedicalDevice device, string command)
    {
        var result = device.Repair();
        return JsonSerializer.Serialize(result);
    }
}