using Bird_Box.Audio;
using Microsoft.EntityFrameworkCore;

namespace Bird_Box.Data
{
    public class MicrophoneRepository : IRepository<Microphone>
    {
        private readonly BirdBoxContext _context;

        public MicrophoneRepository(BirdBoxContext context)
        {
            _context = context;
        }

        Task<int> IRepository<Microphone>.Create(Microphone inputDevice)
        {
            throw new NotImplementedException();
        }

        Task<int> IRepository<Microphone>.DeleteById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Microphone>> GetAll()
        {
            return await _context.InputDevices.ToListAsync();
        }

        public async Task<Microphone> GetByName(string deviceName)
        {
            return await _context.InputDevices
                .Where(x => x.deviceInfo.Contains(deviceName))
                .FirstOrDefaultAsync();
        }

        public async Task<Microphone> GetById(string id)
        {
            return await _context.InputDevices.FirstOrDefaultAsync(x => x.objId == id);
        }
    }
}
