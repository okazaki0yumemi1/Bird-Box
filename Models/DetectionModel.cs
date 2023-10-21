using Bird_Box.Audio;

namespace Bird_Box.Models
{
    public class DetectionModel
    {
        public IdentifiedBird BirdDetection { get; private set; }
        public Microphone InputDevice { get; private set; }

        public DetectionModel(IdentifiedBird bird, Microphone inputDevice)
        {
            BirdDetection = bird;
            InputDevice = inputDevice;
        }
    }
}
