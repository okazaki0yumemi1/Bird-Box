using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bird_Box.Audio;

namespace Bird_Box.Models
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class IdentifiedBird : IEntity
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        [Key]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string objId { get; private set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string birdName { get; private set; } = "";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string detectionThreshold { get; private set; } = "0";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public DateTime recodingDate { get; set; } = DateTime.Now;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Microphone inputDevice { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public IdentifiedBird(string detectedBirdName)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            objId = Guid.NewGuid().ToString();
            birdName = detectedBirdName;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public IdentifiedBird(
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
            string detectedBirdName,
            string threshold,
            DateTime recDate,
            Microphone recordingDevice
        )
        {
            objId = Guid.NewGuid().ToString();
            birdName = detectedBirdName;
            detectionThreshold = threshold;
            recodingDate = recDate;
            if (recordingDevice is not null)
                inputDevice = new Microphone("-1", "Default input device");
            else
                inputDevice = recordingDevice;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public IdentifiedBird()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            objId = Guid.NewGuid().ToString();
        }
    }
}
