using System;
using System.Collections.Generic;
using System.Linq;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Collection;
using CricketStructures.Statistics.Implementation.Player.Model;

namespace CricketStructures.Statistics.Implementation.Player.Bowling
{
    internal class LowStrikeRateStat : ICricketStat
    {
        private readonly PlayerName Name;
        public List<NamedRecord<int, double>> LowStrikeRate
        {
            get;
        } = new List<NamedRecord<int, double>>();

        public LowStrikeRateStat()
        {
        }

        public LowStrikeRateStat(PlayerName name)
        {
            Name = name;
        }

        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            var playerNames = Name == null ? team.Players().Select(player => player.Name).ToList() : new List<PlayerName>() { Name };
            List<PlayerBriefStatistics> playerStats = playerNames.Select(name => new PlayerBriefStatistics(name, team, matchTypes)).ToList();

            foreach (var player in playerStats)
            {
                if (!double.IsNaN(player.BowlingStats.StrikeRate))
                {
                    LowStrikeRate.Add(new NamedRecord<int, double>("LowStrikeRate", player.Name, player.BowlingStats.TotalWickets, player.BowlingStats.StrikeRate));
                }
            }

            LowStrikeRate.Sort((a, b) => a.SecondValue.CompareTo(b.SecondValue));
        }

        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            var playerNames = Name == null ? season.Players(teamName, matchTypes) : new List<PlayerName>() { Name };
            List<PlayerBriefStatistics> playerStats = playerNames.Select(name => new PlayerBriefStatistics(teamName, name, season, matchTypes)).ToList();

            foreach (var player in playerStats)
            {
                if (!double.IsNaN(player.BowlingStats.StrikeRate))
                {
                    LowStrikeRate.Add(new NamedRecord<int, double>("LowStrikeRate", player.Name, player.BowlingStats.TotalWickets, player.BowlingStats.StrikeRate));
                }
            }
            LowStrikeRate.Sort((a, b) => a.SecondValue.CompareTo(b.SecondValue));
        }

        public void ResetStats()
        {
            LowStrikeRate.Clear();
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            throw new NotImplementedException();
        }

        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            if (LowStrikeRate.Any())
            {
                _ = rb.WriteTitle("Low Strike Rate", headerElement)
                    .WriteTableFromEnumerable(new string[] { "Name", "Wickets", "Strike Rate" }, LowStrikeRate.Select(value => new string[] { value.Name.ToString(), value.Value.ToString(), value.SecondValue.ToString() }), headerFirstColumn: false);
            }
        }
    }
}
