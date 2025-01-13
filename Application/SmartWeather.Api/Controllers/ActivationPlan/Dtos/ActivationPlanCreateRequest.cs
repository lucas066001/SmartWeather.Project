namespace SmartWeather.Api.Controllers.ActivationPlan.Dtos;

public class ActivationPlanCreateRequest
{
    public string Name { get; set; } = null!;
    public int StartingDay { get; set; }
    public TimeSpan Period { get; set; }
    public TimeSpan Duration { get; set; }
    public int ComponentId { get; set; }
}
