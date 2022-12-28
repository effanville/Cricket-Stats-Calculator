using System.Collections.Generic;
using System.Linq;
using CricketStructures.Player;
using Common.Structure.Extensions;
using Common.Structure.Validation;
using Common.Structure.ReportWriting;
using System.Xml.Serialization;
using System;
using Common.Structure.ReportWriting.Document;

namespace CricketStructures.Match.Innings
{
    public sealed class CricketInnings : IValidity, IEquatable<CricketInnings>
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

        [XmlElement]
        public Over OversFaced
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

        public void UpdateTeamName(string oldName, string newName)
        {
            if (string.Equals(BattingTeam, oldName))
            {
                BattingTeam = newName;
            }
            if (string.Equals(FieldingTeam, oldName))
            {
                BattingTeam = newName;
            }
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

        public IReadOnlyList<FieldingEntry> GetAllFielding(string teamName)
        {
            var fielding = new Dictionary<PlayerName, FieldingEntry>();
            if (string.Equals(FieldingTeam, teamName))
            {
                foreach (var batting in Batting)
                {
                    if (batting.MethodOut.IsFielderWicket())
                    {
                        if (fielding.TryGetValue(batting.Fielder, out var value))
                        {
                            value.UpdateEntry(batting.MethodOut, batting.WasKeeper);
                        }
                        else
                        {
                            var entry = new FieldingEntry(batting.Fielder);
                            entry.UpdateEntry(batting.MethodOut, batting.WasKeeper);
                            fielding.Add(batting.Fielder, entry);
                        }
                    }
                }
            }

            return fielding.Select(kvp => kvp.Value).ToList();
        }

        public FieldingEntry GetFielding(string team, PlayerName player)
        {
            if (string.Equals(FieldingTeam, team))
            {
                int numberCatches = 0;
                int numberRunOuts = 0;
                int numberStumpings = 0;
                int numberKeeperCatches = 0;
                foreach (var batting in Batting)
                {
                    if (batting.Fielder?.Equals(player) ?? false)
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

                var fielding = new FieldingEntry(player, numberCatches, numberRunOuts, numberStumpings, numberKeeperCatches);
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
            if (string.IsNullOrEmpty(team))
            {
                return new List<PlayerName>();
            }
            if (string.Equals(BattingTeam, team))
            {
                return Batting.Select(info => info.Name).ToList();
            }
            else if (string.Equals(FieldingTeam, team))
            {
                return Bowling.Select(info => info.Name).ToList();
            }
            else
            {
                return new List<PlayerName>();
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
            PlayerName player,
            Wicket howOut,
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

        public void SetBowling(string surname, string forename, Over overs, int maidens, int runsConceded, int wickets, int wides = 0, int noBalls = 0)
        {
            SetBowling(new PlayerName(surname, forename), overs, maidens, runsConceded, wickets, wides, noBalls);
        }

        public void SetBowling(PlayerName player, Over overs, int maidens, int runsConceded, int wickets, int wides = 0, int noBalls = 0)
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

        public Over OversPlayed()
        {
            Over numberOvers = Over.Zero();
            foreach (BowlingEntry bowler in Bowling)
            {
                numberOvers += bowler.OversBowled;
            }

            return  Bowling.Count == 0 ? Over.Unknown() : numberOvers;
        }

        public InningsScore Score()
        {
            var battingScore = BattingScore();
            var bowlingScore = BowlingScore();
            return InningsScore.Combine(battingScore, bowlingScore);
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

            return new InningsScore(runs, wickets, OversPlayed());
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

            return new InningsScore(runs, wickets, OversPlayed());
        }

        public BowlingEntry BowlingTotals()
        {
            int runs = 0;
            int wides = 0;
            int nb = 0;
            int wickets = 0;
            int maidens = 0;
            Over overs = default;
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

        private const string BattingSection = "Batting";
        private const string BowlingSection = "Bowling";
        private const string BowlingTotalsString = "Bowling Totals";
        private const string PartnershipsSection = "Partnerships";
        private const string ExtrasSection = "Extras";
        private const string ScoreSection = "Score";

        public static CricketInnings CreateFromScorecard(DocumentType exportType, string scorecard)
        {
            Document inningsDocument = ReportSplitter.SplitReportString(exportType, scorecard);
            return CreateFromScorecard(inningsDocument);
        }

        public static CricketInnings CreateFromScorecard(Document inningsDocument)
        {
            var innings = new CricketInnings();

            string title = inningsDocument.FirstTextPart(DocumentElement.h2).Text;
            string battingTeam = title.Split(":")[1].Trim('\n').Trim('\r').Trim('.').Trim();
            innings.BattingTeam = battingTeam;

            var battingSection = inningsDocument.GetSubDocumentFrom(part => part.ConstituentString.Contains(BattingSection));
            var battingData = battingSection.FirstTablePart();

            foreach (var batting in battingData.TableRows)
            {
                if (int.TryParse(batting[0], out int order))
                {
                    var data = GetMethodOut(batting[2]);
                    var bowler = PlayerName.FromString(batting[3]);
                    int runsScored = int.Parse(batting[4]);
                    innings.SetBatting(PlayerName.FromString(batting[1]), data.Item1, runsScored, order, 0, 0, data.fielder, data.wasKeeper, bowler);
                }
            }

            (Wicket, PlayerName fielder, bool wasKeeper) GetMethodOut(string methodOutString)
            {
                bool keeper = methodOutString.Contains(CricketConstants.WicketKeeperSymbol);
                int wicketStringEndIndex = methodOutString.IndexOf(' ');
                if (wicketStringEndIndex > 0)
                {
                    string wicketString = methodOutString.Substring(0, wicketStringEndIndex);
                    string nameString = methodOutString.Substring(wicketStringEndIndex + 1);
                    var wicketType = Enum.Parse<Wicket>(wicketString);
                    var name = PlayerName.FromString(nameString.Replace(CricketConstants.WicketKeeperSymbol, ""));
                    return (wicketType, name, keeper);
                }
                else
                {
                    var wicketType = Enum.Parse<Wicket>(methodOutString);
                    return (wicketType, new PlayerName(), keeper);
                }
            }

            var partnershipSection = inningsDocument.GetSubDocumentFrom(part => part.ConstituentString.Contains(PartnershipsSection));
            var partnershipData = partnershipSection.FirstTablePart();

            var fieldingSection = inningsDocument.GetSubDocumentFrom(part => part.ConstituentString.Contains(ExtrasSection) && part.Element != DocumentElement.table);
            var fieldingData = fieldingSection.FirstTablePart();

            foreach (var row in partnershipData.TableRows)
            {
                int wicket = int.Parse(row[0]);
                int runs = int.Parse(row[1]);
                int manOut = int.Parse(row[2]);

                innings.Batting[manOut - 1].WicketFellAt = wicket;
                innings.Batting[manOut - 1].TeamScoreAtWicket = runs;
            }

            int byes = GetExtraValue(fieldingData, "Byes");
            int legbyes = GetExtraValue(fieldingData, "Leg Byes");
            int wides = GetExtraValue(fieldingData, "Wides");
            int noBalls = GetExtraValue(fieldingData, "No Balls");
            int penalties = GetExtraValue(fieldingData, "Penalties");
            innings.SetExtras(byes, legbyes, wides, noBalls, penalties);

            int GetExtraValue(TableDocumentPart input, string extraType)
            {
                return int.Parse(input.TableRows.First(data => data[0].Contains(extraType))[1]);
            }


            var bowlingSection = inningsDocument.GetSubDocumentFrom(part => part.ConstituentString.Contains(BowlingSection));
            var bowlingData = bowlingSection.FirstTablePart();
            foreach (var bowling in bowlingData.TableRows)
            {
                Over overs = Over.FromString(bowling[3]);
                int maidens = int.Parse(bowling[4]);
                int runsConceded = int.Parse(bowling[5]);
                int wickets = int.Parse(bowling[6]);
                int bowlingWides = int.Parse(bowling[1]);
                int bowlingNoBalls = int.Parse(bowling[2]);
                var name = PlayerName.FromString(bowling[0]);
                if (bowling[0] != BowlingTotalsString)
                {
                    innings.SetBowling(name, overs, maidens, runsConceded, wickets, bowlingWides, bowlingNoBalls);
                }
            }

            return innings;
        }

        public ReportBuilder SerializeToString(DocumentType exportType)
        {
            ReportBuilder sb = new ReportBuilder(exportType, new ReportSettings(useColours: true, useDefaultStyle: false, useScripts: true));
            _ = sb.WriteTitle($"Innings of: {BattingTeam}.", DocumentElement.h2);

            _ = sb.WriteTitle(BattingSection, DocumentElement.h3);

            List<string> battingHeaders = new List<string>() { " ", "Batsman", "How Out", "Bowler", "Total" };
            List<List<string>> battingPerBatsman = new List<List<string>>();

            foreach (var batsman in Batting)
            {
                string keeperSymbol = batsman.WasKeeper ? CricketConstants.WicketKeeperSymbol : "";
                battingPerBatsman.Add(new List<string> { batsman.Order.ToString(), batsman.Name.ToString(), $"{batsman.MethodOut} {batsman.Fielder}{keeperSymbol}", batsman.Bowler?.ToString() ?? "", batsman.RunsScored.ToString() });
            }

            battingPerBatsman.Add(new List<string>() { "", "", "", "Batting Total", BatsmenRuns().ToString() });
            battingPerBatsman.Add(new List<string>() { "", "", "", "Total Extras ", InningsExtras.Runs().ToString() });
            battingPerBatsman.Add(new List<string>() { "", "", "", "Total", Score().Runs.ToString() });

            _ = sb.WriteTableFromEnumerable(battingHeaders, battingPerBatsman, false);

            _ = sb.WriteTitle(ExtrasSection, DocumentElement.h3);
            List<string> extrasHeaders = new List<string>() { "", "" };
            List<List<string>> extras = new List<List<string>>
            {
                new List<string>() { "Byes", InningsExtras.Byes.ToString() },
                new List<string>() { "Leg Byes", InningsExtras.LegByes.ToString() },
                new List<string>() { "Wides", InningsExtras.Wides.ToString() },
                new List<string>() { "No Balls", InningsExtras.NoBalls.ToString() },
                new List<string>() { "Penalties", InningsExtras.Penalties.ToString() },
                new List<string>() { "Total Extras", InningsExtras.Runs().ToString() }
            };

            _ = sb.WriteTableFromEnumerable(extrasHeaders, extras, headerFirstColumn: true);

            var partnerships = Partnerships();

            _ = sb.WriteTitle(PartnershipsSection, DocumentElement.h3);

            List<string> partnershipsHeaders = new List<string> { "Wicket", "FallOfWicket", "ManOut" };
            List<List<string>> partnershipsRows = new List<List<string>>();

            int partnershipIndex = 1;
            while (partnershipIndex < partnerships.Count)
            {
                List<string> partnershipsRow = new List<string>();
                var partnership = partnerships[partnershipIndex - 1];
                if (partnership.BatsmanOutAtEnd > 0)
                {
                    partnershipsRow.Add(partnershipIndex.ToString());
                    partnershipsRow.Add(partnership.TeamScoreAtEnd.ToString());
                    partnershipsRow.Add(partnership.BatsmanOutAtEnd.ToString());
                }

                partnershipsRows.Add(partnershipsRow);
                partnershipIndex++;
            }

            _ = sb.WriteTableFromEnumerable(partnershipsHeaders, partnershipsRows, false);


            _ = sb.WriteTitle(BowlingSection, DocumentElement.h3);
            List<string> bowlingHeaders = new List<string>() { "Bowler", "Wides", "NB", "Overs", "Mdns", "Runs", "Wkts", "Avg" };
            List<List<string>> bowlingColumns = new List<List<string>>();
            foreach (var bowler in Bowling)
            {
                bowlingColumns.Add(new List<string>
                {
                    bowler.Name.ToString(),
                    bowler.Wides.ToString(),
                    bowler.NoBalls.ToString(),
                    bowler.OversBowled.ToString(),
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
                    BowlingTotalsString,
                    bowlingTotals.Wides.ToString(),
                    bowlingTotals.NoBalls.ToString(),
                    bowlingTotals.OversBowled.ToString(),
                    bowlingTotals.Maidens.ToString(),
                    bowlingTotals.RunsConceded.ToString(),
                    bowlingTotals.Wickets.ToString(),
                    (bowlingTotals.RunsConceded / (double)bowlingTotals.Wickets).TruncateToString()
                });


            _ = sb.WriteTableFromEnumerable(bowlingHeaders, bowlingColumns, false);


            _ = sb.WriteTitle(ScoreSection, DocumentElement.h3);
            var score = Score();
            _ = sb.WriteParagraph(new[] { $"Final Score: {score.Runs} for {score.Wickets}" });

            return sb;
        }

        /// <inheritdoc/>
        public bool Equals(CricketInnings other)
        {
            return (BattingTeam ?? "").Equals(other.BattingTeam ?? "")
                && (FieldingTeam ?? "").Equals(other.FieldingTeam ?? "")
                && Enumerable.SequenceEqual(Batting, other.Batting)
                && Enumerable.SequenceEqual(Bowling, other.Bowling)
                && InningsExtras.Equals(other.InningsExtras);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return Equals(obj as CricketInnings);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BattingTeam, FieldingTeam, Batting, Bowling, InningsExtras);
        }
    }
}
