using System;
using System.Collections.Generic;

using Common.Structure.NamingStructures;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;

namespace CricketStructures.Statistics.Implementation.Player.Fielding
{
    public sealed class SeasonCatches : ISeasonAggregateStat<NameDatedRecord<int>>
    {
        private readonly int fMinimum;
        public string Title => $"Over {fMinimum} catches in a season.";

        public PlayerName Name
        {
            get;
            private set;
        }

        public IReadOnlyList<string> Headers => new[] { "Name", "Season", "Catches" };

        public Func<NameDatedRecord<int>, string[]> OutputValueSelector => record => record.ArrayOfValues();

        public Func<PlayerName, string, ICricketSeason, MatchType[], NameDatedRecord<int>> StatGenerator => Create;

        NameDatedRecord<int> Create(PlayerName name, string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            var stats = CricketStatsFactory.Generate(CricketStatTypes.PlayerFieldingStats, teamName, season, matchTypes, name) as PlayerFieldingStatistics;
            return new NameDatedRecord<int>("Number Catches", stats.Name, season.Year, stats.Catches, null);
        }

        public Func<NameDatedRecord<int>, bool> SelectorFunc => Selector;

        bool Selector(NameDatedRecord<int> stat)
        {
            return stat.Value > fMinimum;
        }

        public Comparison<NameDatedRecord<int>> Comparison => NameDatedRecordComparisons.ValueCompare<int>();

        public SeasonCatches(int minimum, PlayerName name)
        {
            fMinimum = minimum;
            Name = name;
        }

        public bool IncreaseStatScope()
        {
            return true;
        }
    }
}
