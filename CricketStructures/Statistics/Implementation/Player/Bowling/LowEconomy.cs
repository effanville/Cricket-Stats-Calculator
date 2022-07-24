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
    internal class LowEconomyStat : ICricketStat
    {
        private readonly PlayerName Name;
        public List<NamedRecord<int, double>> LowEconomy
        {
            get;
        } = new List<NamedRecord<int, double>>();

        public LowEconomyStat()
        {
        }

        public LowEconomyStat(PlayerName name)
        {
            Name = name;
        }

        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            var playerNames = Name == null ? team.Players().Select(player => player.Name).ToList() : new List<PlayerName>() { Name };
            List<PlayerBriefStatistics> playerStats = playerNames.Select(name => new PlayerBriefStatistics(name, team, matchTypes)).ToList();

            foreach (var player in playerStats)
            {
                if (!double.IsNaN(player.BowlingStats.Economy))
                {
                    LowEconomy.Add(new NamedRecord<int, double>("LowEconomy", player.Name, player.BowlingStats.TotalWickets, player.BowlingStats.Economy, null, null));
                }
            }

            LowEconomy.Sort((a, b) => a.SecondValue.CompareTo(b.SecondValue));
        }

        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            var playerNames = Name == null ? season.Players(teamName, matchTypes) : new List<PlayerName>() { Name };
            List<PlayerBriefStatistics> playerStats = playerNames.Select(name => new PlayerBriefStatistics(teamName, name, season, matchTypes)).ToList();

            foreach (var player in playerStats)
            {
                if (!double.IsNaN(player.BowlingStats.Economy))
                {
                    LowEconomy.Add(new NamedRecord<int, double>("LowEconomy", player.Name, player.BowlingStats.TotalWickets, player.BowlingStats.Economy, null, null));
                }
            }

            LowEconomy.Sort((a, b) => a.SecondValue.CompareTo(b.SecondValue));
        }

        public void ResetStats()
        {
            LowEconomy.Clear();
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            throw new NotImplementedException();
        }

        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            if (LowEconomy.Any())
            {
                _ = rb.WriteTitle("Low Economy", headerElement)
                    .WriteTableFromEnumerable(new string[] { "Name", "Wickets", "Economy" }, LowEconomy.Select(value => new string[] { value.Name.ToString(), value.Value.ToString(), value.SecondValue.ToString() }), headerFirstColumn: false);
            }
        }
    }
}
