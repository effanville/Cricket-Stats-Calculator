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
    /// Generate a season record of number of wickets.
    /// </summary>
    public sealed class WicketRecord : ISeasonAggregateStat<PlayerBowlingRecord>, IMatchAggregateStat<PlayerBowlingRecord>
    {
        private readonly bool fIsHighest;
        private readonly bool YearCompare;
        private int fMinimum;

        /// <inheritdoc/>
        public string Title
        {
            get
            {
                if (fIsHighest)
                {
                    return "Most Wickets Taken";
                }
                return fMinimum == 0 ? "Bowling Record" : $"Over {fMinimum} wickets in a season";
            }
        }

        /// <inheritdoc/>
        public PlayerName Name
        {
            get; private set;
        }

        /// <inheritdoc/>
        public IReadOnlyList<string> Headers
        {
            get
            {
                if (fIsHighest)
                {
                    return new string[] { "Name", "StartYear", "End Year", "Wickets" };
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
                        value.TotalWickets.ToString()
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
                && playerStat.TotalOvers > 0
                && playerStat.TotalWickets > fMinimum;
        }

        /// <inheritdoc/>
        public Comparison<PlayerBowlingRecord> Comparison
        {
            get
            {
                if (YearCompare)
                {
                    return (a, b) => a.StartYear.CompareTo(b.StartYear);
                }
                else
                {
                    return (a, b) => b.TotalWickets.CompareTo(a.TotalWickets);
                }
            }
        }

        public WicketRecord(bool isHighest, int minimum, bool yearCompare, PlayerName name)
        {
            fIsHighest = isHighest;
            YearCompare = yearCompare;
            fMinimum = minimum;
            Name = name;
        }

        /// <inheritdoc/>
        public bool IncreaseStatScope()
        {
            if (fMinimum == 0)
            {
                return true;
            }

            fMinimum--;
            return fMinimum <= 0;
        }
    }
}
