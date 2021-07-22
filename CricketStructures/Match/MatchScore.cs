using System;
using CricketStructures.Match.Innings;
using Common.Structure.Extensions;

namespace CricketStructures.Match
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

        public MatchScore()
        {
        }

        public MatchScore(string teamName, ICricketMatch match)
        {
            Date = match.MatchData.Date;
            Result = match.Result;
            if (match.BattedFirst(teamName))
            {
                FirstInnings = match.FirstInnings.BattingScore();
                FirstInningsTeam = teamName;

                SecondInnings = match.SecondInnings.BattingScore();
                SecondInningsTeam = match.MatchData.OppositionName(teamName);
            }
            else
            {
                FirstInnings = match.FirstInnings.BattingScore();
                FirstInningsTeam = match.MatchData.OppositionName(teamName);
                SecondInnings = match.SecondInnings.BattingScore();
                SecondInningsTeam = teamName;
            }
        }

        public string ToCSVLine()
        {
            return FirstInningsTeam + "," + FirstInnings.ToString() + "," + SecondInningsTeam + "," + SecondInnings.ToString() + "," + Date.ToUkDateString();
        }
    }
}
