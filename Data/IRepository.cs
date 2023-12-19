using Bird_Box.Models;

namespace Bird_Box.Data
{
     public interface IRepository<T>
         where T : IEntity
    {
         Task<List<T>> GetAll();
          Task<T> GetById(string id);
          Task<T> GetByName(string name);
          Task<int> Create(T entity);
          Task<int> DeleteById(string id);
     }
}
