using System;
using System.Collections.Generic;

using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;
using CricketStructures.Statistics.Implementation.Collection;

namespace CricketStructures.Statistics.Implementation.Player.Fielding
{
    /// <summary>
    /// Generate the most dismissals in an innings.
    /// </summary>
    public sealed class MostInningsDismissals : IMatchAggregateStat<InningsDismissals>
    {
        private int fMinimum;

        /// <inheritdoc/>
        public string Title => "Most Dismissals in an Innings";

        /// <inheritdoc/>
        public PlayerName Name
        {
            get; private set;
        }

        /// <inheritdoc/>
        public IReadOnlyList<string> Headers => InningsDismissals.DisplayHeaders;

        /// <inheritdoc/>
        public Func<InningsDismissals, IReadOnlyList<string>> OutputValueSelector => value => value.ArrayOfValues();

        /// <inheritdoc/>
        public Action<string, ICricketMatch, List<InningsDismissals>> AddStatsAction => Create;

        void Create(string teamName, ICricketMatch match, List<InningsDismissals> stats)
        {
            var fielding = match.GetAllFielding(teamName);
            if (fielding == null)
            {
                return;
            }

            foreach (FieldingEntry field in fielding)
            {
                if (Name == null || field.Name.Equals(Name))
                {
                    if (field.TotalDismissals() > fMinimum)
                    {
                        stats.Add(new InningsDismissals(teamName, field, match.MatchData));
                    }
                }
            }
        }

        /// <inheritdoc/>
        public Comparison<InningsDismissals> Comparison => (a, b) => b.Dismissals.CompareTo(a.Dismissals);

        public MostInningsDismissals(int minimum, PlayerName name)
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
