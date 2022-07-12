using System;
using System.Collections.Generic;
using System.Linq;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Collection;

namespace CricketStructures.Statistics.Implementation.Player.Batting
{
    internal sealed class SeasonRunsOver500 : ICricketStat
    {
        private readonly PlayerName Name;
        public List<SeasonRuns> SeasonRuns
        {
            get;
            set;
        } = new List<SeasonRuns>();

        public SeasonRunsOver500()
        {
        }

        public SeasonRunsOver500(PlayerName name)
        {
            Name = name;
        }
        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            CricketStatsHelpers.SeasonIterator(
                team.Seasons,
                season => CalculateStats(team.TeamName, season, matchTypes));
        }

        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            var playerNames = Name == null ? season.Players(teamName).ToList() : new List<PlayerName>() { Name };
            List<PlayerBriefStatistics> playerStats = playerNames.Select(name => new PlayerBriefStatistics(teamName, name, season, matchTypes)).ToList();

            IEnumerable<PlayerBriefStatistics> manyRuns = playerStats.Where(player => player.BattingStats.TotalRuns > 500);
            SeasonRuns.AddRange(manyRuns.Select(element => new SeasonRuns(element.SeasonYear, element.Name, element.BattingStats.TotalInnings, element.BattingStats.TotalNotOut, element.BattingStats.TotalRuns, element.BattingStats.Average)));

            SeasonRuns.Sort((a, b) => b.Runs.CompareTo(a.Runs));
        }

        public void ResetStats()
        {
            SeasonRuns.Clear();
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            throw new NotImplementedException();
        }

        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            if (SeasonRuns.Any())
            {
                _ = rb.WriteTitle("Over 500 runs in a season", headerElement)
                    .WriteTable(SeasonRuns, headerFirstColumn: false);
            }
        }
    }
}
