using System.Collections.Generic;
using System.Linq;
using CricketStructures.Player;
using Common.Structure.Extensions;
using Common.Structure.Validation;
using System.Text;
using System;

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
            InningsExtras = new Extras();
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

        public bool IsBowlingPlayer(PlayerName player)
        {
            return Bowling.Any(card => card.Name.Equals(player));
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
                fielding.SetScores(numberCatches, numberRunOuts, numberStumpings, numberKeeperCatches);
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

        public void SetExtras(int byes, int legByes, int wides, int noBalls, int penalties = 0)
        {
            InningsExtras.SetExtras(byes, legByes, wides, noBalls, penalties);
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

        public int BatsmenRuns()
        {
            int runs = 0;
            foreach (BattingEntry batsman in Batting)
            {
                runs += batsman.RunsScored;
            }

            return runs;
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

        public StringBuilder SerializeToString()
        {
            StringBuilder sb = new StringBuilder();
            _ = sb.AppendLine($"Innings of: {BattingTeam}.")
                .AppendLine("-------------------------------------");

            int maxNameLength = Batting.Max(entry => (entry.Name?.PrimaryName?.Length ?? 0) + (entry.Name?.SecondaryName?.Length ?? 0) + 1);
            maxNameLength = Math.Max(maxNameLength, "Batsman".Length);

            int maxMethodOutLength = Batting.Max(entry => entry.MethodOut.ToString().Length + (entry.Fielder?.PrimaryName?.Length ?? 0) + (entry.Fielder?.SecondaryName?.Length ?? 0) + 2);
            maxMethodOutLength = Math.Max(maxMethodOutLength, "How Out".Length);

            int maxBowlerLength = Batting.Max(entry => (entry.Bowler?.PrimaryName?.Length ?? 0) + (entry.Bowler?.SecondaryName?.Length ?? 0) + 1);
            maxBowlerLength = Math.Max(maxBowlerLength, "Bowler".Length);

            int maxTotalLength = "Total".Length;
            int maxOrderLength = 4;

            string battingRowSeparator = $"| {"-".PadRight(maxNameLength + maxOrderLength + 1, '-')} | {"-".PadRight(maxMethodOutLength, '-')} | {"-".PadRight(maxBowlerLength, '-')} | {"-".PadRight(maxTotalLength, '-')} |";
            _ = sb.AppendLine(battingRowSeparator)
                .AppendLine($"| {"Batsman".PadRight(maxNameLength + maxOrderLength + 1) } | {"How Out".PadRight(maxMethodOutLength)} | {"Bowler".PadRight(maxBowlerLength)} | Total |")
                .AppendLine(battingRowSeparator);
            foreach (var batsman in Batting)
            {
                string methodOutString = $"{batsman.MethodOut} {batsman.Fielder}";
                _ = sb
                    .AppendLine($"| {batsman.Order.ToString().PadLeft(2)} | {batsman.Name.ToString().PadRight(maxNameLength)} | {methodOutString.PadRight(maxMethodOutLength)} | {(batsman.Bowler?.ToString() ?? "").PadRight(maxBowlerLength)} | {batsman.RunsScored.ToString().PadLeft(maxTotalLength)} |")
                .AppendLine(battingRowSeparator);
            }

            int offset = maxNameLength + maxOrderLength + maxMethodOutLength + maxBowlerLength - 7;
            string totalsRowSeparator = $"{"".PadRight(offset)} | {"-".PadRight(13, '-')} | {"-".PadRight(maxTotalLength, '-')} |";
            _ = sb.AppendLine(totalsRowSeparator)
                .AppendLine($"{"".PadRight(offset)} | Batting Total | {BatsmenRuns().ToString().PadLeft(maxTotalLength)} |")
                .AppendLine(totalsRowSeparator)
                .AppendLine($"{"".PadRight(offset)} | Total Extras  | {InningsExtras.Runs().ToString().PadLeft(maxTotalLength)} |")
                .AppendLine(totalsRowSeparator)
                .AppendLine($"{"".PadRight(offset)} | Total         | {Score().Runs.ToString().PadLeft(maxTotalLength)} |")
                .AppendLine(totalsRowSeparator)
                .AppendLine();

            int extrasMaxLength = "Total Extras".Length;
            string extrasRowSeparator = $"| {"".PadRight(extrasMaxLength, '-')} | --- |";
            _ = sb.AppendLine(extrasRowSeparator)
                .AppendLine($"| {"Byes".PadRight(extrasMaxLength)} | {InningsExtras.Byes.ToString().PadLeft(3)} |")
                .AppendLine(extrasRowSeparator)
                .AppendLine($"| {"Leg Byes".PadRight(extrasMaxLength)} | {InningsExtras.LegByes.ToString().PadLeft(3)} |")
                .AppendLine(extrasRowSeparator)
                .AppendLine($"| {"Wides".PadRight(extrasMaxLength)} | {InningsExtras.Wides.ToString().PadLeft(3)} |")
                .AppendLine(extrasRowSeparator)
                .AppendLine($"| {"No Balls".PadRight(extrasMaxLength)} | {InningsExtras.NoBalls.ToString().PadLeft(3)} |")
                .AppendLine(extrasRowSeparator)
                .AppendLine($"| {"Penalties".PadRight(extrasMaxLength)} | {InningsExtras.Penalties.ToString().PadLeft(3)} |")
                .AppendLine(extrasRowSeparator)
                .AppendLine($"| {"Total Extras".PadRight(extrasMaxLength)} | {InningsExtras.Runs().ToString().PadLeft(3)} |")
                .AppendLine(extrasRowSeparator)
                .AppendLine();

            var score = Score();
            string scoreString = $"Final Score: {score.Runs} for {score.Wickets}";
            int length = scoreString.Length;
            _ = sb.AppendLine($"{"".PadRight(offset)} | {"-".PadRight(length, '-')} |")
                .AppendLine($"{"".PadRight(offset)} | {scoreString} |")
                .AppendLine($"{"".PadRight(offset)} | {"-".PadRight(length, '-')} |")
                .AppendLine();

            int maxBowlerName = Bowling.Any() ? Bowling.Max(entry => entry?.Name?.ToString().Length ?? 0) : 0;
            maxBowlerName = Math.Max("Bowler".Length, maxBowlerName);
            string bowlingRowSeparator = $"| {"-".PadRight(maxBowlerName, '-')} | ----- | -- | ----- | ---- | ---- | ---- | ----- |";
            _ = sb.AppendLine(bowlingRowSeparator)
                .AppendLine($"| {"Bowler".PadRight(maxBowlerName)} | Wides | NB | Overs | Mdns | Runs | Wkts | Avg   |")
                .AppendLine(bowlingRowSeparator);
            foreach (var bowler in Bowling)
            {
                _ = sb.Append($"| {bowler.Name.ToString().PadRight(maxBowlerName)} |")
                    .Append($" {bowler.Wides.ToString().PadLeft(5)} |")
                    .Append($" {bowler.NoBalls.ToString().PadLeft(2)} |")
                    .Append($" {bowler.OversBowled.ToString().PadLeft(5)} |")
                    .Append($" {bowler.Maidens.ToString().PadLeft(4)} |")
                    .Append($" {bowler.RunsConceded.ToString().PadLeft(4)} |")
                    .Append($" {bowler.Wickets.ToString().PadLeft(4)} |")
                    .Append($" {(bowler.RunsConceded / (double)bowler.Wickets).TruncateToString().PadLeft(5)} |")
                    .AppendLine()
                    .AppendLine(bowlingRowSeparator);
            }

            return sb;
        }
    }
}
