using System;

namespace CricketStructures.Match.Innings
{
    public static class InningsHelpers
    {
        public static CricketInnings SelectBattingInnings(CricketInnings firstInnings, CricketInnings secondInnings, string battingTeam)
        {
            return SelectInnings(firstInnings, secondInnings, innings => innings.BattingTeam.Equals(battingTeam));
        }

        public static CricketInnings SelectFieldingInnings(CricketInnings firstInnings, CricketInnings secondInnings, string fieldingTeam)
        {
            return SelectInnings(firstInnings, secondInnings, innings => innings.FieldingTeam.Equals(fieldingTeam));
        }

        public static CricketInnings SelectInnings(CricketInnings firstInnings, CricketInnings secondInnings, Func<CricketInnings, bool> teamSelector)
        {
            if (teamSelector(firstInnings))
            {
                return firstInnings;
            }
            if (teamSelector(secondInnings))
            {
                return secondInnings;
            }

            return null;
        }
    }
}
