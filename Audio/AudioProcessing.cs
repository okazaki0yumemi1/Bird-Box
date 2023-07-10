using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bird_Box.Audio
{
    public class AudioProcessing
    {
        string pathToAudio {get; set;}
        string minConfidence {get; set;} = "0.3";
        string cpuThreads { get; set; } = "1";
        public AudioProcessing(string recordingsPath)
        {
            pathToAudio = recordingsPath;
        }
        public bool ProcessAudio(string fileName)
        {
            string processOutput;
            var processInfo = new System.Diagnostics.ProcessStartInfo();
            processInfo.FileName = "/bin/bash";
            processInfo.Arguments = $"-c \"python3 BirdNET-Analyzer/analyze.py --threads {cpuThreads} --min_conf {minConfidence} --i Recordings/{fileName}.wav --o /home/m918/Bird-Box/Recordings/{fileName}-result.txt";
            processInfo.RedirectStandardOutput = true;
            using (var process = System.Diagnostics.Process.Start(processInfo))
            {
                processOutput = process.StandardOutput.ReadToEnd();
            }
            Console.WriteLine(processOutput);
            return true;
        }
    }
}