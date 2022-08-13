using System.Collections.Generic;
using System.Linq;
using CricketStructures.Player;
using Common.Structure.Extensions;
using Common.Structure.Validation;

namespace CricketStructures.Match.Innings
{
    public class FieldingEntry : IValidity
    {
        public override string ToString()
        {
            if (Name != null)
            {

                return "Fielder-" + Name.ToString();
            }
            return "Fielder: No name";
        }

        public PlayerName Name
        {
            get;
            set;
        }

        public int Catches
        {
            get;
            set;
        }

        public int RunOuts
        {
            get;
            set;
        }

        public int KeeperStumpings
        {
            get;
            set;
        }

        public int KeeperCatches
        {
            get;
            set;
        }

        public int TotalDismissals()
        {
            return Catches + RunOuts + KeeperCatches + KeeperStumpings;
        }

        public int TotalKeeperDismissals()
        {
            return KeeperCatches + KeeperStumpings;
        }

        public int TotalNonKeeperDismissals()
        {
            return Catches + RunOuts;
        }

        public void SetScores(int catches, int runOuts, int stumpings, int keeperCatches)
        {
            Catches = catches;
            RunOuts = runOuts;
            KeeperStumpings = stumpings;
            KeeperCatches = keeperCatches;
        }
        public FieldingEntry()
        {
        }

        public FieldingEntry(PlayerName name)
            : this()
        {
            Name = name;
        }

        public FieldingEntry(PlayerName name, int catches, int runOuts, int stumpings, int keeperCatches)
            : this(name)
        {
            SetScores(catches, runOuts, stumpings, keeperCatches);
        }



        public void UpdateEntry(Wicket wicket, bool wasKeeper)
        {
            if (wicket == Wicket.Caught)
            {
                if (wasKeeper)
                {
                    KeeperCatches++;
                }
                else
                {
                    Catches++;
                }
            }
            if (wicket == Wicket.RunOut)
            {
                RunOuts++;
            }

            if (wicket == Wicket.Stumped)
            {
                KeeperStumpings++;
            }
        }

        public bool Validate()
        {
            return !Validation().Any(validation => !validation.IsValid);
        }

        public List<ValidationResult> Validation()
        {
            List<ValidationResult> results = Name.Validation();
            results.AddIfNotNull(Validating.NotNegative(Catches, nameof(Catches), ToString()));
            results.AddIfNotNull(Validating.NotNegative(RunOuts, nameof(RunOuts), ToString()));
            results.AddIfNotNull(Validating.NotNegative(KeeperStumpings, nameof(KeeperStumpings), ToString()));
            results.AddIfNotNull(Validating.NotNegative(KeeperCatches, nameof(KeeperCatches), ToString()));
            results.AddIfNotNull(Validating.NotGreaterThan(TotalDismissals(), 10, nameof(FieldingEntry), ToString()));
            return results;
        }
    }
}
