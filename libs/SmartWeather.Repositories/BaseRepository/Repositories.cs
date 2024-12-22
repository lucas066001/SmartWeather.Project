namespace SmartWeather.Repositories.BaseRepository;

using Microsoft.EntityFrameworkCore.ChangeTracking;
using SmartWeather.Entities.Common.Exceptions;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.Repositories;

public class Repository<T>(SmartWeatherContext masterContext,
                           SmartWeatherReadOnlyContext readOnlyContext)
                           : IRepository<T> where T : class
{
    public T Create(T entity)
    {
        try
        {
            var create = masterContext.Set<T>().Add(entity);
            masterContext.SaveChanges();

            return create.Entity;
        }
        catch
        {
            throw new EntitySavingException();
        }
    }

    public T Update(T entity)
    {
        try
        {
            var update = masterContext.Set<T>().Update(entity);
            masterContext.SaveChanges();

            return update.Entity;
        }
        catch
        {
            throw new EntitySavingException();

        }
    }

    public T Delete(int entityId)
    {
        var found = masterContext.Set<T>().Find([entityId]);
        EntityEntry<T> trackedDeletion;

        if (found == null)
        {
            throw new EntityFetchingException();
        }

        try
        {
            trackedDeletion = masterContext.Set<T>().Remove(found);
            masterContext.SaveChanges();
            return trackedDeletion.Entity;
        }
        catch
        {
            throw new EntitySavingException();
        }
    }

    public T GetById(int id)
    {
        var entity = readOnlyContext.Set<T>().Find([id]);
        return entity ?? throw new EntityFetchingException();
    }

    public IEnumerable<T> GetAll()
    {
        var entities = readOnlyContext.Set<T>().AsEnumerable();
        return entities ?? throw new EntityFetchingException();
    }
}
