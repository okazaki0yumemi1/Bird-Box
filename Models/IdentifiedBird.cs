using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bird_Box.Models
{
    public class IdentifiedBird
    {
        public Guid objId {get; private set;}
        public string birdName {get; private set;} = "";
        public string detectionThreshold {get; private set;} = "0";
        DateTime recodingDate {get; set;} = DateTime.Now;
        public IdentifiedBird(string detectedBirdName)
        {
            objId = Guid.NewGuid();
            birdName = detectedBirdName;
        }
        public IdentifiedBird(string detectedBirdName, string threshold)
        {
            objId = Guid.NewGuid();
            birdName = detectedBirdName;
            detectionThreshold = threshold;
        }
    }
}