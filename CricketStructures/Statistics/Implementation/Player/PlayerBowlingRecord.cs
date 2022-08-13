using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Season;
using CricketStructures.Player;
using System;
using Common.Structure.ReportWriting;

namespace CricketStructures.Statistics.Implementation.Player
{
    public sealed class PlayerBowlingRecord : ICricketStat
    {
        public PlayerName Name
        {
            get;
            private set;
        }

        public DateTime StartYear
        {
            get;
            private set;
        }

        public DateTime EndYear
        {
            get;
            private set;
        }

        public int MatchesPlayed
        {
            get;
            set;
        }

        public Over TotalOvers
        {
            get;
            private set;
        }

        public int TotalMaidens
        {
            get;
            private set;
        }

        public int TotalRunsConceded
        {
            get;
            private set;
        }

        public int TotalWickets
        {
            get;
            private set;
        }

        public double Average
        {
            get;
            private set;
        }

        public double Economy
        {
            get;
            private set;
        }

        public double StrikeRate
        {
            get;
            private set;
        }

        public BowlingPerformance BestFigures
        {
            get;
            private set;
        }

        public PlayerBowlingRecord()
        {
            StartYear = DateTime.MaxValue;
            EndYear = DateTime.MinValue;
        }

        public PlayerBowlingRecord(PlayerName name)
            : this()
        {
            Name = name;
        }

        public PlayerBowlingRecord(PlayerName name, ICricketTeam team)
            : this(name)
        {
            CalculateStats(team, MatchHelpers.AllMatchTypes);
        }

        /// <inheritdoc/>
        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            CricketStatsHelpers.SeasonIterator(
                team.Seasons,
                season => CalculateStats(team.TeamName, season, matchTypes),
                ResetStats,
                Finalise);
        }

        /// <inheritdoc/>
        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            if (season.Year < StartYear)
            {
                StartYear = season.Year;
            }
            if (season.Year > EndYear)
            {
                EndYear = season.Year;
            }

            CricketStatsHelpers.MatchIterator(
                season,
                matchTypes,
                match => UpdateStats(teamName, match),
                postCycleAction: Finalise);
        }

        /// <inheritdoc/>
        public void UpdateStats(string teamName, ICricketMatch match)
        {
            if (match.Played(teamName, Name))
            {
                MatchesPlayed++;
                BowlingEntry bowling = match.GetBowling(teamName, Name);
                if (bowling != null)
                {
                    TotalOvers += bowling.OversBowled;
                    TotalMaidens += bowling.Maidens;
                    TotalRunsConceded += bowling.RunsConceded;
                    TotalWickets += bowling.Wickets;

                    BowlingPerformance possibleBest = new BowlingPerformance(teamName, bowling, match.MatchData);

                    if (possibleBest.CompareTo(BestFigures) > 0)
                    {
                        BestFigures = possibleBest;
                    }
                }
            }
        }

        private void Finalise()
        {
            if (TotalWickets != 0)
            {
                Average = Math.Round(TotalRunsConceded / (double)TotalWickets, 2);
                StrikeRate = Math.Round(6 * (double)TotalOvers / TotalWickets, 2);
            }
            else
            {
                Average = double.NaN;
                StrikeRate = double.NaN;
            }

            if (TotalOvers != Over.Min)
            {
                Economy = Math.Round(TotalRunsConceded / (double)TotalOvers, 2);
            }
            else
            {
                Economy = double.NaN;
            }
        }

        /// <inheritdoc/>
        public void ResetStats()
        {
            StartYear = DateTime.Today;
            EndYear = new DateTime();
            TotalOvers = Over.Min;
            TotalMaidens = 0;
            TotalRunsConceded = 0;
            TotalWickets = 0;
            BestFigures = new BowlingPerformance();
            Average = double.NaN;
            StrikeRate = double.NaN;
            Economy = double.NaN;
        }

        /// <inheritdoc/>
        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            var headers = new string[]
                {
                    "Name",
                    "Start Year",
                    "End Year",
                    "Overs",
                    "Maidens",
                    "Runs Conceded",
                    "Wickets",
                    "Average",
                    "Economy",
                    "Strike Rate",
                    "Best Figures"
                };
            var fields = new string[]
            {
                Name.ToString(),
                StartYear.Year.ToString(),
                EndYear.Year.ToString(),
                TotalOvers.ToString(),
                TotalMaidens.ToString(),
                TotalRunsConceded.ToString(),
                TotalWickets.ToString(),
                Average.ToString(),
                Economy.ToString(),
                StrikeRate.ToString(),
                BestFigures.ToString(),
            };
            _ = rb.WriteTitle("Bowling Stats", headerElement)
                .WriteTableFromEnumerable(headers, new[] { fields }, headerFirstColumn: false);
        }

        public static string[] PlayerHeaders => new string[] { "Year", "Overs", "Maidens", "Runs Conceded", "Wickets", "Average", "Economy", "Strike Rate", "Best Figures" };
        public static string[] Headers => new string[] { "Year", "Name", "Overs", "Maidens", "Runs Conceded", "Wickets", "Average", "Economy", "Strike Rate", "Best Figures" };


        public string[] ArrayValues()
        {
            return new string[]
                            {
                            StartYear.Year.ToString(),
                            Name.ToString(),
                            TotalOvers.ToString(),
                            TotalMaidens.ToString(),
                            TotalRunsConceded.ToString(),
                            TotalWickets.ToString(),
                            Average.ToString(),
                            Economy.ToString(),
                            StrikeRate.ToString(),
                            BestFigures?.ToString() ?? string.Empty,
                            };
        }

        public string[] PlayerArrayValues()
        {
            return new string[]
                            {
                            StartYear.Year.ToString(),
                            TotalOvers.ToString(),
                            TotalMaidens.ToString(),
                            TotalRunsConceded.ToString(),
                            TotalWickets.ToString(),
                            Average.ToString(),
                            Economy.ToString(),
                            StrikeRate.ToString(),
                            BestFigures?.ToString() ?? string.Empty,
                            };
        }
    }
}
