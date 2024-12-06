using SmartWeather.Entities.MeasurePoint;
using SmartWeather.StationMocker.Constants;
using System;

namespace SmartWeather.StationMocker.Helpers;

public class MeasurePointMocker
{
    private enum TREND
    {
        UNSET,
        INCREASING,
        DECREASING
    };

    private TREND currentTrend = TREND.UNSET;
    private int iterationRemaining = 0;
    private MeasureUnit measureType = MeasureUnit.Unknown;
    private float? previousValue = null;
    private Random rand;
    public MeasurePointMocker(MeasureUnit mType) {
        measureType = mType;
        rand = new Random();

        iterationRemaining = rand.Next(1, Configuration.MAX_TREND_LENGHT);
        currentTrend = rand.Next(0, 10) <= 5 ? TREND.INCREASING : TREND.DECREASING;
    }

    public float GetSensorData()
    {       
        iterationRemaining--;
        if (iterationRemaining <= 0)
        {
            iterationRemaining = rand.Next(1, Configuration.MAX_TREND_LENGHT);
            currentTrend = rand.Next(0, 10) <= 5 ? TREND.INCREASING : TREND.DECREASING;
        }

        switch (measureType)
        {
            case MeasureUnit.Celsius:
                return _generateValue(previousValue ?? (float)rand.NextDouble() * (46 - (-15)) + (-15),
                                        Configuration.MAX_TREND_STRENGH,
                                        46,
                                        currentTrend);
            case MeasureUnit.Percentage:
                return _generateValue(previousValue ?? (float)rand.NextDouble() * (100 - 0) + 0,
                                        Configuration.MAX_TREND_STRENGH,
                                        100,
                                        currentTrend);
            case MeasureUnit.UvStrength:
                return _generateValue(previousValue ?? (float)rand.NextDouble() * (12 - 0) + 0,
                                        Configuration.MAX_TREND_STRENGH,
                                        12,
                                        currentTrend);
            default:
                return 0.0f;
        }
    }

    private float _generateValue(float previousValue, float maxChange, float maxValue, TREND trend)
    {
        if (maxChange < 0 || maxValue < 0)
            throw new ArgumentException("maxChange and maxValue must be non-negative.");

        float change = (float)(rand.NextDouble() * maxChange);

        float newValue = trend switch
        {
            TREND.INCREASING => previousValue + (previousValue * change / 100),
            TREND.DECREASING => previousValue - (previousValue * change / 100),
            _ => throw new ArgumentOutOfRangeException(nameof(trend), "Invalid trend value.")
        };

        return Math.Clamp(newValue, 0, maxValue);
    }
}
