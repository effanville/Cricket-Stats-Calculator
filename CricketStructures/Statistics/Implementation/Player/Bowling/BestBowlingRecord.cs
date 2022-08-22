using System;
using System.Collections.Generic;

using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;
using CricketStructures.Statistics.Implementation.Collection;

namespace CricketStructures.Statistics.Implementation.Player.Bowling
{
    internal sealed class BestBowlingRecord : IMatchAggregateStat<BowlingPerformance>
    {
        private int fMinimum;

        public string Title => fMinimum == 0 ? "All Bowling Performances" : $"Bowling performances over {fMinimum} wickets";

        public PlayerName Name
        {
            get; private set;
        }

        public IReadOnlyList<string> Headers
        {
            get
            {
                if (Name != null)
                {
                    return BowlingPerformance.PlayerHeaders;
                }
                else
                {
                    return BowlingPerformance.Headers;
                }
            }
        }

        public Func<BowlingPerformance, IReadOnlyList<string>> OutputValueSelector
        {
            get
            {
                if (Name != null)
                {
                    return value => value.ArrayOfPlayerValues();
                }
                else
                {
                    return value => value.ArrayOfValues();
                }
            }
        }

        public Action<string, ICricketMatch, List<BowlingPerformance>> AddStatsAction => UpdateStats;
        public void UpdateStats(string teamName, ICricketMatch match, List<BowlingPerformance> stats)
        {
            CricketStatsHelpers.BowlingIterator(
                match,
                teamName,
                AddManyWickets);
            void AddManyWickets(BowlingEntry bowlingEntry)
            {
                if (Name == null || bowlingEntry.Name.Equals(Name))
                {
                    if (bowlingEntry.Wickets >= fMinimum)
                    {
                        stats.Add(new BowlingPerformance(teamName, bowlingEntry, match.MatchData));
                    }
                }
            }
        }

        public Comparison<BowlingPerformance> Comparison => (a, b) => b.Wickets.CompareTo(a.Wickets);

        public BestBowlingRecord(int minimum, PlayerName name)
        {
            fMinimum = minimum;
            Name = name;
        }

        public bool IncreaseStatScope()
        {
            fMinimum--;
            return fMinimum <= 0;
            throw new NotImplementedException();
        }
    }
}
