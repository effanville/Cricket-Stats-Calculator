using System;
using System.Collections.Generic;

using CricketStructures.Match;
using CricketStructures.Player;

namespace CricketStructures.Statistics.Implementation
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
        Func<T, string[]> OutputValueSelector
        {
            get;
        }

        Action<PlayerName, string, ICricketMatch, List<T>> AddStatsAction
        {
            get;
        }

        Comparison<T> Comparison
        {
            get;
        }

        bool IncreaseStatScope();
    }
}
