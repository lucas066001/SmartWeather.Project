namespace SmartWeather.Repositories.BaseRepository;

using Microsoft.EntityFrameworkCore.ChangeTracking;
using SmartWeather.Repositories.BaseRepository.Exceptions;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.Repositories;

public class Repository<T>(SmartWeatherContext masterContext,
                           SmartWeatherReadOnlyContext readOnlyContext)
                           : IRepository<T> where T : class
{
    /// <summary>
    /// Insert Entity in database.
    /// </summary>
    /// <param name="entity">Entity to insert.</param>
    /// <returns>Newly created entity including auto-generated fields (e.g: id).</returns>
    /// <exception cref="EntitySavingException">Thrown if error occurs during insertion.</exception>
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

    /// <summary>
    /// Modify entity in database.
    /// (Requires Id to be set).
    /// </summary>
    /// <param name="entity">Entity to override previous value.</param>
    /// <returns>Modified entity including.</returns>
    /// <exception cref="EntitySavingException">Thrown if error occurs during update.</exception>
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

    /// <summary>
    /// Delete entity based on given id.
    /// (Need proper Type target to act on correct table).
    /// </summary>
    /// <param name="entityId">Int that correspond to entity unique Id.</param>
    /// <returns>Deleted entity from.</returns>
    /// <exception cref="EntityFetchingException">Thrown if id do not match any entity.</exception>
    /// <exception cref="EntitySavingException">Thrown if error occurs during entity deletion.</exception>
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

    /// <summary>
    /// Retreive entity based on it's Id.
    /// (Need proper Type target to act on correct table).
    /// </summary>
    /// <param name="id">Int corresponding to entity unique Id.</param>
    /// <returns>An Entity object.</returns>
    /// <exception cref="EntityFetchingException">Thrown if no data is found.</exception>
    public T GetById(int id)
    {
        var entity = readOnlyContext.Set<T>().Find([id]);
        return entity ?? throw new EntityFetchingException();
    }

    /// <summary>
    /// Retreive all entities from table.
    /// </summary>
    /// <returns>List of Entity object.</returns>
    /// <exception cref="EntityFetchingException">Thrown if no data is found.</exception>
    public IEnumerable<T> GetAll()
    {
        var entities = readOnlyContext.Set<T>().AsEnumerable();
        return entities ?? throw new EntityFetchingException();
    }
}
