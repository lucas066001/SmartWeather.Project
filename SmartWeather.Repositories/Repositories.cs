using Microsoft.EntityFrameworkCore.ChangeTracking;
using SmartWeather.Repositories.Context;
using SmartWeather.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWeather.Repositories
{
    public class Repository<T>(SmartWeatherContext context) : IRepository<T> where T : class
    {
        public T Create(T entity)
        {
            try
            {
                var create = context.Set<T>().Add(entity);
                context.SaveChanges();

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
                var update = context.Set<T>().Update(entity);
                context.SaveChanges();

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
                var found = context.Set<T>().Find([entityId]);
                EntityEntry<T> trackedDeletion;

                if (found != null)
                {
                    trackedDeletion = context.Set<T>().Remove(found);
                    context.SaveChanges();
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
            try
            {
                var entity = context.Set<T>().Find([id]);

                return entity == null
                    ? throw new Exception("Unable to retreive entity in database, Entity : " + nameof(T) + " | id : " + id.ToString())
                    : entity;
            }
            catch 
            {
                throw new Exception("An error occured during GetById database command, Entity : " + nameof(T));
            }
        }

        public IEnumerable<T> GetAll()
        {
            try
            {
                var entities = context.Set<T>().AsEnumerable();
                return entities;
            }
            catch 
            {
                throw new Exception("An error occured during GetAll database command, Entity : " + nameof(T));
            }
        }
    }
}
