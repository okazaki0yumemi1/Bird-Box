using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bird_Box.Audio
{
    public class AudioProcessing
    {
        string pathToAudio {get; set;}
        public AudioProcessing(string recordingsPath)
        {
            pathToAudio = recordingsPath;
        }
        public void ProcessAudio(string fileName)
        {
            string processOutput;
            var processInfo = new System.Diagnostics.ProcessStartInfo();
            processInfo.FileName = "/bin/bash";
            processInfo.Arguments = $"-c \"python3 analyze.py --i ./recordings/{fileName} --o ./results/{DateTime.Today.ToString()}/{DateTime.Now.ToString()}";
            processInfo.RedirectStandardOutput = true;
            using (var process = System.Diagnostics.Process.Start(processInfo))
            {
                processOutput = process.StandardOutput.ReadToEnd();
            }
            Console.WriteLine(processOutput);

        }
    }
}