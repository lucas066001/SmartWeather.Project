namespace SmartWeather.Repositories.Context.Configurations;

using SmartWeather.Entities.ComponentData;
using Elastic.Clients.Elasticsearch.Mapping;

public class MeasureDataConfiguration : IElasticIndexConfigurator
{
    public string IndexName => nameof(MeasureData).ToLower();

    public Properties GetProperties()
    {
        return new Properties<MeasureData>()
        {
            { nameof(MeasureData.MeasurePointId), new IntegerNumberProperty() },
            { nameof(MeasureData.Value), new FloatNumberProperty() },
            { nameof(MeasureData.DateTime), new DateProperty() },
        };
    }
}