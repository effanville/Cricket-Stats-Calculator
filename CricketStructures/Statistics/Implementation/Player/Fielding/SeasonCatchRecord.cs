using System;
using System.Collections.Generic;

using Common.Structure.NamingStructures;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Collection;

namespace CricketStructures.Statistics.Implementation.Player.Fielding
{
    /// <summary>
    /// Generate a season by season record of catches.
    /// </summary>
    public sealed class SeasonCatchRecord : ISeasonAggregateStat<NameDatedRecord<int>>
    {
        private readonly int fMinimum;

        /// <inheritdoc/>
        public string Title => $"Over {fMinimum} catches in a season.";

        /// <inheritdoc/>
        public PlayerName Name
        {
            get;
            private set;
        }

        /// <inheritdoc/>
        public IReadOnlyList<string> Headers => new[] { "Name", "Season", "Catches" };

        /// <inheritdoc/>
        public Func<NameDatedRecord<int>, IReadOnlyList<string>> OutputValueSelector => record => record.Values();

        /// <inheritdoc/>
        public Func<PlayerName, string, ICricketSeason, MatchType[], NameDatedRecord<int>> StatGenerator => Create;

        NameDatedRecord<int> Create(PlayerName name, string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            var stats = CricketStatsFactory.Generate(CricketStatTypes.PlayerFieldingStats, teamName, season, matchTypes, name) as PlayerFieldingRecord;
            return new NameDatedRecord<int>("Number Catches", stats.Name, season.Year, stats.Catches, null);
        }

        /// <inheritdoc/>
        public Func<NameDatedRecord<int>, bool> SelectorFunc => Selector;

        bool Selector(NameDatedRecord<int> stat)
        {
            return stat.Value > fMinimum;
        }

        /// <inheritdoc/>
        public Comparison<NameDatedRecord<int>> Comparison => NameDatedRecordComparisons.ValueCompare<int>();

        public SeasonCatchRecord(int minimum, PlayerName name)
        {
            fMinimum = minimum;
            Name = name;
        }

        /// <inheritdoc/>
        public bool IncreaseStatScope()
        {
            return true;
        }
    }
}
