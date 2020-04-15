using Cricket.Match;
using Cricket.Player;
using System;
using System.Collections.Generic;

namespace Cricket.Interfaces
{
    public interface ICricketMatch
    {
        MatchInfo MatchData
        { 
            get;
        }

        ResultType Result
        { 
            get;
        }

        List<PlayerName> PlayerNames
        { 
            get;
        }

        BattingInnings Batting
        { 
            get;
        }

        BowlingInnings Bowling
        {
            get;
        }

        Fielding FieldingStats
        {
            get;
        }

        PlayerName ManOfMatch
        { 
            get;
        }

        bool PlayNotPlay(PlayerName name);

        bool EditMatchInfo(string opposition, DateTime date, string place, MatchType typeOfMatch);

        bool EditResult(ResultType result);

        bool EditManOfMatch(PlayerName player);

        bool AddBattingEntry(PlayerName player, BattingWicketLossType howOut, int runs, PlayerName fielder = null, PlayerName bowler = null);

        bool EditBattingEntry(PlayerName player, BattingWicketLossType howOut, int runs, PlayerName fielder = null, PlayerName bowler = null);

        bool DeleteBattingEntry(PlayerName player);

        bool AddBowlingEntry(PlayerName player, int overs, int maidens, int runsConceded, int wickets);

        bool EditBowlingEntry(PlayerName player, int overs, int maidens, int runsConceded, int wickets);

        bool DeleteBowlingEntry(PlayerName player);

        bool AddFieldingEntry(PlayerName player, int catches, int runOuts, int stumpings, int keeperCatches);
        bool EditFieldingEntry(PlayerName player, int catches, int runOuts, int stumpings, int keeperCatches);

        bool DeleteFieldingEntry(PlayerName player);
    }
}
