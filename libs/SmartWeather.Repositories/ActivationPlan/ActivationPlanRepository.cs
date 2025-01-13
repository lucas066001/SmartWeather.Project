namespace SmartWeather.Repositories.ActivationPlan;

using SmartWeather.Entities.Common;
using SmartWeather.Services.ActivationPlan;
using SmartWeather.Entities.ActivationPlan;
using SmartWeather.Repositories.Context;
using SmartWeather.Entities.Common.Exceptions;
using SmartWeather.Entities.Station;
using Microsoft.EntityFrameworkCore;

public class ActivationPlanRepository(SmartWeatherReadOnlyContext readOnlyContext) : IActivationPlanRepository
{
    public Result<ActivationPlan> GetById(int activationPlanId, bool includeComponent, bool includeStation)
    {
        ActivationPlan? activationPlanRetreived = null;

        if (includeStation)
        {
            activationPlanRetreived = readOnlyContext.ActivationPlans
                                                     .Include(a => a.Component)
                                                     .ThenInclude(c => c.Station)
                                                     .Where(a => a.Id == activationPlanId)
                                                     .FirstOrDefault();
        }
        else if (includeComponent)
        {
            activationPlanRetreived = readOnlyContext.ActivationPlans
                                         .Include(a => a.Component)
                                         .Where(a => a.Id == activationPlanId)
                                         .FirstOrDefault();
        }
        else
        {
            activationPlanRetreived = readOnlyContext.ActivationPlans
                                                     .Where(c => c.Id == activationPlanId)
                                                     .FirstOrDefault();
        }

        return activationPlanRetreived != null ?
                                        Result<ActivationPlan>.Success(activationPlanRetreived) :
                                        Result<ActivationPlan>.Failure(string.Format(
                                                                        ExceptionsBaseMessages.ENTITY_FETCH,
                                                                        nameof(ActivationPlan)));
    }

    public Result<IEnumerable<ActivationPlan>> GetFromComponent(int componentId)
    {
        var activationPlansRetreived = readOnlyContext.ActivationPlans.Where(a => a.ComponentId == componentId).ToList();

        return activationPlansRetreived != null ?
                    Result<IEnumerable<ActivationPlan>>.Success(activationPlansRetreived) :
                    Result<IEnumerable<ActivationPlan>>.Failure(string.Format(
                                                            ExceptionsBaseMessages.ENTITY_FETCH,
                                                            nameof(ActivationPlan)));
    }
}
