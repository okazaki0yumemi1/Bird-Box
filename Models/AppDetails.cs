using Bird_Box.Audio;
using Bird_Box.Data;
using Bird_Box.Utilities;

namespace Bird_Box.Models
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class AppDetails
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        private readonly BirdRepository _db;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public List<Microphone> Microphones { get; private set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public int ResultsInDb { get; private set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public List<string> UniqueSpecies { get; private set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public AppDetails(BirdRepository db)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            _db = db;
            UniqueSpecies = new List<string>();
            UpdateDetails();
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public async void UpdateDetails()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            Microphones = CommandLine.GetAudioDevices();
            ResultsInDb = await _db.GetRecourdsCount();
            GetUniqueBirdsList();
        }

        private async void GetUniqueBirdsList()
        {
            //Request to a database
            var results = await _db.GetAll();
            //Get distinct values, return only species
            var birds = results.Select(x => x.birdName).Distinct().ToList();
            if (birds.Count == 0)
            {
                return;
            }
            //Merge
            UniqueSpecies.AddRange(birds);
            //Distinct entities
            UniqueSpecies = UniqueSpecies.Distinct().ToList();
        }
    }
}
