using Microsoft.EntityFrameworkCore.ChangeTracking;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.Repositories;

namespace SmartWeather.Repositories
{
    public class Repository<T>(SmartWeatherContext masterContext, Func<SmartWeatherReadOnlyContext> contextFactory) : IRepository<T> where T : class
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
                throw new Exception("Unable to create entity in database, Entity : " + nameof(T));
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
                throw new Exception("Unable to update entity in database, Entity : " + nameof(T));

            }
        }

        public T Delete(int entityId)
        {
            try
            {
                var found = masterContext.Set<T>().Find([entityId]);
                EntityEntry<T> trackedDeletion;

                if (found != null)
                {
                    trackedDeletion = masterContext.Set<T>().Remove(found);
                    masterContext.SaveChanges();
                }
                else
                {
                    throw new Exception("Unable to find entity to delete base on it's Id : " + entityId.ToString());
                }

                return trackedDeletion.Entity;
            }
            catch 
            {
                throw new Exception("Unable to delete entity in database, Entity : " + nameof(T));
            }
        }

        public T GetById(int id)
        {
            using (var roContext = contextFactory())
            {
                try
                {
                    var entity = roContext.Set<T>().Find([id]);

                    return entity == null
                        ? throw new Exception("Unable to retreive entity in database, Entity : " + nameof(T) + " | id : " + id.ToString())
                        : entity;
                }
                catch 
                {
                    throw new Exception("An error occured during GetById database command, Entity : " + nameof(T));
                }
            }
        }

        public IEnumerable<T> GetAll()
        {
            using (var roContext = contextFactory())
            {
                try
                {
                    var entities = roContext.Set<T>().AsEnumerable();
                    return entities;
                }
                catch
                {
                    throw new Exception("An error occured during GetAll database command, Entity : " + nameof(T));
                }
            }
        }
    }
}
