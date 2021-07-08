using System;
using CricketStructures.Interfaces;
using CricketStructures.Player;
using StructureCommon.Extensions;
using CricketStructures.Match.Innings;

namespace CricketStructures.Statistics.DetailedStats
{
    public class BattingWinningMargin
    {
        public InningsScore Score
        {
            get;
            set;
        }

        public PlayerName BatsmanOne
        {
            get;
            set;
        }

        public PlayerName BatsmanTwo
        {
            get;
            set;
        }

        public DateTime Date
        {
            get;
            set;
        }

        public string Opposition
        {
            get;
            set;
        }

        public string Location
        {
            get;
            set;
        }

        public BattingWinningMargin()
        {
        }
        public BattingWinningMargin(string teamName, ICricketMatch match)
        {
            Opposition = match.MatchData.OppositionName();
            Date = match.MatchData.Date;
            Location = match.MatchData.Location;

            Score = match.Score(teamName);
            if (!teamName.Equals(Opposition))
            {
                var batting = match.GetInnings(teamName, batting: true).Batting;
                BatsmanOne = batting[0].Name;
                BatsmanTwo = batting[1].Name;
            }
        }

        public string ToCSVLine()
        {
            return Score.ToString() + "," + Opposition + "," + Date.ToUkDateString() + "," + Date.ToUkDateString() + "," + Location + "," + BatsmanOne?.ToString() + "," + BatsmanTwo?.ToString();
        }
    }
}
