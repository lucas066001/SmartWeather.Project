using MQTTnet;
using SmartWeather.Services.Mqtt.Contract;
using System.Reflection.Metadata.Ecma335;

namespace SmartWeather.Services.Mqtt;

public interface IMqttHandler
{
    public void Handle(MqttRequest message, string originTopic);
    public bool IsAbleToHandle(int requestType);
}
