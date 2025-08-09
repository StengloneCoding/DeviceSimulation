
using MedicalDeviceInterface.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ComputedTomographyScannerSim.CommandHandlers;

public class GetStatusHandler : ICommandHandler
{
    public bool CanHandle(string command) => command == "GET_STATUS";
    public string Handle(IMedicalDevice device, string command)
    {
        var result = device.GetStatus();

        var options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };

        return JsonSerializer.Serialize(new { Status = result }, options);
    }
}
