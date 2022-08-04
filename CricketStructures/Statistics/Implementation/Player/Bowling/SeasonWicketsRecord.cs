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
    internal class SeasonWicketsRecord : ICricketStat
    {
        private readonly int MinimumWickets;
        private readonly PlayerName Name;
        public List<SeasonWickets> SeasonWicketsOver30
        {
            get;
        } = new List<SeasonWickets>();

        public SeasonWicketsRecord()
        {
        }

        public SeasonWicketsRecord(int minimumWickets)
            : this()
        {
            MinimumWickets = minimumWickets;
        }

        public SeasonWicketsRecord(int minimumWickets, PlayerName name)
            : this(minimumWickets)
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

            IEnumerable<PlayerBriefStatistics> manyWickets = playerStats.Where(player => player.BowlingStats.TotalWickets >= MinimumWickets);
            SeasonWicketsOver30.AddRange(manyWickets.Select(lots => new SeasonWickets(season.Year.Year, lots.Name, lots.BowlingStats)));

            if (MinimumWickets == 0)
            {
                SeasonWicketsOver30.Sort((a, b) => a.Year.CompareTo(b.Year));
            }
            else
            {
                SeasonWicketsOver30.Sort((a, b) => b.Wickets.CompareTo(a.Wickets));
            }
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
                string title = MinimumWickets == 0 ? "Yearly bowling record" : $"Over {MinimumWickets} wickets in a season";
                string[] headers = new string[] { "Year", "Overs", "Maidens", "Runs Conceded", "Wickets", "Average", "Economy", "Strike Rate", "Best Figures" };
                var values = SeasonWicketsOver30
                    .Select(value =>
                        new string[]
                        {
                            value.Year.ToString(),
                            value.Overs.ToString(),
                            value.Maidens.ToString(),
                            value.RunsConceded.ToString(),
                            value.Wickets.ToString(),
                            value.Average.ToString(),
                            value.Economy.ToString(),
                            value.StrikeRate.ToString(),
                            value.BestFigures?.ToString() ?? string.Empty,
                        });
                _ = rb.WriteTitle(title, headerElement)
                    .WriteTableFromEnumerable(headers, values, headerFirstColumn: false);
            }
        }
    }
}
