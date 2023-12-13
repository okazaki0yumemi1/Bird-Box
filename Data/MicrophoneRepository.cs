using Bird_Box.Audio;
using Microsoft.EntityFrameworkCore;

namespace Bird_Box.Data
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class MicrophoneRepository : IRepository<Microphone>
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        private readonly BirdBoxContext _context;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public MicrophoneRepository(BirdBoxContext context)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            _context = context;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public async Task<int> Create(Microphone inputDevice)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            await _context.InputDevices.AddAsync(inputDevice);
            return _context.SaveChanges();
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public async Task<int> DeleteById(string id)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            var device = await _context.InputDevices.FirstOrDefaultAsync(x => x.deviceId == id);
            if (device is not null)
            {
                _context.InputDevices.Remove(device);
            }
            return _context.SaveChanges();
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public async Task<List<Microphone>> GetAll()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            return await _context.InputDevices.ToListAsync();
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public async Task<Microphone> GetByName(string deviceName)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            return await _context.InputDevices
                .Where(x => x.deviceInfo.Contains(deviceName))
                .FirstOrDefaultAsync();
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public async Task<Microphone> GetById(string id)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            return await _context.InputDevices.FirstOrDefaultAsync(x => x.objId == id);
        }
    }
}
