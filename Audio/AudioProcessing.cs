using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bird_Box.Audio
{
    public class AudioProcessing
    {
        string pathToAudio {get; set;}
        public string minConfidence {get; set;} = "0.5";
        public string cpuThreads { get; set; } = "1";
        public AudioProcessing(string recordingsPath)
        {
            pathToAudio = recordingsPath;
            cpuThreads = GetCPUThreads();
        }
        string GetCPUThreads()
        {
            string processOutput;
            var processInfo = new System.Diagnostics.ProcessStartInfo();
            processInfo.FileName = "/bin/bash";
            processInfo.Arguments = $"-c \"nproc";
            processInfo.RedirectStandardOutput = true;
            using (var process = System.Diagnostics.Process.Start(processInfo))
            {
                processOutput = process.StandardOutput.ReadToEnd();
            }
            return processOutput;
        }
        public async Task<bool> ProcessAudioAsync(string fileName)
        {
            string processOutput = "";
            var processInfo = new System.Diagnostics.ProcessStartInfo();
            processInfo.FileName = "/bin/bash";
            processInfo.Arguments = $"-c \"python3 BirdNET-Analyzer/analyze.py --min_conf {minConfidence} --sensitivity 1.3 --threads {cpuThreads.Replace("\n", "")} --i Recordings/{fileName} --o Recordings/{fileName}-result.txt";
            processInfo.RedirectStandardOutput = true;
            using (var process = System.Diagnostics.Process.Start(processInfo))
            {
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
            else return false;
        }
    }
}