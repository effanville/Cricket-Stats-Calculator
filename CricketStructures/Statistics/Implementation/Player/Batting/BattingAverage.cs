using System;
using System.Collections.Generic;
using System.Linq;

using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Collection;

namespace CricketStructures.Statistics.Implementation.Player.Batting
{
    internal sealed class BattingAverage : ISeasonAggregateStat<PlayerBattingRecord>, IMatchAggregateStat<PlayerBattingRecord>
    {
        private int fMinimum;
        private readonly bool fDisplayHighestOnly = false;

        public string Title
        {
            get
            {
                if (fDisplayHighestOnly)
                {
                    return "Highest Batting Averages";
                }

                return $"Average over {fMinimum} in a season";
            }
        }

        public PlayerName Name
        {
            get;
            private set;
        }

        public IReadOnlyList<string> Headers
        {
            get
            {
                if (fDisplayHighestOnly)
                {
                    return new[] { "Name", "Start Year", "End Year", "Average" };
                }

                return PlayerBattingRecord.Headers(Name == null, true);
            }
        }

        public Func<PlayerBattingRecord, IReadOnlyList<string>> OutputValueSelector
        {
            get
            {
                if (fDisplayHighestOnly)
                {
                    return value => new string[]
                    {
                        value.Name.ToString(),
                        value.StartYear.ToShortDateString(),
                        value.EndYear.ToShortDateString(),
                        value.Average.ToString()
                    };
                }

                return value => value.Values(Name == null, true);
            }
        }

        public Action<string, ICricketMatch, List<PlayerBattingRecord>> AddStatsAction => UpdateStats;
        private void UpdateStats(string teamName, ICricketMatch match, List<PlayerBattingRecord> stats)
        {
            CricketStatsHelpers.BattingIterator(
                match,
                teamName,
                entry => CalculateAverage(entry));
            void CalculateAverage(BattingEntry batting)
            {
                var playerRuns = stats.FirstOrDefault(run => run.Name.Equals(batting.Name));
                if (playerRuns != null)
                {
                    if (batting.MethodOut.DidBat())
                    {
                        playerRuns.UpdateStats(teamName, match);
                    }
                }
                else
                {
                    var newEntry = new PlayerBattingRecord(batting.Name);
                    newEntry.UpdateStats(teamName, match);
                    stats.Add(newEntry);
                }
            }
        }

        public Func<PlayerName, string, ICricketSeason, MatchType[], PlayerBattingRecord> StatGenerator =>
        (name, teamName, season, matchTypes) => CricketStatsFactory.Generate(CricketStatTypes.PlayerBattingRecord, teamName, season, matchTypes, name) as PlayerBattingRecord;

        public Func<PlayerBattingRecord, bool> SelectorFunc => Selector;
        bool Selector(PlayerBattingRecord playerStat)
        {
            if (fDisplayHighestOnly)
            {
                return playerStat.TotalRuns > 500;
            }

            return playerStat.MatchesPlayed > 5
                && playerStat.Average > fMinimum;
        }

        public Comparison<PlayerBattingRecord> Comparison => (a, b) => b.Average.CompareTo(a.Average);

        public BattingAverage(bool highestOnly, int minimum, PlayerName name)
        {
            fDisplayHighestOnly = highestOnly;
            fMinimum = minimum;
            Name = name;
        }

        public bool IncreaseStatScope()
        {
            if (fMinimum <= 0)
            {
                return true;
            }

            fMinimum -= 4;
            return fMinimum <= 0;
        }
    }
}
