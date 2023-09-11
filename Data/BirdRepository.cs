using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bird_Box.Data
{
    public class BirdRepository : IRepository
    {
        private BirdBoxContext _context;

        public BirdRepository(BirdBoxContext context)
        {
            _context = context;
        }

        public List<Models.IdentifiedBird> GetAll()
        {
            return _context.BirdRecords.ToList();
        }

        public Models.IdentifiedBird GetByGuid(string recordId)
        {
            return _context.BirdRecords.FirstOrDefault(x => x.objId.ToString() == recordId);
        }

        public Models.IdentifiedBird GetByBirdName(string speciesName)
        {
            return _context.BirdRecords.FirstOrDefault(x => x.birdName == speciesName);
        }

        public List<Models.IdentifiedBird> GetByDate(DateTime exactDate)
        {
            return _context.BirdRecords
                .Where(x => (x.recodingDate.Date == exactDate.Date))
                .ToList();
        }

        public int Create(Models.IdentifiedBird newBird)
        {
            _context.Add(newBird);
            return (_context.SaveChanges());
        }

        public int CreateRange(List<Models.IdentifiedBird> newBirdList)
        {
            _context.AddRange(newBirdList);
            return (_context.SaveChanges());
        }

        public int Delete(string recordId)
        {
            var toDelete = _context.BirdRecords.FirstOrDefault(x => x.objId.ToString() == recordId);
            if (toDelete is null)
                return 0;
            _context.BirdRecords.Remove(toDelete);
            return (_context.SaveChanges());
        }

        public int DeleteById(string id)
        {
            var toDelete = _context.BirdRecords
                .Where(x => x.objId.ToString() == id)
                .FirstOrDefault();
            if (toDelete is null)
                return 0;
            _context.BirdRecords.Remove(toDelete);
            return (_context.SaveChanges());
        }
    }
}
