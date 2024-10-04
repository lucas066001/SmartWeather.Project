namespace SmartWeather.Services.Mqtt.Handlers;

using SmartWeather.Entities.Station;
using SmartWeather.Services.Components;
using SmartWeather.Services.Mqtt.Contract;
using SmartWeather.Services.Mqtt.Dtos.Converters;
using SmartWeather.Services.Mqtt.Dtos;
using SmartWeather.Services.Stations;
using SmartWeather.Services.MeasurePoints;
using SmartWeather.Entities.Component;
using SmartWeather.Entities.MeasurePoint;

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

        Station? retrievedStation = _stationService.GetStationByMacAddress(macAdress);

        if (retrievedStation != null)
        {
            retrievedStation.Components = _componentService.GetFromStation(retrievedStation.Id).ToList();

            // User may added sensors, so we need to add them to db
            var componentsToAdd = configRequest.ComponentsConfigs
                                                .Where(cc => 
                                                !retrievedStation.Components.Any(c => c.GpioPin == cc.GpioPin))
                                                .ToList();
            
            
            if (componentsToAdd != null && componentsToAdd.Any())
            {
                foreach (var compConf in componentsToAdd)
                {
                    var createdComponent = _componentService.AddNewComponent(compConf.DefaultName,
                                                                                 "#000000",
                                                                                 compConf.ComponentType,
                                                                                 retrievedStation.Id,
                                                                                 compConf.GpioPin);
                    createdComponent.MeasurePoints = new List<MeasurePoint>();
                    foreach (var mpConf in compConf.MeasurePoints)
                    {
                        var createdMeasurePoint = _measurePointService.AddNewMeasurePoint(mpConf.LocalId,
                                                                                            mpConf.DefaultName,
                                                                                            "#000000",
                                                                                            mpConf.Unit,
                                                                                            createdComponent.Id);
                        _ = createdComponent.MeasurePoints.Append(createdMeasurePoint);
                    }
                    retrievedStation.Components.Add(createdComponent);
                }
            }

            foreach (var component in retrievedStation.Components)
            {
                component.MeasurePoints = _measurePointService.GetFromComponent(component.Id).ToList();
            }
            var formattedData = StationConfigResponseConverter.ConvertStationToStationConfigResponse(retrievedStation);
            await _mqttSingleton.SendSuccessResponse(request, originTopic, ObjectTypes.CONFIG_RESPONSE, formattedData);
        }
        else
        {
            Station newStation;
            try
            {
                newStation = _stationService.AddGenericStation(macAdress);
            }
            catch (Exception ex)
            {
                await _mqttSingleton.SendErrorResponse(request, originTopic, "Unable to create a station : " + ex.Message, Status.DATABASE_ERROR);
                return;
            }

            if (configRequest.ComponentsConfigs.Any())
            {
                try
                {
                    var createdComponents = new List<Component>();

                    foreach (var compConf in configRequest.ComponentsConfigs)
                    {
                        var createdComponent = _componentService.AddNewComponent(compConf.DefaultName,
                                                                                     "#000000",
                                                                                     compConf.ComponentType,
                                                                                     newStation.Id,
                                                                                     compConf.GpioPin);
                        createdComponent.MeasurePoints = new List<MeasurePoint>();
                        foreach (var mpConf in compConf.MeasurePoints)
                        {
                            var createdMeasurePoint = _measurePointService.AddNewMeasurePoint(mpConf.LocalId,
                                                                                                mpConf.DefaultName,
                                                                                                "#000000",
                                                                                                mpConf.Unit,
                                                                                                createdComponent.Id);
                            createdComponent.MeasurePoints.Add(createdMeasurePoint);
                        }
                        createdComponents.Add(createdComponent);
                    }

                    newStation.Components = createdComponents;
                }
                catch (Exception ex)
                {
                    await _mqttSingleton.SendErrorResponse(request, originTopic, "Unable to create components for your station : " + ex.Message, Status.DATABASE_ERROR);
                    return;
                }

                var formattedData = StationConfigResponseConverter.ConvertStationToStationConfigResponse(newStation);
                await _mqttSingleton.SendSuccessResponse(request, originTopic, ObjectTypes.CONFIG_RESPONSE, formattedData);
            }

        }
    }

    public bool IsAbleToHandle(int requestType)
    {
        return Enum.IsDefined(typeof(ObjectTypes), requestType) 
            && (ObjectTypes)requestType == ObjectTypes.CONFIG_REQUEST;
    }
}
