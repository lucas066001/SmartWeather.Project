using SmartWeather.Services.Constants;
using SmartWeather.Services.Mqtt;
using SmartWeather.StationMocker.Constants;
using static SmartWeather.Services.Mqtt.Dtos.StationConfigRequest;
using SmartWeather.Entities.Component;
using SmartWeather.Entities.MeasurePoint;
using SmartWeather.Services.Mqtt.Dtos;
using System;
using SmartWeather.Services.Stations;

namespace SmartWeather.StationMocker.Services;

public class MockerHosted : IHostedService
{
    private readonly MqttSingleton _mqttSingleton;
    private readonly MqttService _mqttService;
    private int _dataFreq;
    private int _stationNumber;
    private int _componentNumber;
    private List<StationConfigResponse> stationConfs = new List<StationConfigResponse>();
    public MockerHosted(MqttSingleton mqttSingleton, IServiceScopeFactory scopeFactory)
    {
        Console.WriteLine("MockerService constructor");
 
        _mqttSingleton = mqttSingleton;
        _mqttService = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<MqttService>(); ;
        if (!int.TryParse(Environment.GetEnvironmentVariable(Configuration.DATA_FREQUENCY), out _dataFreq))
        {
            Console.WriteLine("Unable to retreive DATA_FREQUENCY value, defaulting to 1000ms");
            _dataFreq = 1000;
        }

        if (!int.TryParse(Environment.GetEnvironmentVariable(Configuration.STATION_NUMBER), out _stationNumber))
        {
            Console.WriteLine("Unable to retreive STATION_NUMBER value, defaulting to 20");
            _stationNumber = 20;
        }

        if (!int.TryParse(Environment.GetEnvironmentVariable(Configuration.COMPONENT_NUMBER), out _componentNumber))
        {
            Console.WriteLine("Unable to retreive STATION_NUMBER value, defaulting to 5");
            _componentNumber = 5;
        }

        Console.WriteLine("_dataFreq" + _dataFreq);
        Console.WriteLine("_stationNumber" + _stationNumber);
        Console.WriteLine("_componentNumber" + _componentNumber);

        Console.WriteLine("EXIT MockerService constructor");
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("StartAsync MockerService");

        // Setup mqtt singleton
        await _mqttSingleton.ConnectAsync();
        Console.WriteLine("_mqttSingleton.ConnectAsync()");

        var stationsConfigsTopic = string.Format(CommunicationConstants.MQTT_CONFIG_TOPIC_FORMAT,
                                CommunicationConstants.MQTT_STATION_TARGET);
        await _mqttSingleton.SubscribeAsync(stationsConfigsTopic);
        Console.WriteLine("_mqttSingleton.SubscribeAsync()");

        // Configure all mocking stations

        for (int stationId = 0; stationId < _stationNumber; stationId++)
        {
            string macAdress = "MOCK_STATION_" + stationId;
            Console.WriteLine("looop " + macAdress);
            List<PinConfig> pins = new List<PinConfig>();
            for (int compId = 0; compId < _componentNumber; compId++)
            {
                Console.WriteLine("looop Components" + macAdress);
                var mpConf = new List<MeasurePointConfig>
                {
                    new()
                    {
                        DefaultName = "Mocked temp",
                        LocalId = 1,
                        Unit = (int)MeasureUnit.Celsius
                    },
                    new()
                    {
                        DefaultName = "Mocked humidity",
                        LocalId = 2,
                        Unit = (int)MeasureUnit.Percentage
                    },
                    new()
                    {
                        DefaultName = "Mocked UV",
                        LocalId = 3,
                        Unit = (int)MeasureUnit.UvStrength
                    }
                };

                pins.Add(new PinConfig()
                {
                    ComponentType = (int)ComponentType.Sensor,
                    DefaultName = "Mocked sensor",
                    GpioPin = compId,
                    MeasurePoints = mpConf
                });
                Console.WriteLine("pin.Add" + macAdress);

            }
            Console.WriteLine("BEFORE _mqttService.SendConfigRequest()");

            StationConfigResponse? rep = await _mqttService.SendConfigRequest(macAdress, pins);
            
            Console.WriteLine("AFTER _mqttService.SendConfigRequest()");

            if (rep != null)
            {
                Console.WriteLine("Station " + stationId + "configured successfully");
                stationConfs.Add(rep);
            }
            else
            {
                Console.WriteLine("Failed to configure station " + stationId);
            }
        }

        // Launch mocking loop

        _mockingLoop(cancellationToken);

        Console.WriteLine("EXIT StartAsync MockerService");
    }

    private async void _mockingLoop(CancellationToken cancellationToken)
    {

        DateTime lastSend = DateTime.Now;

        Console.WriteLine("_mockingLoop");

        Random random = new Random();
        while (!cancellationToken.IsCancellationRequested)
        {
            foreach (var stationConf in stationConfs)
            {
                foreach (var compConf in stationConf.ConfigComponents)
                {
                    foreach (var mpConf in compConf.MeasurePointsConfigs)
                    {
                        switch (mpConf.Id)
                        {
                            case 1:
                                await _mqttService.SendSensorSavingRequest(stationConf.StationDatabaseId, mpConf.DatabaseId, (float)random.NextDouble() * (50.0f - 0.0f) + 0.0f);
                                break;
                            case 2:
                                await _mqttService.SendSensorSavingRequest(stationConf.StationDatabaseId, mpConf.DatabaseId, (float)random.NextDouble() * (100.0f - 0.0f) + 0.0f);
                                break;
                            case 3:
                                await _mqttService.SendSensorSavingRequest(stationConf.StationDatabaseId, mpConf.DatabaseId, (float)random.NextDouble() * (12 - 0) + 0);
                                break;
                        }
                    }
                }
            }
            DateTime tmpNow = DateTime.Now;
            Console.WriteLine($"All data send in {lastSend.Subtract(tmpNow).TotalMilliseconds}ms, should be around {_dataFreq}ms, +/-10ms");
            lastSend = tmpNow;
            Thread.Sleep(_dataFreq);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}