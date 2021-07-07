using System;
using System.Collections.Generic;
using Cricket.Match;
using Cricket.Player;
using Cricket.Statistics;

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

        TeamInnings BattingFirstOrSecond
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

        void EditPlayerName(PlayerName oldName, PlayerName newName);

        bool EditInfo(string opposition, DateTime date, string place, Location homeOrAway, MatchType typeOfMatch, ResultType result, TeamInnings firstOrSecond);

        bool EditMatchInfo(string opposition, DateTime date, string place, Location homeOrAway, MatchType typeOfMatch);

        bool EditResult(ResultType result);

        bool EditManOfMatch(PlayerName player);

        BattingEntry GetBatting(PlayerName player);

        BowlingEntry GetBowling(PlayerName player);

        FieldingEntry GetFielding(PlayerName player);

        bool AddPlayer(PlayerName player);

        void SetBatting(BattingInnings innings);

        bool AddBattingEntry(PlayerName player, Wicket howOut, int runs, int order, int wicketFellAt, int teamScoreAtWicket, PlayerName fielder = null, PlayerName bowler = null);

        bool EditBattingEntry(PlayerName player, Wicket howOut, int runs, int order, int wicketFellAt, int teamScoreAtWicket, PlayerName fielder = null, PlayerName bowler = null);

        bool DeleteBattingEntry(PlayerName player);

        void SetBowling(BowlingInnings innings);

        bool AddBowlingEntry(PlayerName player, double overs, int maidens, int runsConceded, int wickets);

        bool EditBowlingEntry(PlayerName player, double overs, int maidens, int runsConceded, int wickets);

        bool DeleteBowlingEntry(PlayerName player);

        void SetFielding(Fielding innings);

        bool AddFieldingEntry(PlayerName player, int catches, int runOuts, int stumpings, int keeperCatches);
        bool EditFieldingEntry(PlayerName player, int catches, int runOuts, int stumpings, int keeperCatches);

        bool DeleteFieldingEntry(PlayerName player);

        List<Partnership> Partnerships();
    }
}
