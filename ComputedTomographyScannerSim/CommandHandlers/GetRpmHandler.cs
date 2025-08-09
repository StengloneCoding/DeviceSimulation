
using MedicalDeviceInterface.Interfaces;

namespace ComputedTomographyScannerSim.CommandHandlers;

public class GetRpmHandler : ICommandHandler
{
    public bool CanHandle(string command) => command == "GET_RPM";
    public string Handle(IMedicalDevice device, string command) => device.GetRpm().ToString();
}