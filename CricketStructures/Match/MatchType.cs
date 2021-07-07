using System;
using System.Linq;

namespace Cricket.Match
{
    public enum MatchType
    {
        League = 0,
        Friendly = 1,
        Evening = 2,
    }

    public static class MatchHelpers
    {
        public static readonly MatchType[] AllMatchTypes = Enum.GetValues(typeof(MatchType)).Cast<MatchType>().ToArray();
    }
}
