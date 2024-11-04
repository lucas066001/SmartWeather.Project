using Elastic.Clients.Elasticsearch;
using SmartWeather.Entities.ComponentData;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.ComponentDatas;

namespace SmartWeather.Repositories.ComponentDatas;

public class MeasureDataRepository(SmartWeatherDocumentsContext elasticContext) : IMeasureDataRepository
{
    public async void Create(MeasureData data)
    {
        var response = await elasticContext.Client.IndexAsync(data, idx => idx.Index(elasticContext.MeasureDataIndex));
        if (!response.IsSuccess())
        {
            if (response.ElasticsearchServerError != null)
            {
                throw new Exception($"Unable to Create MeasureData into elasticContext : {response.ElasticsearchServerError}");
            }
            else
            {
                throw new Exception($"Unable to Create MeasureData into elasticContext : UNKNOWN ERROR");
            }
        }
    }

    public async Task<IEnumerable<MeasureData>> GetFromMeasurePoint(int idMeasurePoint, DateTime? startPeriod = null, DateTime? endPeriod = null)
    {
        SearchResponse<MeasureData>? response = null;

        try
        {
            if (startPeriod != null && endPeriod != null)
            {
                response = await elasticContext.Client.SearchAsync<MeasureData>(s => s
                            .Index(elasticContext.MeasureDataIndex)
                            .Query(q => q
                                .Bool(b => b
                                    .Must(
                                        m => m.Term(t => t
                                            .Field(f => f.MeasurePointId)
                                            .Value(idMeasurePoint)
                                        ),
                                        m => m.Range(dr => dr
                                        .DateRange(dt => dt.From(startPeriod))
                                        ),
                                        m => m.Range(dr => dr
                                        .DateRange(dt => dt.To(endPeriod))
                                        )
                                    )
                                )
                            )
                        );
            }
            else
            {
                response = await elasticContext.Client.SearchAsync<MeasureData>(s => s
                    .Index(elasticContext.MeasureDataIndex)
                    .Query(q => q
                        .Bool(b => b
                            .Must(
                                m => m.Term(t => t
                                    .Field(f => f.MeasurePointId)
                                    .Value(idMeasurePoint)
                                )
                            )
                        )
                    )
                );
            }



            if (response.IsValidResponse)
            {
                return response.Documents.ToList();
            }
            else
            {
                Console.WriteLine($"Search Error: {response.ElasticsearchServerError?.ToString()}");
                return new List<MeasureData>();
            }

        }
        catch (Exception ex)
        {
            throw new Exception("Unable to retreive components from station in database : " + ex.Message);
        }
    }
}
