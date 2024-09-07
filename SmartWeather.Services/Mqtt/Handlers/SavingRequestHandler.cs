﻿using MQTTnet;
using SmartWeather.Services.Mqtt.Contract;

namespace SmartWeather.Services.Mqtt.Handlers;

public class SavingRequestHandler : IMqttHandler
{
    public void Handle(MqttRequest message, string originTopic)
    {
        throw new NotImplementedException();
    }

    public bool IsAbleToHandle(int requestType)
    {
        return Enum.IsDefined(typeof(ObjectTypes), requestType)
                && (ObjectTypes)requestType == ObjectTypes.SAVING_REQUEST;
    }
}
