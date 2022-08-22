using System;
using System.Collections.Generic;
using System.Linq;

using Common.Structure.Extensions;

using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Collection;

namespace CricketStructures.Statistics.Implementation.Player.Batting
{
    public sealed class RunsRecord : ISeasonAggregateStat<PlayerBattingRecord>, IMatchAggregateStat<PlayerBattingRecord>
    {
        private readonly bool fIsHighest;
        private int fMinimum;
        public string Title
        {
            get
            {
                if (fIsHighest)
                {
                    return "Most Club Runs";
                }

                return fMinimum == 0 ? "Yearly Batting Record" : $"Over {fMinimum} runs in a season";
            }
        }

        public PlayerName Name
        {
            get; private set;
        }

        public IReadOnlyList<string> Headers
        {
            get
            {
                if (fIsHighest)
                {
                    return new string[] { "Name", "Start Year", "End Year", "Runs Scored" };
                }

                return PlayerBattingRecord.Headers(Name == null, true);
            }
        }

        public Func<PlayerBattingRecord, IReadOnlyList<string>> OutputValueSelector
        {
            get
            {
                if (fIsHighest)
                {
                    return value => new string[]
                    {
                        value.Name.ToString(),
                        value.StartYear.ToUkDateString(),
                        value.EndYear.ToUkDateString(),
                        value.TotalRuns.ToString()
                    };
                }

                return value => value.Values(Name == null, true);
            }
        }

        public Func<PlayerName, string, ICricketSeason, MatchType[], PlayerBattingRecord> StatGenerator =>
            (name, teamName, season, matchTypes) => CricketStatsFactory.Generate(CricketStatTypes.PlayerBattingRecord, teamName, season, matchTypes, name) as PlayerBattingRecord;

        public Action<string, ICricketMatch, List<PlayerBattingRecord>> AddStatsAction => UpdateStats;
        public void UpdateStats(string teamName, ICricketMatch match, List<PlayerBattingRecord> stats)
        {
            CricketStatsHelpers.BattingIterator(
                match,
                teamName,
                entry => UpdateRuns(entry));
            void UpdateRuns(BattingEntry batting)
            {
                var playerRuns = stats.FirstOrDefault(run => run.Name.Equals(batting.Name));
                if (playerRuns != null)
                {
                    playerRuns.UpdateStats(teamName, match);
                }
                else
                {
                    var newStat = new PlayerBattingRecord(batting.Name);
                    newStat.UpdateStats(teamName, match);
                    stats.Add(newStat);
                }
            }
        }

        public Func<PlayerBattingRecord, bool> SelectorFunc => Selector;
        bool Selector(PlayerBattingRecord playerStat)
        {
            if (fIsHighest)
            {
                return true;
            }

            return playerStat.TotalRuns > fMinimum;
        }

        public Comparison<PlayerBattingRecord> Comparison
        {
            get
            {
                if (fIsHighest)
                {
                    return (a, b) => b.TotalRuns.CompareTo(a.TotalRuns);
                }

                if (fMinimum <= 0)
                {
                    return (a, b) => a.StartYear.CompareTo(b.StartYear);
                }
                else
                {
                    return (a, b) => b.TotalRuns.CompareTo(a.TotalRuns);
                }
            }
        }

        public RunsRecord(bool isHighest, int minimum, PlayerName name)
        {
            fIsHighest = isHighest;
            fMinimum = minimum;
            Name = name;
        }

        public bool IncreaseStatScope()
        {
            if (fIsHighest)
            {
                return true;
            }

            if (fMinimum == 0)
            {
                return true;
            }

            fMinimum -= 5;
            return fMinimum <= 0;
        }
    }
}
