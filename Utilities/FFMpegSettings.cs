namespace Bird_Box.Audio
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public struct FFMpegSettings
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string ffmpegExecutable { get; private set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static string outputPath { get; set; } = "./Recordings";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void FindFFmpegExecPath()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            var processOutput = "";
            var processInfo = new System.Diagnostics.ProcessStartInfo();
            processInfo.FileName = "/bin/bash";
            //Execute find ffmpeg command, then get columns from second to last
            processInfo.Arguments = "-c \"whereis ffmpeg | awk '{$1=\"\"; print $0}' \"";
            processInfo.RedirectStandardOutput = true;
            using (var process = System.Diagnostics.Process.Start(processInfo))
            {
                processOutput = process.StandardOutput.ReadToEnd();
            }
            var paths = processOutput.Split(" ").ToList();
            foreach (var path in paths)
            {
                //Execute ffmpeg path --version , then get first lind, first column.
                //If ffmpeg is available, it should print "ffmpeg"
                processInfo.Arguments = $"-c \"{path} -version | head -n 1  | awk '{{print $1}}'\"";
                processInfo.RedirectStandardOutput = true;
                var process = System.Diagnostics.Process.Start(processInfo);
                processOutput = process.StandardOutput.ReadToEnd();
                if (processOutput == "ffmpeg")
                {
                    ffmpegExecutable = processOutput;
                    return;
                }
            }
            if (ffmpegExecutable == "")
            {
                //no ffmpeg found, falling back to ffmpeg
                ffmpegExecutable = "ffmpeg";
            }
            return;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void SetFFmpegExecPath(string newPath)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            ffmpegExecutable = newPath;
        }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public void SetOutputPath(string newPath) => outputPath = newPath;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public FFMpegSettings()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            outputPath = "./Recordings";
            ffmpegExecutable = "ffmpeg";
        }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public FFMpegSettings(string path)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            outputPath = path;
            ffmpegExecutable = "ffmpeg";
        }
    }
}
