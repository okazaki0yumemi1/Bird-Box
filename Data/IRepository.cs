using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bird_Box.Data
{
    public interface IRepository
    {
        Task<List<Models.IdentifiedBird>> GetAll();
        Task<Models.IdentifiedBird> GetByGuid(string id);
        Task<Models.IdentifiedBird> GetByBirdName(string species);
        Task<int> Create(Models.IdentifiedBird bird);

        //int Update(Models.IdentifiedBird bird);
        Task<int> DeleteById(string id);
    }
}
