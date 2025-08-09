
using MedicalDeviceInterface.Interfaces;
using System.Text.Json;

namespace ComputedTomographyScannerSim.CommandHandlers;

public class CalibrateHandler : ICommandHandler
{
    public bool CanHandle(string command) => command == "CALIBRATE";
    public string Handle(IMedicalDevice device, string command)
    {
        var result = device.Calibrate();
        return JsonSerializer.Serialize(result);
    }
}