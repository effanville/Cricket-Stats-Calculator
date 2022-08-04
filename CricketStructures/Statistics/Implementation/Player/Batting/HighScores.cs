using System;
using System.Collections.Generic;
using System.Linq;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Player.Model;

namespace CricketStructures.Statistics.Implementation.Player.Batting
{
    internal class HighScores : ICricketStat
    {
        private int MinimumScoreValue;
        private readonly PlayerName Name;

        public List<PlayerScore> Centuries
        {
            get;
            set;
        } = new List<PlayerScore>();

        public HighScores()
        {
        }

        public HighScores(int minimumScoreValue)
            : this()
        {
            MinimumScoreValue = minimumScoreValue;
        }

        public HighScores(int minimumScoreValue, PlayerName name)
            : this(minimumScoreValue)
        {
            Name = name;
        }

        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            // try to display many scores.
            int minimumNumbertoDisplay = 10;
            int numberEntries = 0;
            int previousMinNumber = MinimumScoreValue;
            do
            {
                CricketStatsHelpers.SeasonIterator(
                team.Seasons,
                season => CalculateStats(team.TeamName, season, matchTypes),
                preCycleAction: ResetStats);
                numberEntries = Centuries.Count;
                if (numberEntries < minimumNumbertoDisplay)
                {
                    previousMinNumber = MinimumScoreValue;
                    MinimumScoreValue = Math.Max(0, MinimumScoreValue - 5);
                }
            }
            while (numberEntries < minimumNumbertoDisplay && previousMinNumber != MinimumScoreValue);
        }

        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            CricketStatsHelpers.MatchIterator(
                season,
                matchTypes,
                match => UpdateStats(teamName, match),
                postCycleAction: () => Centuries.Sort((a, b) => b.Runs.CompareTo(a.Runs)));
        }

        public void ResetStats()
        {
            Centuries.Clear();
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            var innings = match.GetInnings(teamName, batting: true);
            var battingInnings = innings?.Batting;
            if (battingInnings == null)
            {
                return;
            }
            foreach (BattingEntry battingEntry in battingInnings)
            {
                if (battingEntry.RunsScored >= MinimumScoreValue && battingEntry.MethodOut != Wicket.DidNotBat)
                {
                    if (Name == null || battingEntry.Name.Equals(Name))
                    {
                        Centuries.Add(new PlayerScore(teamName, battingEntry, match.MatchData, innings.Score()));
                    }
                }
            }
        }

        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            if (Centuries.Any())
            {
                string title = MinimumScoreValue == 0 ? "All Scores" : $"Scores over {MinimumScoreValue}";
                _ = rb.WriteTitle(title, headerElement)
                    .WriteTable(Centuries, headerFirstColumn: false);
            }
        }
    }
}
