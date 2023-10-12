namespace Bird_Box.Data
{
    public class BirdRepository : IRepository
    {
        private BirdBoxContext _context;

        public BirdRepository(BirdBoxContext context)
        {
            _context = context;
        }

        public async Task<List<Models.IdentifiedBird>> GetAll()
        {
            return _context.BirdRecords.ToList();
        }

        public async Task<Models.IdentifiedBird> GetByGuid(string recordId)
        {
            return _context.BirdRecords.FirstOrDefault(x => x.objId == recordId);
        }

        public async Task<Models.IdentifiedBird> GetByBirdName(string speciesName)
        {
            return _context.BirdRecords.FirstOrDefault(x => x.birdName == speciesName);
        }

        public async Task<List<Models.IdentifiedBird>> GetByDate(DateTime exactDate)
        {
            return _context.BirdRecords
                .Where(x => (x.recodingDate.Date == exactDate.Date))
                .ToList();
        }

        public async Task<int> Create(Models.IdentifiedBird newBird)
        {
            _context.Add(newBird);
            return (_context.SaveChanges());
        }

        public async Task<int> CreateRange(List<Models.IdentifiedBird> newBirdList)
        {
            _context.AddRange(newBirdList);
            return (_context.SaveChanges());
        }

        public async Task<int> DeleteById(string recordId)
        {
            var toDelete = _context.BirdRecords.FirstOrDefault(x => x.objId == recordId);
            if (toDelete is null)
                return 0;
            _context.BirdRecords.Remove(toDelete);
            return (_context.SaveChanges());
        }
    }
}
