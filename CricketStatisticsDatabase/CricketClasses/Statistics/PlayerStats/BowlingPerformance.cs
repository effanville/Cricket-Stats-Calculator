using System;
using Cricket.Match;
using Cricket.Player;

namespace Cricket.Statistics.PlayerStats
{
    public class BowlingPerformance
    {
        public PlayerName Name
        {
            get;
            set;
        }

        public double Overs
        {
            get;
            set;
        }

        public int Maidens
        {
            get;
            set;
        }

        public int RunsConceded
        {
            get;
            set;
        }

        public int Wickets
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

        public BowlingPerformance()
        {
        }

        public BowlingPerformance(BowlingEntry bowlingEntry, MatchInfo matchData)
        {
            Name = bowlingEntry.Name;
            Date = matchData.Date;
            Overs = bowlingEntry.OversBowled;
            Maidens = bowlingEntry.Maidens;
            RunsConceded = bowlingEntry.RunsConceded;
            Wickets = bowlingEntry.Wickets;
            Opposition = matchData.Opposition;
            HomeOrAway = matchData.HomeOrAway;
        }
    }
}
