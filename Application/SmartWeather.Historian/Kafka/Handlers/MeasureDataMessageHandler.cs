using Confluent.Kafka;
using SmartWeather.Services.ComponentDatas;
using SmartWeather.Services.Components;
using SmartWeather.Services.MeasurePoints;
using SmartWeather.Services.Mqtt;
using SmartWeather.Services.Stations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SmartWeather.Services.Kafka.Handlers;

public class MeasureDataMessageHandler : IKafkaMessageHandler
{
    private MeasureDataService _measureDataService;
    private static readonly Regex _measureDataTopicRegex = new Regex(
        @"^station/([^/]+)/measure_point/([^/]+)/toserver$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public MeasureDataMessageHandler(MeasureDataService measureDataService)
    {
        _measureDataService = measureDataService;
    }

    public void Handle(ConsumeResult<string, string> message)
    {
        var messageValue = message.Message.Value;
        var keyValue = message.Message.Key;

        if (!string.IsNullOrEmpty(messageValue) &&
            !string.IsNullOrEmpty(keyValue))
        {
            Match match = _measureDataTopicRegex.Match(message.Message.Key);

            if (match.Success)
            {
                if (!int.TryParse(match.Groups[2].Value, out int measurePointId))
                {
                    return;
                }

                if (!float.TryParse(messageValue, NumberStyles.Float, CultureInfo.InvariantCulture, out float measureDataValue))
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

    public bool IsAbleToHandle(string kafkaTopic)
    {
        return "smartweather.measure.data" == kafkaTopic;
    }
}
