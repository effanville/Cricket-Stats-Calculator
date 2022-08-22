using System;
using System.Collections.Generic;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Collection;

namespace CricketStructures.Statistics.Implementation.Player.Fielding
{
    /// <summary>
    /// Generate a season by season record of fielding.
    /// </summary>
    public sealed class SeasonFieldingRecord : ISeasonAggregateStat<PlayerFieldingRecord>
    {
        private readonly int fMinimum;

        /// <inheritdoc/>
        public string Title => "Yearly Fielding";

        /// <inheritdoc/>
        public PlayerName Name
        {
            get; private set;
        }

        /// <inheritdoc/>
        public IReadOnlyList<string> Headers => PlayerFieldingRecord.Headers(Name == null, true);

        /// <inheritdoc/>
        public Func<PlayerFieldingRecord, IReadOnlyList<string>> OutputValueSelector
        {
            get
            {
                return value => value.Values(Name == null, true);
            }
        }

        /// <inheritdoc/>
        public Func<PlayerName, string, ICricketSeason, MatchType[], PlayerFieldingRecord> StatGenerator => Create;

        PlayerFieldingRecord Create(PlayerName name, string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            return CricketStatsFactory.Generate(CricketStatTypes.PlayerFieldingStats, teamName, season, matchTypes, name) as PlayerFieldingRecord;
        }

        /// <inheritdoc/>
        public Func<PlayerFieldingRecord, bool> SelectorFunc => Selector;

        bool Selector(PlayerFieldingRecord stat)
        {
            return stat.TotalDismissals > fMinimum;
        }

        /// <inheritdoc/>
        public Comparison<PlayerFieldingRecord> Comparison => (a, b) => a.StartYear.CompareTo(b.StartYear);

        public SeasonFieldingRecord(int minimum, PlayerName name)
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
