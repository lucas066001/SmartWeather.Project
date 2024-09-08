using SmartWeather.Services.Mqtt.Contract;
using SmartWeather.Services.Mqtt.Dtos;
using SmartWeather.Services.ComponentDatas;

namespace SmartWeather.Services.Mqtt.Handlers;

public class SavingRequestHandler : IMqttRequestHandler
{
    private MqttSingleton _mqttSingleton;
    private ComponentDataService _componentDataService;

    public SavingRequestHandler(MqttSingleton mqttSingleton,
                                ComponentDataService componentDataService)
    {
        _mqttSingleton = mqttSingleton;
        _componentDataService = componentDataService;
    }

    public async void Handle(MqttRequest request, string originTopic)
    {
        if (!Enum.IsDefined(typeof(ObjectTypes), request.JsonType) || request.JsonType != (int)ObjectTypes.SENSOR_SAVING_REQUEST)
        {
            await _mqttSingleton.SendErrorResponse(request, originTopic, "Unknown object type -> " + request.JsonType.ToString(), Status.CONTRACT_ERROR);
            return;
        }

        var componentSavingRequest = await _mqttSingleton.RetreiveMqttObject<ComponentDataSavingRequest>(request.JsonObject, originTopic, request);

        if (componentSavingRequest == null)
        {
            await _mqttSingleton.SendErrorResponse(request, originTopic);
            return;
        }

        try
        {
            _componentDataService.AddNewComponentData(componentSavingRequest.ComponentId, componentSavingRequest.Value, componentSavingRequest.DateTime);
            await _mqttSingleton.SendSuccessResponse(request, originTopic, ObjectTypes.SENSOR_SAVING_RESPONSE, new EmptyResponse());
        }
        catch(Exception ex)
        {
            await _mqttSingleton.SendErrorResponse(request, originTopic, "Unable to create component data : " + ex.Message, Status.DATABASE_ERROR);
        }
    }

    public bool IsAbleToHandle(int requestType)
    {
        return Enum.IsDefined(typeof(ObjectTypes), requestType)
                && (ObjectTypes)requestType == ObjectTypes.SENSOR_SAVING_REQUEST;
    }
}
