using System.Collections.Generic;

using CricketStructures.Match;

namespace CSD
{
    public sealed class CricketConstants
    {
        public const string DefaultOppositionPlayerSurname = CricketStructures.CricketConstants.DefaultOppositionPlayerSurname;
        public const string DefaultOppositionPlayerForename = CricketStructures.CricketConstants.DefaultOppositionPlayerForename;

        public const string WicketKeeperSymbol = CricketStructures.CricketConstants.WicketKeeperSymbol;

        public static IReadOnlyList<MatchType> MatchTypes => CricketStructures.CricketConstants.MatchTypes;
        public static IReadOnlyList<ResultType> MatchResultTypes => CricketStructures.CricketConstants.MatchResultTypes;

        public CricketConstants()
        {
        }
    }
}
