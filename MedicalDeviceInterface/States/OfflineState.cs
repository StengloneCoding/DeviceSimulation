using MedicalDeviceInterface.Enums;

namespace MedicalDeviceInterface.States;

public class OfflineState : IDeviceState
{
    public DeviceStatus Status => DeviceStatus.Offline;

    public bool CanCalibrate() => false;
    public bool CanRepair() => false;
    public bool CanGetTemperature() => false;
    public bool CanGetRpm() => false;


    public IDeviceState PowerOn() => new NeedsCalibrationState();
    public IDeviceState PowerOff() => this; 
    public IDeviceState Repair() => this;   
    public IDeviceState Calibrate() => this;
    public IDeviceState Reset() => this;   
    public IDeviceState OnError(string errorType) => this;
}