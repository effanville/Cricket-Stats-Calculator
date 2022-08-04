using System;

using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;

namespace CricketStructures.Statistics.Implementation.Player.Model
{
    internal sealed class PlayerScore
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

        public PlayerScore()
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
    }
}
