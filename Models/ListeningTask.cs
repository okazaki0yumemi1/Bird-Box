using Bird_Box.Audio;
using Bird_Box.Utilities;
using System.ComponentModel.DataAnnotations;

namespace Bird_Box.Models
{
    public class ListeningTask : IEntity
    {
        [Key]
        public string objId { get; init; } = Guid.NewGuid().ToString();
        public string OutputFolder { get; private set; }
        public int Hours { get; private set; }
        public Microphone InputDevice {  get; private set; } 
        public AnalyzerOptions Options { get; private set; }
        public ListeningTask() { }
        public ListeningTask(string outputFolder, int hours, Microphone inputDevice, AnalyzerOptions options)
        {
            OutputFolder = outputFolder;
            Hours = hours;
            InputDevice = inputDevice;
            Options = options;
        }
    }
}
