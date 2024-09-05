﻿namespace SmartWeather.Services.Mqtt.Dtos;

public class StationConfigRequest
{
    public required string MacAddress { get; set; }
    public IEnumerable<int> ActivePins { get; set; } = null!;
}
