using Confluent.Kafka;
using SmartWeather.Services.Constants;
using SmartWeather.Services.Mqtt.Contract;
using SmartWeather.Services.Mqtt.Dtos;
using System.Text.Json;
using System.Text.Json.Nodes;
using static SmartWeather.Services.Mqtt.Dtos.StationConfigRequest;
using static SmartWeather.Services.Mqtt.MqttSingleton;

namespace SmartWeather.Services.Mqtt;

public class MqttService(MqttSingleton mqttSingleton)
{
    public async Task<bool> SendActuatorCommand(int stationId, int gpioPin, int newValue)
    {
        var request = new ActuatorCommandRequest()
        {
            GpioPin = gpioPin,
            Value = newValue
        };
        var targetTopic = string.Format(CommunicationConstants.MQTT_ACTUATOR_TOPIC_FORMAT,
                                        stationId.ToString(),
                                        CommunicationConstants.MQTT_STATION_TARGET);

        var requestHeader = MqttHeader.Generate();
        var completionSource = new TaskCompletionSource<MqttResponse>();
        MqttPendingRequest pendingRequest = new MqttPendingRequest()
        {
            OriginalRequestHeader = requestHeader,
            TaskSource = completionSource
        };
        mqttSingleton.PendingRequestList.Add(pendingRequest);

        Task timeoutTask = Task.Delay(100000);

        _ = mqttSingleton.SendRequest(requestHeader,
                                        targetTopic,
                                        ObjectTypes.ACTUATOR_REQUEST,
                                        request);

        Task completedTask = await Task.WhenAny(completionSource.Task, timeoutTask);


        if (completedTask == timeoutTask)
        {
            return false;
        }
        else
        {
            var response = await completionSource.Task;
            return response?.ExecutionResult == Status.OK;
        }
    }

    public async Task<StationConfigResponse?> SendConfigRequest(string stationMacAdress, IEnumerable<PinConfig> pinConfigs)
    {
        var request = new StationConfigRequest()
        {
            MacAddress = stationMacAdress,
            ComponentsConfigs = pinConfigs
        };

        var targetTopic = string.Format(CommunicationConstants.MQTT_CONFIG_TOPIC_FORMAT,
                                        CommunicationConstants.MQTT_SERVER_TARGET);

        var requestHeader = MqttHeader.Generate();
        var completionSource = new TaskCompletionSource<MqttResponse>();
        MqttPendingRequest pendingRequest = new MqttPendingRequest()
        {
            OriginalRequestHeader = requestHeader,
            TaskSource = completionSource
        };

        mqttSingleton.PendingRequestList.Add(pendingRequest);

        Task timeoutTask = Task.Delay(5000);

        _ = mqttSingleton.SendRequest(requestHeader,
                                        targetTopic,
                                        ObjectTypes.CONFIG_REQUEST,
                                        request);

        Task completedTask = await Task.WhenAny(completionSource.Task, timeoutTask);


        if (completedTask == timeoutTask)
        {
            return null;
        }
        else
        {
            var response = await completionSource.Task;

            try
            {
                return JsonSerializer.Deserialize<StationConfigResponse>(response.JsonObject);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }

    public async Task SendSensorSavingRequest(int stationId, int measurePointId, float value)
    {
        var targetTopic = string.Format(CommunicationConstants.MQTT_SENSOR_TOPIC_FORMAT,
                                        stationId,
                                        measurePointId,
                                        CommunicationConstants.MQTT_SERVER_TARGET);

        await mqttSingleton.PublishAsync(targetTopic, value.ToString());
    }
}
