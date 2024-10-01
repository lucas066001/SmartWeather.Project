using SmartWeather.Services.ComponentDatas;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SmartWeather.Services.Mqtt.Handlers;

public class MeasureDataMessageHandler : IMqttMessageHandler
{
    private MqttSingleton _mqttSingleton;
    private MeasureDataService _measureDataService;
    private static readonly Regex _measureDataTopicRegex = new Regex(
    @"^station/([^/]+)/measure_point/([^/]+)/toserver$",
    RegexOptions.Compiled | RegexOptions.IgnoreCase);


    public MeasureDataMessageHandler(MqttSingleton mqttSingleton,
                                MeasureDataService measureDataService)
    {
        _mqttSingleton = mqttSingleton;
        _measureDataService = measureDataService;
    }

    public void Handle(string payload, string originTopic)
    {
        Match match = _measureDataTopicRegex.Match(originTopic);
        if (match.Success)
        {
            if(!int.TryParse(match.Groups[2].Value, out int measurePointId))
            {
                return;
            }

            if (!float.TryParse(payload, NumberStyles.Float, CultureInfo.InvariantCulture, out float measureDataValue))
            {
                return;
            }

            try
            {
                _measureDataService.AddNewMeasureData(measurePointId, measureDataValue, DateTime.Now);
            }
            catch
            {
                return;
            }

        }

    }

    public bool IsAbleToHandle(string originTopic)
    {
        return _measureDataTopicRegex.IsMatch(originTopic);
    }
}
