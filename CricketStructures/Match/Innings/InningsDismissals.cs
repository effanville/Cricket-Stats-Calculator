using System;

using CricketStructures.Match;
using CricketStructures.Player;

namespace CricketStructures.Match.Innings
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

        public string Location
        {
            get;
            set;
        }

        public InningsDismissals()
        {
        }

        public InningsDismissals(string teamName, FieldingEntry entry, MatchInfo info)
        {
            Name = entry.Name;
            Dismissals = entry.TotalDismissals();
            Opposition = info.OppositionName(teamName);
            Date = info.Date;
            Location = info.Location;
        }
    }
}
