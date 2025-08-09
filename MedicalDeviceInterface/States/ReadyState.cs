using MedicalDeviceInterface.Enums;

namespace MedicalDeviceInterface.States;

public class ReadyState : IDeviceState
{
    public DeviceStatus Status => DeviceStatus.Ready;

    public bool CanCalibrate() => true;
    public bool CanRepair() => false; 
    public bool CanGetTemperature() => true;  
    public bool CanGetRpm() => true;         

    public IDeviceState PowerOn() => this;
    public IDeviceState PowerOff() => new OfflineState();
    public IDeviceState Repair() => this; 
    public IDeviceState Calibrate() => this;
    public IDeviceState Reset() => new OfflineState();
    public IDeviceState OnError(string errorType) => CreateErrorState(errorType);

    private IDeviceState CreateErrorState(string errorType) => errorType switch
    {
        "SENSOR_ERROR" => new SensorErrorState(),
        "TEMPERATURE_ERROR" => new OverheatedErrorState(),
        "POWER_FAILURE" => new OfflineState(),
        _ => new ErrorState()
    };
}