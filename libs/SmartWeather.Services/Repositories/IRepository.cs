namespace SmartWeather.Services.Repositories;

using System.Collections.Generic;
using SmartWeather.Entities.Common.Exceptions;


public interface IRepository<T> where T : class
{
    /// <summary>
    /// Insert Entity in database.
    /// </summary>
    /// <param name="entity">Entity to insert.</param>
    /// <returns>Newly created entity including auto-generated fields (e.g: id).</returns>
    /// <exception cref="EntitySavingException">Thrown if error occurs during insertion.</exception>
    public T Create(T entity);

    /// <summary>
    /// Modify entity in database.
    /// (Requires Id to be set).
    /// </summary>
    /// <param name="entity">Entity to override previous value.</param>
    /// <returns>Modified entity including.</returns>
    /// <exception cref="EntitySavingException">Thrown if error occurs during update.</exception>
    public T Update(T entity);

    /// <summary>
    /// Delete entity based on given id.
    /// (Need proper Type target to act on correct table).
    /// </summary>
    /// <param name="entityId">Int that correspond to entity unique Id.</param>
    /// <returns>Deleted entity from database.</returns>
    /// <exception cref="EntityFetchingException">Thrown if id do not match any entity.</exception>
    /// <exception cref="EntitySavingException">Thrown if error occurs during entity deletion.</exception>
    public T Delete(int entityId);

    /// <summary>
    /// Retreive entity based on it's Id.
    /// (Need proper Type target to act on correct table).
    /// </summary>
    /// <param name="id">Int corresponding to entity unique Id.</param>
    /// <returns>An Entity object.</returns>
    /// <exception cref="EntityFetchingException">Thrown if no data is found.</exception>
    public T GetById(int id);

    /// <summary>
    /// Retreive all entities from table.
    /// </summary>
    /// <returns>List of Entity object.</returns>
    /// <exception cref="EntityFetchingException">Thrown if no data is found.</exception>
    public IEnumerable<T> GetAll();
}
