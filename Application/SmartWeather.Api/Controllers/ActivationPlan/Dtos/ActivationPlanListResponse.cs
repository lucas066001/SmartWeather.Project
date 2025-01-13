using SmartWeather.Api.Controllers.Component.Dtos;

namespace SmartWeather.Api.Controllers.ActivationPlan.Dtos;

public class ActivationPlanListResponse
{
    public List<ActivationPlanResponse> ActivationPlanList { get; set; } = [];
}
