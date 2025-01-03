namespace SmartWeather.Repositories.BaseRepository;

using Microsoft.EntityFrameworkCore.ChangeTracking;
using SmartWeather.Entities.Common;
using SmartWeather.Entities.Common.Exceptions;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.Repositories;

public class Repository<T>(SmartWeatherContext masterContext,
                           SmartWeatherReadOnlyContext readOnlyContext)
                           : IRepository<T> where T : class
{
    public Result<T> Create(T entity)
    {
        try
        {
            var create = masterContext.Set<T>().Add(entity);
            masterContext.SaveChanges();

            return Result<T>.Success(create.Entity);
        }
        catch
        {
            return Result<T>.Failure(string.Format(
                                        ExceptionsBaseMessages.ENTITY_SAVE,
                                        typeof(T).Name));
        }
    }

    public Result<T> Update(T entity)
    {
        try
        {
            var update = masterContext.Set<T>().Update(entity);
            masterContext.SaveChanges();

            return Result<T>.Success(update.Entity);
        }
        catch
        {
            return Result<T>.Failure(string.Format(
                                            ExceptionsBaseMessages.ENTITY_SAVE,
                                            typeof(T).Name));
        }
    }

    public Result<T> Delete(int entityId)
    {
        var found = masterContext.Set<T>().Find([entityId]);
        EntityEntry<T> trackedDeletion;

        if (found == null)
        {
            return Result<T>.Failure(string.Format(
                                ExceptionsBaseMessages.ENTITY_FETCH,
                                typeof(T).Name));
        }

        try
        {
            trackedDeletion = masterContext.Set<T>().Remove(found);
            masterContext.SaveChanges();
            return Result<T>.Success(trackedDeletion.Entity);
        }
        catch
        {
            return Result<T>.Failure(string.Format(
                                ExceptionsBaseMessages.ENTITY_SAVE,
                                typeof(T).Name));
        }
    }

    public Result<T> GetById(int id)
    {
        var entity = readOnlyContext.Set<T>().Find([id]);
        return entity != null ?
            Result<T>.Success(entity) : 
            Result<T>.Failure(string.Format(
                                ExceptionsBaseMessages.ENTITY_FETCH,
                                typeof(T).Name));
    }

    public Result<IEnumerable<T>> GetAll()
    {
        var entities = readOnlyContext.Set<T>().AsEnumerable();
        return entities != null ?
            Result<IEnumerable<T>>.Success(entities) :
            Result<IEnumerable<T>>.Failure(string.Format(
                                    ExceptionsBaseMessages.ENTITY_FETCH,
                                    typeof(T).Name));
    }
}
