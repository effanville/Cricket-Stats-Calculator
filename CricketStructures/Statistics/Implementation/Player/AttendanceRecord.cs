using System;
using System.Collections.Generic;
using System.Linq;

using Common.Structure.Extensions;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Collection;

namespace CricketStructures.Statistics.Implementation.Player
{
    public sealed class AttendanceRecord : ISeasonAggregateStat<PlayerAttendanceRecord>, IMatchAggregateStat<PlayerAttendanceRecord>
    {
        private readonly bool fIsTotal;
        private int fMinimum;

        /// <inheritdoc/>
        public PlayerName Name
        {
            get;
            private set;
        }

        /// <inheritdoc/>
        public string Title
        {
            get
            {
                if (fIsTotal)
                {
                    return "Most Club Appearances";
                }

                return "Yearly Attendance Records";
            }
        }

        /// <inheritdoc/>
        public IReadOnlyList<string> Headers
        {
            get
            {
                if (fIsTotal)
                {
                    return new string[] { "Name", "StartDate", "EndDate", "Appearances" };
                }

                return PlayerAttendanceRecord.Headers(Name == null, true);
            }
        }

        /// <inheritdoc/>
        public Func<PlayerAttendanceRecord, IReadOnlyList<string>> OutputValueSelector
        {
            get
            {
                if (fIsTotal)
                {
                    return value => new string[]
                    {
                        value.Name.ToString(),
                        value.StartYear.ToUkDateString(),
                        value.EndYear.ToUkDateString(),
                        value.MatchesPlayed.ToString()
                    };
                }
                return value => value.Values(Name == null, true);
            }
        }

        /// <inheritdoc/>
        public Func<PlayerName, string, ICricketSeason, MatchType[], PlayerAttendanceRecord> StatGenerator =>
            (name, teamName, season, matchTypes) => CricketStatsFactory.Generate(CricketStatTypes.PlayerAttendanceRecord, teamName, season, matchTypes, name) as PlayerAttendanceRecord;

        public Action<string, ICricketMatch, List<PlayerAttendanceRecord>> AddStatsAction => UpdateStats;

        public void UpdateStats(string teamName, ICricketMatch match, List<PlayerAttendanceRecord> stats)
        {
            foreach (var playerName in match.Players(teamName))
            {
                var playerApps = stats.FirstOrDefault(run => run.Name.Equals(playerName));
                if (playerApps != null)
                {
                    playerApps.UpdateStats(teamName, match);
                }
                else
                {
                    var newStats = new PlayerAttendanceRecord(playerName);
                    newStats.UpdateStats(teamName, match);
                    stats.Add(newStats);
                }
            }
        }

        /// <inheritdoc/>
        public Func<PlayerAttendanceRecord, bool> SelectorFunc => Selector;
        bool Selector(PlayerAttendanceRecord playerStat)
        {
            return playerStat.MatchesPlayed > fMinimum;
        }

        /// <inheritdoc/>
        public Comparison<PlayerAttendanceRecord> Comparison
        {
            get
            {
                if (fIsTotal)
                {
                    return (a, b) => b.MatchesPlayed.CompareTo(a.MatchesPlayed);
                }

                return (a, b) => a.StartYear.CompareTo(b.StartYear);
            }
        }

        public AttendanceRecord()
        {
        }

        public AttendanceRecord(bool isTotal, int minimum, PlayerName name)
        {
            fIsTotal = isTotal;
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

            fMinimum -= 5;
            return fMinimum <= 0;
        }
    }
}
