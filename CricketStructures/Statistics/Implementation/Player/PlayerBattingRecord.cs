using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Season;
using CricketStructures.Player;
using System;
using Common.Structure.ReportWriting;
using System.Collections.Generic;

namespace CricketStructures.Statistics.Implementation.Player
{
    public sealed class PlayerBattingRecord : ICricketStat
    {
        public PlayerName Name
        {
            get;
            set;
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

        public int TotalInnings
        {
            get;
            set;
        }

        public int TotalNotOut
        {
            get;
            set;
        }

        public int TotalRuns
        {
            get;
            set;
        }

        public double Average
        {
            get;
            set;
        }

        public double RunsPerInnings
        {
            get;
            set;
        }
        public int Centuries
        {
            get;
            set;
        }

        public int Fifties
        {
            get;
            set;
        }

        private IDictionary<Wicket, int> WicketLossNumbers
        {
            get;
            set;
        } = new Dictionary<Wicket, int>(Enum.GetValues(typeof(Wicket)).Length);

        public PlayerScore Best
        {
            get;
            set;
        }

        public PlayerBattingRecord()
        {
            StartYear = DateTime.MaxValue;
            EndYear = DateTime.MinValue;
        }

        public PlayerBattingRecord(PlayerName name)
            : this()
        {
            Name = name;
        }

        public PlayerBattingRecord(PlayerName name, ICricketTeam team)
            : this(name)
        {
            Name = name;
            CalculateStats(team, MatchHelpers.AllMatchTypes);
        }

        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            CricketStatsHelpers.SeasonIterator(
                team.Seasons,
                season => CalculateStats(team.TeamName, season, matchTypes),
                ResetStats,
                Finalise);
        }

        public void Finalise()
        {
            if (TotalInnings != TotalNotOut)
            {
                Average = Math.Round(TotalRuns / (TotalInnings - (double)TotalNotOut), 2);
            }

            RunsPerInnings = Math.Round((double)TotalRuns / TotalInnings, 2);
        }

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

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            if (match.Played(teamName, Name))
            {
                MatchesPlayed++;
                BattingEntry batting = match.GetBatting(teamName, Name);
                if (batting != null)
                {
                    if (batting.MethodOut.DidBat())
                    {
                        TotalInnings++;
                        if (!batting.Out())
                        {
                            TotalNotOut++;
                        }

                        if (WicketLossNumbers.ContainsKey(batting.MethodOut))
                        {
                            WicketLossNumbers[batting.MethodOut] += 1;
                        }
                        else
                        {
                            WicketLossNumbers[batting.MethodOut] = 1;
                        }
                        TotalRuns += batting.RunsScored;

                        if (batting.RunsScored >= 50 && batting.RunsScored < 100)
                        {
                            Fifties++;
                        }
                        if (batting.RunsScored >= 100)
                        {
                            Centuries++;
                        }

                        PlayerScore possibleBest = new PlayerScore(
                            teamName,
                            batting,
                            match.MatchData,
                            match.GetInnings(teamName, batting: true).BattingScore());


                        if (possibleBest.CompareTo(Best) > 0)
                        {
                            Best = possibleBest;
                        }
                    }
                }
            }
        }

        public void ResetStats()
        {
            MatchesPlayed = 0;
            TotalInnings = 0;
            TotalNotOut = 0;
            TotalRuns = 0;
            Best = new PlayerScore();
            StartYear = DateTime.Today;
            EndYear = new DateTime();
        }

        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            var headers = new string[]
            {
                "Name",
                "Start Year",
                "End Year",
                "Matches Played",
                "Innings",
                "Not Out",
                "Runs",
                "Best",
                "Average",
                "Centuries",
                "Fifties"
            };
            var fields = new string[]
            {
                Name.ToString(),
                StartYear.Year.ToString(),
                EndYear.Year.ToString(),
                MatchesPlayed.ToString(),
                TotalInnings.ToString(),
                TotalNotOut.ToString(),
                TotalRuns.ToString(),
                Average.ToString(),
                Best.ToString(),
                Centuries.ToString(),
                Fifties.ToString()
            };

            _ = rb.WriteTitle("Batting Stats", headerElement)
                .WriteTableFromEnumerable(headers, new[] { fields }, headerFirstColumn: false);
        }

        public static string[] PlayerHeaders => new string[] { "Year", "Innings", "Not Out", "Runs", "Average", "Runs Per Innings" };
        public static string[] Headers => new string[] { "Year", "Name", "Innings", "Not Out", "Runs", "Average", "Runs Per Innings" };


        public string[] PlayerArrayValues()
        {
            return new string[]
                    {
                        StartYear.Year.ToString(),
                        TotalInnings.ToString(),
                        TotalNotOut.ToString(),
                        TotalRuns.ToString(),
                        Average.ToString(),
                        RunsPerInnings.ToString()
                    };
        }

        public string[] ArrayValues()
        {
            return new string[]
                    {
                        StartYear.Year.ToString(),
                        Name.ToString(),
                        TotalInnings.ToString(),
                        TotalNotOut.ToString(),
                        TotalRuns.ToString(),
                        Average.ToString(),
                        RunsPerInnings.ToString()
                    };
        }
    }
}
