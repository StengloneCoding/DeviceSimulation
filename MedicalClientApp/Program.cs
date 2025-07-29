using System.IO.Ports;

class Program
{
    static void Main()
    {
        var port = new SerialPort("COM10", 9600)
        {
            NewLine = "\n",
            ReadTimeout = 2000,
            WriteTimeout = 1000
        };


        port.Open();
        Console.WriteLine("Verbindung zum Gerät (COM10) hergestellt.");

        try
        {
            SendCommand(port, "GET_STATUS");
            SendCommand(port, "GET_VERSION");
            SendCommand(port, "GET_TEMP");
            SendCommand(port, "CALIBRATE");
            SendCommand(port, "GET_STATUS");
            SendCommand(port, "RESET");
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