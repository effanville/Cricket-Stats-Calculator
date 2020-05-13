using Cricket.Player;
using Cricket.Statistics;
using StructureCommon.Extensions;
using StructureCommon.Validation;
using System.Collections.Generic;
using System.Linq;

namespace Cricket.Match
{
    public sealed class BattingInnings : IValidity
    {
        public override string ToString()
        {
            if (MatchData != null)
            {
                return MatchData.ToString();
            }
            return "BattingInnings";
        }

        public MatchInfo MatchData
        {
            get;
            set;
        }

        /// <summary>
        /// list of players that play in this innings
        /// </summary>
        private List<BattingEntry> fBattingInfo = new List<BattingEntry>();
        public List<BattingEntry> BattingInfo
        {
            get
            {
                return fBattingInfo;
            }
            set
            {
                fBattingInfo = value;
            }
        }
        private int fExtras;

        public int Extras
        {
            get
            {
                return fExtras;
            }
            set
            {
                fExtras = value;
            }
        }

        public bool SetScores(PlayerName player, Wicket howOut, int runs, int order, int wicketToFallAt, int teamScoreAtWicket, PlayerName fielder = null, PlayerName bowler = null)
        {
            var result = BattingInfo.Find(entry => entry.Name.Equals(player));
            if (result != null)
            {
                result.SetScores(howOut, runs, order, wicketToFallAt, teamScoreAtWicket, fielder, bowler);
                BattingInfo.Sort((entry, entryOther) => entry.Order.CompareTo(entryOther.Order));
                return true;
            }

            return false;
        }

        public void AddPlayer(PlayerName player)
        {
            BattingInfo.Add(new BattingEntry(player));
        }

        public void AddScore(PlayerName player, Wicket howOut, int runs, int order, int wicketToFallAt, int teamScoreAtWicket, PlayerName fielder = null, PlayerName bowler = null)
        {
            AddPlayer(player);
            SetScores(player, howOut, runs, order, wicketToFallAt, teamScoreAtWicket, fielder, bowler);
        }

        public bool PlayerListed(PlayerName player)
        {
            return BattingInfo.Any(card => card.Name.Equals(player));
        }

        public bool Remove(PlayerName player)
        {
            int removed = BattingInfo.RemoveAll(card => card.Name.Equals(player));
            return removed == 1;
        }

        public InningsScore Score()
        {
            int runs = Extras;
            int wickets = 0;
            foreach (var batsman in BattingInfo)
            {
                wickets += batsman.Out() ? 1 : 0;
                runs += batsman.RunsScored;
            }

            return new InningsScore(runs, wickets);
        }
        public BattingInnings Copy()
        {
            return new BattingInnings()
            {
                MatchData = MatchData,
                Extras = Extras,
                BattingInfo = new List<BattingEntry>(BattingInfo)
            };
        }

        public bool Validate()
        {
            return !Validation().Any(validation => !validation.IsValid);
        }

        public List<ValidationResult> Validation()
        {
            var results = new List<ValidationResult>();
            foreach (var info in BattingInfo)
            {
                results.AddValidations(info.Validation(), GetType().Name);
            }

            var teamResult = Score();
            results.AddIfNotNull(Validating.NotGreaterThan(teamResult.Wickets, 10, nameof(teamResult.Wickets), ToString()));
            results.AddIfNotNull(Validating.NotGreaterThan(BattingInfo.Count, 11, nameof(BattingInfo), ToString()));
            results.AddIfNotNull(Validating.NotNegative(Extras, nameof(Extras), ToString()));
            return results;
        }

        /// <summary>
        /// Calculate the partnerships of the team for this match.
        /// </summary>
        public List<Partnership> Partnerships()
        {
            var inningsScore = Score();
            var partnerships = new List<Partnership>(new Partnership[10]);
            if (BattingInfo.Count > 2)
            {
                var batsmanOne = BattingInfo[0];
                var batsmanTwo = BattingInfo[1];
                int nextBatsmanIndex = 2;
                int lastWicketScore = 0;
                int numberPartnerships = System.Math.Min(inningsScore.Wickets + 1, BattingInfo.Count - 1);
                for (int i = 0; i < numberPartnerships; i++)
                {
                    var partnership = new Partnership(batsmanOne.Name, batsmanTwo.Name, MatchData);
                    int partnershipRuns;
                    if (!batsmanOne.Out() && !batsmanTwo.Out())
                    {
                        partnershipRuns = inningsScore.Runs - lastWicketScore;
                    }
                    else if (!batsmanTwo.Out() && batsmanOne.Out())
                    {
                        partnershipRuns = batsmanOne.TeamScoreAtWicket - lastWicketScore;
                        batsmanOne = batsmanTwo;
                    }
                    else if (!batsmanOne.Out() && batsmanTwo.Out())
                    {
                        partnershipRuns = batsmanTwo.TeamScoreAtWicket - lastWicketScore;
                    }
                    else
                    {
                        if (batsmanOne.WicketFellAt < batsmanTwo.WicketFellAt)
                        {
                            partnershipRuns = batsmanOne.TeamScoreAtWicket - lastWicketScore;
                            batsmanOne = batsmanTwo;
                        }
                        else
                        {
                            partnershipRuns = batsmanTwo.TeamScoreAtWicket - lastWicketScore;
                        }
                    }

                    if (nextBatsmanIndex < BattingInfo.Count)
                    {
                        batsmanTwo = BattingInfo[nextBatsmanIndex];
                    }

                    partnership.SetScores(i + 1, partnershipRuns);
                    partnerships[i] = partnership;
                    lastWicketScore += partnershipRuns;
                    nextBatsmanIndex++;
                }
            }

            return partnerships;
        }

        public BattingInnings(MatchInfo info, List<PlayerName> playerNames)
        {
            MatchData = info;
            foreach (var name in playerNames)
            {
                BattingInfo.Add(new BattingEntry(name));
            }

            Extras = 0;
        }

        public BattingInnings()
        {
        }
    }
}
