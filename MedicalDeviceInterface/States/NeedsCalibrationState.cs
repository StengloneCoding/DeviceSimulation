using MedicalDeviceInterface.Enums;

namespace MedicalDeviceInterface.States;

public class NeedsCalibrationState : IDeviceState
{
    public DeviceStatus Status => DeviceStatus.NeedsCalibration;

    public bool CanCalibrate() => true; 
    public bool CanRepair() => false;
    public bool CanGetTemperature() => false; 
    public bool CanGetRpm() => false;

    public IDeviceState PowerOn() => this; 
    public IDeviceState PowerOff() => new OfflineState();
    public IDeviceState Repair() => this;
    public IDeviceState Calibrate() => new ReadyState(); 
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