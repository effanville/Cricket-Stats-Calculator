using System;
using System.Collections.Generic;
using System.Linq;

using Common.Structure.Extensions;

using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Collection;

namespace CricketStructures.Statistics.Implementation.Player.Bowling
{
    /// <summary>
    /// Generate a season record of bowling average.
    /// </summary>
    public sealed class BowlingAverage : ISeasonAggregateStat<PlayerBowlingRecord>, IMatchAggregateStat<PlayerBowlingRecord>
    {
        private readonly bool fIsHighest;
        private readonly double fInitialMaximum;
        private double fMaximum;

        /// <inheritdoc/>
        public string Title
        {
            get
            {
                if (fIsHighest)
                {
                    return "Most Wickets Taken";
                }

                return $"Average under {fMaximum} in a season";
            }
        }

        /// <inheritdoc/>
        public PlayerName Name
        {
            get;
            private set;
        }

        /// <inheritdoc/>
        public IReadOnlyList<string> Headers
        {
            get
            {
                if (fIsHighest)
                {
                    return new string[] { "Name", "StartYear", "End Year", "Bowling Average" };
                }

                return PlayerBowlingRecord.Headers(Name == null, true);
            }
        }

        /// <inheritdoc/>
        public Func<PlayerBowlingRecord, IReadOnlyList<string>> OutputValueSelector
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
                        value.Average.ToString()
                    };
                }

                return value => value.Values(Name == null, true);
            }
        }

        /// <inheritdoc/>
        public Func<PlayerName, string, ICricketSeason, MatchType[], PlayerBowlingRecord> StatGenerator =>
                (name, teamName, season, matchTypes) => CricketStatsFactory.Generate(CricketStatTypes.PlayerBowlingRecord, teamName, season, matchTypes, name) as PlayerBowlingRecord;

        public Action<string, ICricketMatch, List<PlayerBowlingRecord>> AddStatsAction => UpdateStats;
        private void UpdateStats(string teamName, ICricketMatch match, List<PlayerBowlingRecord> stats)
        {
            CricketStatsHelpers.BowlingIterator(match, teamName, entry => CalculateWickets(entry));
            void CalculateWickets(BowlingEntry bowling)
            {
                var playerWickets = stats.FirstOrDefault(run => run.Name.Equals(bowling.Name));
                if (playerWickets != null)
                {
                    playerWickets.UpdateStats(teamName, match);
                }
                else
                {
                    var newStat = new PlayerBowlingRecord(bowling.Name);
                    newStat.UpdateStats(teamName, match);
                    stats.Add(newStat);
                }
            }
        }

        /// <inheritdoc/>
        public Func<PlayerBowlingRecord, bool> SelectorFunc => Selector;
        bool Selector(PlayerBowlingRecord playerStat)
        {
            return playerStat.MatchesPlayed > 5
                && playerStat.TotalWickets > 15
                && playerStat.Average < fMaximum;
        }

        /// <inheritdoc/>
        public Comparison<PlayerBowlingRecord> Comparison => (a, b) => b.Average.CompareTo(a.Average);

        public BowlingAverage(bool isHighest, double maximum, PlayerName name)
        {
            fIsHighest = isHighest;
            fInitialMaximum = maximum;
            fMaximum = maximum;
            Name = name;
        }

        /// <inheritdoc/>
        public bool IncreaseStatScope()
        {
            fMaximum++;
            return fMaximum >= fInitialMaximum + 30.0;
        }
    }
}
