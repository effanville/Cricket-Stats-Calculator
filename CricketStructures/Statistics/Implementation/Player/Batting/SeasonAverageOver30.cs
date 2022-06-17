using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Collection;

namespace CricketStructures.Statistics.Implementation.Player.Batting
{
    internal sealed class SeasonAverageOver30 : ICricketStat
    {
        private readonly PlayerName Name;
        public List<SeasonRuns> SeasonAverage
        {
            get;
            set;
        } = new List<SeasonRuns>();


        public SeasonAverageOver30()
        {
        }

        public SeasonAverageOver30(PlayerName name)
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
            IEnumerable<PlayerBriefStatistics> goodAverage = playerStats.Where(player => player.Played.TotalGamesPlayed > 5 && player.BattingStats.Average > 30);
            SeasonAverage.AddRange(goodAverage.Select(element => new SeasonRuns(element.SeasonYear, element.Name, element.BattingStats.TotalInnings, element.BattingStats.TotalNotOut, element.BattingStats.TotalRuns, element.BattingStats.Average)));

            SeasonAverage.Sort((a, b) => b.Average.CompareTo(a.Average));
        }

        public void ResetStats()
        {
            SeasonAverage.Clear();
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            throw new NotImplementedException();
        }

        public StringBuilder ExportStats(DocumentType exportType, DocumentElement headerElement)
        {
            var writer = new StringBuilder();
            if (SeasonAverage.Any())
            {
                TextWriting.WriteTitle(writer, exportType, "Average over 30 in a season", headerElement);
                TableWriting.WriteTable(writer, exportType, SeasonAverage, headerFirstColumn: false);
            }

            return writer;
        }
    }
}
