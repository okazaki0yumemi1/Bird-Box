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

        /// <summary>
        /// Get list of all text files - output of BirdNET Analyzer
        /// </summary>
        /// <returns>List of *.txt files</returns>
        List<string> GetAllTextFiles()
        {
            var result = new List<string>();
            CommandLine bash = new CommandLine();
            var lines = bash.ExecuteCommand($"ls -A {textResultsPath}*.txt").Split("\n").ToList();
            ;
            foreach (var line in lines)
            {
                if (line == "")
                    break;
                var fileName = line.Replace($"{textResultsPath}/", "");
                result.Add(fileName.Replace("\n", ""));
            }
            return result;
        }

        /// <summary>
        ///Process a text file
        /// </summary>
        /// <param name="fileName">text file name</param>
        /// <returns>List of identified birds</returns>
        List<Models.IdentifiedBird> ProcessTextFile(string fileName)
        {
            var birds = new List<IdentifiedBird>();
            string lines = "";
            try
            {
                // Open the text file using a stream reader.
                using (var sr = new StreamReader(fileName))
                {
                    // Read the stream as a string, and write the string to the console.
                    lines = sr.ReadToEnd();
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            //CommandLine bash = new CommandLine();
            //var linesTotal = bash.ExecuteCommand($" cat {fileName} | head -n 2 | tail -n 1")
            //    .Split("\t");
            var linesTotal = lines.Split("\t");
            if (linesTotal.Length < 10)
            {
                birds.Add(new IdentifiedBird("No detection"));
                return birds;
            }
            //linesTotal.RemoveAt(0);
            int birdsAdded = 0;
            int i = 10;
            while (linesTotal.Length > i)
            {
                if (linesTotal[i].Contains("Spectrogram"))
                {
                    var birdName = linesTotal[i + 7];
                    var threshold = linesTotal[i + 8];
                    var fileNameTrimmed = fileName.Replace("Recordings/", "").Substring(0, 19);
                    var time = DateTime.ParseExact(
                        fileNameTrimmed,
                        "yyyy'-'MM'-'dd'-'HH'-'mm'-'ss",
                        null
                    );
                    var newBird = new Models.IdentifiedBird(
                        birdName,
                        threshold.Replace("\n", ""),
                        time.ToUniversalTime()
                    );
                    birds.Add(newBird);
                    i++;
                }
                else
                    i++;
            }
            return birds;
        }

        /// <summary>
        /// Process all text files in a /Recordings directory
        /// </summary>
        /// <returns>List of birds</returns>
        public List<Models.IdentifiedBird> ProcessAllFiles()
        {
            var birds = new List<Models.IdentifiedBird>();
            int detections = 0;
            var files = GetAllTextFiles();
            var bash = new CommandLine();
            foreach (var file in files)
            {
                var birdsInAFile = ProcessSingleFile(file);
                birds.AddRange(birdsInAFile);
            }
            return birds;
        }

        public List<Models.IdentifiedBird> ProcessSingleFile(string file)
        {
            var birds = new List<Models.IdentifiedBird>();
            var bash = new CommandLine();
            var birdsInASingleFile = ProcessTextFile(file);
            foreach (var birdEntity in birdsInASingleFile)
            {
                //Filtering background noise
                if (
                    (birdEntity.birdName == "No detection")
                    || (birdEntity.detectionThreshold == "0")
                    || (birdEntity.birdName.Contains("Human"))
                    || (birdEntity.birdName == "Power tools")
                    || (birdEntity.birdName == "Siren")
                    || (birdEntity.birdName == "Engine")
                    || (birdEntity.birdName == "Gun")
                    || (birdEntity.birdName == "Fireworks")
                    || (birdEntity.birdName == "Environmental")
                    || (birdEntity.birdName == "Noise")
                )
                {
                    break;
                }
                birds.Add(birdEntity);
            }
            bash.ExecuteCommand($"rm {file}");
            return birds;
        }
    }
}
