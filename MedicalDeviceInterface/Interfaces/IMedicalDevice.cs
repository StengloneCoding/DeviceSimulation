using MedicalDeviceInterface.Enums;
using MedicalDeviceInterface.DTOs;

namespace MedicalDeviceInterface.Interfaces;
public interface IMedicalDevice
{
    TemperatureResult GetTemperature();
    string GetRpm();

    DeviceResult Calibrate();

    StatusResult GetStatus();

    string GetVersion();

    DeviceResult Reset();

    DeviceResult Repair();

    DeviceResult PowerOn();

    DeviceResult PowerOff();
    DeviceResult Debug();
}


