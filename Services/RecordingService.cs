using Bird_Box.Audio;
using Bird_Box.Models;
using Bird_Box.Utilities;

namespace Bird_Box.Services
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class RecordingService
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        private List<Task<string>> _listeningTasks = new List<Task<string>>();

        //CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private Dictionary<int, CancellationTokenSource> _tokenAndTaskIDs =
            new Dictionary<int, CancellationTokenSource>();
        private List<Microphone> InputDevices = new List<Microphone>();

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public RecordingService() 
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        { 
            InputDevices = CommandLine.GetAudioDevices();
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void StartRecording(
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public TaskStatus RecordingStatus(int id)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public async Task<bool> StopRecording(int id)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if (_listeningTasks is null)
                return true;
            var tokenSource = _tokenAndTaskIDs
                .Where(x => x.Key == id)
                .Select(x => x.Value)
                .FirstOrDefault();
            if (tokenSource is null)
                return false;

            //Stop the task & set input device "in use" status to false 
            tokenSource.Cancel();

            var deviceObjId = await _listeningTasks.First(x => x.Id == id);
            _tokenAndTaskIDs.Remove(id);
            tokenSource.Dispose();

            var device = InputDevices.FirstOrDefault(x => x.objId == deviceObjId);
            if (device is null)
            {
                Console.WriteLine($"Task {id} was stopped successfully, but the attempt to set flag \"inUse\" to false is failed.");
                return true;
            }
            device.inUse = false;

            return true;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public List<int> GetRunningRecordingServices()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            return _tokenAndTaskIDs.Select(x => x.Key).ToList();
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public List<Microphone> GetInputDevices()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            return InputDevices;
        }
    }
}
