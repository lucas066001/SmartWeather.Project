using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using MySqlX.XDevAPI;
using SmartWeather.Entities.ComponentData;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.ComponentDatas;

namespace SmartWeather.Repositories.ComponentDatas;

public class MeasureDataRepository(SmartWeatherReadOnlyContext readOnlyContext, MeasureDataContext influxContext) : IMeasureDataRepository
{
    public void Create(MeasureData data)
    {
        var point = PointData
                        .Measurement("sensor_data")
                        .Tag(nameof(data.MeasurePointId), data.MeasurePointId.ToString())
                        .Field(nameof(data.Value), data.Value)
                        .Timestamp(DateTime.UtcNow, WritePrecision.Ns);
            
        var writeApi = influxContext.Client.GetWriteApi();
        writeApi.WritePoint(point, influxContext.Bucket, influxContext.Org);
    }

    public IEnumerable<MeasureData> GetFromMeasurePoint(int idMeasurePoint, DateTime? startPeriod = null, DateTime? endPeriod = null)
    {
        IEnumerable<MeasureData> componentDatasRetreived = null!;
        try
        {
            if (startPeriod != null && endPeriod != null)
            {
                componentDatasRetreived = readOnlyContext.MeasureDatas.Where(cd => cd.MeasurePointId == idMeasurePoint && (cd.DateTime >= startPeriod && cd.DateTime <= endPeriod)).ToList();
            }
            else
            {
                componentDatasRetreived = readOnlyContext.MeasureDatas.Where(cd => cd.MeasurePointId == idMeasurePoint).ToList();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Unable to retreive components from station in database : " + ex.Message);
        }

        return componentDatasRetreived;
    }
}
