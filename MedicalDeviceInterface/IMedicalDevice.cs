namespace MedicalDeviceInterface;
public interface IMedicalDevice
{
    string GetTemperature();
    string Calibrate();

    string GetStatus();

    string GetVersion();

    string Reset();
}


