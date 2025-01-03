namespace SmartWeather.Services.Repositories;

using System.Collections.Generic;
using SmartWeather.Entities.Common;

public interface IRepository<T> where T : class
{
    /// <summary>
    /// Insert Entity in database.
    /// </summary>
    /// <param name="entity">Entity to insert.</param>
    /// <returns>Result containing newly created entity including auto-generated fields (e.g: id).</returns>
    public Result<T> Create(T entity);

    /// <summary>
    /// Modify entity in database.
    /// (Requires Id to be set).
    /// </summary>
    /// <param name="entity">Entity to override previous value.</param>
    /// <returns>Result containing modified entity including.</returns>
    public Result<T> Update(T entity);

    /// <summary>
    /// Delete entity based on given id.
    /// (Need proper Type target to act on correct table).
    /// </summary>
    /// <param name="entityId">Int that correspond to entity unique Id.</param>
    /// <returns>Result containing deleted entity from database.</returns>
    public Result<T> Delete(int entityId);

    /// <summary>
    /// Retreive entity based on it's Id.
    /// (Need proper Type target to act on correct table).
    /// </summary>
    /// <param name="id">Int corresponding to entity unique Id.</param>
    /// <returns>Result containing an Entity object.</returns>
    public Result<T> GetById(int id);

    /// <summary>
    /// Retreive all entities from table.
    /// </summary>
    /// <returns>Result containing list of Entity object.</returns>
    public Result<IEnumerable<T>> GetAll();
}
