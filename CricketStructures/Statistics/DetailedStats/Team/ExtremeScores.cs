using System.Collections.Generic;
using System.Linq;
using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Season;
using Common.Structure.ReportWriting;
using System.Text;

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
            if (teamScore.Runs > 0 && teamScore.Runs <= 25)
            {
                ScoresUnder25.Add(new TeamScore(teamScore, match.MatchData));
                ScoresUnder25.Sort((a, b) => a.Score.CompareTo(b.Score));
            }

            InningsScore oppoScore = match.GetInnings(teamName, batting: false).BowlingScore();
            if (oppoScore.Runs >= 200)
            {
                OppositionScoresOver200.Add(new TeamScore(oppoScore, match.MatchData));
                OppositionScoresOver200.Sort((a, b) => b.Score.CompareTo(a.Score));
            }
            if (oppoScore.Runs > 0 && oppoScore.Runs <= 25)
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

            if (match.BattedFirst(teamName) && teamScore.Runs > 0 && teamScore.Runs < 100 && match.Result != ResultType.Loss)
            {
                LowestScoresBattingFirstNotLose.Add(new TeamScore(teamScore, match.MatchData));
                LowestScoresBattingFirstNotLose.Sort((a, b) => a.Score.CompareTo(b.Score));
            }
        }

        public void ExportStats(StringBuilder writer, DocumentType exportType)
        {
            if (ScoresOver200.Any())
            {
                TextWriting.WriteTitle(writer, exportType, "Scores Over 200", DocumentElement.h2);
                TableWriting.WriteTable(writer, exportType, ScoresOver200, headerFirstColumn: false);
            }

            if (OppositionScoresOver200.Any())
            {
                TextWriting.WriteTitle(writer, exportType, "Opposition scores Over 200", DocumentElement.h2);
                TableWriting.WriteTable(writer, exportType, OppositionScoresOver200, headerFirstColumn: false);
            }

            if (BothScoresOver200.Any())
            {
                TextWriting.WriteTitle(writer, exportType, "Both Team scores Over 200", DocumentElement.h2);
                TableWriting.WriteTable(writer, exportType, BothScoresOver200, headerFirstColumn: false);
            }

            if (ScoresUnder25.Any())
            {
                TextWriting.WriteTitle(writer, exportType, "Scores Under 25");
                TableWriting.WriteTable(writer, exportType, ScoresUnder25, headerFirstColumn: false);
            }

            if (OppositionScoresUnder25.Any())
            {
                TextWriting.WriteTitle(writer, exportType, "Opposition scores Under 25");
                TableWriting.WriteTable(writer, exportType, OppositionScoresUnder25, headerFirstColumn: false);
            }

            if (HighestScoresBattingSecond.Any())
            {
                TextWriting.WriteTitle(writer, exportType, "Highest Scores batting second");
                TableWriting.WriteTable(writer, exportType, HighestScoresBattingSecond, headerFirstColumn: false);
            }

            if (LowestScoresBattingFirstNotLose.Any())
            {
                TextWriting.WriteTitle(writer, exportType, "Lowest Scores Batting first not to lose");
                TableWriting.WriteTable(writer, exportType, LowestScoresBattingFirstNotLose, headerFirstColumn: false);
            }
        }
    }
}
