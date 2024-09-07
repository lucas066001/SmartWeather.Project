using MQTTnet;
using SmartWeather.Entities.Component;
using SmartWeather.Entities.Station;
using SmartWeather.Services.Components;
using SmartWeather.Services.Mqtt.Contract;
using SmartWeather.Services.Mqtt.Dtos.Converters;
using SmartWeather.Services.Mqtt.Dtos;
using SmartWeather.Services.Stations;
using System.Text.Json;

namespace SmartWeather.Services.Mqtt.Handlers;

public class ConfigRequestHandler : IMqttHandler
{
    private MqttService _mqttService;
    private StationService _stationService;
    private ComponentService _componentService;

    public ConfigRequestHandler(MqttService mqttService, 
                                StationService stationService, 
                                ComponentService componentService)
    {
        _mqttService = mqttService;
        _stationService = stationService;
        _componentService = componentService;
    }

    public async void Handle(MqttRequest request, string originTopic)
    {
        if (!Enum.IsDefined(typeof(ObjectTypes), request.JsonType) || request.JsonType != (int)ObjectTypes.CONFIG_REQUEST)
        {
            await _mqttService.SendErrorResponse(request, originTopic, "Unknown object type -> " + request.JsonType.ToString(), Status.CONTRACT_ERROR);
            return;
        }

        var configRequest = await _mqttService.RetreiveMqttObject<StationConfigRequest>(request.JsonObject, originTopic, request);

        if (configRequest == null)
        {
            await _mqttService.SendErrorResponse(request, originTopic);
            return;
        }

        var macAdress = configRequest.MacAddress;

        Station? retrievedStation = _stationService.GetStationByMacAddress(macAdress);

        if (retrievedStation != null)
        {
            retrievedStation.Components = _componentService.GetFromStation(retrievedStation.Id).ToList();
            var formattedData = StationConfigResponseConverter.ConvertStationToStationConfigResponse(retrievedStation);
            await _mqttService.SendSuccessResponse(request, originTopic, ObjectTypes.CONFIG_RESPONSE, formattedData);
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
                await _mqttService.SendErrorResponse(request, originTopic, "Unable to create a station : " + ex.Message, Status.DATABASE_ERROR);
                return;
            }

            if (configRequest.ActivePins.Any())
            {
                try
                {
                    newStation.Components = _componentService.AddGenericComponentPool(newStation.Id, configRequest.ActivePins).ToList();
                }
                catch (Exception ex)
                {
                    await _mqttService.SendErrorResponse(request, originTopic, "Unable to create components for your station : " + ex.Message, Status.DATABASE_ERROR);
                    return;
                }

                var formattedData = StationConfigResponseConverter.ConvertStationToStationConfigResponse(newStation);
                await _mqttService.SendSuccessResponse(request, originTopic, ObjectTypes.CONFIG_RESPONSE, formattedData);
            }

        }
    }

    public bool IsAbleToHandle(int requestType)
    {
        return Enum.IsDefined(typeof(ObjectTypes), requestType) 
            && (ObjectTypes)requestType == ObjectTypes.CONFIG_REQUEST;
    }
}
