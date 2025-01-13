namespace SmartWeather.Services.ActivationPlan;

using SmartWeather.Services.Repositories;
using SmartWeather.Entities.ActivationPlan;
using SmartWeather.Entities.Common.Exceptions;
using SmartWeather.Entities.Common;
using SmartWeather.Entities.ActivationPlan.Exceptions;
using SmartWeather.Services.Components;

public class ActivationPlanService(IRepository<ActivationPlan> activationPlanBaseRepository, IActivationPlanRepository activationPlanRepository)
{
    /// <summary>
    /// Create new ActivationPlan in database based on given infos.
    /// </summary>
    /// <param name="name">String representing activation plan name.</param>
    /// <param name="startingDay">int representing enum corresponding DayOfWeek.</param>
    /// <param name="period">TimeSpan representing the time elapsed between two activations</param>
    /// <param name="duration">TimeSpan representing the time an activativation will last</param>
    /// <param name="componentId">Int representing activation plan target component unique id.</param>
    /// <returns>Result containing newly created entity.</returns>
    public Result<ActivationPlan> AddNewActivationPlan(string name,
                                      int startingDay,
                                      TimeSpan period,
                                      TimeSpan duration,
                                      int componentId)
    {
        try
        {
            ActivationPlan activationPlanToCreate = new(name, 
                                                        startingDay, 
                                                        period,
                                                        duration, 
                                                        componentId);
            return activationPlanBaseRepository.Create(activationPlanToCreate);
        }
        catch (Exception ex) when (ex is InvalidActivationPlanNameException ||
                                   ex is InvalidActivationPlanStartingDayException ||
                                   ex is InvalidActivationPlanComponentIdException)
        {
            return Result<ActivationPlan>.Failure(string.Format(
                                                    ExceptionsBaseMessages.ENTITY_FORMAT,
                                                    nameof(ActivationPlan),
                                                    ex.Message));
        }
    }

    /// <summary>
    /// Delete ActivationPlan based on given id.
    /// </summary>
    /// <param name="idActivationPlan">Int that correspond to activation plan unique Id.</param>
    /// <returns>
    /// Bool indicating deletion activation plan success.
    /// (True : Success | False : Failure)
    /// </returns>
    public bool DeleteActivationPlan(int idComponent)
    {
        return activationPlanBaseRepository.Delete(idComponent).IsSuccess;
    }

    /// <summary>
    /// Modify ActivationPlan in database.
    /// </summary>
    /// <param name="id">Int representing activation plan unique Id.</param>
    /// <param name="name">String representing activation plan name.</param>
    /// <param name="startingDay">int representing enum corresponding DayOfWeek.</param>
    /// <param name="period">TimeSpan representing the time elapsed between two activations</param>
    /// <param name="duration">TimeSpan representing the time an activativation will last</param>
    /// <param name="componentId">Int representing activation plan target component unique id.</param>
    /// <returns>Result containing modified component in database.</returns>
    public Result<ActivationPlan> UpdateComponent(int id,
                                                  string name,
                                                  int startingDay,
                                                  TimeSpan period,
                                                  TimeSpan duration,
                                                  int componentId)
    {
        try
        {
            ActivationPlan activationPlanToUpdate = new(name,
                                                       startingDay,
                                                       period,
                                                       duration,
                                                       componentId)
            {
                Id = id
            };
            return activationPlanBaseRepository.Update(activationPlanToUpdate);
        }
        catch (Exception ex) when (ex is InvalidActivationPlanNameException ||
                                  ex is InvalidActivationPlanStartingDayException ||
                                  ex is InvalidActivationPlanComponentIdException)
        {
            return Result<ActivationPlan>.Failure(string.Format(
                                                    ExceptionsBaseMessages.ENTITY_FORMAT,
                                                    nameof(ActivationPlan),
                                                    ex.Message));
        }
    }

    /// <summary>
    /// Retreive ActivationPlan from a given Component.
    /// </summary>
    /// <param name="idStation">Int representing ActivationPlan unique Id.</param>
    /// <returns>Result containing list of ActivationPlan.</returns>
    public Result<IEnumerable<ActivationPlan>> GetFromComponent(int idComponent)
    {
        return activationPlanRepository.GetFromComponent(idComponent);
    }

    /// <summary>
    /// Check wether or not a User owns an ActivationPlan.
    /// </summary>
    /// <param name="userId">Int representing User unique Id.</param>
    /// <param name="idActivationPlan">Int representing ActivationPlan unique Id.</param>
    /// <returns>
    /// A boolean representing if User own the ActivationPlan.
    /// (True : Owner | False : Not owner)
    /// </returns>
    public bool IsOwnerOfActivationPlan(int userId, int idActivationPlan)
    {
        var activationPlan = activationPlanRepository.GetById(idActivationPlan, true, true);
        return activationPlan.IsSuccess && activationPlan.Value.Component.Station.UserId == userId;
    }

    /// <summary>
    /// Retreive ActivationPlan based on it's Id.
    /// </summary>
    /// <param name="activationPlanId">Int corresponding to activation plan unique Id.</param>
    /// <returns>Result containing desired ActivationPlan.</returns>
    public Result<ActivationPlan> GetById(int activationPlanId)
    {
        return activationPlanBaseRepository.GetById(activationPlanId);
    }
}
