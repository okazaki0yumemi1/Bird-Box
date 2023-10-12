using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bird_Box.Audio
{
    public class Recording
    {
        FFMpegSettings settings { get; set; }
        Microphone inputDevice { get; set; }

        public Recording(Microphone newInputDevice, FFMpegSettings newSettings)
        {
            inputDevice = newInputDevice;
            settings = newSettings;
        }

        public string RecordAudio()
        {
            var fileName = DateTime.Now.ToString("yyyy'-'MM'-'dd'-'HH'-'mm'-'ss"); //DateTime.Now.ToString();
            var outputFile = settings.outputPath;
            if (inputDevice is not null)
            {
                var result = ExecuteCommand(
                    $"{settings.ffmpegExecutable} -f pulse -i hw:{inputDevice.deviceId} -t 10 {settings.outputPath}/{fileName}.wav"
                );
                return fileName + ".wav";
            }
            else 
            {
                var result = ExecuteCommand(
                    $"{settings.ffmpegExecutable} -f pulse -i default -t 10 {settings.outputPath}/{fileName}.wav"
                );
                return fileName + ".wav";
            }
            throw new NotImplementedException("No microphone to record with! Check arecord -l for any input devices.");
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
