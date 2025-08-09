using MedicalDeviceInterface.Enums;
namespace MedicalDeviceInterface.DTOs;
public class DeviceResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public DeviceStatus? Status { get; set; } // optional

    public DeviceResult(bool success, string message, DeviceStatus? status = DeviceStatus.Offline)
    {
        Success = success;
        Message = message;
        Status = status;
    }
}

