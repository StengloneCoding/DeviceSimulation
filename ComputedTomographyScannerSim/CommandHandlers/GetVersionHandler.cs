
using MedicalDeviceInterface.Interfaces;

namespace ComputedTomographyScannerSim.CommandHandlers;

public class GetVersionHandler : ICommandHandler
{
    public bool CanHandle(string command) => command == "GET_VERSION";
    public string Handle(IMedicalDevice device, string command) => device.GetVersion().ToString();
}