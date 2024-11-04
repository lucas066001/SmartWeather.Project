using Elastic.Clients.Elasticsearch.Mapping;

namespace SmartWeather.Repositories.Context.Configurations;

public interface IElasticIndexConfigurator
{
    public string IndexName { get; }
    public Properties GetProperties();
}
