using System;
using System.Collections.Generic;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;

namespace CricketStructures.Statistics.Implementation
{
    public interface ISeasonAggregateStat<T> where T : class
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

        bool IncreaseStatScope();

        Func<PlayerName, string, ICricketSeason, MatchType[], T> StatGenerator
        {
            get;
        }

        Func<T, bool> SelectorFunc
        {
            get;
        }

        Comparison<T> Comparison
        {
            get;
        }
    }
}
