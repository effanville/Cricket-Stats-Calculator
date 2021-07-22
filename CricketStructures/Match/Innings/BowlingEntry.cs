using System.Collections.Generic;
using System.Linq;
using CricketStructures.Player;
using Common.Structure.Extensions;
using Common.Structure.Validation;

namespace CricketStructures.Match.Innings
{
    public class BowlingEntry : IValidity
    {
        public PlayerName Name
        {
            get;
            set;
        }

        public double OversBowled
        {
            get;
            set;
        }

        public int Maidens
        {
            get;
            set;
        }

        public int RunsConceded
        {
            get;
            set;
        }

        public int Wickets
        {
            get;
            set;
        }

        public int Wides
        {
            get;
            set;
        }

        public int NoBalls
        {
            get;
            set;
        }

        public BowlingEntry(PlayerName name)
        {
            Name = name;
        }

        public BowlingEntry()
        {
        }

        public override string ToString()
        {
            if (Name != null)
            {
                return "Bowler-" + Name.ToString();
            }

            return "Bowler: No name";
        }

        public void SetBowling(double overs, int maidens, int runsConceded, int wickets, int wides = 0, int noBalls = 0)
        {
            OversBowled = overs;
            Maidens = maidens;
            RunsConceded = runsConceded;
            Wickets = wickets;
            Wides = wides;
            NoBalls = noBalls;
        }

        public bool Validate()
        {
            return !Validation().Any(validation => !validation.IsValid);
        }

        public List<ValidationResult> Validation()
        {
            List<ValidationResult> results = Name.Validation();
            results.AddIfNotNull(Validating.NotNegative(OversBowled, nameof(OversBowled), ToString()));
            results.AddIfNotNull(Validating.NotNegative(Maidens, nameof(Maidens), ToString()));
            results.AddIfNotNull(Validating.NotNegative(RunsConceded, nameof(RunsConceded), ToString()));
            results.AddIfNotNull(Validating.NotNegative(Wickets, nameof(Wickets), ToString()));
            results.AddIfNotNull(Validating.NotGreaterThan(Wickets, 10, nameof(Wickets), ToString()));
            return results;
        }
    }
}
