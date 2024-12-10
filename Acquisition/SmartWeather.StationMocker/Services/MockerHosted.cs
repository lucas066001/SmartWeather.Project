using SmartWeather.Services.Constants;
using SmartWeather.Services.Mqtt;
using SmartWeather.StationMocker.Constants;
using static SmartWeather.Services.Mqtt.Dtos.StationConfigRequest;
using SmartWeather.Entities.Component;
using SmartWeather.Entities.MeasurePoint;
using SmartWeather.Services.Mqtt.Dtos;
using SmartWeather.StationMocker.Helpers;
using System.Text.Json;
using System.Text;
using Elastic.Clients.Elasticsearch.Core.TermVectors;
using Elastic.Clients.Elasticsearch;

namespace SmartWeather.StationMocker.Services;

public class MockerHosted : IHostedService
{
    private const double MinLatitude = 41.0;
    private const double MaxLatitude = 51.0;
    private const double MinLongitude = -5.0;
    private const double MaxLongitude = 9.0;
    private readonly MqttSingleton _mqttSingleton;
    private readonly MqttService _mqttService;
    private string _adminToken = Environment.GetEnvironmentVariable("ADMIN_TOKEN") ?? string.Empty;
    private int _dataFreq;
    private int _stationNumber;
    private int _componentNumber;
    private record StationMocking
    {
        public required StationConfigResponse StationConf;
        public required List<MeasurePointMocker> MpMocker;
    }
    private List<StationMocking> stationMockers = new List<StationMocking>();
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
        Console.WriteLine("_adminToken" + _adminToken);

        Console.WriteLine("EXIT MockerService constructor");
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("StartAsync MockerService");

        var random = new Random();
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _adminToken);

        string updateStationUrl = "http://smart-weather-api:8081/api/Station/Update";

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
            var tmpMockers = new List<MeasurePointMocker>();
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

                tmpMockers.Add(new MeasurePointMocker(MeasureUnit.Celsius));
                tmpMockers.Add(new MeasurePointMocker(MeasureUnit.Percentage));
                tmpMockers.Add(new MeasurePointMocker(MeasureUnit.UvStrength));

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
                stationMockers.Add(new StationMocking() { StationConf = rep, MpMocker = tmpMockers });
                //Generate random coordinates
                double latitude = random.NextDouble() * (MaxLatitude - MinLatitude) + MinLatitude;
                double longitude = random.NextDouble() * (MaxLongitude - MinLongitude) + MinLongitude;
                var updateRequest = new
                {
                    Id = rep.StationDatabaseId,
                    Name = "Mocked Station - " + rep.StationDatabaseId,
                    MacAddress = macAdress,
                    Latitude = latitude,
                    Longitude = longitude,
                    UserId = 1
                };
                
                var response = await httpClient.PutAsync(
                    updateStationUrl,
                    new StringContent(JsonSerializer.Serialize(updateRequest), Encoding.UTF8, "application/json")
                );

                response.EnsureSuccessStatusCode();
                Console.WriteLine(response.ToString());
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
            var errorRate = random.Next(0, 30);
            foreach (var stationMock in stationMockers)
            {
                var currentMockerIndex = 0;
                foreach (var compConf in stationMock.StationConf.ConfigComponents)
                {
                    foreach (var mpConf in compConf.MeasurePointsConfigs)
                    {

                        if (random.Next(0, 100) > errorRate)
                        {
                            await _mqttService.SendSensorSavingRequest(stationMock.StationConf.StationDatabaseId,
                                            mpConf.DatabaseId,
                                            stationMock.MpMocker[currentMockerIndex].GetSensorData());
                        }
                        
                        currentMockerIndex++;
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