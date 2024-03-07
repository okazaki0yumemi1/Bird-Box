using Bird_Box.Audio;
using Bird_Box.Models;
using Microsoft.EntityFrameworkCore;

namespace Bird_Box.Data
{
    public class ListeningTasksRepository : IRepository<ListeningTask>
    {
        private readonly BirdBoxContext _context;
        public ListeningTasksRepository(BirdBoxContext context)
        {
            _context = context;
        }
        public async Task<int> Create(ListeningTask entity)
        {
            _context.Add(entity);
            return _context.SaveChanges();
        }

        public async Task<int> DeleteById(string id)
        {
            var toDelete = _context.ListeningTasks.FirstOrDefault(x => x.objId == id);
            if (toDelete is null)
                return 0;
            _context.ListeningTasks.Remove(toDelete);
            return (_context.SaveChanges());
        }

        public async Task<List<ListeningTask>> GetAll()
        {
            return _context.ListeningTasks.ToList();
        }

        public async Task<ListeningTask> GetById(string id)
        {
            return _context.ListeningTasks.Where(x => x.objId == id).FirstOrDefault();
        }

        //Makes no sense.
        public async Task<ListeningTask> GetByName(string inputDeviceName)
        {
            return _context.ListeningTasks.Include(x => x.InputDevice)
                .FirstOrDefault(x => x.InputDevice.deviceInfo == inputDeviceName);
        }
    }
}
