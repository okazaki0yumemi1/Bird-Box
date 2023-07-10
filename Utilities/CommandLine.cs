using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bird_Box.Utilities
{
    public class CommandLine
    {
        public CommandLine(){}
        public List<Audio.Microphone> GetAudioDevices()
        {
            var devices = new List<Audio.Microphone>();
            var result = ExecuteCommand("arecord -l | awk '{print $2}' | grep -Eo '[1-9]:{1,2}'");
            if (String.IsNullOrEmpty(result)) return devices;
            var lines = result.Split("\n");
            foreach (var line in lines)
            {
                if (line == "") return devices;
                line.Trim();
                string info = ExecuteCommand($"arecord -l | grep 'card {line}'");
                devices.Add(new Audio.Microphone(line.TrimEnd(':'), info));
                //Test devices here!
            }
            return devices;
        }
        public async Task<string> ExecuteCommandAsync(string parameters)
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
        public string ExecuteCommand(string parameters)
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