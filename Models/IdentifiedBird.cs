using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
          public Microphone inputDevice { get; set; }
 
         public IdentifiedBird(string detectedBirdName)
         {
            objId = Guid.NewGuid().ToString();
            birdName = detectedBirdName;
        }

         public IdentifiedBird(
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

         public IdentifiedBird()
         {
            objId = Guid.NewGuid().ToString();
        }
    }
}
