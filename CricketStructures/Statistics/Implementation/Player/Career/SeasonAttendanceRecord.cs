using System;
using System.Collections.Generic;
using System.Linq;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;

namespace CricketStructures.Statistics.Implementation.Player
{
    public class SeasonAttendanceRecord : ICricketStat
    {
        public PlayerName Name
        {
            get;
            set;
        }

        public Dictionary<DateTime, PlayerAttendanceStatistics> SeasonAttendance
        {
            get;
        } = new Dictionary<DateTime, PlayerAttendanceStatistics>();

        public SeasonAttendanceRecord()
        {
        }

        public SeasonAttendanceRecord(PlayerName name)
        {
            Name = name;
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
            var attendance = new PlayerAttendanceStatistics(Name);
            attendance.CalculateStats(teamName, season, matchTypes);
            SeasonAttendance.Add(season.Year, attendance);
        }

        /// <inheritdoc/>
        public void UpdateStats(string teamName, ICricketMatch match)
        {
        }

        /// <inheritdoc/>
        public void ResetStats()
        {
            SeasonAttendance.Clear();
        }

        /// <inheritdoc/>
        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            _ = rb.WriteTitle("Yearly Records", headerElement);
            var yearRecords = SeasonAttendance
                .ToList()
                .OrderBy(value => value.Key)
                .Select(record => new List<string>()
                {
                    record.Key.Year.ToString(),
                    record.Value.TotalGamesPlayed.ToString(),
                    record.Value.TotalGamesWon.ToString(),
                    record.Value.TotalGamesLost.ToString(),
                    record.Value.TotalMom.ToString(),
                    record.Value.WinRatio.ToString()
                });
            _ = rb.WriteTableFromEnumerable(new string[] { "Year", "Played", "Won", "Lost", "MoM", "Win Ratio" }, yearRecords, headerFirstColumn: false);

        }
    }
}
