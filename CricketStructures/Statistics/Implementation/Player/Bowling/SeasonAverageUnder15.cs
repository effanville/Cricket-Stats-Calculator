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
    internal class SeasonAverageUnder15 : ICricketStat
    {
        private readonly PlayerName Name;
        public List<SeasonWickets> SeasonAvUnder15
        {
            get;
        } = new List<SeasonWickets>();

        public SeasonAverageUnder15()
        {
        }

        public SeasonAverageUnder15(PlayerName name)
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

            IEnumerable<PlayerBriefStatistics> lowAverage = playerStats.Where(player => player.BowlingStats.TotalWickets > 15 && player.BowlingStats.Average < 15);
            SeasonAvUnder15.AddRange(lowAverage.Select(lots => new SeasonWickets(lots.Name, lots.BowlingStats.TotalWickets, season.Year.Year, lots.BowlingStats.Average)));

            SeasonAvUnder15.Sort((a, b) => b.Wickets.CompareTo(a.Wickets));
        }

        public void ResetStats()
        {
            SeasonAvUnder15.Clear();
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            throw new NotImplementedException();
        }

        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            if (SeasonAvUnder15.Any())
            {
                _ = rb.WriteTitle("Season Average under 15", headerElement)
                    .WriteTable(SeasonAvUnder15, headerFirstColumn: false);
            }
        }
    }
}
