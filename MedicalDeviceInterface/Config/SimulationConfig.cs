namespace MedicalDeviceInterface.Config;

public class SimulationConfig
{
    public double InitialXrayTubeTemperature { get; set; }
    public double InitialGantryTemperature { get; set; }
    public int InitialRpm { get; set; }
    public double TemperatureMin { get; set; }
    public double TemperatureMax { get; set; }
    public double OverheatThreshold { get; set; }
    public int RpmMin { get; set; }
    public int RpmMax { get; set; }
    public double ErrorProbability { get; set; }
    public int UpdateIntervalMs { get; set; }
}