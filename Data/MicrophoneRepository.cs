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
            var devices = await GetAll();
            if (devices.FirstOrDefault(inputDevice) is null)
                await _context.InputDevices.AddAsync(inputDevice);
            else return 0;
            return _context.SaveChanges();
        }

        public async Task<int> DeleteById(string id)
        {
            var device = await _context.InputDevices.FirstOrDefaultAsync(x => x.deviceId == id);
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
