using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bird_Box.Audio
{
    public class Recording
    {
        readonly Utilities.CommandLine bash = new Utilities.CommandLine();
        FFMpegSettings settings {get; set;}
        Microphone inputDevice { get; set; }
        public Recording(Microphone newInputDevice, FFMpegSettings newSettings)
        {
            inputDevice = newInputDevice;
            settings = newSettings;
        }
        public string RecordAudio()
        {
            var fileName = Guid.NewGuid().ToString();
            var outputFile = settings.outputPath;
            var result = ExecuteCommand($"{settings.ffmpegExecutable} -f pulse -i hw:{inputDevice.deviceId} -ac 2 -t 10 {settings.outputPath}/{fileName}.wav");
            return fileName;
        }
        async Task<string> ExecuteCommandAsync(string parameters)
        {
            Task<string> processOutput;
            var processInfo = new System.Diagnostics.ProcessStartInfo();
            processInfo.FileName = "/bin/bash";
            processInfo.Arguments = $"-c \"{parameters}";
            processInfo.RedirectStandardOutput = true;
            using (var process = System.Diagnostics.Process.Start(processInfo))
            {
                processOutput = process.StandardOutput.ReadToEndAsync();
            }
            return processOutput.Result;
        }
        string ExecuteCommand(string parameters)
        {
            string processOutput;
            var processInfo = new System.Diagnostics.ProcessStartInfo();
            processInfo.FileName = "/bin/bash";
            processInfo.Arguments = $"-c \"{parameters}";
            processInfo.RedirectStandardOutput = true;
            using (var process = System.Diagnostics.Process.Start(processInfo))
            {
                processOutput = process.StandardOutput.ReadToEnd();
            }
            return processOutput;
        }
    }
    
}