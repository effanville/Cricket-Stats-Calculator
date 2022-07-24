using System;
using System.Collections.Generic;
using System.Linq;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Collection;

namespace CricketStructures.Statistics.Implementation.Player.Bowling
{
    internal class SeasonOver30Wickets : ICricketStat
    {
        private readonly PlayerName Name;
        public List<SeasonWickets> SeasonWicketsOver30
        {
            get;
        } = new List<SeasonWickets>();

        public SeasonOver30Wickets()
        {
        }

        public SeasonOver30Wickets(PlayerName name)
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
            var playerNames = Name == null ? season.Players(teamName, matchTypes) : new List<PlayerName>() { Name };
            List<PlayerBriefStatistics> playerStats = playerNames.Select(name => new PlayerBriefStatistics(teamName, name, season, matchTypes)).ToList();

            IEnumerable<PlayerBriefStatistics> manyWickets = playerStats.Where(player => player.BowlingStats.TotalWickets >= 30);
            SeasonWicketsOver30.AddRange(manyWickets.Select(lots => new SeasonWickets(lots.Name, lots.BowlingStats.TotalWickets, season.Year.Year, lots.BowlingStats.Average)));

            SeasonWicketsOver30.Sort((a, b) => b.Wickets.CompareTo(a.Wickets));
        }

        public void ResetStats()
        {
            SeasonWicketsOver30.Clear();
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            throw new NotImplementedException();
        }

        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            if (SeasonWicketsOver30.Any())
            {
                _ = rb.WriteTitle("Over 30 Wickets in Season", headerElement)
                    .WriteTable(SeasonWicketsOver30, headerFirstColumn: false);
            }
        }
    }
}
