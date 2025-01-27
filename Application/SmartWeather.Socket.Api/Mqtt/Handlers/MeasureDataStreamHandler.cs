using Microsoft.AspNetCore.SignalR;
using SmartWeather.Services.Mqtt;
using SmartWeather.Socket.Api.Contract;
using SmartWeather.Socket.Api.Hubs.MeasurePoint;
using SmartWeather.Socket.Api.Hubs.MeasurePoint.Dtos;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SmartWeather.Socket.Api.Mqtt.Handlers;

public class MeasureDataStreamHandler : IMqttMessageHandler
{
    private IHubContext<MeasurePointHub> _measureDataHub;
    private static readonly Regex _measureDataTopicRegex = new Regex(
        @"^station/([^/]+)/measure_point/([^/]+)/toserver$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public MeasureDataStreamHandler(IHubContext<MeasurePointHub> measureDataHub)
    {
        _measureDataHub = measureDataHub;
    }

    public bool IsAbleToHandle(string originTopic)
    {
        return _measureDataTopicRegex.Match(originTopic).Success;
    }

    public void Handle(string payload, string originTopic)
    {
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
}
