using MQTTnet;
using SmartWeather.Services.Mqtt.Contract;
using System.Reflection.Metadata.Ecma335;

namespace SmartWeather.Services.Mqtt;

public interface IMqttMessageHandler
{
    public void Handle(string payload, string originTopic);
    public bool IsAbleToHandle(string originTopic);
}
