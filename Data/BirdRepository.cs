using Bird_Box.Models;
using Microsoft.EntityFrameworkCore;

namespace Bird_Box.Data
{
    public class BirdRepository : IRepository<IdentifiedBird>
    {
        private readonly BirdBoxContext _context;

        public BirdRepository(BirdBoxContext context)
        {
            _context = context;
        }

        public async Task<List<IdentifiedBird>> GetAll()
        {
            return _context.BirdRecords.Include(x => x.inputDevice).ToList();
        }

        public async Task<IdentifiedBird> GetById(string recordId)
        {
            return _context
                .BirdRecords.Where(x => x.objId == recordId)
                .Include(x => x.inputDevice)
                .FirstOrDefault();
        }

        public async Task<IdentifiedBird> GetByName(string speciesName)
        {
            return _context
                .BirdRecords.Include(x => x.inputDevice)
                .FirstOrDefault(x => x.birdName == speciesName);
        }

        public async Task<List<IdentifiedBird>> GetByDate(DateTime exactDate)
        {
            return _context
                .BirdRecords.Where(x => (x.recodingDate.Date == exactDate.Date))
                .Include(x => x.inputDevice)
                .ToList();
        }

        public async Task<int> Create(IdentifiedBird newBird)
        {
            _context.Add(newBird);
            return _context.SaveChanges();
        }

        public async Task<int> CreateRange(List<IdentifiedBird> newBirdList)
        {
            _context.AddRange(newBirdList);
            return _context.SaveChanges();
        }

        public async Task<int> DeleteById(string recordId)
        {
            var toDelete = _context.BirdRecords.FirstOrDefault(x => x.objId == recordId);
            if (toDelete is null)
                return 0;
            _context.BirdRecords.Remove(toDelete);
            return (_context.SaveChanges());
        }

        public Task<int> GetRecourdsCount()
        {
            return _context.BirdRecords.CountAsync();
        }
    }
}
