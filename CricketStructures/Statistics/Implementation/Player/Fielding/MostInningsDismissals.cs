using System;
using System.Collections.Generic;

using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;

namespace CricketStructures.Statistics.Implementation.Player.Fielding
{
    public sealed class MostInningsDismissals : IMatchAggregateStat<InningsDismissals>
    {
        private int fMinimum;
        public string Title => "Most Dismissals in an Innings";

        public PlayerName Name
        {
            get; private set;
        }

        public IReadOnlyList<string> Headers => InningsDismissals.DisplayHeaders;

        public Func<InningsDismissals, string[]> OutputValueSelector => value => value.ArrayOfValues();
        public Action<PlayerName, string, ICricketMatch, List<InningsDismissals>> AddStatsAction => create;

        void create(PlayerName Name, string teamName, ICricketMatch match, List<InningsDismissals> stats)
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

        public bool IncreaseStatScope()
        {
            fMinimum--;
            return fMinimum <= 0;
        }

        public Comparison<InningsDismissals> Comparison => (a, b) => b.Dismissals.CompareTo(a.Dismissals);

        public MostInningsDismissals(int minimum, PlayerName name)
        {
            fMinimum = minimum;
            Name = name;
        }

    }
}
