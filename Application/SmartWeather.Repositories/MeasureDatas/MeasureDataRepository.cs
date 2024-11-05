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
        List<MeasureData> allResults = new List<MeasureData>();
        string scrollTimeout = "2m";  // Durée de validité du scroll
        SearchResponse<MeasureData>? response;

        try
        {
            // Initialisation de la requête de scroll
            if (startPeriod != null && endPeriod != null)
            {
                response = await elasticContext.Client.SearchAsync<MeasureData>(s => s
                    .Index(elasticContext.MeasureDataIndex)
                    .Size(1000)  // Nombre de résultats par "page" de scroll
                    .Scroll(scrollTimeout)  // Activer le scroll avec une durée
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
                    .Size(1000)  // Nombre de résultats par "page" de scroll
                    .Scroll(scrollTimeout)  // Activer le scroll avec une durée
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

            // Vérifier que la réponse initiale est valide
            if (!response.IsValidResponse)
            {
                Console.WriteLine($"Search Error: {response.ElasticsearchServerError?.ToString()}");
                return new List<MeasureData>();
            }

            // Boucle pour collecter tous les documents
            while (response.Documents.Any())
            {
                allResults.AddRange(response.Documents);

                // Création de ScrollRequest pour la page suivante
                var scrollRequest = new ScrollRequest();

                // Récupérer la page suivante en utilisant le ScrollRequest
                var scrollResponse = await elasticContext.Client.ScrollAsync<MeasureData>(scrollRequest);

                // Vérifier si le scroll est valide
                if (!scrollResponse.IsValidResponse)
                {
                    Console.WriteLine("Scroll finished, total elements in request : " + allResults.Count());
                    break;
                }
            }

            // Nettoyer le contexte de scroll après la récupération des documents
            await elasticContext.Client.ClearScrollAsync(new ClearScrollRequest());

            return allResults;
        }
        catch (Exception ex)
        {
            throw new Exception("Unable to retrieve components from station in database: " + ex.Message);
        }
    }
}
