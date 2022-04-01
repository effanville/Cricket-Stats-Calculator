using System;
using CricketStructures.Match;
using CricketStructures.Player;
using Common.Structure.Extensions;
using CricketStructures.Match.Innings;
using System.Linq;

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
            Opposition = match.MatchData.OppositionName(teamName);
            Date = match.MatchData.Date;
            Location = match.MatchData.Location;

            var result = match.MatchResult();
            Score = match.Score(result.WinningTeam);
            if (result.HasResult)
            {
                var batting = match.GetInnings(result.WinningTeam, batting: true).Batting;
                if (batting.Any())
                {
                    if (!batting[0].Name.PrimaryName.Contains(CricketConstants.DefaultOppositionPlayerSurname))
                    {
                        BatsmanOne = batting[0].Name;
                        BatsmanTwo = batting[1].Name;
                    }
                }
            }
        }

        public string ToCSVLine()
        {
            return Score.ToString() + "," + Opposition + "," + Date.ToUkDateString() + "," + Date.ToUkDateString() + "," + Location + "," + BatsmanOne?.ToString() + "," + BatsmanTwo?.ToString();
        }
    }
}
