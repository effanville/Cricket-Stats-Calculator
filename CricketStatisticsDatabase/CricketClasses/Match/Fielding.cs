using System.Collections.Generic;
using System.Linq;
using Cricket.Player;
using Common.Structure.Extensions;
using Common.Structure.Validation;

namespace Cricket.Match
{
    public class Fielding : IValidity
    {
        public override string ToString()
        {
            if (MatchData != null)
            {
                return MatchData.ToString();
            }
            return "Fielding";
        }

        public MatchInfo MatchData
        {
            get;
            set;
        }

        /// <summary>
        /// list of players that play in this innings
        /// </summary>
        private List<FieldingEntry> fFielding = new List<FieldingEntry>();
        public List<FieldingEntry> FieldingInfo
        {
            get
            {
                return fFielding;
            }
            set
            {
                fFielding = value;
            }
        }

        public bool SetFielding(PlayerName player, int catches, int runOuts, int stumpings, int keeperCatches)
        {
            FieldingEntry result = FieldingInfo.Find(entry => entry.Name.Equals(player));
            if (result != null)
            {
                result.SetScores(catches, runOuts, stumpings, keeperCatches);
                return true;
            }

            return false;
        }

        public void AddPlayer(PlayerName player)
        {
            FieldingInfo.Add(new FieldingEntry(player));
        }

        public bool PlayerListed(PlayerName player)
        {
            return FieldingInfo.Any(card => card.Name.Equals(player));
        }

        public bool Remove(PlayerName player)
        {
            int removed = FieldingInfo.RemoveAll(card => card.Name.Equals(player));
            return removed == 1;
        }

        public Fielding Copy()
        {
            return new Fielding()
            {
                MatchData = MatchData,
                FieldingInfo = new List<FieldingEntry>(FieldingInfo)
            };
        }

        public bool Validate()
        {
            return !Validation().Any(validation => !validation.IsValid);
        }

        public List<ValidationResult> Validation()
        {
            List<ValidationResult> results = new List<ValidationResult>();
            int total = 0;
            foreach (FieldingEntry info in FieldingInfo)
            {
                results.AddValidations(info.Validation(), ToString());
                total += info.TotalDismissals();
            }

            results.AddIfNotNull(Validating.NotGreaterThan(total, 10, nameof(Fielding), ToString()));
            results.AddIfNotNull(Validating.NotGreaterThan(FieldingInfo.Count, 11, nameof(FieldingInfo), ToString()));
            return results;
        }

        public Fielding(MatchInfo info, List<PlayerName> playerNames)
        {
            MatchData = info;
            foreach (PlayerName name in playerNames)
            {
                FieldingInfo.Add(new FieldingEntry(name));
            }
        }
        public Fielding()
        {
        }
    }
}
