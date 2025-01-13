namespace SmartWeather.Api.Controllers.ActivationPlan.Dtos.Converters;

using SmartWeather.Entities.ActivationPlan;

public class ActivationPlanListResponseConverter
{
    public static ActivationPlanListResponse ConvertActivationPlanListToActivationPlanResponseList(IEnumerable<ActivationPlan> activationPlans)
    {
        ActivationPlanListResponse result = new ActivationPlanListResponse()
        {
            ActivationPlanList = new List<ActivationPlanResponse>()
        };

        foreach (var activationPlan in activationPlans)
        {
            result.ActivationPlanList.Add(ActivationPlanResponseConverter.ConvertActivationPlanToActivationPlanResponse(activationPlan));
        }
        return result;
    }
}
