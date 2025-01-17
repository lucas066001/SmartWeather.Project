namespace SmartWeather.Api.Mqtt.Handlers;

using SmartWeather.Entities.Station;
using SmartWeather.Services.Components;
using SmartWeather.Services.Mqtt.Contract;
using SmartWeather.Services.Mqtt.Dtos.Converters;
using SmartWeather.Services.Mqtt.Dtos;
using SmartWeather.Services.Stations;
using SmartWeather.Services.MeasurePoints;
using SmartWeather.Entities.Component;
using SmartWeather.Entities.MeasurePoint;
using SmartWeather.Entities.Common.Exceptions;
using SmartWeather.Services.Mqtt;
using SmartWeather.Services.ActivationPlan;
using SmartWeather.Entities.ActivationPlan;
using static SmartWeather.Services.Mqtt.Dtos.ActivationPlanResponse;

public class ActivationPlanRequestHandler : IMqttRequestHandler
{
    private MqttSingleton _mqttSingleton;
    private ActivationPlanService _activationPlanService;

    public ActivationPlanRequestHandler(MqttSingleton mqttSingleton,
                                ActivationPlanService activationPlanService)
    {
        _mqttSingleton = mqttSingleton;
        _activationPlanService = activationPlanService;
    }

    public async void Handle(MqttRequest request, string originTopic)
    {
        if (!Enum.IsDefined(typeof(ObjectTypes), request.JsonType) || request.JsonType != (int)ObjectTypes.ACTIVATION_PLAN_REQUEST)
        {
            await _mqttSingleton.SendErrorResponse(request, originTopic, "Unknown object type -> " + request.JsonType.ToString(), Status.CONTRACT_ERROR);
            return;
        }

        var activationPlanRequest = await _mqttSingleton.RetreiveMqttObject<ActivationPlanRequest>(request.JsonObject, originTopic, request);

        if (activationPlanRequest == null)
        {
            await _mqttSingleton.SendErrorResponse(request, originTopic);
            return;
        }

        ActivationPlanResponse response = new();
         
        foreach (var actuatorId in activationPlanRequest.ActuatorIds)
        {
            var activationPlan = _activationPlanService.GetFromComponent(actuatorId);
            
            if (activationPlan.IsSuccess && activationPlan.Value.Any())
            {
                foreach (var acp in activationPlan.Value)
                {
                    var acpData = acp.GetNextActivation();
                    if(acpData != null)
                    {
                        response.ActivationPlanDatas.Add(
                            new ActivationPlanData()
                            {
                                ActuatorId = acp.ComponentId,
                                Period = acp.PeriodInDay,
                                Duration = acp.Duration.TotalSeconds,
                                TimeUntilNextActivation = acpData.Item1.TotalSeconds,
                                NbCycles = acpData.Item2
                            }
                        );
                    }
                }
            }
        }

        await _mqttSingleton.SendSuccessResponse(request, originTopic, ObjectTypes.ACTIVATION_PLAN_RESPONSE, response);
    }

    public bool IsAbleToHandle(int requestType)
    {
        return Enum.IsDefined(typeof(ObjectTypes), requestType) 
            && (ObjectTypes)requestType == ObjectTypes.ACTIVATION_PLAN_REQUEST;
    }
}
