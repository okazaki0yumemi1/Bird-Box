using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bird_Box.Audio
{
    public class Microphone
    {
        [Key]
        public string objId { get; private set; }
        public string deviceId { get; private set; } = String.Empty;
        public string deviceInfo { get; set; } = String.Empty;

        public Microphone(string newDeviceId)
        {
            objId = Guid.NewGuid().ToString();
            deviceId = newDeviceId;
        }

        public Microphone(string newDeviceId, string newDeviceInfo)
        {
            objId = Guid.NewGuid().ToString();
            deviceId = newDeviceId;
            deviceInfo = newDeviceInfo;
        }

        public Microphone()
        {
            objId = Guid.NewGuid().ToString();
        }
    }
}
