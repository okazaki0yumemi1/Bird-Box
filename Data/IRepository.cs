using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bird_Box.Data
{
    public interface IRepository
    {
        List<Models.IdentifiedBird> GetAll();
        Models.IdentifiedBird GetByGuid(string id);
        Models.IdentifiedBird GetByBirdName(string species);
        int Create(Models.IdentifiedBird bird);

        //int Update(Models.IdentifiedBird bird);
        int Delete(string id);
    }
}
