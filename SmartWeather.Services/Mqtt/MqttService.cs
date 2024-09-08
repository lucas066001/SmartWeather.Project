using SmartWeather.Services.Constants;
using SmartWeather.Services.Mqtt.Contract;
using SmartWeather.Services.Mqtt.Dtos;
using static SmartWeather.Services.Mqtt.MqttSingleton;

namespace SmartWeather.Services.Mqtt;

public class MqttService(MqttSingleton mqttSingleton)
{
    public async Task<bool> SendActuatorCommand(int stationId, int componentId, int newValue)
    {
        var request = new ActuatorCommandRequest()
        {
            ComponentId = componentId,
            NewValue = newValue
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
}
