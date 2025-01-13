namespace SmartWeather.Services.ActivationPlan;

using SmartWeather.Entities.ActivationPlan;
using SmartWeather.Entities.Common;

public interface IActivationPlanRepository
{
    /// <summary>
    /// Retreives all activation plan related to desired component.
    /// </summary>
    /// <param name="componentId">Int representing Compenent unique Id.</param>
    /// <returns>Result containing the list of activation plan found.</returns>
    Result<IEnumerable<ActivationPlan>> GetFromComponent(int componentId);

    /// <summary>
    /// Retreive an ActivationPlan based on it's unique Id.
    /// </summary>
    /// <param name="activationPlanId">Int representing ActivationPlan unique Id.</param>
    /// <param name="includeComponent">bool specifying to include corresponding Component entity.</param>
    /// <param name="includeStation">bool specifying to include corresponding Station entity.</param>
    /// <returns>Result containing the activation plan found.</returns>
    Result<ActivationPlan> GetById(int activationPlanId, bool includeComponent, bool includeStation);
}
