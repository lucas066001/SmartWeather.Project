using SmartWeather.Services.Mqtt.Contract;
using SmartWeather.Services.Mqtt.Dtos;
using SmartWeather.Services.ComponentDatas;

namespace SmartWeather.Services.Mqtt.Handlers;

public class SavingRequestHandler : IMqttHandler
{
    private MqttService _mqttService;
    private ComponentDataService _componentDataService;

    public SavingRequestHandler(MqttService mqttService,
                                ComponentDataService componentDataService)
    {
        _mqttService = mqttService;
        _componentDataService = componentDataService;
    }

    public async void Handle(MqttRequest request, string originTopic)
    {
        if (!Enum.IsDefined(typeof(ObjectTypes), request.JsonType) || request.JsonType != (int)ObjectTypes.SAVING_REQUEST)
        {
            await _mqttService.SendErrorResponse(request, originTopic, "Unknown object type -> " + request.JsonType.ToString(), Status.CONTRACT_ERROR);
            return;
        }

        var componentSavingRequest = await _mqttService.RetreiveMqttObject<ComponentDataSavingRequest>(request.JsonObject, originTopic, request);

        if (componentSavingRequest == null)
        {
            await _mqttService.SendErrorResponse(request, originTopic);
            return;
        }

        try
        {
            _componentDataService.AddNewComponentData(componentSavingRequest.ComponentId, componentSavingRequest.Value, componentSavingRequest.DateTime);
            await _mqttService.SendSuccessResponse(request, originTopic, ObjectTypes.SAVING_RESPONSE, new EmptyResponse());
        }
        catch(Exception ex)
        {
            await _mqttService.SendErrorResponse(request, originTopic, "Unable to create component data : " + ex.Message, Status.DATABASE_ERROR);
        }
    }

    public bool IsAbleToHandle(int requestType)
    {
        return Enum.IsDefined(typeof(ObjectTypes), requestType)
                && (ObjectTypes)requestType == ObjectTypes.SAVING_REQUEST;
    }
}
