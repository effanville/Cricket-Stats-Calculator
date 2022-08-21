using System;

using Common.Structure.Extensions;

using CricketStructures.Player;

namespace CricketStructures.Match.Innings
{
    public sealed class PlayerScore : IComparable, IComparable<PlayerScore>
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

        public InningsScore TeamTotalScore
        {
            get;
            set;
        }

        internal PlayerScore()
        {
        }

        public PlayerScore(string teamName, BattingEntry battingEntry, MatchInfo matchData, InningsScore score)
        {
            Name = battingEntry.Name;
            Date = matchData.Date;
            Runs = battingEntry.RunsScored;
            HowOut = battingEntry.MethodOut;
            GameType = matchData.Type;
            Opposition = matchData.OppositionName(teamName);
            Location = matchData.Location;
            TeamTotalScore = score;
        }

        public int CompareTo(object obj)
        {
            if (obj is PlayerScore otherBest)
            {
                return CompareTo(otherBest);
            }

            return 0;
        }

        public int CompareTo(PlayerScore other)
        {
            if (other == null)
            {
                return 1;
            }
            if (string.IsNullOrEmpty(Opposition))
            {
                return -1;
            }

            if (string.IsNullOrEmpty(other.Opposition))
            {
                return 1;
            }

            if (Runs.Equals(other.Runs))
            {
                if (HowOut.IsNotOut() && other.HowOut.IsNotOut())
                {
                    return 0;
                }

                if (HowOut.IsNotOut())
                {
                    return 1;
                }

                if (other.HowOut.IsNotOut())
                {
                    return -1;
                }
            }

            return Runs.CompareTo(other.Runs);
        }

        public override string ToString()
        {
            string outname = HowOut == Wicket.NotOut ? " not out" : "";

            if (string.IsNullOrEmpty(Opposition))
            {
                return Runs + outname + " vs unknown opposition";
            }

            return Runs + outname + " vs " + Opposition + " on " + Date.ToUkDateString();
        }

        public static string[] Headers => new string[] { "Runs", "Name", "Opposition", "Date", "Game Type", "Location", "Team Total Score" };

        public string[] ArrayOfValues()
        {
            return new string[]
                {
                    Runs.ToString(),
                    Name.ToString(),
                    Opposition.ToString(),
                    Date.ToUkDateString(),
                    GameType.ToString(),
                    Location.ToString(),
                    TeamTotalScore.ToString()
                };
        }
    }
}
