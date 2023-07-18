using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bird_Box.Models
{
    public class AnalyzerOptions
    {
        //Locales:
                public List<string> Locales = new List<string>{"af", "ar", "cs", "da", "de", "es", "fi", "fr", "hu", "it", "ja", "ko", "nl", "no", "pl", "pt", "ro", "ru", "sk", "sl", "sv", "th", "tr", "uk", "zh"};
        //--lat, Recording location latitude. Set -1 to ignore.
        public string? latitude {get; set;} = "-1";
        //-lon, Recording location longitude. Set -1 to ignore.
        public string? longitude {get; set;} = "-1";
        //--week, Week of the year when the recording was made. Values in [1, 48] (4 weeks per month). 
        //Set -1 for year-round species list.
        public string? weekOfTheYear {get; set;} = "-1";
        //--slist, Path to species list file or folder. If folder is provided, species list needs to be named "species_list.txt". 
        //If lat and lon are provided, this list will be ignored.
        public string? slist {get; set;}
        //--sensitivity, Detection sensitivity; Higher values result in higher sensitivity. 
        //Values in [0.5, 1.5]. Defaults to 1.0.

        public string? sensitivity {get; set;} = "1";
        //--min_conf, Minimum confidence threshold. Values in [0.01, 0.99]. Defaults to 0.1.
        public string? minimumConfidence {get; set;} = "0.1";
        //--overlap, Overlap of prediction segments. Values in [0.0, 2.9]. Defaults to 0.0.
        public string? overlapSegments {get; set;} = "0";
        //--threads, Number of CPU threads.
        public string? cpuThreads {get; set;} = Environment.ProcessorCount.ToString();
        //--batchsize, Number of samples to process at the same time. Defaults to 1.
        public string? processingBatchSize {get; set;} = "1";
        //--locale, Locale for translated species common names. Values in ['af', 'de', 'it', ...] Defaults to 'en'.
        public string? locale {get; set;} = "en";
        //--sf_thresh, Minimum species occurrence frequency threshold for location filter. Values in [0.01, 0.99]. Defaults to 0.03.
        public string? speciesFrequencyThreshold {get; set;} = "0.03";
        //--classifier, Path to custom trained classifier. Defaults to None. If set, --lat, --lon and --locale are ignored.
        public string? classifier {get; set;} = string.Empty;
        public AnalyzerOptions(bool? setWeek = false, bool? setThreads = true)
        {
            //Set CPU threads:
            if (setThreads == true)
            {
                cpuThreads = Environment.ProcessorCount.ToString();
            }
            //Set weekOfYear:
            if (setWeek == true) weekOfTheYear = ((int)DateTime.Now.Day / 7).ToString();
        }
        public AnalyzerOptions() {}
    }
}