using MedicalDeviceInterface;

namespace MedicalDeviceLib;

public class SimulatedDevice : IMedicalDevice
{

    private bool isCalibrated = false;
    private double temperature = new Random().NextDouble() * 2;
    private bool isWorking = true;
    private string version = "1.0.0";

    public string GetTemperature()
    {
        return $"TEMP:{temperature:F1}";
    }

    public string Calibrate()
    {
        isCalibrated = true;
        isWorking = true;
        return "CALIBRATION:DONE";
    }

    public string GetStatus()
    {
        if (isWorking)
        {
            if (isCalibrated)
            {
                return "STATUS:READY";
            }
            else if (!isCalibrated)
            {
                return "STATUS:NEEDS_CALIBRATION";
            }
        }
        return "STATUS:ERROR";
    }

    public string GetVersion()
    {
        return $"VERSION: {version}";
    }

    public string Reset()
    {
        isCalibrated = false;
        isWorking = false;
        temperature = 36.5;
        return "RESET:DONE";
    }
}

