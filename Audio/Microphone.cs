using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Bird_Box.Models;

namespace Bird_Box.Audio
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class Microphone : IEntity
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        [Key]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string objId { get; private set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string deviceId { get; private set; } = String.Empty;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string deviceInfo { get; set; } = String.Empty;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        [NotMapped]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public bool inUse { get; set; } = false;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Microphone(string newDeviceId)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            objId = Guid.NewGuid().ToString();
            deviceId = newDeviceId;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Microphone(string newDeviceId, string newDeviceInfo)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            objId = Guid.NewGuid().ToString();
            deviceId = newDeviceId;
            deviceInfo = newDeviceInfo;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Microphone()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            objId = Guid.NewGuid().ToString();
        }
    }
}
