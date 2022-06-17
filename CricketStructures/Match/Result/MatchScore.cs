using System;

using CricketStructures.Match.Innings;

namespace CricketStructures.Match.Result
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

        public ResultType Result
        {
            get;
            set;
        }

        public string Location
        {
            get;
            set;
        }

        public MatchScore()
        {
        }

        public MatchScore(string teamName, ICricketMatch match)
        {
            Date = match.MatchData.Date;
            Result = match.Result;
            Location = match.MatchData.Location;
            if (match.BattedFirst(teamName))
            {
                FirstInnings = match.FirstInnings.BattingScore();
                FirstInningsTeam = teamName;

                SecondInnings = match.SecondInnings.BowlingScore();
                SecondInningsTeam = match.MatchData.OppositionName(teamName);
            }
            else
            {
                FirstInnings = match.FirstInnings.BowlingScore();
                FirstInningsTeam = match.MatchData.OppositionName(teamName);
                SecondInnings = match.SecondInnings.BattingScore();
                SecondInningsTeam = teamName;
            }
        }
    }
}
