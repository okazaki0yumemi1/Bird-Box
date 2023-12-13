using System.Globalization;

namespace Bird_Box.Models
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class AnalyzerOptions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        //Locales:
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public List<string> Locales = new List<string>
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            "af",
            "ar",
            "cs",
            "da",
            "de",
            "es",
            "fi",
            "fr",
            "hu",
            "it",
            "ja",
            "ko",
            "nl",
            "no",
            "pl",
            "pt",
            "ro",
            "ru",
            "sk",
            "sl",
            "sv",
            "th",
            "tr",
            "uk",
            "zh"
        };

        //--lat, Recording location latitude. Set -1 to ignore.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string? latitude { get; set; } = "-1";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        //-lon, Recording location longitude. Set -1 to ignore.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string? longitude { get; set; } = "-1";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        //--week, Week of the year when the recording was made. Values in [1, 48] (4 weeks per month).
        //Set -1 for year-round species list.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string? weekOfTheYear { get; set; } = "-1";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        //--slist, Path to species list file or folder. If folder is provided, species list needs to be named "species_list.txt".
        //If lat and lon are provided, this list will be ignored.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string? slist { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        //--sensitivity, Detection sensitivity; Higher values result in higher sensitivity.
        //Values in [0.5, 1.5]. Defaults to 1.0.

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string? sensitivity { get; set; } = "1";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        //--min_conf, Minimum confidence threshold. Values in [0.01, 0.99]. Defaults to 0.1.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string? minimumConfidence { get; set; } = "0.1";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        //--overlap, Overlap of prediction segments. Values in [0.0, 2.9]. Defaults to 0.0.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string? overlapSegments { get; set; } = "0";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        //--threads, Number of CPU threads.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string? cpuThreads { get; set; } = Environment.ProcessorCount.ToString();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        //--batchsize, Number of samples to process at the same time. Defaults to 1.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string? processingBatchSize { get; set; } = "1";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        //--locale, Locale for translated species common names. Values in ['af', 'de', 'it', ...] Defaults to 'en'.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string? locale { get; set; } = "en";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        //--sf_thresh, Minimum species occurrence frequency threshold for location filter. Values in [0.01, 0.99]. Defaults to 0.03.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string? speciesFrequencyThreshold { get; set; } = "0.03";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        //--classifier, Path to custom trained classifier. Defaults to None. If set, --lat, --lon and --locale are ignored.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string? classifier { get; set; } = string.Empty;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public AnalyzerOptions(bool setWeek, bool? setThreads = true)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            //Set CPU threads:
            if (setThreads == true)
            {
                cpuThreads = Environment.ProcessorCount.ToString();
            }
            //Set weekOfYear:
            if (setWeek == true)
            {
                var time = DateTime.Today;
                DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
                if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
                {
                    time = DateTime.Today.AddDays(3);
                }

                // Return the week of our adjusted day
                weekOfTheYear = CultureInfo.InvariantCulture.Calendar
                    .GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)
                    .ToString();
            }
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public AnalyzerOptions() { }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
