namespace SmartWeather.Services.Mqtt.Dtos;

public class ActivationPlanResponse
{
    public record ActivationPlanData
    {
        public int ActuatorId { get; set; }
        public double TimeUntilNextActivation { get; set; }
        public double Duration { get; set; }
        public double Period { get; set; }
        public int NbCycles { get; set; }
    }

    public List<ActivationPlanData> ActivationPlanDatas { get; set; } = new List<ActivationPlanData>();
}
