using System.Collections.Generic;
using System.IO;
using System.Linq;
using CricketStructures.Interfaces;
using CricketStructures.Match;
using CricketStructures.Match.Innings;
using StructureCommon.FileAccess;

namespace CricketStructures.Statistics.DetailedStats
{
    public class ExtremeScores
    {/// <summary>
     /// List of all scores of the team which were over 200.
     /// </summary>
        public List<TeamScore> ScoresOver200
        {
            get;
            set;
        } = new List<TeamScore>();

        /// <summary>
        /// List of all scores of the opposing team which were over 200.
        /// </summary>
        public List<TeamScore> OppositionScoresOver200
        {
            get;
            set;
        } = new List<TeamScore>();

        /// <summary>
        /// List of all matches where both teams scored over 200.
        /// </summary>
        public List<MatchScore> BothScoresOver200
        {
            get;
            set;
        } = new List<MatchScore>();

        /// <summary>
        /// List of all scores by the team which were under 25.
        /// </summary>
        public List<TeamScore> ScoresUnder25
        {
            get;
            set;
        } = new List<TeamScore>();

        /// <summary>
        /// List of all scores by the opposing team under 25.
        /// </summary>
        public List<TeamScore> OppositionScoresUnder25
        {
            get;
            set;
        } = new List<TeamScore>();

        /// <summary>
        /// Scores batting second over 200
        /// </summary>
        public List<TeamScore> HighestScoresBattingSecond
        {
            get;
            set;
        } = new List<TeamScore>();

        /// <summary>
        /// Scores under 100 in which the team didnt lose.
        /// </summary>
        public List<TeamScore> LowestScoresBattingFirstNotLose
        {
            get;
            set;
        } = new List<TeamScore>();

        public ExtremeScores()
        {
        }

        public void CalculateStats(ICricketTeam team)
        {
            foreach (ICricketSeason season in team.Seasons)
            {
                CalculateStats(team.TeamName, season);
            }
        }

        public void CalculateStats(string teamName, ICricketSeason season)
        {
            foreach (ICricketMatch match in season.Matches)
            {
                UpdateStats(teamName, match);
            }
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            InningsScore teamScore = match.GetInnings(teamName, batting: true).BattingScore();
            if (teamScore.Runs >= 200)
            {
                ScoresOver200.Add(new TeamScore(teamScore, match.MatchData));
                ScoresOver200.Sort((a, b) => b.Score.CompareTo(a.Score));
            }
            if (teamScore.Runs <= 25)
            {
                ScoresUnder25.Add(new TeamScore(teamScore, match.MatchData));
                ScoresUnder25.Sort((a, b) => a.Score.CompareTo(b.Score));
            }

            InningsScore oppoScore = match.GetInnings(teamName, batting: false).BattingScore();
            if (oppoScore.Runs >= 200)
            {
                OppositionScoresOver200.Add(new TeamScore(oppoScore, match.MatchData));
                OppositionScoresOver200.Sort((a, b) => b.Score.CompareTo(a.Score));
            }
            if (oppoScore.Runs <= 25)
            {
                OppositionScoresUnder25.Add(new TeamScore(oppoScore, match.MatchData));
                OppositionScoresUnder25.Sort((a, b) => a.Score.CompareTo(b.Score));
            }

            if (teamScore.Runs > 200 && oppoScore.Runs > 200)
            {
                BothScoresOver200.Add(new MatchScore(teamName, match));
            }

            if (!match.BattedFirst(teamName) && teamScore.Runs > 200)
            {
                HighestScoresBattingSecond.Add(new TeamScore(teamScore, match.MatchData));
                HighestScoresBattingSecond.Sort((a, b) => b.Score.CompareTo(a.Score));
            }

            if (match.BattedFirst(teamName) && teamScore.Runs < 100 && match.Result != ResultType.Loss)
            {
                LowestScoresBattingFirstNotLose.Add(new TeamScore(teamScore, match.MatchData));
                LowestScoresBattingFirstNotLose.Sort((a, b) => a.Score.CompareTo(b.Score));
            }
        }

        public void ExportStats(StreamWriter writer, ExportType exportType)
        {
            if (ScoresOver200.Any())
            {
                FileWritingSupport.WriteTitle(writer, exportType, "Scores Over 200", HtmlTag.h2);
                FileWritingSupport.WriteTable(writer, exportType, ScoresOver200, headerFirstColumn: false);
            }

            if (OppositionScoresOver200.Any())
            {
                FileWritingSupport.WriteTitle(writer, exportType, "Opposition scores Over 200", HtmlTag.h2);
                FileWritingSupport.WriteTable(writer, exportType, OppositionScoresOver200, headerFirstColumn: false);
            }

            if (BothScoresOver200.Any())
            {
                FileWritingSupport.WriteTitle(writer, exportType, "Both Team scores Over 200", HtmlTag.h2);
                FileWritingSupport.WriteTable(writer, exportType, BothScoresOver200, headerFirstColumn: false);
            }

            if (ScoresUnder25.Any())
            {
                FileWritingSupport.WriteTitle(writer, exportType, "Scores Under 25");
                FileWritingSupport.WriteTable(writer, exportType, ScoresUnder25, headerFirstColumn: false);
            }

            if (OppositionScoresUnder25.Any())
            {
                FileWritingSupport.WriteTitle(writer, exportType, "Opposition scores Under 25");
                FileWritingSupport.WriteTable(writer, exportType, OppositionScoresUnder25, headerFirstColumn: false);
            }

            if (HighestScoresBattingSecond.Any())
            {
                FileWritingSupport.WriteTitle(writer, exportType, "Highest Scores batting second");
                FileWritingSupport.WriteTable(writer, exportType, HighestScoresBattingSecond, headerFirstColumn: false);
            }

            if (LowestScoresBattingFirstNotLose.Any())
            {
                FileWritingSupport.WriteTitle(writer, exportType, "Lowest Scores Batting first not to lose");
                FileWritingSupport.WriteTable(writer, exportType, LowestScoresBattingFirstNotLose, headerFirstColumn: false);
            }
        }
    }
}
