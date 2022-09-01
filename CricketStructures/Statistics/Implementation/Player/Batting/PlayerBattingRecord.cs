using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Season;
using CricketStructures.Player;
using System;
using Common.Structure.ReportWriting;
using System.Collections.Generic;

namespace CricketStructures.Statistics.Implementation.Player.Batting
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

        public PlayerBattingRecord(PlayerName name, ICricketTeam team, MatchType[] matchTypes)
            : this(name)
        {
            Name = name;
            CalculateStats(team, matchTypes);
        }

        public PlayerBattingRecord(PlayerName name, string teamName, ICricketSeason season, MatchType[] matchTypes)
            : this(name)
        {
            CalculateStats(teamName, season, matchTypes);
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
                if (match.MatchData.Date < StartYear)
                {
                    StartYear = match.MatchData.Date;
                }
                if (match.MatchData.Date > EndYear)
                {
                    EndYear = match.MatchData.Date;
                }

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
            _ = rb.WriteTitle("Batting Stats", headerElement)
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

            headers.Add("Innings");
            headers.Add("Not Out");
            headers.Add("Runs");
            headers.Add("Average");
            headers.Add("Runs Per Innings");
            headers.Add("Centuries");
            headers.Add("Fifties");
            headers.Add("Best");

            return headers;
        }

        public IReadOnlyList<string> Values(
            bool includeName,
            bool singleSeason,
            bool includeYear = true)
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

            values.Add(TotalInnings.ToString());
            values.Add(TotalNotOut.ToString());
            values.Add(TotalRuns.ToString());
            values.Add(Average.ToString());
            values.Add(RunsPerInnings.ToString());
            values.Add(Centuries.ToString());
            values.Add(Fifties.ToString());
            values.Add(Best?.ToString() ?? "");

            return values;
        }
    }
}
