namespace SmartWeather.Repositories.ComponentDatas;

using Elastic.Clients.Elasticsearch;
using SmartWeather.Entities.ComponentData;
using SmartWeather.Repositories.BaseRepository.Exceptions;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.ComponentDatas;

public class MeasureDataRepository(SmartWeatherDocumentsContext elasticContext) : IMeasureDataRepository
{
    /// <summary>
    /// Insert MeasureData into database/
    /// </summary>
    /// <param name="data">MeasureData object to be inserted.</param>
    /// <exception cref="EntitySavingException">Thrown if error occurs during saving.</exception>
    public async void Create(MeasureData data)
    {
        var response = await elasticContext.Client.IndexAsync(data, idx => idx.Index(elasticContext.MeasureDataIndex));
        if(!response.IsSuccess()) throw new EntitySavingException();
    }

    /// <summary>
    /// Retreive MeasureData within desired period.
    /// </summary>
    /// <param name="idMeasurePoint">Int representing MeasurePoint unique Id.</param>
    /// <param name="startPeriod">DateTime representing start time period.</param>
    /// <param name="endPeriod">DateTime representing end time period.</param>
    /// <returns>List of MeasureData.</returns>
    /// <exception cref="EntityFetchingException">Thrown if no data can be read within requested params.</exception>
    public async Task<IEnumerable<MeasureData>> GetFromMeasurePoint(int idMeasurePoint, DateTime startPeriod, DateTime endPeriod)
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
                            .DateRange(dt => dt.From(startPeriod))
                        ),
                        m => m.Range(dr => dr
                            .DateRange(dt => dt.To(endPeriod))
                        )
                    )
                )
            )
        );

        if (!response.IsValidResponse) throw new EntityFetchingException();

        try
        {
            while (response.Documents.Any())
            {
                allResults.AddRange(response.Documents);
                var scrollRequest = new ScrollRequest();
                var scrollResponse = await elasticContext.Client.ScrollAsync<MeasureData>(scrollRequest);

                // Scrolling finished
                if (!scrollResponse.IsValidResponse) break;
            }

            await elasticContext.Client.ClearScrollAsync(new ClearScrollRequest());
            return allResults;
        }
        catch
        {
            throw new EntityFetchingException();
        }
    }
}
