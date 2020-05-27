using System;
using Cricket.Match;
using Cricket.Player;

namespace Cricket.Statistics.PlayerStats
{
    public class Century
    {
        public int Runs
        {
            get;
            set;
        }

        public Wicket HowOut
        {
            get;
            set;
        }

        public PlayerName Name
        {
            get;
            set;
        }

        public string Opposition
        {
            get;
            set;
        }
        public DateTime Date
        {
            get;
            set;
        }
        public Location HomeOrAway
        {
            get;
            set;
        }

        public MatchType GameType
        {
            get;
            set;
        }

        public Century()
        {
        }

        public Century(BattingEntry battingEntry, MatchInfo matchData)
        {
            Name = battingEntry.Name;
            Date = matchData.Date;
            Runs = battingEntry.RunsScored;
            HowOut = battingEntry.MethodOut;
            GameType = matchData.Type;
            Opposition = matchData.Opposition;
            HomeOrAway = matchData.HomeOrAway;
        }
    }
}
