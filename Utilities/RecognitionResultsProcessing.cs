using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bird_Box.Utilities
{
    public class RecognitionResultsProcessing
    {
        string textResultsPath { get; set; }
        public RecognitionResultsProcessing(string textFilesPath)
        {
            textResultsPath = textFilesPath;
        }
        List<string> GetAllTextFiles()
        {
            var result = new List<string>();
            CommandLine bash = new CommandLine();
            var lines = bash.ExecuteCommand($"ls -A {textResultsPath}*.txt").Split("\n").ToList();;
            foreach (var line in lines)
            {
                if (line == "") break;
                var fileName = line.Replace($"{textResultsPath}/", "");
                result.Add(fileName.Replace("\n", ""));
            }
            return result;
        }
        Models.IdentifiedBird ProcessTextFile(string fileName)
        {
            CommandLine bash = new CommandLine();
            var lines = bash.ExecuteCommand($" cat {fileName} | head -n 2 | tail -n 1").Split("\t").ToList();
            if (lines[0] != "1") return (new Models.IdentifiedBird("No detection"));
            else 
            {
                var threshold = lines.Last();
                lines.RemoveAt(lines.Count - 1);
                var fileNameTrimmed = fileName.Replace("Recordings/", "").Substring(0, 19);
                var time = DateTime.ParseExact(fileNameTrimmed, "yyyy'-'MM'-'dd'-'HH'-'mm'-'ss", null);
                var newBird = new Models.IdentifiedBird(lines.Last(), threshold.Replace("\n", ""), time.ToUniversalTime());
                return newBird;
            }
        }
        public List<Models.IdentifiedBird> ProcessAllFiles()
        {
            var birds = new List<Models.IdentifiedBird>();
            int detections = 0;
            var files = GetAllTextFiles();
            var bash = new CommandLine();
            foreach (var file in files)
            {
                var bird = ProcessTextFile(file);
                if ((bird.birdName == "No detection") || (bird.detectionThreshold == "0") || (bird.birdName.Contains("Human"))) 
                {
                    bash.ExecuteCommand($"rm {file}");
                    break;
                }
                birds.Add(bird);
                bash.ExecuteCommand($"rm {file}");
                detections++;
            }
            return birds;
        }
        public int ReturnProcessedResults(string path)
        {
            RecognitionResultsProcessing rrp = new RecognitionResultsProcessing(path);
            var results = rrp.ProcessAllFiles();
            return results.Count;
        }
    }
}