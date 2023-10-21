using System.ComponentModel.DataAnnotations;
using Bird_Box.Audio;

namespace Bird_Box.Models
{
    public class IdentifiedBird : IEntity
    {
        [Key]
        public string objId { get; private set; }
        public string birdName { get; private set; } = "";
        public string detectionThreshold { get; private set; } = "0";
        public DateTime recodingDate { get; set; } = DateTime.Now;
        public Microphone recordingDevice { get; set; } = new Microphone("-1", "Unknown device");
        public IdentifiedBird(string detectedBirdName)
        {
            objId = Guid.NewGuid().ToString();
            birdName = detectedBirdName;
        }

        public IdentifiedBird(string detectedBirdName, string threshold, DateTime recDate, Microphone inputDevice)
        {
            objId = Guid.NewGuid().ToString();
            birdName = detectedBirdName;
            detectionThreshold = threshold;
            recodingDate = recDate;
            recordingDevice = inputDevice;
        }

        public IdentifiedBird()
        {
            objId = Guid.NewGuid().ToString();
        }
    }
}
