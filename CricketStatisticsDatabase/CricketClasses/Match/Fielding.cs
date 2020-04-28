using Cricket.Player;
using ExtensionMethods;
using System.Collections.Generic;
using System.Linq;
using Validation;

namespace Cricket.Match
{
    public class Fielding : IValidity
    {
        /// <summary>
        /// list of players that play in this innings
        /// </summary>
        private List<FieldingEntry> fFielding = new List<FieldingEntry>();
        public List<FieldingEntry> FieldingInfo
        {
            get { return fFielding; }
            set{ fFielding = value; }
        }

        public bool SetFielding(PlayerName player, int catches, int runOuts, int stumpings, int keeperCatches)
        {
            var result = FieldingInfo.Find(entry => entry.Name.Equals(player));
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
                FieldingInfo = new List<FieldingEntry>(this.FieldingInfo)
            };
        }

        public bool Validate()
        {
            return !Validation().Any(validation => !validation.IsValid);
        }

        public List<ValidationResult> Validation()
        {
            var results = new List<ValidationResult>();
            int total = 0;
            foreach (var info in FieldingInfo)
            {
                results.AddRange(info.Validation());
                total += info.TotalDismissals();
            }
            
            results.AddIfNotNull(Validating.NotGreaterThan(total, 10, nameof(Fielding)));
            results.AddIfNotNull(Validating.NotGreaterThan(FieldingInfo.Count, 11, nameof(FieldingInfo)));
            return results;
        }

        public Fielding(List<PlayerName> playerNames)
        {
            foreach (var name in playerNames)
            {
                FieldingInfo.Add(new FieldingEntry(name));
            }
        }
        public Fielding()
        {
        }
    }
}
