using System.Text.Json;

namespace ComputedTomographyScannerSim.Config;
public class Config
{
    public CommunicationConfig Communication { get; set; } = new();
    public LoggingConfig Logging { get; set; } = new();

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

    // Singleton-Instanz
    private static Config? _instance;
    public static Config Instance => _instance ??= Load();

    private static Config Load()
    {
        var json = File.ReadAllText("appsettings.json");
        return JsonSerializer.Deserialize<Config>(json) ?? new Config();
    }
}



