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

public class ConfigRequestHandler : IMqttRequestHandler
{
    private MqttSingleton _mqttSingleton;
    private StationService _stationService;
    private ComponentService _componentService;
    private MeasurePointService _measurePointService;

    public ConfigRequestHandler(MqttSingleton mqttSingleton, 
                                StationService stationService, 
                                ComponentService componentService,
                                MeasurePointService measurePointService)
    {
        _mqttSingleton = mqttSingleton;
        _stationService = stationService;
        _componentService = componentService;
        _measurePointService = measurePointService;
    }

    public async void Handle(MqttRequest request, string originTopic)
    {
        if (!Enum.IsDefined(typeof(ObjectTypes), request.JsonType) || request.JsonType != (int)ObjectTypes.CONFIG_REQUEST)
        {
            await _mqttSingleton.SendErrorResponse(request, originTopic, "Unknown object type -> " + request.JsonType.ToString(), Status.CONTRACT_ERROR);
            return;
        }

        var configRequest = await _mqttSingleton.RetreiveMqttObject<StationConfigRequest>(request.JsonObject, originTopic, request);

        if (configRequest == null)
        {
            await _mqttSingleton.SendErrorResponse(request, originTopic);
            return;
        }

        var macAdress = configRequest.MacAddress;

        var retrievedStation = _stationService.GetStationByMacAddress(macAdress);

        if (retrievedStation.IsSuccess)
        {
            _handleExistingStationConfigRequest(retrievedStation.Value, configRequest, request, originTopic);
        }
        else
        {
            _handleNewStationConfigRequest(macAdress, configRequest, request, originTopic);
        }
        
    }

    private async void _handleExistingStationConfigRequest(Station retrievedStation, StationConfigRequest configRequest, MqttRequest request, string originTopic)
    {
        var componentResult = _componentService.GetFromStation(retrievedStation.Id);

        if (componentResult.IsSuccess)
        {
            retrievedStation.Components = componentResult.Value.ToList();
        }

        var componentsToAdd = configRequest.ComponentsConfigs
                                            .Where(cc =>
                                            !retrievedStation.Components.Any(c => c.GpioPin == cc.GpioPin))
                                            .ToList();


        if (componentsToAdd != null && componentsToAdd.Any())
        {
            var createdComponent = _createComponentsFromConfig(componentsToAdd, retrievedStation.Id);
            foreach (var component in createdComponent)
            {
                retrievedStation.Components.Add(component);
            }
        }

        foreach (var component in retrievedStation.Components)
        {
            component.MeasurePoints = _measurePointService.GetFromComponent(component.Id).Value.ToList();
        }
        var formattedData = StationConfigResponseConverter.ConvertStationToStationConfigResponse(retrievedStation);
        await _mqttSingleton.SendSuccessResponse(request, originTopic, ObjectTypes.CONFIG_RESPONSE, formattedData);
    }

    private async void _handleNewStationConfigRequest(string macAdress, StationConfigRequest configRequest, MqttRequest request, string originTopic)
    {
        var newStation = _stationService.AddGenericStation(macAdress);

        if(newStation.IsFailure)
        {
            await _mqttSingleton.SendErrorResponse(request, originTopic, "Unable to create a station", Status.DATABASE_ERROR);
            return;
        }

        if (configRequest.ComponentsConfigs.Any())
        {
            try
            {
                newStation.Value.Components = _createComponentsFromConfig(configRequest.ComponentsConfigs, newStation.Value.Id);
                var formattedData = StationConfigResponseConverter.ConvertStationToStationConfigResponse(newStation.Value);
                await _mqttSingleton.SendSuccessResponse(request, originTopic, ObjectTypes.CONFIG_RESPONSE, formattedData);
            }
            catch (Exception ex)
            {
                await _mqttSingleton.SendErrorResponse(request, originTopic, "Unable to create components for your station : " + ex.Message, Status.DATABASE_ERROR);
                return;
            }
        }
    }

    private List<Component> _createComponentsFromConfig(IEnumerable<StationConfigRequest.PinConfig> componentsToAdd, int stationId)
    {
        var result = new List<Component>();
        foreach (var compConf in componentsToAdd)
        {
            var createdComponent = _componentService.AddNewComponent(compConf.DefaultName,
                                                                        "#000000",
                                                                        compConf.ComponentType,
                                                                        stationId,
                                                                        compConf.GpioPin);
            if (createdComponent.IsFailure)
            {
                return result;
            }

            createdComponent.Value.MeasurePoints = new List<MeasurePoint>();
            foreach (var mpConf in compConf.MeasurePoints)
            {
                var createdMeasurePoint = _measurePointService.AddNewMeasurePoint(mpConf.LocalId,
                                                                                    mpConf.DefaultName,
                                                                                    "#000000",
                                                                                    mpConf.Unit,
                                                                                    createdComponent.Value.Id);
                if (createdMeasurePoint.IsSuccess)
                {
                    _ = createdComponent.Value.MeasurePoints.Append(createdMeasurePoint.Value);
                }
            }
            result.Add(createdComponent.Value);
            
        }
        return result;
    }

    public bool IsAbleToHandle(int requestType)
    {
        return Enum.IsDefined(typeof(ObjectTypes), requestType) 
            && (ObjectTypes)requestType == ObjectTypes.CONFIG_REQUEST;
    }
}
