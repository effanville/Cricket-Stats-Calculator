using System.Collections.Generic;
using System.Linq;
using CricketStructures.Match;
using CricketStructures.Season;
using Common.Structure.ReportWriting;
using System;
using CricketStructures.Statistics.Implementation.Collection;

namespace CricketStructures.Statistics.Implementation.Team
{
    internal sealed class TeamResultStats : ICricketStat
    {
        private readonly CricketStatsCollection Stats;

        /// <summary>
        /// Year by year record of the performance of the team.
        /// </summary>
        public Dictionary<DateTime, TeamRecord> YearByYearRecords
        {
            get;
            set;
        } = new Dictionary<DateTime, TeamRecord>();

        /// <summary>
        /// Record by opposition of the performance of the team
        /// </summary>
        public Dictionary<string, TeamRecord> TeamAgainstRecords
        {
            get;
            set;
        } = new Dictionary<string, TeamRecord>();

        public TeamResultStats()
        {
            var stats = new List<CricketStatTypes>()
            {
                CricketStatTypes.ExtremeScores,
                CricketStatTypes.LargestVictories,
                CricketStatTypes.HeaviestDefeats
            };

            Stats = new CricketStatsCollection("Team Record", stats);
        }

        /// <inheritdoc/>
        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            CricketStatsHelpers.SeasonIterator(
                team.Seasons,
                season => CalculateStats(team.TeamName, season, matchTypes));
        }

        /// <inheritdoc/>
        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            Stats.CalculateStats(teamName, season, matchTypes);
            var newEntry = new TeamRecord();
            newEntry.CalculateStats(teamName, season, matchTypes);
            YearByYearRecords.Add(season.Year, newEntry);

            foreach (ICricketMatch match in season.Matches)
            {
                if (matchTypes.Contains(match.MatchData.Type))
                {
                    UpdateStats(teamName, match);
                }
            }
        }

        /// <inheritdoc/>
        public void UpdateStats(string teamName, ICricketMatch match)
        {
            var oppositionName = match.MatchData.OppositionName(teamName);
            if (TeamAgainstRecords.TryGetValue(oppositionName, out var value))
            {
                value.UpdateStats(teamName, match);
            }
            else
            {
                var teamRecord = new TeamRecord();
                teamRecord.UpdateStats(teamName, match);
                TeamAgainstRecords.Add(oppositionName, teamRecord);
            }
        }

        /// <inheritdoc/>
        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            _ = rb.WriteTitle("Team Records", headerElement);

            DocumentElement lowerLevelElement = headerElement.GetNext();

            _ = rb.WriteTitle("Yearly Records", lowerLevelElement);
            var yearRecords = YearByYearRecords
                .ToList()
                .Select(record => new List<string>()
                {
                    record.Key.ToShortDateString(),
                    record.Value.Played.ToString(),
                    record.Value.Won.ToString(),
                    record.Value.Lost.ToString(),
                    record.Value.WinRatio.ToString()
                });
            _ = rb.WriteTableFromEnumerable(new string[] { "Year", "Played", "Won", "Lost", "Win Ratio" }, yearRecords, headerFirstColumn: false);

            _ = rb.WriteTitle("Record against each team", lowerLevelElement);
            var oppoRecords = TeamAgainstRecords
                .ToList()
                .Select(record => new List<string>()
                {
                                record.Key.ToString(),
                                record.Value.Played.ToString(),
                                record.Value.Won.ToString(),
                                record.Value.Lost.ToString(),
                                record.Value.WinRatio.ToString()
                });
            _ = rb.WriteTableFromEnumerable(new string[] { "Opposition", "Played", "Won", "Lost", "Win Ratio" }, oppoRecords, headerFirstColumn: false);

            Stats.ExportStats(rb, lowerLevelElement);

        }

        /// <inheritdoc/>
        public void ResetStats()
        {
            YearByYearRecords.Clear();
            TeamAgainstRecords.Clear();
        }
    }
}
