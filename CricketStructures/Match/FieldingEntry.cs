using System.Collections.Generic;
using System.Linq;
using Cricket.Interfaces;
using Cricket.Player;
using StructureCommon.Extensions;
using StructureCommon.Validation;

namespace Cricket.Match
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
            get
            {
                return keeperFielding.Stumpings;
            }
            set
            {
                keeperFielding.Stumpings = value;
            }
        }

        public int KeeperCatches
        {
            get
            {
                return keeperFielding.Catches;
            }
            set
            {
                keeperFielding.Catches = value;
            }
        }

        public WicketKeeperStats keeperFielding
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
            keeperFielding.SetScores(stumpings, keeperCatches);
        }

        public FieldingEntry(PlayerName name)
        {
            Name = name;
            keeperFielding = new WicketKeeperStats(name);
        }

        public FieldingEntry()
        {
            keeperFielding = new WicketKeeperStats();
        }

        public void SetSeasonStats(ICricketSeason season)
        {
            Catches = 0;
            RunOuts = 0;
            KeeperStumpings = 0;
            KeeperCatches = 0;
            foreach (ICricketMatch match in season.Matches)
            {
                FieldingEntry fielding = match.GetFielding(Name);
                if (fielding != null)
                {
                    Catches += fielding.Catches;
                    RunOuts += fielding.RunOuts;
                    KeeperCatches += fielding.KeeperCatches;
                    KeeperStumpings += fielding.KeeperStumpings;
                }
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
