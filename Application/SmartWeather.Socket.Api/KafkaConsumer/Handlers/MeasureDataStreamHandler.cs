using Confluent.Kafka;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.SignalR;
using SmartWeather.Services.ComponentDatas;
using SmartWeather.Services.Kafka;
using SmartWeather.Socket.Api.Contract;
using SmartWeather.Socket.Api.Hubs.MeasurePoint;
using SmartWeather.Socket.Api.Hubs.MeasurePoint.Dtos;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SmartWeather.Socket.Api.Kafka.Handlers;

public class MeasureDataStreamHandler : IKafkaMessageHandler
{
    private IHubContext<MeasurePointHub> _measureDataHub;
    private static readonly Regex _measureDataTopicRegex = new Regex(
        @"^station/([^/]+)/measure_point/([^/]+)/toserver$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public MeasureDataStreamHandler(IHubContext<MeasurePointHub> measureDataHub)
    {
        _measureDataHub = measureDataHub;
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
                    _measureDataHub.Clients.Group(string.Format(Constants.GROUP_NAMING_FORMAT,
                            nameof(MeasurePointHub),
                            measurePointId.ToString()))
                           .SendAsync(
                                ClientMethods.RECEIVED_MP_DATA,
                                new MeasurePointDataDto()
                                {
                                    Id = measurePointId,
                                    Value = measureDataValue
                                });

                    _measureDataHub.Clients.Group(Constants.STREAM_MONITORING)
                           .SendAsync(
                                ClientMethods.RECEIVED_MP_DATA,
                                new MeasurePointDataDto()
                                {
                                    Id = measurePointId,
                                    Value = measureDataValue
                                });
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
