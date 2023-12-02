using Bird_Box.Audio;
using Bird_Box.Models;
using Bird_Box.Utilities;

namespace Bird_Box.Services
{
    public class RecordingService
    {
        private List<Task<int>> _listeningTasks = new List<Task<int>>();

        //CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private Dictionary<int, CancellationTokenSource> _tokenAndTaskIDs =
            new Dictionary<int, CancellationTokenSource>();
        private List<Microphone> InputDevices = new List<Microphone>();

        public RecordingService() 
        { 
            InputDevices = CommandLine.GetAudioDevices();
        }

        public void StartRecording(
            TimeSpan hours,
            AnalyzerOptions optionsInput,
            string? inputDeviceID = null
        )
        {
            var tokenSource = new CancellationTokenSource();
            var device = InputDevices.Find(x => x.deviceId == inputDeviceID && x.inUse == false);

            if (device is null)
                // _listeningTasks.Add(
                //     scheduleRecording.RecordAndRecognize(optionsInput, tokenSource.Token)
                // );
            {
                Console.WriteLine("The device does not exist or is busy.");
                return;
            }
            else
            {
                RecordingSchedule scheduleRecording = new RecordingSchedule(hours, FFMpegSettings.outputPath + $"/Microphone-{device.deviceId}");
                _listeningTasks.Add(
                    scheduleRecording.RecordAndRecognize(
                        optionsInput,
                        tokenSource.Token,
                        device
                    )
                );
                //Set status "In use"
                //Check if it works!!
                device.inUse = true;
            }
            _tokenAndTaskIDs.Add(_listeningTasks.Last().Id, tokenSource);
        }

        public TaskStatus RecordingStatus(int id)
        {
            if (_listeningTasks is null)
                return TaskStatus.Faulted;
            else
            {
                var task = _listeningTasks.Where(x => x.Id == id).FirstOrDefault();
                if (task is null)
                    return TaskStatus.Faulted;
                return task.Status;
            }
        }

        public bool StopRecording(int id)
        {
            if (_listeningTasks is null)
                return true;
            var tokenSource = _tokenAndTaskIDs
                .Where(x => x.Key == id)
                .Select(x => x.Value)
                .FirstOrDefault();
            if (tokenSource is null)
                return false;

            tokenSource.Cancel();
            _tokenAndTaskIDs.Remove(id);
            tokenSource.Dispose();
            return true;
        }

        public List<int> GetRunningRecordingServices()
        {
            return _tokenAndTaskIDs.Select(x => x.Key).ToList();
        }

        public List<Microphone> GetInputDevices()
        {
            return InputDevices;
        }
    }
}
