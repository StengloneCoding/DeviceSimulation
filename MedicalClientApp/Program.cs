using ComputedTomographyScannerSim.Config;
using System.IO.Ports;

class Program
{
    static void Main()
    {
        var config = Config.Instance;

        var port = new SerialPort(config.Communication.ComPort, config.Communication.BaudRate)
        {
            NewLine = "\n",
            ReadTimeout = SerialPort.InfiniteTimeout,
            WriteTimeout = config.Communication.WriteTimeout
        };


        port.Open();
        Console.WriteLine($"Verbindung zum Gerät ({config.Communication.ComPort}) hergestellt.");

        try
        {
            Console.WriteLine("Wählen Sie einen Befehl aus:");
            Console.WriteLine("+: POWER_ON");
            Console.WriteLine("-: POWER_OFF");
            Console.WriteLine("1: GET_STATUS");
            Console.WriteLine("2: GET_VERSION");
            Console.WriteLine("3: GET_TEMPERATURE");
            Console.WriteLine("4: GET_RPM");
            Console.WriteLine("5: CALIBRATE");
            Console.WriteLine("6: REPAIR");
            Console.WriteLine("Debug Commands");
            Console.WriteLine("7: RESET");
            Console.WriteLine("8: DEBUG");
            Console.WriteLine("q: Beenden");

            while (true)
            {
                string entry = Console.ReadLine()?.Trim() ?? "";
                switch (entry)
                {
                    case "+":
                        SendCommand(port, "POWER_ON");
                        break;
                    case "-":
                        SendCommand(port, "POWER_OFF");
                        break;
                    case "1":
                        SendCommand(port, "GET_STATUS");
                        break;
                    case "2":
                        SendCommand(port, "GET_VERSION");
                        break;
                    case "3":
                        SendCommand(port, "GET_TEMPERATURE");
                        break;
                    case "4":
                        SendCommand(port, "GET_RPM");
                        break;
                    case "5":
                        SendCommand(port, "CALIBRATE");
                        break;
                    case "6":
                        SendCommand(port, "REPAIR");
                        break;
                    case "7":
                        SendCommand(port, "RESET");
                        break;
                    case "8": 
                        SendCommand(port, "DEBUG");
                        break;  
                    case "q":
                    case "Q":
                        Console.WriteLine("Programm beendet.");
                        return;
                    case "":
                        Console.WriteLine("Keine Wahl erkannt.");
                        break;
                    default:
                        Console.WriteLine("Ungültige Eingabe, bitte erneut versuchen.");
                        break;
                }
            }
        }
        catch (TimeoutException)
        {
            Console.WriteLine("Timeout: Keine Antwort vom Gerät erhalten.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler: {ex.Message}");
        }
        finally
        {
            if (port.IsOpen)
                port.Close();
        }
        Console.WriteLine("Verbindung zum Gerät geschlossen.");
        Console.ReadKey();
    }

    static void SendCommand(SerialPort port, string command)
    {
        Console.WriteLine($"Befehl {command} gesendet...");
        port.WriteLine(command);
        string response = port.ReadLine();
        Console.WriteLine("Antwort: " + response);
    }
}