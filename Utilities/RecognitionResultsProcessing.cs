using Bird_Box.Audio;
using Bird_Box.Models;

namespace Bird_Box.Utilities
{
     public class RecognitionResultsProcessing
     {
        private string textResultsPath { get; set; }
        //private Microphone inputDevice { get; set; }

         public RecognitionResultsProcessing(string path)
         {
            textResultsPath = "Recordings/" + path;
        }

        /// <summary>
        /// Get list of all text files - output of BirdNET Analyzer
        /// </summary>
        /// <returns>List of *.txt files</returns>
        List<string> GetAllTextFiles()
        {
            var result = new List<string>();
            //CommandLine bash = new CommandLine();
            // var lines = CommandLine
            //     .ExecuteCommand($"ls -A {textResultsPath}*.txt")
            //     .Split("\n")
            //     .ToList();
            if (!Directory.Exists(textResultsPath))
            {
                //Early return - directory does not exist
                return result;
            }
            var lines = Directory.GetFiles(textResultsPath);
            foreach (var line in lines)
            {
                if (line == "")
                    break;
                string fileName = line.Substring(textResultsPath.Length);
                result.Add(fileName.Replace("\n", ""));
            }
            return result;
        }

        /// <summary>
        ///Process a text file
        /// </summary>
        /// <param name="fileName">text file name</param>
        /// <returns>List of identified birds</returns>
        List<IdentifiedBird> ProcessTextFile(string fileName)//, Microphone inputDevice)
        {
            var birds = new List<IdentifiedBird>();
            string lines = "";
            try
            {
                // Open the text file using a stream reader.
                using (var sr = new StreamReader(textResultsPath + fileName))
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
            var linesTotal = lines.Split("\t");
            if (linesTotal.Length < 10)
            {
                birds.Add(new IdentifiedBird("No detection"));
                return birds;
            }
            int i = 10;
            while (linesTotal.Length > i)
            {
                if (linesTotal[i].Contains("Spectrogram"))
                {
                    var birdName = linesTotal[i + 9];
                    var threshold = linesTotal[i + 10];
                    var fileNameTrimmed = fileName.Replace("Recordings", "").Substring(0, 19);
                    var time = DateTime.ParseExact(
                        fileNameTrimmed,
                        "yyyy'-'MM'-'dd'-'HH'-'mm'-'ss",
                        null
                    );
                    var newBird = new IdentifiedBird(
                        birdName,
                        threshold.Replace("\n", ""),
                        time.ToUniversalTime(), null
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
        /// Process all text files in a directory
        /// </summary>
        /// <returns>List of birds</returns>
        public List<IdentifiedBird> ProcessAllFiles()
        {
            var birds = new List<IdentifiedBird>();
            var files = GetAllTextFiles();
            if (!Directory.Exists(textResultsPath))
            {
                //Early return - directory does not exist
                return birds;
            }
            foreach (var file in files)
            {
                var birdsInAFile = ProcessSingleFile(file);
                birds.AddRange(birdsInAFile);
            }
            Directory.Delete(textResultsPath);
            return birds;
        }

         public List<IdentifiedBird> ProcessSingleFile(string file)
         {
            var birds = new List<IdentifiedBird>();
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
            File.Delete($"{textResultsPath + file}");
            return birds;
        }
    }
}
