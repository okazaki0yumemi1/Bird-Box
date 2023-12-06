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

            var devId = "hw:";
            var deviceInfo = string.Empty;

            var resultInfo = ExecuteCommand($"arecord -l | grep card")
                .Split(Environment.NewLine);

            foreach (var line in resultInfo)
            {
                if (line == "") break;
                int i = 0;
                //card number
                while (line[i] != ' ') i++;
                int j = i;
                while (line[j] != ':') j++;
                devId += line[++i..j];

                //skip card info
                while (line[i] != ',') i++;
                i+=2;

                //device number
                while (line[i] != ' ') i++;
                j = i;
                while (line[j] != ':') j++;
                
                devId += ',' + line[++i..j];
                j += 2;
                //device info
                deviceInfo = line[j..line.Length];
            }
            Console.WriteLine($"Added input device: {devId} with following info: {deviceInfo}");
            devices.Add(new Audio.Microphone(devId, deviceInfo));
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
                if (process is null)
                {
                    Console.WriteLine($"Command output is empty. Command is: {parameters}");
                    return String.Empty;
                }
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
                if (process is null)
                {
                    Console.WriteLine($"Command output is empty. Command is: {parameters}");
                    return String.Empty;
                }
                processOutput = process.StandardOutput.ReadToEnd();
            }
            return processOutput;
        }
    }
}
