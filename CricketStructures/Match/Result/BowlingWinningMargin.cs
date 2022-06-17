﻿using System;

namespace CricketStructures.Match.Result
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
            WinningRuns = Math.Abs(match.Score(teamName).Runs - match.Score(Opposition).Runs);
        }
    }
}