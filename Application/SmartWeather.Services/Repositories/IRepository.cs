using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWeather.Services.Repositories
{
    public interface IRepository<T> where T : class
    {
        public T Create(T entity);

        public T Update(T entity);

        public T Delete(int entityId);

        public T GetById(int id);

        public IEnumerable<T> GetAll();
    }
}
