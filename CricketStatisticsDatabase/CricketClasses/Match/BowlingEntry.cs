using Cricket.Player;
using ExtensionMethods;
using System.Collections.Generic;
using System.Linq;
using Validation;

namespace Cricket.Match
{
    public class BowlingEntry : IValidity
    {
        public PlayerName Name
        {
            get;
            set;
        }

        private int fOversBowled;
        public int OversBowled
        {
            get { return fOversBowled; }
            set { fOversBowled = value; }
        }

        private int fMaidens;
        public int Maidens
        {
            get { return fMaidens; }
            set { fMaidens = value; }
        }

        private int fRunsConceded;
        public int RunsConceded
        {
            get { return fRunsConceded; }
            set { fRunsConceded = value; }
        }

        private int fWickets;
        public int Wickets
        {
            get { return fWickets; }
            set { fWickets = value; }
        }

        public void SetBowling(int overs, int maidens, int runsConceded, int wickets)
        {
            OversBowled = overs;
            Maidens = maidens;
            RunsConceded = runsConceded;
            Wickets = wickets;
        }

        public bool Validate()
        {
            return !Validation().Any(validation => !validation.IsValid);
        }

        public List<ValidationResult> Validation()
        {
            var results = Name.Validation();
            results.AddIfNotNull(Validating.NotNegative(OversBowled, nameof(OversBowled)));
            results.AddIfNotNull(Validating.NotNegative(Maidens, nameof(Maidens)));
            results.AddIfNotNull(Validating.NotNegative(RunsConceded, nameof(RunsConceded)));
            results.AddIfNotNull(Validating.NotNegative(Wickets, nameof(Wickets)));
            results.AddIfNotNull(Validating.NotGreaterThan(Wickets, 10, nameof(Wickets)));
            return results;
        }

        public BowlingEntry(PlayerName name)
        {
            Name = name;
        }

        public BowlingEntry()
        { }
    }
}
