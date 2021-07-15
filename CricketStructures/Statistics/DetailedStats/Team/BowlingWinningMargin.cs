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

        public string Location
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
            Opposition = match.MatchData.OppositionName(teamName);
            Date = match.MatchData.Date;
            Location = match.MatchData.Location;
            GameType = match.MatchData.Type;

            if (teamName.Equals(Opposition))
            {
                WinningRuns = match.Score(teamName).Runs - match.Score(Opposition).Runs;
            }
            else
            {
                WinningRuns = match.Score(Opposition).Runs - match.Score(teamName).Runs;
            }
        }

        public string ToCSVLine()
        {
            return WinningRuns + "," + Opposition + "," + Date.ToUkDateString() + "," + Location + "," + GameType.ToString();
        }
    }
}
