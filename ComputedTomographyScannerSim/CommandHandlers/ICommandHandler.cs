using MedicalDeviceInterface.Interfaces;

namespace ComputedTomographyScannerSim.CommandHandlers;
public interface ICommandHandler
{
    bool CanHandle(string command);
    string Handle(IMedicalDevice device, string command);
}
