namespace Bird_Box.Audio
{
    public struct FFMpegSettings
    {
        public string ffmpegExecutable { get; private set; }
        public static string outputPath { get; set; }

        public void FindFFmpegExecPath()
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

        public void SetFFmpegExecPath(string newPath)
        {
            ffmpegExecutable = newPath;
        }
        public void SetOutputPath(string newPath) => outputPath = newPath;

        public FFMpegSettings()
        {
            outputPath = "./Recordings";
            ffmpegExecutable = "ffmpeg";
        }
        public FFMpegSettings(string path)
        {
            outputPath = path;
            ffmpegExecutable = "ffmpeg";
        }
    }
}
