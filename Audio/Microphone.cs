using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bird_Box.Audio
{
    public class Microphone
    {
        Guid objId { get; set; }
        public string deviceId { get; private set; } = String.Empty;
        public string deviceInfo { get; set; } = String.Empty;

        public Microphone(string newDeviceId)
        {
            objId = Guid.NewGuid();
            deviceId = newDeviceId;
        }

        public Microphone(string newDeviceId, string newDeviceInfo)
        {
            objId = Guid.NewGuid();
            deviceId = newDeviceId;
            deviceInfo = newDeviceInfo;
        }
    }
}
