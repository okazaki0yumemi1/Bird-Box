using Bird_Box.Models;

namespace Bird_Box.Data
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface IRepository<T>
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        where T : IEntity
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Task<List<T>> GetAll();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Task<T> GetById(string id);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Task<T> GetByName(string name);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Task<int> Create(T entity);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Task<int> DeleteById(string id);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
