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
            { "measurePointId", new IntegerNumberProperty() },
            { "value", new FloatNumberProperty() },
            { "dateTime", new DateProperty() },
        };
    }
}