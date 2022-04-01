using System.Collections.Generic;
using System.Linq;
using CricketStructures.Player;
using Common.Structure.Extensions;
using Common.Structure.Validation;
using System.Text;
using System;
using Common.Structure.ReportWriting;
using Common.Structure.FileAccess;
using System.Xml.Serialization;

namespace CricketStructures.Match.Innings
{
    public sealed class CricketInnings : IValidity
    {
        [XmlAttribute(AttributeName = "B")]
        public string BattingTeam
        {
            get;
            set;
        }

        [XmlAttribute(AttributeName = "F")]
        public string FieldingTeam
        {
            get;
            set;
        }

        [XmlArray]
        public List<BattingEntry> Batting
        {
            get;
            set;
        }

        [XmlArray]
        public List<BowlingEntry> Bowling
        {
            get;
            set;
        }

        [XmlElement]
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

        public void DidNotBat(string surname, string forename, int order)
        {
            SetBatting(surname, forename, Wicket.DidNotBat, 0, order, 0, 0);
        }

        public void SetBatting(
            string surname,
            string forename,
            Wicket howOut,
            int runs,
            int order,
            int wicketToFallAt,
            int teamScoreAtWicket,
            string fielderSurname = null,
            string fielderForename = null,
            bool wasKeeper = false,
            string bowlerSurname = null,
            string bowlerForename = null)
        {
            SetBatting(
                new PlayerName(surname, forename),
                howOut,
                runs,
                order,
                wicketToFallAt,
                teamScoreAtWicket,
                new PlayerName(fielderSurname, fielderForename),
                wasKeeper,
                new PlayerName(bowlerSurname, bowlerForename));
        }

        public void SetBatting(
            PlayerName player, Wicket howOut,
            int runs,
            int order,
            int wicketToFallAt,
            int teamScoreAtWicket,
            PlayerName fielder = null,
            bool wasKeeper = false,
            PlayerName bowler = null)
        {
            BattingEntry result = Batting.Find(entry => entry.Name.Equals(player));
            if (result == null)
            {
                result = new BattingEntry(player);
                Batting.Add(result);
            }

            result.SetScores(howOut, runs, order, wicketToFallAt, teamScoreAtWicket, fielder, wasKeeper, bowler);
            Batting.Sort((entry, entryOther) => OrderComparer(entry.Order, entryOther.Order));
        }

        private static int OrderComparer(int entry, int second)
        {
            if (entry <= 0)
            {
                return 1;
            }
            else if (second <= 0)
            {
                return -1;
            }
            else
            {
                return entry.CompareTo(second);
            }
        }

        public void SetExtras(int byes, int legByes, int wides, int noBalls, int penalties = 0)
        {
            InningsExtras.SetExtras(byes, legByes, wides, noBalls, penalties);
        }

        public bool DeleteBatting(PlayerName player)
        {
            return Batting.RemoveAll(item => item.Name.Equals(player)) != 0;
        }

        public void SetBowling(string surname, string forename, double overs, int maidens, int runsConceded, int wickets, int wides = 0, int noBalls = 0)
        {
            SetBowling(new PlayerName(surname, forename), overs, maidens, runsConceded, wickets, wides, noBalls);
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
        }

        public bool DeleteBowling(PlayerName player)
        {
            return Bowling.RemoveAll(item => item.Name.Equals(player)) != 0;
        }

