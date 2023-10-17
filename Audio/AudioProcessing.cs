using Bird_Box.Models;

namespace Bird_Box.Audio
{
    public class AudioProcessing
    {
        readonly AnalyzerOptions options;
        string pathToAudio { get; set; }

        public AudioProcessing(string recordingsPath, AnalyzerOptions _options)
        {
            pathToAudio = recordingsPath;
            options = _options;
        }

        public async Task<bool> ProcessAudioAsync(string fileName)
        {
            string processOutput = "";
            var processInfo = new System.Diagnostics.ProcessStartInfo();
            processInfo.FileName = "/bin/bash";
            processInfo.Arguments =
                "-c \"python3 BirdNET-Analyzer/analyze.py "
                + $"--min_conf {options.minimumConfidence} --sensitivity {options.sensitivity} --threads {options.cpuThreads} "
                + $"--lat {options.latitude} --lon {options.longitude} --week {options.weekOfTheYear} --overlap {options.overlapSegments} "
                + $"--batchsize {options.processingBatchSize} --locale {options.locale} --sf_thresh {options.speciesFrequencyThreshold} "
                + $"--i Recordings/{fileName} --o Recordings/{fileName}-result.txt";
            processInfo.RedirectStandardOutput = true;
            using (var process = System.Diagnostics.Process.Start(processInfo))
            {
                if (process is null) 
                {
                    Console.WriteLine($"Can't open analyze.py file, the output is null.");
                    return false;
                }
                processOutput = process.StandardOutput.ReadToEnd();
            }
            if (processOutput.Contains("Finished"))
            {
                //Remove successfully decoded audio recording
                processInfo.Arguments = $"-c \"rm Recordings/{fileName}";
                var process = System.Diagnostics.Process.Start(processInfo);
                process.WaitForExit();
                return true;
            }
            else
                return false;
        }
    }
}
