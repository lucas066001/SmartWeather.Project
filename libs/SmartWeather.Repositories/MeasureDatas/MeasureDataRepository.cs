﻿namespace SmartWeather.Repositories.ComponentDatas;

using Elastic.Clients.Elasticsearch;
using SmartWeather.Entities.ComponentData;
using SmartWeather.Entities.Common.Exceptions;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.ComponentDatas;
using SmartWeather.Entities.Common;

public class MeasureDataRepository(SmartWeatherDocumentsContext elasticContext) : IMeasureDataRepository
{
    public async void Create(MeasureData data)
    {
        var response = await elasticContext.Client.IndexAsync(data, idx => idx.Index(elasticContext.MeasureDataIndex));
        if (response.IsSuccess())
        {
            Console.WriteLine("Insertion de MeasureData avec succès");
        }
    }

    public async Task<Result<IEnumerable<MeasureData>>> GetFromMeasurePoint(int idMeasurePoint, DateTime startPeriod, DateTime endPeriod)
    {
        List<MeasureData> allResults = new List<MeasureData>();
        string scrollTimeout = "2m";
        SearchResponse<MeasureData>? response;

        response = await elasticContext.Client.SearchAsync<MeasureData>(s => s
            .Index(elasticContext.MeasureDataIndex)
            .Size(1000)  
            .Scroll(scrollTimeout)
            .Query(q => q
                .Bool(b => b
                    .Must(
                        m => m.Term(t => t
                            .Field(f => f.MeasurePointId)
                            .Value(idMeasurePoint)
                        ),
                        m => m.Range(dr => dr
                            .DateRange(dt => dt.Field(f => f.DateTime).From(startPeriod))
                        ),
                        m => m.Range(dr => dr
                            .DateRange(dt => dt.Field(f => f.DateTime).To(endPeriod))
                        )
                    )
                )
            )
        );

        if (!response.IsValidResponse) return Result<IEnumerable<MeasureData>>.Failure(string.Format(
                                                                    ExceptionsBaseMessages.ENTITY_FETCH,
                                                                    nameof(MeasureData)));

        try
        {
            while (response.Documents.Any())
            {
                allResults.AddRange(response.Documents);
                var scrollRequest = new ScrollRequest();
                var scrollResponse = await elasticContext.Client.ScrollAsync<MeasureData>(scrollRequest);

                if (!scrollResponse.IsValidResponse) break;
            }

            await elasticContext.Client.ClearScrollAsync(new ClearScrollRequest());
            return Result<IEnumerable<MeasureData>>.Success(allResults);
        }
        catch
        {
            return Result<IEnumerable<MeasureData>>.Failure(string.Format(
                                                                    ExceptionsBaseMessages.ENTITY_FETCH,
                                                                    nameof(MeasureData)));
        }
    }
}
