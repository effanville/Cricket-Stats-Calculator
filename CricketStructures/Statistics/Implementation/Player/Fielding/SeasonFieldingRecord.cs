using System;
using System.Collections.Generic;
using System.Linq;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;

namespace CricketStructures.Statistics.Implementation.Player.Fielding
{
    public class SeasonFieldingRecord : ICricketStat
    {
        public PlayerName Name
        {
            get;
            set;
        }

        public Dictionary<DateTime, PlayerFieldingStatistics> SeasonFielding
        {
            get;
        } = new Dictionary<DateTime, PlayerFieldingStatistics>();

        public SeasonFieldingRecord()
        {
        }

        public SeasonFieldingRecord(PlayerName name)
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
            var attendance = new PlayerFieldingStatistics(Name);
            attendance.CalculateStats(teamName, season, matchTypes);
            SeasonFielding.Add(season.Year, attendance);
        }

        /// <inheritdoc/>
        public void UpdateStats(string teamName, ICricketMatch match)
        {
        }

        /// <inheritdoc/>
        public void ResetStats()
        {
            SeasonFielding.Clear();
        }

        /// <inheritdoc/>
        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            _ = rb.WriteTitle("Yearly Fielding", headerElement);
            var yearRecords = SeasonFielding
                .ToList()
                .OrderBy(value => value.Key)
                .Select(record => new List<string>()
                {
                    record.Key.Year.ToString(),
                    record.Value.Catches.ToString(),
                    record.Value.RunOuts.ToString(),
                    record.Value.KeeperCatches.ToString(),
                    record.Value.KeeperStumpings.ToString(),
                    record.Value.TotalDismissals.ToString()
                });
            _ = rb.WriteTableFromEnumerable(new string[] { "Year", "Catches", "RunOuts", "Keeper Catches", "Keeper Stumpings", "Total" }, yearRecords, headerFirstColumn: false);
        }
    }
}
