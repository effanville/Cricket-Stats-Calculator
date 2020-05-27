using System;
using Cricket.Interfaces;
using Cricket.Match;
using StructureCommon.Extensions;

namespace Cricket.Statistics.DetailedStats
{
    public class MatchScore
    {
        public string FirstInningsTeam
        {
            get;
            set;
        }

        public InningsScore FirstInnings
        {
            get;
            set;
        }

        public string SecondInningsTeam
        {
            get;
            set;
        }

        public InningsScore SecondInnings
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

        public ResultType Result
        {
            get;
            set;
        }

        public MatchScore()
        {
        }

        public MatchScore(ICricketMatch match)
        {
            Date = match.MatchData.Date;
            HomeOrAway = match.MatchData.HomeOrAway;
            Result = match.Result;

            if (match.BattingFirstOrSecond == TeamInnings.First)
            {
                FirstInnings = match.Batting.Score();
                FirstInningsTeam = "Ardeley Walkern";
                SecondInnings = match.Bowling.Score();
                SecondInningsTeam = match.MatchData.Opposition;
            }

            if (match.BattingFirstOrSecond == TeamInnings.Second)
            {
                FirstInnings = match.Bowling.Score();
                FirstInningsTeam = match.MatchData.Opposition;
                SecondInnings = match.Batting.Score();
                SecondInningsTeam = "Ardeley Walkern";
            }
        }

        public string ToCSVLine()
        {
            return FirstInningsTeam + "," + FirstInnings.ToString() + "," + SecondInningsTeam + "," + SecondInnings.ToString() + "," + Date.ToUkDateString() + "," + HomeOrAway;
        }
    }
}
