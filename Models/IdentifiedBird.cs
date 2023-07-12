using System.ComponentModel.DataAnnotations;

namespace Bird_Box.Models
{
    public class IdentifiedBird
    {
        [Key]
        public Guid objId {get; private set;}
        public string birdName {get; private set;} = "";
        public string detectionThreshold {get; private set;} = "0";
        public DateTime recodingDate {get; set;} = DateTime.Now;
        public IdentifiedBird(string detectedBirdName)
        {
            objId = Guid.NewGuid();
            birdName = detectedBirdName;
        }
        public IdentifiedBird(string detectedBirdName, string threshold, DateTime recDate)
        {
            objId = Guid.NewGuid();
            birdName = detectedBirdName;
            detectionThreshold = threshold;
            recodingDate = recDate;
        }
        public IdentifiedBird()
        {
             objId = Guid.NewGuid();
        }
    }
}