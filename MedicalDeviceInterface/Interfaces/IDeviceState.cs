using MedicalDeviceInterface.Enums;

namespace MedicalDeviceInterface.States;

public interface IDeviceState
{
    DeviceStatus Status { get; }
    bool CanCalibrate();
    bool CanRepair();
    bool CanGetTemperature();
    bool CanGetRpm();
    IDeviceState PowerOn();
    IDeviceState PowerOff();
    IDeviceState Repair();
    IDeviceState Calibrate();
    IDeviceState Reset();
    IDeviceState OnError(string errorType);
}