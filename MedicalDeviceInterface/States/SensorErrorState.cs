using MedicalDeviceInterface.Enums;

namespace MedicalDeviceInterface.States;

public class SensorErrorState : IDeviceState
{
    public DeviceStatus Status => DeviceStatus.SensorError;

    public bool CanCalibrate() => false;
    public bool CanRepair() => true;  
    public bool CanGetTemperature() => false; 
    public bool CanGetRpm() => false;

    public IDeviceState PowerOn() => this; 
    public IDeviceState PowerOff() => new OfflineState();
    public IDeviceState Repair() => new NeedsCalibrationState(); 
    public IDeviceState Calibrate() => this; 
    public IDeviceState Reset() => new OfflineState();
    public IDeviceState OnError(string errorType) => this; 
}