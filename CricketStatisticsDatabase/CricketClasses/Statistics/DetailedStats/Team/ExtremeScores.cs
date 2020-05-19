using Cricket.Interfaces;
using Cricket.Match;
using ExportHelpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cricket.Statistics.DetailedStats
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
            foreach (var season in team.Seasons)
            {
                CalculateStats(season);
            }
        }

        public void CalculateStats(ICricketSeason season)
        {
            foreach (var match in season.Matches)
            {
                UpdateStats(match);
            }
        }

        public void UpdateStats(ICricketMatch match)
        {
            var teamScore = match.Batting.Score();
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

            var oppoScore = match.Bowling.Score();
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

            if (match.Batting.Score().Runs > 200 && match.Bowling.Score().Runs > 200)
            {
                BothScoresOver200.Add(new MatchScore(match));
            }

            if (match.BattingFirstOrSecond == Match.TeamInnings.Second && match.Batting.Score().Runs > 200)
            {
                HighestScoresBattingSecond.Add(new TeamScore(match.Batting.Score(), match.MatchData));
                HighestScoresBattingSecond.Sort((a, b) => b.Score.CompareTo(a.Score));
            }

            if (match.BattingFirstOrSecond == Match.TeamInnings.First && match.Batting.Score().Runs < 100 && match.Result != ResultType.Loss)
            {
                LowestScoresBattingFirstNotLose.Add(new TeamScore(match.Batting.Score(), match.MatchData));
                LowestScoresBattingFirstNotLose.Sort((a, b) => a.Score.CompareTo(b.Score));
            }
        }

        public void ExportStats(StreamWriter writer)
        {
            var headerDummy = new TeamScore();
            if (ScoresOver200.Any())
            {
                writer.WriteLine("");
                writer.WriteLine("Scores Over 200");
                writer.WriteLine("");


                writer.WriteLine(GenericHeaderWriter.TableHeader(headerDummy, ","));
                foreach (var record in ScoresOver200)
                {
                    writer.WriteLine(record.ToCSVLine());
                }
            }

            if (OppositionScoresOver200.Any())
            {
                writer.WriteLine("");
                writer.WriteLine("Opposition Scores Over 200");
                writer.WriteLine("");

                writer.WriteLine(GenericHeaderWriter.TableHeader(headerDummy, ","));
                foreach (var record in OppositionScoresOver200)
                {
                    writer.WriteLine(record.ToCSVLine());
                }
            }

            if (BothScoresOver200.Any())
            {
                writer.WriteLine("");
                writer.WriteLine("Both Scores Over 200");
                writer.WriteLine("");

                writer.WriteLine(GenericHeaderWriter.TableHeader(new MatchScore(), ","));
                foreach (var record in BothScoresOver200)
                {
                    writer.WriteLine(record.ToCSVLine());
                }
            }

            if (ScoresUnder25.Any())
            {
                writer.WriteLine("");
                writer.WriteLine("Scores Under 25");
                writer.WriteLine("");

                writer.WriteLine(GenericHeaderWriter.TableHeader(headerDummy, ","));
                foreach (var record in ScoresUnder25)
                {
                    writer.WriteLine(record.ToCSVLine());
                }
            }

            if (OppositionScoresUnder25.Any())
            {
                writer.WriteLine("");
                writer.WriteLine("Opposition scores Under 25");
                writer.WriteLine("");

                writer.WriteLine(GenericHeaderWriter.TableHeader(headerDummy, ","));
                foreach (var record in OppositionScoresUnder25)
                {
                    writer.WriteLine(record.ToCSVLine());
                }
            }

            if (HighestScoresBattingSecond.Any())
            {
                writer.WriteLine("");
                writer.WriteLine("Highest Scores batting second");
                writer.WriteLine("");

                writer.WriteLine(GenericHeaderWriter.TableHeader(headerDummy, ","));
                foreach (var record in HighestScoresBattingSecond)
                {
                    writer.WriteLine(record.ToCSVLine());
                }
            }

            if (LowestScoresBattingFirstNotLose.Any())
            {
                writer.WriteLine("");
                writer.WriteLine("Lowest scores batting first not to lose");
                writer.WriteLine("");

                writer.WriteLine(GenericHeaderWriter.TableHeader(headerDummy, ","));
                foreach (var record in LowestScoresBattingFirstNotLose)
                {
                    writer.WriteLine(record.ToCSVLine());
                }
            }
        }
    }
}
