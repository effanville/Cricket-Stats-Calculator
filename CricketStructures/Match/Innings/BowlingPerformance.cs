using System;

using Common.Structure.Extensions;

using CricketStructures.Player;

namespace CricketStructures.Match.Innings
{
    public sealed class BowlingPerformance : IComparable, IComparable<BowlingPerformance>
    {
        public PlayerName Name
        {
            get;
            set;
        }
        public DateTime Date
        {
            get;
            set;
        }
        public Over Overs
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

        public MatchType GameType
        {
            get;
            set;
        }

        public string Location
        {
            get;
            set;
        }

        public BowlingPerformance()
        {
        }

        public BowlingPerformance(string teamName, BowlingEntry bowlingEntry, MatchInfo matchData)
        {
            Name = bowlingEntry.Name;
            Overs = bowlingEntry.OversBowled;
            Maidens = bowlingEntry.Maidens;
            RunsConceded = bowlingEntry.RunsConceded;
            Wickets = bowlingEntry.Wickets;
            Date = matchData.Date;
            GameType = matchData.Type;
            Opposition = matchData.OppositionName(teamName);
            Location = matchData.Location;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Opposition))
            {
                return Wickets + "-" + RunsConceded + " vs unknown opposition";
            }

            return Wickets + "-" + RunsConceded + " vs " + Opposition + " on " + Date.ToUkDateString();
        }

        public int CompareTo(BowlingPerformance other)
        {
            if(other == null)
            {
                return 1;
            }   

            if (!Wickets.Equals(other.Wickets))
            {
                return Wickets.CompareTo(other.Wickets);
            }

            return other.RunsConceded.CompareTo(RunsConceded);
        }

        public int CompareTo(object obj)
        {
            if (obj is BowlingPerformance otherBowling)
            {
                return CompareTo(otherBowling);
            }

            return 0;
        }
    }
}
