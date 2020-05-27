using System;
using Cricket.Interfaces;
using Cricket.Match;
using StructureCommon.Extensions;

namespace Cricket.Statistics.DetailedStats
{
    public class BowlingWinningMargin
    {
        public int WinningRuns
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

        public MatchType GameType
        {
            get;
            set;
        }

        public BowlingWinningMargin()
        {
        }

        public BowlingWinningMargin(ICricketMatch match, bool isTeam = true)
        {
            Opposition = match.MatchData.Opposition;
            Date = match.MatchData.Date;
            HomeOrAway = match.MatchData.HomeOrAway;
            GameType = match.MatchData.Type;

            if (isTeam)
            {
                WinningRuns = match.Batting.Score().Runs - match.Bowling.Score().Runs;
            }
            else
            {
                WinningRuns = match.Bowling.Score().Runs - match.Batting.Score().Runs;
            }
        }

        public string ToCSVLine()
        {
            return WinningRuns + "," + Opposition + "," + Date.ToUkDateString() + "," + HomeOrAway + "," + GameType.ToString();
        }
    }
}
