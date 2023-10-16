using Bird_Box.Audio;
using Bird_Box.Models;

namespace Bird_Box.Utilities
{
    public class RecordingSchedule
    {
        TimeSpan timer { get; set; }
        Queue<string> UnprocessedRecordings { get; set; } = new Queue<string>();
        List<Task> ProcessingAudio { get; set; } = new List<Task>();
        string recordingsPath { get; set; } = "Recordings";
        private bool _continue = true;

        public RecordingSchedule(TimeSpan timespan)
        {
            timer = timespan;
        }

        public RecordingSchedule(TimeSpan timespan, string resultsPath)
        {
            timer = timespan;
            recordingsPath = resultsPath;
        }

        /// <summary>
        /// This is a task responsible for recording and analysing audio recordings.
        /// </summary>
        /// <param name="options">BirdNET Analyzer parameters</param>
        /// <returns></returns>
        public async Task<int> RecordAndRecognize(AnalyzerOptions options, CancellationToken ct)
        {
            //var secondsElapsed = new TimeSpan(0, 0, 0);
            var periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(10));
            int recordingsMade = 0;
            while (await periodicTimer.WaitForNextTickAsync())
            {
                if (ct.IsCancellationRequested) return recordingsMade;
                //Remove completed tasks:
                ProcessingAudio.RemoveAll(x => x.IsCompleted);
                //If there are more than 5 tasks, they should be finished first:
                if (ProcessingAudio.Count >= 5)
                {
                    Task.WaitAll(ProcessingAudio.ToArray());
                }
                Record();
                recordingsMade++;
                ProcessingAudio.Add(RecognizeBird(options));
                if ((recordingsMade * 10) >= timer.TotalSeconds)
                    break;
            }
            return recordingsMade;
        }

        /// <summary>
        /// Starts recording via USB input device.
        /// </summary>
        public void Record()
        {
            Audio.FFMpegSettings newSettings = new Audio.FFMpegSettings();
            var inputDevices = CommandLine.GetAudioDevices();
            Audio.Recording recordingObj = new Audio.Recording(
                inputDevices.Where(x => x.deviceInfo.Contains("USB")).FirstOrDefault()
                    ?? inputDevices.FirstOrDefault(),
                newSettings
            );
            if (recordingObj is null)
            {
                Console.WriteLine("No input devices detected, no tasks will be run.");
                return; 
            }
            Console.WriteLine($"Using input device with ID={recordingObj.inputDevice.deviceId}");
            UnprocessedRecordings.Enqueue(recordingObj.RecordAudio());
        }

        /// <summary>
        /// This method lets you choose mic by entering device id.
        /// </summary>
        /// <param name="deviceId">input device id. You can see it by running "arecord -l" in terminal</param>
        public void Record(string deviceId)
        {
            FFMpegSettings newSettings = new FFMpegSettings();
            var inputDevices = CommandLine.GetAudioDevices();
            Recording recordingObj = new Recording(
                inputDevices.Where(x => x.deviceId == deviceId).FirstOrDefault(),
                newSettings
            );
            if (recordingObj is null)
            {
                Console.WriteLine("No input devices detected, no tasks will be run.");
                return; 
            }
            UnprocessedRecordings.Enqueue(recordingObj.RecordAudio());
        }

        /// <summary>
        /// Starts analysis of recording with passed parameters. See BirdNET Analyzer documentation for details.
        /// </summary>
        /// <param name="options">BirdNET Analyzer parameters</param>
        /// <returns></returns>
        public Task<bool> RecognizeBird(AnalyzerOptions options)
        {
            AudioProcessing audio = new AudioProcessing(recordingsPath, options);
            double confidenceInput = 0;
            if (Double.TryParse(options.minimumConfidence, out confidenceInput))
            {
                if ((confidenceInput < 1) && (confidenceInput > 0.01))
                {
                    options.minimumConfidence = confidenceInput.ToString();
                }
                else
                    options.minimumConfidence = 0.75d.ToString();
            }
            else
                options.minimumConfidence = 0.75d.ToString();

            return audio.ProcessAudioAsync(UnprocessedRecordings.Dequeue());
        }

        public void StopTask() => _continue = false;
    }
}
