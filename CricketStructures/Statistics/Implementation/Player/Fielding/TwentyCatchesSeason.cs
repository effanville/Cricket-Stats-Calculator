using System;
using System.Collections.Generic;
using System.Linq;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Collection;
using CricketStructures.Statistics.Implementation.Player.Model;

namespace CricketStructures.Statistics.Implementation.Player.Fielding
{
    internal sealed class TwentyCatchesSeason : ICricketStat
    {
        private readonly PlayerName Name;
        public List<NameDatedRecord<int>> TwentyCatches
        {
            get;
            set;
        } = new List<NameDatedRecord<int>>();

        public TwentyCatchesSeason()
        {
        }

        public TwentyCatchesSeason(PlayerName name)
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

            IEnumerable<PlayerBriefStatistics> manyCatches = playerStats.Where(player => player.FieldingStats.Catches > 20);
            TwentyCatches.AddRange(manyCatches.Select(catches => new NameDatedRecord<int>("Number Catches", catches.Name, season.Year, catches.FieldingStats.Catches, null)));

            TwentyCatches.Sort((a, b) => b.Value.CompareTo(a.Value));
        }

        public void ResetStats()
        {
            TwentyCatches.Clear();
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            throw new NotImplementedException();
        }

        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            if (TwentyCatches.Any())
            {
                _ = rb.WriteTitle("Twenty catches in one season", headerElement)
                    .WriteTableFromEnumerable(TwentyCatches.Select(value => new string[] { value.Name.ToString(), value.Date.ToShortDateString(), value.Value.ToString() }), headerFirstColumn: false);
            }
        }
    }
}
