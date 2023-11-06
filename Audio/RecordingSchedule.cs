using Bird_Box.Audio;
using Bird_Box.Models;

namespace Bird_Box.Utilities
{
    public class RecordingSchedule
    {
        private TimeSpan timer { get; set; }
        private Queue<string> UnprocessedRecordings { get; set; } = new Queue<string>();
        private List<Task> ProcessingAudio { get; set; } = new List<Task>();
        public string recordingsPath { get; private set; } = "Recordings";
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
        /// This is a task responsible for recording and analysing audio recordings. The system default input device will be used
        /// </summary>
        /// <param name="options">BirdNET Analyzer parameters</param>
        /// <returns></returns>
        // public async Task<int> RecordAndRecognize(AnalyzerOptions options, CancellationToken ct)
        // {
        //     //var secondsElapsed = new TimeSpan(0, 0, 0);
        //     var periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(10));
        //     int recordingsMade = 0;
        //     while (await periodicTimer.WaitForNextTickAsync())
        //     {
        //         if (ct.IsCancellationRequested)
        //             return recordingsMade;
        //         //Remove completed tasks:
        //         ProcessingAudio.RemoveAll(x => x.IsCompleted);
        //         //If there are more than 5 tasks, they should be finished first:
        //         if (ProcessingAudio.Count >= 5)
        //         {
        //             Task.WaitAll(ProcessingAudio.ToArray());
        //         }
        //         Record();
        //         recordingsMade++;
        //         ProcessingAudio.Add(RecognizeBird(options));
        //         if ((recordingsMade * 10) >= timer.TotalSeconds)
        //             break;
        //     }
        //     return recordingsMade;
        // }

        /// <summary>
        /// This is a task responsible for recording and analysing audio recordings.
        /// </summary>
        /// <param name="options">BirdNET Analyzer parameters</param>
        /// <returns></returns>
        public async Task<int> RecordAndRecognize(
            AnalyzerOptions options,
            CancellationToken ct,
            Microphone inputDevice
        )
        {
            //var secondsElapsed = new TimeSpan(0, 0, 0);
            var periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(10));
            int recordingsMade = 0;
            while (await periodicTimer.WaitForNextTickAsync())
            {
                if (ct.IsCancellationRequested)
                    return recordingsMade;
                //Remove completed tasks:
                ProcessingAudio.RemoveAll(x => x.IsCompleted);
                //If there are more than 5 tasks, they should be finished first:
                if (ProcessingAudio.Count >= 5)
                {
                    Task.WaitAll(ProcessingAudio.ToArray());
                }
                Record(inputDevice);
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
        // public void Record()
        // {
        //     //var inputDevices = CommandLine.GetAudioDevices();
        //     //if (inputDevices is null) 
        //     //{
        //     //    Console.WriteLine("No input devices found. Check pulseaudio."); 
        //     //    return;
        //     //}
        //     FFMpegSettings newSettings = new FFMpegSettings($"Recordings/");
        //     Recording recordingObj = new Recording(
        //         inputDevices.Where(x => x.deviceInfo.Contains("USB")).FirstOrDefault() ?? null, //inputDevices.FirstOrDefault(),
        //         newSettings
        //     );
        //     if (recordingObj is null)
        //     {
        //         Console.WriteLine("No input devices detected, no tasks will be run.");
        //         return;
        //     }
        //     Console.WriteLine($"Using input device with ID={recordingObj.inputDevice.deviceId}");
        //     UnprocessedRecordings.Enqueue(recordingObj.RecordAudio());
        // }
        
        /// <summary>
        /// This method lets you choose mic by entering device id.
        /// </summary>
        /// <param name="device">Input device.</param>
        public void Record(Microphone device)
        {
            FFMpegSettings newSettings = new FFMpegSettings($"Recordings/Microphone-{device.deviceId}");
            var inputDevices = CommandLine.GetAudioDevices();
                        if (inputDevices is null) 
            {
                Console.WriteLine("No input devices found. Check pulseaudio."); 
                return;
            }
            Recording recordingObj = new Recording(
                device,
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
