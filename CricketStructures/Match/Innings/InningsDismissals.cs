using System;

using Common.Structure.Extensions;

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

        public static string[] DisplayHeaders => new[] { "Dismissals", "Name", "Opposition", "Date", "Location" };

        public string[] ArrayOfValues()
        {
            return new string[] { Dismissals.ToString(), Name.ToString(), Opposition, Date.ToUkDateString(), Location };
        }
    }
}
