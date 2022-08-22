using System;
using System.Collections.Generic;

using CricketStructures.Match;
using CricketStructures.Player;

namespace CricketStructures.Statistics.Implementation.Collection
{
    public interface IMatchAggregateStat<T>
    {
        string Title
        {
            get;
        }
        PlayerName Name
        {
            get;
        }
        IReadOnlyList<string> Headers
        {
            get;
        }
        Func<T, IReadOnlyList<string>> OutputValueSelector
        {
            get;
        }

        Action<string, ICricketMatch, List<T>> AddStatsAction
        {
            get;
        }
        public Func<T, bool> SelectorFunc
        {
            get
            {
                return val => true;
            }
        }

        Comparison<T> Comparison
        {
            get;
        }

        bool IncreaseStatScope();
    }
}
