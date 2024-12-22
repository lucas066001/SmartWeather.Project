namespace SmartWeather.Repositories.Context;

using Elastic.Clients.Elasticsearch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartWeather.Entities.ComponentData;
using SmartWeather.Repositories.Context.Configurations;
using System.Text.RegularExpressions;

public class SmartWeatherDocumentsContext
{
    private static readonly Regex VersionRegex = new(
         @"(\d+\.\d+\.\d+)$",
         RegexOptions.Compiled | RegexOptions.IgnoreCase);
    public ElasticsearchClient Client { get; set; }

    public string MeasureDataIndex = nameof(MeasureData).ToLower();

    public SmartWeatherDocumentsContext(IConfiguration configuration)
    {
        var elasticString = configuration.GetConnectionString("ElasticsearchDb") ?? string.Empty;
        var elasticConf = elasticString.Split(";").ToDictionary(
            conf => conf.Split('=')[0],
            conf => conf.Split('=')[1]
        );

        var settings = new ElasticsearchClientSettings(new Uri(elasticConf["Host"]));
            //.Authentication(new BasicAuthentication(elasticConf["User"], elasticConf["Password"]));

        Client = new ElasticsearchClient(settings);
    }

    public async Task ConfigureIndexesAsync()
    {
        await ConfigureIndexAsync(new MeasureDataConfiguration());
    }

    private async Task ConfigureIndexAsync(IElasticIndexConfigurator indexConfigurator)
    {
        var indexExistsResponse = await Client.Indices.ExistsAsync(indexConfigurator.IndexName);

        if (indexExistsResponse.Exists)
        {
            await MigrateIndexAsync(indexConfigurator);
        }
        else
        {
            await CreateIndexAsync(indexConfigurator);
        }
    }

    private async Task MigrateIndexAsync(IElasticIndexConfigurator indexConfigurator)
    {
        var response = await Client.Indices.PutMappingAsync(indexConfigurator.IndexName, m => m
                    .Properties(
                        indexConfigurator.GetProperties()
                    )
                );

        if (response.IsValidResponse)
        {
            Console.WriteLine("Mapping updated successfully.");
        }
        else
        {
            Console.WriteLine($"Error updating mapping: {response.DebugInformation}");
        }
    }

    public async Task CreateIndexAsync(IElasticIndexConfigurator indexConfigurator)
    {
        var createIndexResponse = await Client.Indices.CreateAsync(indexConfigurator.IndexName, c => c
            .Settings(s => s
                .NumberOfShards(1)
                .NumberOfReplicas(0)
            )
            .Mappings(m => m
                .Properties(
                   indexConfigurator.GetProperties()
                )
            )
        );

        if (!createIndexResponse.IsSuccess())
        {
            if (createIndexResponse.TryGetOriginalException(out var ex) && ex != null)
            {
                Console.WriteLine($"Error while creating index : {ex.Message}");
            }
            else
            {
                Console.WriteLine($"Error while creating index : UNKNOWN ERROR");
            }
        }
        else
        {
            Console.WriteLine($"Index '{indexConfigurator.IndexName}' created successfully !!");
        }
    }
}
