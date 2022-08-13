﻿using System;
using System.Collections.Generic;
using System.Linq;

using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;

namespace CricketStructures.Statistics.Implementation.Player.Batting
{
    public sealed class CarryingBat : IMatchAggregateStat<PlayerScore>
    {
        public string Title => "Carrying of Bat";

        public PlayerName Name
        {
            get; private set;
        }

        public IReadOnlyList<string> Headers => PlayerScore.Headers;

        public Func<PlayerScore, string[]> OutputValueSelector => value => value.ArrayOfValues();

        public Action<PlayerName, string, ICricketMatch, List<PlayerScore>> AddStatsAction => Create;

        void Create(PlayerName name, string teamName, ICricketMatch match, List<PlayerScore> stats)
        {
            var innings = match.GetInnings(teamName, batting: true);
            var battingInnings = innings?.Batting;
            if (battingInnings == null)
            {
                return;
            }

            if (name != null && !(battingInnings[0].Name.Equals(name) || battingInnings[1].Name.Equals(name)))
            {
                return;
            }

            bool battedFirst = match.BattedFirst(teamName);
            if (battedFirst || !battedFirst && match.Result != ResultType.Win)
            {
                if (battingInnings.Any())
                {
                    BattingEntry bat = battingInnings[0];
                    if (!bat.Out())
                    {
                        stats.Add(new PlayerScore(teamName,
                            bat,
                            match.MatchData,
                            innings.BattingScore()));
                    }

                    bat = battingInnings[1];
                    if (!bat.Out())
                    {
                        stats.Add(new PlayerScore(
                            teamName,
                            bat,
                            match.MatchData,
                            innings.BattingScore()));
                    }
                }
            }
        }

        public Comparison<PlayerScore> Comparison => (a, b) => a.Runs.CompareTo(b.Runs);

        public CarryingBat(PlayerName name)
        {
            Name = name;
        }

        public bool IncreaseStatScope()
        {
            return true;
        }
    }
}
