using Bird_Box.Audio;
using Bird_Box.Data;
using Bird_Box.Utilities;

namespace Bird_Box.Models
{
     public class AppDetails
     {
        private readonly BirdRepository _db;
         public List<Microphone> Microphones { get; private set; }
          public int ResultsInDb { get; private set; }
          public List<string> UniqueSpecies { get; private set; }
 
         public AppDetails(BirdRepository db)
         {
            _db = db;
            UniqueSpecies = new List<string>();
            UpdateDetails();
        }

         public async void UpdateDetails()
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
