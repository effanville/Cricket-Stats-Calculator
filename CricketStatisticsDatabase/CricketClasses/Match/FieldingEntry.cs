using Cricket.Interfaces;
using Cricket.Player;
using ExtensionMethods;
using System.Collections.Generic;
using System.Linq;
using Validation;

namespace Cricket.Match
{
    public class FieldingEntry : IValidity
    {
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
            get { return keeperFielding.Stumpings; }
            set { keeperFielding.Stumpings = value; } 
        }

        public int KeeperCatches
        {
            get {return keeperFielding.Catches; }
            set { keeperFielding.Catches = value; } 
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
            foreach (var match in season.Matches)
            {
                var fielding = match.GetFielding(Name);
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
            var results = Name.Validation();
            results.AddIfNotNull(Validating.NotNegative(Catches, nameof(Catches)));
            results.AddIfNotNull(Validating.NotNegative(RunOuts, nameof(RunOuts)));
            results.AddIfNotNull(Validating.NotNegative(KeeperStumpings, nameof(KeeperStumpings)));
            results.AddIfNotNull(Validating.NotNegative(KeeperCatches, nameof(KeeperCatches)));
            results.AddIfNotNull(Validating.NotGreaterThan(TotalDismissals(), 10, nameof(Match.FieldingEntry)));
            return results;
        }
    }
}
