using System;
using System.Collections.Generic;
using System.Linq;

using CricketStructures.Match;
using CricketStructures.Statistics;

namespace CricketStructures
{
    public static class CricketConstants
    {
        public const string DefaultOppositionPlayerSurname = "Oppo";
        public const string DefaultOppositionPlayerForename = "Forename";

        public const string WicketKeeperSymbol = "†";

        public static IReadOnlyList<MatchType> MatchTypes => Enum.GetValues(typeof(MatchType)).Cast<MatchType>().ToList();
        public static IReadOnlyList<ResultType> MatchResultTypes => Enum.GetValues(typeof(ResultType)).Cast<ResultType>().ToList();
        public static IReadOnlyList<StatCollection> StatisticTypes => Enum.GetValues(typeof(StatCollection)).Cast<StatCollection>().ToList();
    }
}
