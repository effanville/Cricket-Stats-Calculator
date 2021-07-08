using System;
using CricketStructures.Interfaces;
using CricketStructures.Match;
using StructureCommon.Extensions;

namespace CricketStructures.Statistics.DetailedStats
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

        public bool AtHome
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
        public BowlingWinningMargin(string teamName, ICricketMatch match)
        {
            Opposition = match.MatchData.OppositionName();
            Date = match.MatchData.Date;
            AtHome = match.MatchData.AtHome;
            GameType = match.MatchData.Type;

            if (teamName.Equals(Opposition))
            {
                WinningRuns = match.Score(teamName).Runs - match.Score(match.MatchData.OppositionName()).Runs;
            }
            else
            {
                WinningRuns = match.Score(match.MatchData.OppositionName()).Runs - match.Score(teamName).Runs;
            }
        }

        public string ToCSVLine()
        {
            return WinningRuns + "," + Opposition + "," + Date.ToUkDateString() + "," + AtHome + "," + GameType.ToString();
        }
    }
}
