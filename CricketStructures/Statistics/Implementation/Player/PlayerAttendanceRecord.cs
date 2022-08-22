using System;
using System.Collections.Generic;
using System.Linq;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;

namespace CricketStructures.Statistics.Implementation.Player
{
    public sealed class PlayerAttendanceRecord : ICricketStat
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

        public int MatchesWon
        {
            get;
            set;
        }

        public int MatchesLost
        {
            get;
            set;
        }

        public int TotalMom
        {
            get;
            set;
        }

        public double WinRatio => Math.Round(MatchesWon / (double)MatchesPlayed, 2);

        public PlayerAttendanceRecord()
        {
            StartYear = DateTime.MaxValue;
            EndYear = DateTime.MinValue;
        }

        public PlayerAttendanceRecord(PlayerName name)
            : this()
        {
            Name = name;
        }

        public PlayerAttendanceRecord(PlayerName name, ICricketTeam team, MatchType[] matchTypes)
            : this(name)
        {
            Name = name;
            CalculateStats(team, matchTypes);
        }

        public PlayerAttendanceRecord(PlayerName name, string teamName, ICricketSeason season, MatchType[] matchTypes)
            : this(name)
        {
            Name = name;
            CalculateStats(teamName, season, matchTypes);
        }

        /// <inheritdoc/>
        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            CricketStatsHelpers.SeasonIterator(
                team.Seasons,
                season => CalculateStats(team.TeamName, season, matchTypes),
                preCycleAction: ResetStats);
        }

        /// <inheritdoc/>
        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            CricketStatsHelpers.MatchIterator(
                season,
                matchTypes,
                match => UpdateStats(teamName, match));
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

                MatchesPlayed += 1;
                if (match.MenOfMatch != null && match.MenOfMatch.Contains(Name))
                {
                    TotalMom += 1;
                }
                if (match.Result == ResultType.Win)
                {
                    MatchesWon += 1;
                }
                if (match.Result == ResultType.Loss)
                {
                    MatchesLost += 1;
                }
            }
        }

        /// <inheritdoc/>
        public void ResetStats()
        {
            MatchesWon = 0;
            MatchesPlayed = 0;
            MatchesLost = 0;
            TotalMom = 0;
            StartYear = DateTime.Today;
            EndYear = new DateTime();
        }

        /// <inheritdoc/>
        public void Finalise()
        {
        }

        /// <inheritdoc/>
        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            _ = rb.WriteTitle("Appearances", headerElement)
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

            headers.Add("Matches Played");
            headers.Add("Matches Won");
            headers.Add("Matches Lost");
            headers.Add("Total Mom");
            headers.Add("Win Ratio");

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

            values.Add(MatchesPlayed.ToString());
            values.Add(MatchesWon.ToString());
            values.Add(MatchesLost.ToString());
            values.Add(TotalMom.ToString());
            values.Add(WinRatio.ToString());

            return values;
        }
    }
}
