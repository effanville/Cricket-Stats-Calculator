using System.Collections.Generic;
using System.Linq;
using CricketStructures.Player;
using StructureCommon.Extensions;
using StructureCommon.Validation;

namespace CricketStructures.Match.Innings
{
    public sealed class CricketInnings : IValidity
    {
        public string BattingTeam
        {
            get;
            set;
        }

        public string FieldingTeam
        {
            get;
            set;
        }

        public List<BattingEntry> Batting
        {
            get;
            set;
        }

        public List<BowlingEntry> Bowling
        {
            get;
            set;
        }

        public Extras InningsExtras
        {
            get;
            set;
        }

        public CricketInnings()
        {
            Batting = new List<BattingEntry>();
            Bowling = new List<BowlingEntry>();
        }

        public CricketInnings(string battingTeam, string fieldingTeam)
            : this()
        {
            BattingTeam = battingTeam;
            FieldingTeam = fieldingTeam;
        }

        public BattingEntry GetBatting(string team, PlayerName player)
        {
            if (BattingTeam.Equals(team))
            {
                return Batting.First(batsman => batsman.Name.Equals(player));
            }

            return null;
        }

        public bool IsBattingPlayer(PlayerName player)
        {
            return Batting.Any(card => card.Name.Equals(player));
        }

        public BowlingEntry GetBowling(string team, PlayerName player)
        {
            if (FieldingTeam.Equals(team))
            {
                return Bowling.First(batsman => batsman.Name.Equals(player));
            }

            return null;
        }

        public FieldingEntry GetFielding(string team, PlayerName player)
        {
            if (FieldingTeam.Equals(team))
            {
                int numberCatches = 0;
                int numberRunOuts = 0;
                int numberStumpings = 0;
                int numberKeeperCatches = 0;
                foreach (var batting in Batting)
                {
                    if (batting.Fielder.Equals(player))
                    {
                        if (batting.MethodOut == Wicket.Caught)
                        {
                            if (batting.WasKeeper)
                            {
                                numberKeeperCatches++;
                            }
                            else
                            {
                                numberCatches++;
                            }
                        }
                        if (batting.MethodOut == Wicket.RunOut)
                        {
                            numberRunOuts++;
                        }
                        if (batting.MethodOut == Wicket.Stumped)
                        {
                            numberStumpings++;
                        }
                    }
                }

                var fielding = new FieldingEntry(player);
                fielding.SetScores(numberCatches, numberRunOuts, numberStumpings, 0);
                return fielding;
            }

            return null;
        }

        public bool IsFieldingPlayer(PlayerName player)
        {
            bool players = Bowling.Any(card => card.Name.Equals(player));
            bool fielding = Batting.Any(card => card.Fielder?.Equals(player) ?? false);
            return players | fielding;
        }

        public List<PlayerName> Players(string team)
        {
            if (BattingTeam.Equals(team))
            {
                return Batting.Select(info => info.Name).ToList();
            }
            else if (FieldingTeam.Equals(team))
            {
                return Bowling.Select(info => info.Name).ToList();
            }
            else
            {
                return null;
            }
        }

        public void EditPlayerName(PlayerName oldName, PlayerName newName)
        {
            foreach (var entry in Batting)
            {
                if (entry.Name.Equals(oldName))
                {
                    entry.Name = newName;
                }
            }

            foreach (var entry in Bowling)
            {
                if (entry.Name.Equals(oldName))
                {
                    entry.Name = newName;
                }
            }
        }

        public void SetBatting(PlayerName player, Wicket howOut, int runs, int order, int wicketToFallAt, int teamScoreAtWicket, PlayerName fielder = null, bool wasKeeper = false, PlayerName bowler = null)
        {
            BattingEntry result = Batting.Find(entry => entry.Name.Equals(player));
            if (result == null)
            {
                result = new BattingEntry(player);
                Batting.Add(result);
            }

            result.SetScores(howOut, runs, order, wicketToFallAt, teamScoreAtWicket, fielder, wasKeeper, bowler);
            Batting.Sort((entry, entryOther) => entry.Order.CompareTo(entryOther.Order));
        }

