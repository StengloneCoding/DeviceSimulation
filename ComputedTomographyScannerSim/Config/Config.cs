using System.Text.Json;

namespace ComputedTomographyScannerSim.Config;
public class Config
{
    public DeviceConfig Device { get; set; } = new();
    public CommunicationConfig Communication { get; set; } = new();
    public LoggingConfig Logging { get; set; } = new();
    public SimulationConfig Simulation { get; set; } = new();
    public MonitoringConfig Monitoring { get; set; } = new();
    public string dllPath { get; set; } = "";

    public class DeviceConfig
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Version { get; set; } = "";
    }

    public class CommunicationConfig
    {
        public string ComPort { get; set; } = "";
        public int BaudRate { get; set; }
        public int WriteTimeout { get; set; }
    }

    public class LoggingConfig
    {
        public string LogLevel { get; set; } = "";
        public string LogFilePath { get; set; } = "";
    }

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

    public class MonitoringConfig
    {
        public int PrometheusPort { get; set; }
        public int ApiPort { get; set; }
    }

    // Singleton-Instanz
    private static Config? _instance;
    public static Config Instance => _instance ??= Load();

    private static Config Load()
    {
        var basePath = AppContext.BaseDirectory;
        var jsonPath = Path.Combine(basePath, "appsettings.json");
        var json = File.ReadAllText(jsonPath);
        return JsonSerializer.Deserialize<Config>(json) ?? new Config();
    }
}