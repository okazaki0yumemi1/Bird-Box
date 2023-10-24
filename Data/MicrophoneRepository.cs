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

        public async Task<int> Create(Microphone inputDevice)
        {
            _context.InputDevices.Add(inputDevice);
            return _context.SaveChanges();
        }

        public async Task<int> DeleteById(string id)
        {
            var device = _context.InputDevices.FirstOrDefault(x => x.deviceId == id);
            if (device is not null)
            {
                _context.InputDevices.Remove(device);
            }
            return _context.SaveChanges();
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
