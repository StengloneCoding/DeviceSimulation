using MedicalDeviceInterface.Enums;
namespace MedicalDeviceInterface.DTOs;
public class TemperatureResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public double GantryTemperature { get; set; }
    public double XrayTubeTemperature { get; set; }

    public TemperatureResult(bool success, string message, double gantryTemperature, double xrayTubeTemperature)
    {
        GantryTemperature = gantryTemperature;
        XrayTubeTemperature = xrayTubeTemperature;
    }
}

