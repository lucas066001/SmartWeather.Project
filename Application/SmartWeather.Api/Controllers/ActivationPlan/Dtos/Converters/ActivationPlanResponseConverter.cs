namespace SmartWeather.Api.Controllers.ActivationPlan.Dtos.Converters;

using SmartWeather.Api.Controllers.Component.Dtos.Converters;
using SmartWeather.Entities.ActivationPlan;

public class ActivationPlanResponseConverter
{
    public static ActivationPlanResponse ConvertActivationPlanToActivationPlanResponse(ActivationPlan activationPlan)
    {
        return new ActivationPlanResponse()
        {
            Id = activationPlan.Id,
            Name = activationPlan.Name,
            StartingDate = activationPlan.StartingDate,
            EndingDate = activationPlan.EndingDate,
            ActivationTime = activationPlan.ActivationTime,
            PeriodInDay = activationPlan.PeriodInDay,
            Duration = activationPlan.Duration,
            ComponentId = activationPlan.ComponentId,
            Component = activationPlan.Component != null ? ComponentResponseConverter.ConvertComponentToComponentResponse(activationPlan.Component) : null
        };
    }
}
