namespace SmartWeather.Repositories.Context;

using InfluxDB.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class MeasureDataContext
{
    public InfluxDBClient Client { get; set; }
    public string Bucket { get; set; }
    public string Org { get; set; }

    public MeasureDataContext(IConfiguration configuration)
    {
        var influxString = configuration.GetConnectionString("InfluxDb") ?? string.Empty;
        var influxConf = influxString.Split(";").ToDictionary(
            conf => conf.Split('=')[0],
            conf => conf.Split('=')[1]
        );
        Client = new InfluxDBClient(influxConf["Host"], influxConf["Token"]);
        Bucket = influxConf["Bucket"];
        Org = influxConf["Org"];
    }
}
