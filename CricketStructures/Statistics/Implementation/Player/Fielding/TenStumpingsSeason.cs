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
    internal sealed class TenStumpingsSeason : ICricketStat
    {
        private readonly PlayerName Name;
        public List<NameDatedRecord<int>> TenStumpings
        {
            get;
            set;
        } = new List<NameDatedRecord<int>>();

        public TenStumpingsSeason()
        {
        }

        public TenStumpingsSeason(PlayerName name)
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

            IEnumerable<PlayerBriefStatistics> manyStumpings = playerStats.Where(player => player.FieldingStats.KeeperStumpings > 10);
            TenStumpings.AddRange(manyStumpings.Select(catches => new NameDatedRecord<int>("NumberStumpings", catches.Name, season.Year, catches.FieldingStats.KeeperStumpings, null)));

            TenStumpings.Sort((a, b) => b.Value.CompareTo(a.Value));
        }

        public void ResetStats()
        {
            TenStumpings.Clear();
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            throw new NotImplementedException();
        }

        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            if (TenStumpings.Any())
            {
                _ = rb.WriteTitle("Ten Stumpings in one season", headerElement)
                    .WriteTableFromEnumerable(new string[] { "Name", "Season", "NumberStumpings" }, TenStumpings.Select(value => new string[] { value.Name.ToString(), value.Date.ToShortDateString(), value.Value.ToString() }), headerFirstColumn: false);
            }
        }
    }
}
