using System;
using System.Collections.Generic;
using System.Linq;

using Common.Structure.NamingStructures;

using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;
using CricketStructures.Statistics.Implementation.Collection;

namespace CricketStructures.Statistics.Implementation.Player.Bowling
{
    /// <summary>
    /// Generate a collection of high numbers of wickets in matches.
    /// </summary>
    public sealed class HighWicketHauls : IMatchAggregateStat<NamedRecord<int>>
    {
        private int fMinimum;

        /// <inheritdoc/>
        public string Title => $"Individual Wicket hauls over {fMinimum} wickets.";

        /// <inheritdoc/>
        public PlayerName Name
        {
            get; private set;
        }

        public IReadOnlyList<string> Headers => new string[] { "Name", "Number Hauls" };

        /// <inheritdoc/>
        public Func<NamedRecord<int>, IReadOnlyList<string>> OutputValueSelector => value => value.ArrayValues();

        /// <inheritdoc/>
        public Action<string, ICricketMatch, List<NamedRecord<int>>> AddStatsAction => Create;
        void Create(string teamName, ICricketMatch match, List<NamedRecord<int>> stats)
        {
            CricketStatsHelpers.BowlingIterator(
                match,
                teamName,
                UpdateFiveFors);
            void UpdateFiveFors(BowlingEntry bowlingEntry)
            {
                if (Name == null || bowlingEntry.Name.Equals(Name))
                {
                    if (bowlingEntry.Wickets >= fMinimum)
                    {
                        if (stats.Any(entry => entry.Name.Equals(bowlingEntry.Name)))
                        {
                            var value = stats.First(entry => entry.Name.Equals(bowlingEntry.Name));
                            value.UpdateValue(1);
                        }
                        else
                        {
                            stats.Add(new NamedRecord<int>("NumberFiveFor", bowlingEntry.Name, 1, Update));
                        }
                    }
                }

                int Update(int a, int b)
                {
                    return a + b;
                }
            }
        }

        /// <inheritdoc/>
        public Comparison<NamedRecord<int>> Comparison => NamedRecordComparers.ValueCompare<int>();

        public HighWicketHauls(int minimum, PlayerName name)
        {
            fMinimum = minimum;
            Name = name;
        }

        /// <inheritdoc/>
        public bool IncreaseStatScope()
        {
            fMinimum--;
            return fMinimum <= 0;
        }
    }
}
