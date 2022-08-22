using System;
using System.Collections.Generic;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Collection;

namespace CricketStructures.Statistics.Implementation.Player.Bowling
{
    internal class LowEconomyStat : ISeasonAggregateStat<PlayerBowlingRecord>
    {
        public List<PlayerBowlingRecord> LowEconomy
        {
            get;
        } = new List<PlayerBowlingRecord>();

        public string Title => "Low Economy";

        public PlayerName Name
        {
            get;
            private set;
        }

        public IReadOnlyList<string> Headers => new string[] { "Name", "Wickets", "Economy" };

        public Func<PlayerBowlingRecord, IReadOnlyList<string>> OutputValueSelector
        {
            get
            {
                return val => new string[]
                {
                    val.Name.ToString(),
                    val.TotalWickets.ToString(),
                    val.Economy.ToString()
                };
            }
        }

        public Func<PlayerName, string, ICricketSeason, MatchType[], PlayerBowlingRecord> StatGenerator =>
            (name, teamName, season, matchTypes) => CricketStatsFactory.Generate(CricketStatTypes.PlayerBowlingRecord, teamName, season, matchTypes, name) as PlayerBowlingRecord;
        public Func<PlayerBowlingRecord, bool> SelectorFunc => val => !double.IsNaN(val.Economy) && val.MatchesPlayed > 10 && val.TotalOvers > 20;

        public Comparison<PlayerBowlingRecord> Comparison => (a, b) => a.Economy.CompareTo(b.Economy);

        public LowEconomyStat()
        {
        }

        public LowEconomyStat(PlayerName name)
        {
            Name = name;
        }

        public bool IncreaseStatScope()
        {
            return true;
        }
    }
}
