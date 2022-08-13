﻿using System;
using System.Collections.Generic;

using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;

namespace CricketStructures.Statistics.Implementation.Player.Batting
{
    public sealed class HighScores : IMatchAggregateStat<PlayerScore>
    {
        private int MinimumScoreValue;
        public string Title => MinimumScoreValue == 0 ? "All Scores" : $"Scores over {MinimumScoreValue}";

        public PlayerName Name
        {
            get; private set;
        }

        public IReadOnlyList<string> Headers => throw new NotImplementedException();

        public Func<PlayerScore, string[]> OutputValueSelector => throw new NotImplementedException();

        public Action<PlayerName, string, ICricketMatch, List<PlayerScore>> AddStatsAction => Create;
        void Create(PlayerName name, string teamName, ICricketMatch match, List<PlayerScore> stats)
        {
            CricketStatsHelpers.BattingIterator(
                match,
                teamName,
                Update);
            void Update(BattingEntry battingEntry, CricketInnings innings)
            {
                if (battingEntry.RunsScored >= MinimumScoreValue && battingEntry.MethodOut.DidBat())
                {
                    if (Name == null || battingEntry.Name.Equals(Name))
                    {
                        stats.Add(new PlayerScore(teamName, battingEntry, match.MatchData, innings.Score()));
                    }
                }
            }
        }

        public Comparison<PlayerScore> Comparison => (a, b) => b.Runs.CompareTo(a.Runs);

        public HighScores(int minScore, PlayerName name)
        {
            MinimumScoreValue = minScore;
            Name = name;
        }

        public bool IncreaseStatScope()
        {
            MinimumScoreValue = Math.Max(0, MinimumScoreValue - 5);
            return MinimumScoreValue == 0;
        }
    }
}
