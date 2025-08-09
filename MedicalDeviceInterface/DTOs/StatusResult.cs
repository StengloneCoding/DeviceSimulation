using MedicalDeviceInterface.Enums;
namespace MedicalDeviceInterface.DTOs;
public class StatusResult
{
    public string Id { get; set; }
    public string Name { get; set; }
    public DeviceStatus? Status { get; set; } // optional

    public StatusResult(string id, string name, DeviceStatus? status = DeviceStatus.Offline)
    {
        Id = id;
        Name = name;
        Status = status;
    }
}

