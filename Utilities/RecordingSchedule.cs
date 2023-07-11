using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bird_Box.Utilities
{
    public class RecordingSchedule
    {
        TimeSpan timer { get; set; }
        Queue<string> UnprocessedRecordings {get; set;} = new Queue<string>();
        List<Task> ProcessingAudio {get; set;} = new List<Task>();
        public RecordingSchedule(TimeSpan timespan)
        {
            timer = timespan;
        }
        public async Task<int> RecordAndRecognize()
        {
            var periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(10));
            int recordingsMade = 0;
            while (await periodicTimer.WaitForNextTickAsync())
            {
                //Remove completed tasks:
                ProcessingAudio.RemoveAll(x => x.IsCompleted);
                //If there are more than 5 tasks, they should be finished first:
                if (ProcessingAudio.Count >= 5)
                {
                    Task.WaitAll(ProcessingAudio.ToArray());
                }
                Record();
                recordingsMade++;
                ProcessingAudio.Add(RecognizeBird());
                if (TimeSpan.FromSeconds(recordingsMade * 10).Hours >= timer.Hours) break;
            }
            return recordingsMade;
        }
        public void Record()
        {
            Utilities.CommandLine bash = new Utilities.CommandLine();
            Audio.FFMpegSettings newSettings = new Audio.FFMpegSettings();
            var inputDevices = bash.GetAudioDevices();
            Audio.Recording recordingObj = new Audio.Recording(inputDevices.Where(x => x.deviceInfo.Contains("USB")).First(), newSettings);
            UnprocessedRecordings.Enqueue(recordingObj.RecordAudio());
        }
        public Task<bool> RecognizeBird()
        {
            Audio.AudioProcessing audio = new Audio.AudioProcessing("/Recordings");
            return audio.ProcessAudioAsync(UnprocessedRecordings.Dequeue());
        }
    }
}