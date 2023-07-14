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
            var exactDateToUTC = exactDate.ToUniversalTime().Date;
            return _context.BirdRecords
            .Where(x => x.recodingDate.Date == exactDateToUTC)
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
            if (toDelete is null) return 0; 
            _context.BirdRecords.Remove(toDelete);
            return (_context.SaveChanges());
        }
        public int DeleteJunk()
        {
            var toDelete = _context.BirdRecords.Where(x => x.birdName.Contains("Human")).ToList();
            if (toDelete is null) return 0; 
            _context.BirdRecords.RemoveRange(toDelete);
            return (_context.SaveChanges());
        }
    }
}