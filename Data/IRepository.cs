using Bird_Box.Models;

namespace Bird_Box.Data
{
    public interface IRepository<T> where T : IEntity
    {
        Task<List<T>> GetAll();
        Task<T> GetByGuid(string id);
        Task<T> GetByBirdName(string species);
        Task<int> Create(T bird);
        Task<int> DeleteById(string id);
    }
}
