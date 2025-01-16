using SmartWeather.Api.Controllers.Component.Dtos;

namespace SmartWeather.Api.Controllers.ActivationPlan.Dtos;

public class ActivationPlanResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime StartingDate { get; set; }
    public DateTime EndingDate { get; set; }
    public int PeriodInDay { get; set; }
    public TimeSpan ActivationTime { get; set; }
    public TimeSpan Duration { get; set; }
    public int ComponentId { get; set; }
    public ComponentResponse? Component { get; set; } = null;
}