        public InningsScore Score()
        {
            var battingScore = BattingScore();
            var bowlingScore = BowlingScore();
            int comparison = battingScore.CompareTo(bowlingScore);

            return comparison < 0 ? bowlingScore : battingScore;
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

        public BowlingEntry BowlingTotals()
        {
            int runs = 0;
            int wides = 0;
            int nb = 0;
            int wickets = 0;
            int maidens = 0;
            double overs = 0;
            foreach (BowlingEntry bowler in Bowling)
            {
                overs += bowler.OversBowled;
                maidens += bowler.Maidens;
                runs += bowler.RunsConceded;
                wickets += bowler.Wickets;
                wides += bowler.Wides;
                nb += bowler.NoBalls;
            }
            var bowlingTotals = new BowlingEntry(new PlayerName("Totals", "Bowling"));
            bowlingTotals.SetBowling(overs, maidens, runs, wickets, wides, nb);
            return bowlingTotals;
        }

        /// <summary>
        /// Calculate the partnerships of the team for this match.
        /// </summary>
        public List<Partnership> Partnerships()
        {
            InningsScore inningsScore = BattingScore();
            List<Partnership> partnerships = new List<Partnership>();
            if (Batting.Count > 2)
            {
                BattingEntry batsmanOne = Batting[0];
                BattingEntry batsmanTwo = Batting[1];
                int nextBatsmanIndex = 2;
                int lastWicketScore = 0;
                int numberPartnerships = System.Math.Min(inningsScore.Wickets + 1, Batting.Count - 1);
                for (int partnershipIndex = 0; partnershipIndex < numberPartnerships; partnershipIndex++)
                {
                    Partnership partnership = new Partnership(batsmanOne.Name, batsmanTwo.Name);
                    int partnershipRuns;
                    int teamScoreAtEnd;
                    int batsmanOut;
                    if (!batsmanOne.Out() && !batsmanTwo.Out())
                    {
                        partnershipRuns = inningsScore.Runs - lastWicketScore;
                        teamScoreAtEnd = inningsScore.Runs;
                        batsmanOut = -1;
                    }
                    else if (!batsmanTwo.Out() && batsmanOne.Out())
                    {
                        partnershipRuns = batsmanOne.TeamScoreAtWicket - lastWicketScore;
                        teamScoreAtEnd = batsmanOne.TeamScoreAtWicket;
                        batsmanOut = batsmanOne.Order;
                        batsmanOne = batsmanTwo;
                    }
                    else if (!batsmanOne.Out() && batsmanTwo.Out())
                    {
                        partnershipRuns = batsmanTwo.TeamScoreAtWicket - lastWicketScore;
                        teamScoreAtEnd = batsmanTwo.TeamScoreAtWicket;
                        batsmanOut = batsmanTwo.Order;
                    }
                    else
                    {
                        if (batsmanOne.WicketFellAt < batsmanTwo.WicketFellAt)
                        {
                            partnershipRuns = batsmanOne.TeamScoreAtWicket - lastWicketScore;
                            teamScoreAtEnd = batsmanOne.TeamScoreAtWicket;
                            batsmanOut = batsmanOne.Order;
                            batsmanOne = batsmanTwo;
                        }
                        else
                        {
                            partnershipRuns = batsmanTwo.TeamScoreAtWicket - lastWicketScore;
                            teamScoreAtEnd = batsmanTwo.TeamScoreAtWicket;
                            batsmanOut = batsmanTwo.Order;
                        }
                    }

                    if (nextBatsmanIndex < Batting.Count)
                    {
                        batsmanTwo = Batting[nextBatsmanIndex];
                    }

                    partnership.SetScores(partnershipIndex + 1, partnershipRuns, teamScoreAtEnd, batsmanOut);
                    partnerships.Add(partnership);
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

        private static string ToOversString(double overs)
        {
            double numberBalls = overs * 6;
            _ = int.TryParse(numberBalls.ToString(), out int numBalls);
            double wholeOvers = Math.DivRem(numBalls, 6, out int result);
            return $"{wholeOvers}.{result}";
        }

        public static CricketInnings CreateFromScorecard(string scorecard)
        {
            return null;
        }

        public StringBuilder SerializeToString(ExportType exportType)
        {
            StringBuilder sb = new StringBuilder();
            TextWriting.WriteTitle(sb, exportType, $"Innings of: {BattingTeam}.", HtmlTag.h2);

            TextWriting.WriteTitle(sb, exportType, $"Batting", HtmlTag.h3);

            List<string> battingHeaders = new List<string>() { "", "Batsman", "How Out", "Bowler", "Total" };
            List<List<string>> battingPerBatsman = new List<List<string>>();

            foreach (var batsman in Batting)
            {
                string keeperSymbol = batsman.WasKeeper ? CricketConstants.WicketKeeperSymbol : "";
                battingPerBatsman.Add(new List<string> { batsman.Order.ToString(), batsman.Name.ToString(), $"{batsman.MethodOut} {batsman.Fielder}{keeperSymbol}", batsman.Bowler?.ToString() ?? "", batsman.RunsScored.ToString() });
            }

            battingPerBatsman.Add(new List<string>() { "", "", "", "Batting Total", BatsmenRuns().ToString() });
            battingPerBatsman.Add(new List<string>() { "", "", "", "Total Extras ", InningsExtras.Runs().ToString() });
            battingPerBatsman.Add(new List<string>() { "", "", "", "Total", Score().Runs.ToString() });

            TableWriting.WriteTableFromEnumerable(sb, exportType, battingHeaders, battingPerBatsman, false);

            List<string> extrasHeaders = new List<string>() { "", "" };
            List<List<string>> extras = new List<List<string>>();
            extras.Add(new List<string>() { "Byes", InningsExtras.Byes.ToString() });
            extras.Add(new List<string>() { "Leg Byes", InningsExtras.LegByes.ToString() });
            extras.Add(new List<string>() { "Wides", InningsExtras.Wides.ToString() });
            extras.Add(new List<string>() { "No Balls", InningsExtras.NoBalls.ToString() });
            extras.Add(new List<string>() { "Penalties", InningsExtras.Penalties.ToString() });
            extras.Add(new List<string>() { "Total Extras", InningsExtras.Runs().ToString() });

            TableWriting.WriteTableFromEnumerable(sb, exportType, extrasHeaders, extras, headerFirstColumn: true);

            var partnerships = Partnerships();

            TextWriting.WriteTitle(sb, exportType, $"Partnerships", HtmlTag.h3);

            List<string> partnershipsHeaders = new List<string>();
            List<string> partnershipsRow = new List<string>();
            int partnershipIndex = 1;
            while (partnershipIndex < partnerships.Count)
            {
                partnershipsHeaders.Add($" {partnershipIndex.ToString().PadLeft(2)} For");
                partnershipsHeaders.Add("ManOut");
                var partnership = partnerships[partnershipIndex - 1];
                if (partnership.BatsmanOutAtEnd > 0)
                {
                    partnershipsRow.Add(partnership.TeamScoreAtEnd.ToString());
                    partnershipsRow.Add(partnership.BatsmanOutAtEnd.ToString());
                }
                partnershipIndex++;
            }

            TableWriting.WriteTableFromEnumerable(sb, exportType, partnershipsHeaders, new List<List<string>> { partnershipsRow }, false);


            TextWriting.WriteTitle(sb, exportType, $"Bowling", HtmlTag.h3);
            List<string> bowlingHeaders = new List<string>() { "Bowler", "Wides", "NB", "Overs", "Mdns", "Runs", "Wkts", "Avg" };
            List<List<string>> bowlingColumns = new List<List<string>>();
            foreach (var bowler in Bowling)
            {
                bowlingColumns.Add(new List<string>
                {
                    bowler.Name.ToString(),
                    bowler.Wides.ToString(),
                    bowler.NoBalls.ToString(),
                    ToOversString(bowler.OversBowled),
                    bowler.Maidens.ToString(),
                    bowler.RunsConceded.ToString(),
                    bowler.Wickets.ToString(),
                    (bowler.RunsConceded / (double)bowler.Wickets).TruncateToString()
                });
            }

            var bowlingTotals = BowlingTotals();
            bowlingColumns.Add(
                new List<string>
                {
                    "Bowling Totals",
                    bowlingTotals.Wides.ToString(),
                    bowlingTotals.NoBalls.ToString(),
                    ToOversString(bowlingTotals.OversBowled),
                    bowlingTotals.Maidens.ToString(),
                    bowlingTotals.RunsConceded.ToString(),
                    bowlingTotals.Wickets.ToString(),
                    (bowlingTotals.RunsConceded / (double)bowlingTotals.Wickets).TruncateToString()
                });


            TableWriting.WriteTableFromEnumerable(sb, exportType, bowlingHeaders, bowlingColumns, false);


            TextWriting.WriteTitle(sb, exportType, $"Score", HtmlTag.h3);
            var score = Score();
            TextWriting.WriteParagraph(sb, exportType, new[] { $"Final Score: {score.Runs} for {score.Wickets}" });

            return sb;
        }
    }
}
