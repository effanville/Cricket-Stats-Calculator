using System;
using Cricket.Interfaces;
using Cricket.Match;
using Cricket.Player;
using Common.Structure.Extensions;

namespace Cricket.Statistics.DetailedStats
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

        public Location HomeOrAway
        {
            get;
            set;
        }

        public BattingWinningMargin()
        {
        }

        public BattingWinningMargin(ICricketMatch match, bool isTeam = true)
        {
            Opposition = match.MatchData.Opposition;
            Date = match.MatchData.Date;
            HomeOrAway = match.MatchData.HomeOrAway;

            if (isTeam)
            {
                Score = match.Batting.Score();
                BatsmanOne = match.Batting.BattingInfo[0].Name;
                BatsmanTwo = match.Batting.BattingInfo[1].Name;
            }
            else
            {
                Score = match.Bowling.Score();
            }
        }

        public string ToCSVLine()
        {
            return Score.ToString() + "," + Opposition + "," + Date.ToUkDateString() + "," + Date.ToUkDateString() + "," + HomeOrAway + "," + BatsmanOne?.ToString() + "," + BatsmanTwo?.ToString();
        }
    }
}
