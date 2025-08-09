
using MedicalDeviceInterface.Interfaces;
using System.Text.Json;

namespace ComputedTomographyScannerSim.CommandHandlers;

public class GetTemperatureHandler : ICommandHandler
{
    public bool CanHandle(string command) => command == "GET_TEMPERATURE";
    public string Handle(IMedicalDevice device, string command)
    {
        var result = device.GetTemperature();
        return JsonSerializer.Serialize(result);
    }
}