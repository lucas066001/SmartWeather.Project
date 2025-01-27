using SmartWeather.Services.ComponentDatas;
using SmartWeather.Services.Mqtt;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SmartWeather.Services.Kafka.Handlers;

public class MeasureDataMessageHandler : IMqttMessageHandler
{
    private MeasureDataService _measureDataService;
    private static readonly Regex _measureDataTopicRegex = new Regex(
        @"^station/([^/]+)/measure_point/([^/]+)/toserver$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public MeasureDataMessageHandler(MeasureDataService measureDataService)
    {
        _measureDataService = measureDataService;
    }

    public bool IsAbleToHandle(string originTopic)
    {
        return _measureDataTopicRegex.Match(originTopic).Success;
    }

    public void Handle(string payload, string originTopic)
    {
        Console.WriteLine($"Handling incoming message : {payload}");
        if (!string.IsNullOrEmpty(payload) &&
            !string.IsNullOrEmpty(originTopic))
        {
            Match match = _measureDataTopicRegex.Match(originTopic);

            if (match.Success)
            {
                if (!int.TryParse(match.Groups[2].Value, out int measurePointId))
                {
                    return;
                }

                if (!float.TryParse(payload, NumberStyles.Float, CultureInfo.InvariantCulture, out float measureDataValue))
                {
                    return;
                }

                try
                {
                    _measureDataService.InsertMeasureData(measurePointId, measureDataValue, DateTime.Now);
                }
                catch
                {
                    return;
                }
            }
        }
    }
}
