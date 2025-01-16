namespace SmartWeather.Api.Controllers.ActivationPlan.Dtos;

public class ActivationPlanUpdateRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime StartingDate { get; set; }
    public DateTime EndingDate { get; set; }
    public TimeSpan ActivationTime { get; set; }
    public TimeSpan Duration { get; set; }
    public int PeriodInDay { get; set; }
    public int ComponentId { get; set; }
}
