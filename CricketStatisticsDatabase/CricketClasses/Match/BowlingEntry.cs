using Cricket.Player;
using StructureCommon.Extensions;
using StructureCommon.Validation;
using System.Collections.Generic;
using System.Linq;

namespace Cricket.Match
{
    public class BowlingEntry : IValidity
    {
        public override string ToString()
        {
            return "Bowler-" + Name.ToString();
        }

        public PlayerName Name
        {
            get;
            set;
        }

        private double fOversBowled;
        public double OversBowled
        {
            get
            {
                return fOversBowled;
            }
            set
            {
                fOversBowled = value;
            }
        }

        private int fMaidens;
        public int Maidens
        {
            get
            {
                return fMaidens;
            }
            set
            {
                fMaidens = value;
            }
        }

        private int fRunsConceded;
        public int RunsConceded
        {
            get
            {
                return fRunsConceded;
            }
            set
            {
                fRunsConceded = value;
            }
        }

        private int fWickets;
        public int Wickets
        {
            get
            {
                return fWickets;
            }
            set
            {
                fWickets = value;
            }
        }

        public void SetBowling(double overs, int maidens, int runsConceded, int wickets)
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
            results.AddIfNotNull(Validating.NotNegative(OversBowled, nameof(OversBowled), ToString()));
            results.AddIfNotNull(Validating.NotNegative(Maidens, nameof(Maidens), ToString()));
            results.AddIfNotNull(Validating.NotNegative(RunsConceded, nameof(RunsConceded), ToString()));
            results.AddIfNotNull(Validating.NotNegative(Wickets, nameof(Wickets), ToString()));
            results.AddIfNotNull(Validating.NotGreaterThan(Wickets, 10, nameof(Wickets), ToString()));
            return results;
        }

        public BowlingEntry(PlayerName name)
        {
            Name = name;
        }

        public BowlingEntry()
        {
        }
    }
}