        public bool DeleteBatting(PlayerName player)
        {
            return Batting.RemoveAll(item => item.Name.Equals(player)) != 0;
        }

        public void SetBowling(PlayerName player, double overs, int maidens, int runsConceded, int wickets, int wides = 0, int noBalls = 0)
        {
            BowlingEntry result = Bowling.Find(entry => entry.Name.Equals(player));
            if (result == null)
            {
                result = new BowlingEntry(player);
                Bowling.Add(result);
            }

            result.SetBowling(overs, maidens, runsConceded, wickets, wides, noBalls);
            Batting.Sort((entry, entryOther) => entry.Order.CompareTo(entryOther.Order));
        }

        public bool DeleteBowling(PlayerName player)
        {
            return Bowling.RemoveAll(item => item.Name.Equals(player)) != 0;
        }

        public InningsScore Score()
        {
            var battingScore = BattingScore();
            var bowlingScore = BowlingScore();
            if (!battingScore.Equals(bowlingScore))
            {
                return bowlingScore;
            }

            return battingScore;
        }

        public InningsScore BattingScore()
        {
            int runs = InningsExtras.Runs();
            int wickets = 0;
            foreach (BattingEntry batsman in Batting)
            {
                wickets += batsman.Out() ? 1 : 0;
                runs += batsman.RunsScored;
            }

            return new InningsScore(runs, wickets);
        }

        public InningsScore BowlingScore()
        {
            int runs = InningsExtras.NonBowlerRuns();
            int wickets = 0;
            foreach (BowlingEntry bowler in Bowling)
            {
                wickets += bowler.Wickets;
                runs += bowler.RunsConceded;
            }

            return new InningsScore(runs, wickets);
        }

        /// <summary>
        /// Calculate the partnerships of the team for this match.
        /// </summary>
        public List<Partnership> Partnerships()
        {
            InningsScore inningsScore = BattingScore();
            List<Partnership> partnerships = new List<Partnership>(new Partnership[10]);
            if (Batting.Count > 2)
            {
                BattingEntry batsmanOne = Batting[0];
                BattingEntry batsmanTwo = Batting[1];
                int nextBatsmanIndex = 2;
                int lastWicketScore = 0;
                int numberPartnerships = System.Math.Min(inningsScore.Wickets + 1, Batting.Count - 1);
                for (int i = 0; i < numberPartnerships; i++)
                {
                    Partnership partnership = new Partnership(batsmanOne.Name, batsmanTwo.Name);
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

                    if (nextBatsmanIndex < Batting.Count)
                    {
                        batsmanTwo = Batting[nextBatsmanIndex];
                    }

                    partnership.SetScores(i + 1, partnershipRuns);
                    partnerships[i] = partnership;
                    lastWicketScore += partnershipRuns;
                    nextBatsmanIndex++;
                }
            }

            return partnerships;
        }

        public bool Validate()
        {
            return !Validation().Any(validation => !validation.IsValid);
        }

        public List<ValidationResult> Validation()
        {
            List<ValidationResult> results = new List<ValidationResult>();
            // batting validations
            foreach (BattingEntry info in Batting)
            {
                results.AddValidations(info.Validation(), GetType().Name);
            }

            InningsScore teamResult = BattingScore();
            results.AddIfNotNull(Validating.NotGreaterThan(teamResult.Wickets, 10, nameof(teamResult.Wickets), ToString()));
            results.AddIfNotNull(Validating.NotGreaterThan(Batting.Count, 11, nameof(Batting), ToString()));


            // bowling validations
            int total = 0;
            foreach (BowlingEntry info in Bowling)
            {
                results.AddValidations(info.Validation(), ToString());
                total += info.Wickets;
            }

            results.AddIfNotNull(Validating.NotGreaterThan(total, 10, nameof(Bowling), ToString()));
            results.AddIfNotNull(Validating.NotGreaterThan(Bowling.Count, 11, nameof(Bowling), ToString()));

            results.AddValidations(InningsExtras.Validation(), ToString());
            return results;
        }
    }
}
