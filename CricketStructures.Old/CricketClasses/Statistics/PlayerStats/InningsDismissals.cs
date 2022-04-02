using System;
using Cricket.Match;
using Cricket.Player;

namespace Cricket.Statistics.PlayerStats
{
    public class InningsDismissals
    {
        public int Dismissals
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

        public InningsDismissals()
        {
        }

        public InningsDismissals(FieldingEntry entry, MatchInfo info)
        {
            Name = entry.Name;
            Dismissals = entry.TotalDismissals();
            Opposition = info.Opposition;
            Date = info.Date;
            HomeOrAway = info.HomeOrAway;
        }
    }
}
