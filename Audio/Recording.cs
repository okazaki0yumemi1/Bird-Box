using Bird_Box.Utilities;

namespace Bird_Box.Audio
{
    public class Recording
    {
        private FFMpegSettings settings { get; set; }
        public Microphone inputDevice { get; private set; }

        public Recording(Microphone newInputDevice, FFMpegSettings newSettings)
        {
            inputDevice = newInputDevice;
            settings = newSettings;
            var path = settings.outputPath + "/Microphone-" + inputDevice.deviceId;
            var directoryExists = Directory.Exists(path);
            if (!directoryExists)
            {
                Console.WriteLine($"The directory {path} does not exist, creating new one.");
                CommandLine.ExecuteCommand($"mkdir {settings.outputPath}/Microphone-{inputDevice.deviceId}");
            }
        }

        public string RecordAudio()
        {
            var fileName = DateTime.Now.ToString("yyyy'-'MM'-'dd'-'HH'-'mm'-'ss"); //DateTime.Now.ToString();
            //var outputPath = settings.outputPath + "/Microphone-" + inputDevice.deviceId;
            if (inputDevice is not null)
            {
                var result = CommandLine.ExecuteCommand(
                    $"{settings.ffmpegExecutable} -f pulse -i {inputDevice.deviceId} -t 10 {settings.outputPath}/{fileName}.wav"
                );
                return fileName + ".wav";
            }
            else
            {
                var result = CommandLine.ExecuteCommand(
                    $"{settings.ffmpegExecutable} -f pulse -i default -t 10 {settings.outputPath}/{fileName}.wav"
                );
                return fileName + ".wav";
            }
            throw new NotImplementedException(
                "No microphone to record with! Check arecord -l for any input devices."
            );
        }

        // public string RecordAudio(Microphone inputDevice)
        // {
        //     var fileName = DateTime.Now.ToString("yyyy'-'MM'-'dd'-'HH'-'mm'-'ss"); //DateTime.Now.ToString();
        //     var outputFile = settings.outputPath;
        //     var result = ExecuteCommand(
        //         $"{settings.ffmpegExecutable} -f pulse -i hw:{inputDevice.deviceId} -t 10 {settings.outputPath}/{fileName}.wav"
        //     );
        //     return fileName + ".wav";
        // }

        // string ExecuteCommand(string parameters)
        // {
        //     string processOutput;
        //     var processInfo = new System.Diagnostics.ProcessStartInfo();
        //     processInfo.FileName = "/bin/bash";
        //     processInfo.Arguments = $"-c \"{parameters}";
        //     processInfo.RedirectStandardOutput = true;
        //     using (var process = System.Diagnostics.Process.Start(processInfo))
        //     {
        //         processOutput = process.StandardOutput.ReadToEnd();
        //     }
        //     return processOutput;
        // }
    }
}
