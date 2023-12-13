using Bird_Box.Models;
using Microsoft.EntityFrameworkCore;

namespace Bird_Box.Data
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class BirdRepository : IRepository<IdentifiedBird>
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        private readonly BirdBoxContext _context;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public BirdRepository(BirdBoxContext context)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            _context = context;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public async Task<List<IdentifiedBird>> GetAll()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            return _context.BirdRecords.ToList();
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public async Task<IdentifiedBird> GetById(string recordId)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            return _context.BirdRecords.Where(x => x.objId == recordId).Include(x => x.inputDevice).FirstOrDefault();
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public async Task<IdentifiedBird> GetByName(string speciesName)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            return _context.BirdRecords.FirstOrDefault(x => x.birdName == speciesName);
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public async Task<List<IdentifiedBird>> GetByDate(DateTime exactDate)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            return _context.BirdRecords
                .Where(x => (x.recodingDate.Date == exactDate.Date))
                .ToList();
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public async Task<int> Create(IdentifiedBird newBird)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            _context.Add(newBird);
            return _context.SaveChanges();
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public async Task<int> CreateRange(List<IdentifiedBird> newBirdList)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            _context.AddRange(newBirdList);
            return _context.SaveChanges();
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public async Task<int> DeleteById(string recordId)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            var toDelete = _context.BirdRecords.FirstOrDefault(x => x.objId == recordId);
            if (toDelete is null)
                return 0;
            _context.BirdRecords.Remove(toDelete);
            return (_context.SaveChanges());
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Task<int> GetRecourdsCount()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            return _context.BirdRecords.CountAsync();
        }
    }
}
