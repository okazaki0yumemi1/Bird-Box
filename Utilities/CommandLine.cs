using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bird_Box.Utilities
{
    public class CommandLine
    {
        public CommandLine() { }

        /// <summary>
        /// Get list of audio input devices.
        /// </summary>
        /// <returns>List of audio devices: device Id, device info</returns>
        public static List<Audio.Microphone> GetAudioDevices()
        {
            var devices = new List<Audio.Microphone>();
            //var result = ExecuteCommand("arecord -l | grep -o 'card [0-99]'").Replace("card ", "");
            var resultIds = ExecuteCommand("pacmd list-sources | grep 'index'");
            if (String.IsNullOrEmpty(resultIds))
                return devices;
            var lines = resultIds.Split("\n");

            List<string> info = new List<string>();
            var resultInfo = ExecuteCommand($"pacmd list-sources | grep device.description").Split(Environment.NewLine);
            int i = 0;
            foreach (var line in lines)
            {
                if (line == "")
                    return devices;
                if (resultInfo.Count() == 0)
                    return devices;
                //Read only index
                var devId = line.Split(':').Last().TrimStart(' ');

                var tmp = resultInfo[i].Split(" = ");
                var deviceInfo = tmp.Last();
                i++;
                Console.WriteLine($"Added input device: {devId} with following info: {deviceInfo}");
                // line.Trim();
                // string info = ExecuteCommand($"pacmd list-sources | grep alsa.card_name")
                //     .Replace(Environment.NewLine, "");
                // var devId = ExecuteCommand($"arecord -l | grep -o 'card {line}' | grep -o '[0-9]'");
                // devId = devId.Replace(Environment.NewLine, "");
                // devId = ExecuteCommand($"arecord -l | grep -o 'card {line}' | grep -o '[1-9]'")
                //     .Replace(Environment.NewLine, "");
                devices.Add(new Audio.Microphone(devId, deviceInfo));
                // //Test devices here!
            }
            return devices;
        }

        /// <summary>
        /// Run a bash command asynchroniously.
        /// </summary>
        /// <param name="parameters">bash command</param>
        /// <returns>Command output</returns>
        public static async Task<string> ExecuteCommandAsync(string parameters)
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

        /// <summary>
        /// Run a bash command.
        /// </summary>
        /// <param name="parameters">bash command</param>
        /// <returns>Command output</returns>
        public static string ExecuteCommand(string parameters)
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
