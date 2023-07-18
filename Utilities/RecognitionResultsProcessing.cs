using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bird_Box.Models;

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
        List<Models.IdentifiedBird> ProcessTextFile(string fileName)
        {
            var birds = new List<IdentifiedBird>();
            CommandLine bash = new CommandLine();
            var linesTotal = bash.ExecuteCommand($" cat {fileName} | head -n 2 | tail -n 1").Split("\t");
            if (linesTotal[0] != "1") 
            {
                birds.Add(new IdentifiedBird("No detection"));
                return birds;
            }
            //linesTotal.RemoveAt(0);
            int birdsAdded = 0;
            while (linesTotal.Length > birdsAdded*10)
            {
                birdsAdded++;
                var threshold = linesTotal[9*birdsAdded];
                var birdName = linesTotal[8*birdsAdded];
                var fileNameTrimmed = fileName.Replace("Recordings/", "").Substring(0, 19);
                var time = DateTime.ParseExact(fileNameTrimmed, "yyyy'-'MM'-'dd'-'HH'-'mm'-'ss", null);
                var newBird = new Models.IdentifiedBird(birdName, threshold.Replace("\n", ""), time.ToUniversalTime());
                birds.Add(newBird);
            }
            return birds;
        }
        public List<Models.IdentifiedBird> ProcessAllFiles()
        {
            var birds = new List<Models.IdentifiedBird>();
            int detections = 0;
            var files = GetAllTextFiles();
            var bash = new CommandLine();
            foreach (var file in files)
            {
                var birdsInASingleFile = ProcessTextFile(file);
                foreach (var birdEntity in birdsInASingleFile)
                {
                    //Filtering background noise
                    if (
                        (birdEntity.birdName == "No detection") || 
                        (birdEntity.detectionThreshold == "0") || 
                        (birdEntity.birdName.Contains("Human")) ||
                        (birdEntity.birdName == "Power tools") ||
                        (birdEntity.birdName == "Siren") ||
                        (birdEntity.birdName == "Engine") ||
                        (birdEntity.birdName == "Gun") ||
                        (birdEntity.birdName == "Fireworks") ||
                        (birdEntity.birdName == "Environmental") ||
                        (birdEntity.birdName == "Noise")
                        )
                    {
                        break;
                    }
                    birds.Add(birdEntity);
                }
                bash.ExecuteCommand($"rm {file}");
                detections++;
            }
            return birds;
        }
    }
}