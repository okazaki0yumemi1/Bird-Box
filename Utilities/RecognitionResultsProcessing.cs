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
            var lines = bash.ExecuteCommand($"ls -A {textResultsPath}/*.txt").Split("\n").ToList();;
            foreach (var line in lines)
            {
                if (line == "") break;
                result.Add(line.Replace("\n", ""));
            }
            return result;
        }
        Models.IdentifiedBird ProcessTextFile(string fileName)
        {
            CommandLine bash = new CommandLine();
            var lines = bash.ExecuteCommand($" cat {fileName} | head -n 2 | tail -n 1").Split("\t").ToList();
            if (lines.Count <= 1) return (new Models.IdentifiedBird("No detection"));
            else 
            {
                var threshold = lines.Last();
                lines.RemoveAt(lines.Count - 1);
                var newBird = new Models.IdentifiedBird(lines.Last(), threshold.Replace("\n", ""), Convert.ToDateTime(fileName));
                return newBird;
            }
        }
        public List<Models.IdentifiedBird> ProcessAllFiles()
        {
            var birds = new List<Models.IdentifiedBird>();
            int detections = 0;
            var files = GetAllTextFiles();
            foreach (var file in files)
            {
                var bird = ProcessTextFile(file);
                if (bird.birdName != "No detection") 
                {
                    birds.Add(bird); 
                    detections++;
                }
            }
            return birds;
        }
    }
}