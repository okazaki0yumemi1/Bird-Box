using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bird_Box.Utilities
{
    public class RecordingSchedule
    {
        readonly int cpuThreads = 2;
        TimeSpan timer { get; set; }
        Queue<string> UnprocessedRecordings {get; set;} = new Queue<string>();
        public RecordingSchedule(int threads, int seconds)
        {
            cpuThreads = threads;
            timer = TimeSpan.FromSeconds(seconds);
        }
        public async Task<int> RecordAndRecognize()
        {
            var periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(10));
            int recordingsMade = 0;
            while (await periodicTimer.WaitForNextTickAsync())
            {
                Record();
                recordingsMade++;
                RecognizeBird();
                if ((recordingsMade * 10) > timer.Seconds) break;
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
            audio.cpuThreads = cpuThreads.ToString();
            return audio.ProcessAudioAsync(UnprocessedRecordings.Dequeue());
        }
    }
}