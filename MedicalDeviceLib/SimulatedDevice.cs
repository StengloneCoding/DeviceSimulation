using MedicalDeviceInterface.Config;
using MedicalDeviceInterface.DTOs;
using MedicalDeviceInterface.Enums;
using MedicalDeviceInterface.Interfaces;
using MedicalDeviceInterface.States;

namespace MedicalDeviceLib;

public class SimulatedDevice : IMedicalDevice
{
    private readonly DeviceConfig _deviceConfig;
    private readonly SimulationConfig _simulationConfig;
    private IDeviceState _currentState;

    private double gantry_temperature = 0.0;
    private double xray_tube_temperature = 0.0;
    private int rpm = 0;
    private string version = "";
    private string id = "";
    private Random rnd = new();

    private CancellationTokenSource? _simulationCancellationToken;
    private Task? _simulationTask;
    private bool _isSimulationRunning = false;

    public SimulatedDevice(DeviceConfig deviceConfig, SimulationConfig simulationConfig)
    {
        _deviceConfig = deviceConfig;
        _simulationConfig = simulationConfig;
        _currentState = new OfflineState();
    }


    public void SimulateRandomError()
    {
        var errorType = rnd.Next(0, 4) switch
        {
            0 => "Power Failure",
            1 => "Sensor Error",
            2 => "Temperature Error",
            _ => "Error"
        };

        _currentState = _currentState.OnError(errorType);
        ApplyErrorEffects(errorType);
    }

    private void ApplyErrorEffects(string errorType)
    {
        switch (errorType)
        {
            case "POWER_FAILURE":
                SimulatePowerFailure();
                break;
            case "SENSOR_ERROR":
                if (rnd.Next(2) == 1)
                    SimulateXraySensorDisconnection();
                else
                    SimulateGantrySensorDisconnection();
                break;
            case "TEMPERATURE_ERROR":
                SimulateTemperatureError();
                break;
        }
    }

    public void SimulatePowerFailure()
    {
        gantry_temperature = 0.0;
        xray_tube_temperature = 0.0;
        rpm = 0;
        version = "";
        id = "";
    }
    public void SimulateXraySensorDisconnection()
    {
        xray_tube_temperature = double.NaN;
    }
    public void SimulateGantrySensorDisconnection()
    {
        gantry_temperature = double.NaN;
        rpm = 0;
    }
    public void SimulateTemperatureError()
    {
        xray_tube_temperature = 276.8;
        gantry_temperature = 83.2;
    }


    public TemperatureResult GetTemperature()
    {
        if (rnd.NextDouble() < _simulationConfig.ErrorProbability)
        {
            SimulateRandomError();
        }

        if (!_currentState.CanGetTemperature())
        {
            return new TemperatureResult(
                false,
                "Retrieving temperatures failed: Sensor error",
                0.0,
                0.0
                );
        }

        return new TemperatureResult(
            true,
            "Temperatures retrieved successfully.",
            gantryTemperature: gantry_temperature,
            xrayTubeTemperature: xray_tube_temperature
        );
    }

    public string GetRpm()
    {
        if (rnd.NextDouble() < _simulationConfig.ErrorProbability)
        {
            SimulateRandomError();
        }

        if (!_currentState.CanGetRpm())
        {
            return "ERROR: RPM not available";
        }

        return $"{rpm}";
    }

    public StatusResult GetStatus()
    {
        return new StatusResult(
                    id: _deviceConfig.Id,
                    name: _deviceConfig.Name,
                    status: _currentState.Status
                );
    }

    public string GetVersion()
    {
        return version;
    }

    public DeviceResult Reset()
    {
        StopSimulation();
        _currentState = _currentState.Reset();

        if (_currentState.Status == DeviceStatus.Offline)
        {
            // Variablen zurücksetzen
            gantry_temperature = 0.0;
            xray_tube_temperature = 0.0;
            rpm = 0;
            version = "";
            id = "";
        }


        return new DeviceResult(
            success: true,
            message: "Reset completed successfully.",
            status: _currentState.Status
        );
    }

    public DeviceResult Repair()
    {
        _currentState = _currentState.Repair();

        if (!_currentState.CanRepair())
        {
            return new DeviceResult(
                success: false,
                message: "Device is already operational.",
                status: GetStatus().Status
            );
        }


        _currentState = _currentState.Repair();

        return new DeviceResult(
            success: true,
            message: "Device repaired and operational. Calibration required.",
            status: DeviceStatus.NeedsCalibration
        );
    }

    public DeviceResult Calibrate()
    {
        if (!_currentState.CanCalibrate())
        {
            return new DeviceResult(
                success: false,
                message: "Calibration failed: Device is not operational.",
                status: DeviceStatus.Error
            );
        }

        _currentState = _currentState.Calibrate();

        return new DeviceResult(
            success: true,
            message: "Calibration completed successfully.",
            status: _currentState.Status
        );
    }

    public DeviceResult Debug()
    {
        if (_isSimulationRunning)
        {
            StopSimulation();
        }
        else
        {
            StartSimulation();
        }
        return new DeviceResult(
            success: true,
            message: _isSimulationRunning ? "Simulation started" : "Simulation stopped",
            status: _currentState.Status
        );
    }

    private async Task SimulateVariablesAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                lock (this)
                {
                    if (_currentState.Status == DeviceStatus.Ready || _currentState.Status == DeviceStatus.NeedsCalibration)
                    {
                        xray_tube_temperature += (rnd.NextDouble() * 3.0) - 1.5;
                        xray_tube_temperature = Math.Round(Math.Clamp(xray_tube_temperature, 50.0, 200.0), 2);

                        gantry_temperature += (rnd.NextDouble() * 2.0) - 1.0;
                        gantry_temperature = Math.Round(Math.Clamp(gantry_temperature, 36.5, 45.0), 2);

                        rpm += rnd.Next(-5, 6);
                        rpm = Math.Clamp(rpm, 60, 200);
                    }
                }

                await Task.Delay(_simulationConfig.UpdateIntervalMs, cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Simulation error: {ex.Message}");
        }
        finally
        {
            _isSimulationRunning = false;
        }
    }
    private void StartSimulation()
    {
        if (_isSimulationRunning) return;

        _simulationCancellationToken = new CancellationTokenSource();
        _isSimulationRunning = true;

        _simulationTask = Task.Run(async () => await SimulateVariablesAsync(_simulationCancellationToken.Token));
    }

    private void StopSimulation()
    {
        if (!_isSimulationRunning) return;

        _simulationCancellationToken?.Cancel();
        _isSimulationRunning = false;
        _simulationTask = null;
    }

    ~SimulatedDevice()
    {
        StopSimulation();
        _simulationCancellationToken?.Dispose();
    }

    public DeviceResult PowerOn()
    {
        _currentState = _currentState.PowerOn();

        gantry_temperature = _simulationConfig.InitialGantryTemperature;
        xray_tube_temperature = _simulationConfig.InitialXrayTubeTemperature;
        rpm = _simulationConfig.InitialRpm;
        version = _deviceConfig.Version;
        id = _deviceConfig.Id;
        Console.WriteLine(gantry_temperature);
        return new DeviceResult(
            success: true,
            message: "Power switched on successfully.",
            status: DeviceStatus.Ready
        );
    }

    public DeviceResult PowerOff()
    {
        StopSimulation();
        _currentState = _currentState.PowerOff();
        gantry_temperature = 0.0;
        xray_tube_temperature = 0.0;
        rpm = 0;
        version = "";
        id = "";

        return new DeviceResult(
            success: true,
            message: "Power switched off successfully.",
            status: DeviceStatus.Offline
        );
    }
}


