using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bird_Box.Utilities
{
    public class RecordingSchedule
    {
        TimeSpan timer { get; set; }
        Queue<string> UnprocessedRecordings {get; set;} = new Queue<string>();
        List<Task> ProcessingAudio {get; set;} = new List<Task>();
        string recordingsPath { get; set;} = "Recordings";
        public RecordingSchedule(TimeSpan timespan)
        {
            timer = timespan;
        }
        public RecordingSchedule(TimeSpan timespan, string resultsPath)
        {
            timer = timespan;
            recordingsPath = resultsPath;
        }
        public async Task<int> RecordAndRecognize(string minConfidence)
        {
            //var secondsElapsed = new TimeSpan(0, 0, 0);
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
                ProcessingAudio.Add(RecognizeBird(minConfidence));
                if ((recordingsMade*10) >= timer.TotalSeconds) break;
            }
            return recordingsMade;
        }
        public void Record()
        {
            Utilities.CommandLine bash = new Utilities.CommandLine();
            Audio.FFMpegSettings newSettings = new Audio.FFMpegSettings();
            var inputDevices = bash.GetAudioDevices();
            Audio.Recording recordingObj = new Audio.Recording(inputDevices.Where(x => x.deviceInfo.Contains("USB")).FirstOrDefault() ?? inputDevices.FirstOrDefault(), newSettings);
            UnprocessedRecordings.Enqueue(recordingObj.RecordAudio());
        }
        public Task<bool> RecognizeBird(string minConfidence)
        {
            double confidenceInput = 0.7d;
            Audio.AudioProcessing audio = new Audio.AudioProcessing(recordingsPath);
            if (Double.TryParse(minConfidence, out confidenceInput)) 
            {
                if ((confidenceInput < 1) && (confidenceInput > 0.01))
                {
                    audio.minConfidence = confidenceInput.ToString();
                }
                else audio.minConfidence = 0.75d.ToString();
            }
            else audio.minConfidence = 0.75d.ToString();
            return audio.ProcessAudioAsync(UnprocessedRecordings.Dequeue());
        }
    }
}