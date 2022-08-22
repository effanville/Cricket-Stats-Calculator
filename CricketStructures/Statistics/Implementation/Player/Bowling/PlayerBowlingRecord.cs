using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Season;
using CricketStructures.Player;
using System;
using Common.Structure.ReportWriting;
using System.Collections.Generic;

namespace CricketStructures.Statistics.Implementation.Player.Bowling
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

        public PlayerBowlingRecord(PlayerName name, ICricketTeam team, MatchType[] matchTypes)
            : this(name)
        {
            CalculateStats(team, matchTypes);
        }

        public PlayerBowlingRecord(PlayerName name, string teamName, ICricketSeason season, MatchType[] matchTypes)
            : this(name)
        {
            CalculateStats(teamName, season, matchTypes);
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
                if (match.MatchData.Date < StartYear)
                {
                    StartYear = match.MatchData.Date;
                }
                if (match.MatchData.Date > EndYear)
                {
                    EndYear = match.MatchData.Date;
                }

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

        public void Finalise()
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
            _ = rb.WriteTitle("Bowling Stats", headerElement)
                .WriteTableFromEnumerable(Headers(true, false), new[] { Values(true, false) }, headerFirstColumn: false);
        }

        public static IReadOnlyList<string> Headers(bool includeName, bool singleSeason, bool includeYear = true)
        {
            var headers = new List<string>();
            if (includeName)
            {
                headers.Add("Name");
            }

            if (includeYear)
            {
                if (singleSeason)
                {
                    headers.Add("Year");
                }
                else
                {
                    headers.Add("Start Year");
                    headers.Add("End Year");
                }
            }
            headers.Add("Overs");
            headers.Add("Maidens");
            headers.Add("Runs Conceded");
            headers.Add("Wickets");
            headers.Add("Average");
            headers.Add("Economy");
            headers.Add("Strike Rate");
            headers.Add("Best");

            return headers;
        }

        public IReadOnlyList<string> Values(bool includeName, bool singleSeason, bool includeYear = true)
        {
            var values = new List<string>();
            if (includeName)
            {
                values.Add(Name.ToString());
            }

            if (includeYear)
            {
                if (singleSeason)
                {
                    values.Add(StartYear.Year.ToString());
                }
                else
                {
                    values.Add(StartYear.Year.ToString());
                    values.Add(EndYear.Year.ToString());
                }
            }

            values.Add(TotalOvers.ToString());
            values.Add(TotalMaidens.ToString());
            values.Add(TotalRunsConceded.ToString());
            values.Add(TotalWickets.ToString());
            values.Add(Average.ToString());
            values.Add(Economy.ToString());
            values.Add(StrikeRate.ToString());
            values.Add(BestFigures.ToString());

            return values;
        }
    }
}
