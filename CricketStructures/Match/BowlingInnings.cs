using System.Collections.Generic;
using System.Linq;
using Cricket.Player;
using StructureCommon.Extensions;
using StructureCommon.Validation;

namespace Cricket.Match
{
    public class BowlingInnings : IValidity
    {
        public override string ToString()
        {
            if (MatchData != null)
            {
                return MatchData.ToString();
            }
            return "BowlingInnings";
        }

        public MatchInfo MatchData
        {
            get;
            set;
        }

        /// <summary>
        /// list of players that play in this innings
        /// </summary>
        public List<BowlingEntry> BowlingInfo
        {
            get;
            set;
        }

        public int ByesLegByes
        {
            get;
            set;
        }

        public bool SetScores(PlayerName player, double overs, int maidens, int runsConceded, int wickets)
        {
            BowlingEntry result = BowlingInfo.Find(entry => entry.Name.Equals(player));
            if (result != null)
            {
                result.SetBowling(overs, maidens, runsConceded, wickets);
                return true;
            }

            return false;
        }
        public void AddPlayer(PlayerName player)
        {
            BowlingInfo.Add(new BowlingEntry(player));
        }

        public List<PlayerName> Players()
        {
            return BowlingInfo.Select(info => info.Name).ToList();
        }

        public bool PlayerListed(PlayerName player)
        {
            return BowlingInfo.Any(card => card.Name.Equals(player));
        }

        public bool Remove(PlayerName player)
        {
            int removed = BowlingInfo.RemoveAll(card => card.Name.Equals(player));
            return removed == 1;
        }

        public InningsScore Score()
        {
            int runs = ByesLegByes;
            int wickets = 0;
            foreach (BowlingEntry bowler in BowlingInfo)
            {
                wickets += bowler.Wickets;
                runs += bowler.RunsConceded;
            }

            return new InningsScore(runs, wickets);
        }

        public BowlingInnings(MatchInfo info, List<PlayerName> playerNames)
        {
            MatchData = info;
            foreach (PlayerName name in playerNames)
            {
                BowlingInfo.Add(new BowlingEntry(name));
            }
        }

        public BowlingInnings Copy()
        {
            return new BowlingInnings()
            {
                MatchData = MatchData,
                ByesLegByes = ByesLegByes,
                BowlingInfo = new List<BowlingEntry>(BowlingInfo)
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
            foreach (BowlingEntry info in BowlingInfo)
            {
                results.AddValidations(info.Validation(), ToString());
                total += info.Wickets;
            }

            results.AddIfNotNull(Validating.NotGreaterThan(total, 10, nameof(BowlingInnings), ToString()));
            results.AddIfNotNull(Validating.NotGreaterThan(BowlingInfo.Count, 11, nameof(BowlingInfo), ToString()));
            results.AddIfNotNull(Validating.NotNegative(ByesLegByes, nameof(ByesLegByes), ToString()));
            return results;
        }

        /// <summary>
        /// Standard default generator
        /// </summary>
        public BowlingInnings()
        {
        }
    }
}
