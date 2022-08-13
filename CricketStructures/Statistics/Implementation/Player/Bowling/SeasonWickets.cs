using System;
using System.Collections.Generic;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;

namespace CricketStructures.Statistics.Implementation.Player.Bowling
{
    public sealed class SeasonWickets : ISeasonAggregateStat<PlayerBowlingRecord>
    {
        private readonly bool YearCompare;
        private int fMinimum;
        public string Title => fMinimum == 0 ? "Bowling Record" : $"Over {fMinimum} wickets in a season";

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
                    return PlayerBowlingRecord.PlayerHeaders;
                }
                else
                {
                    return PlayerBowlingRecord.Headers;
                }
            }
        }

        public Func<PlayerBowlingRecord, string[]> OutputValueSelector
        {
            get
            {
                if (Name != null)
                {
                    return value => value.PlayerArrayValues();
                }
                else
                {
                    return value => value.ArrayValues();
                }
            }
        }

        public Func<PlayerName, string, ICricketSeason, MatchType[], PlayerBowlingRecord> StatGenerator =>
(name, teamName, season, matchTypes) => CricketStatsFactory.Generate(CricketStatTypes.PlayerBowlingRecord, teamName, season, matchTypes, name) as PlayerBowlingRecord;

        public Func<PlayerBowlingRecord, bool> SelectorFunc => Selector;
        bool Selector(PlayerBowlingRecord playerStat)
        {

            return playerStat.MatchesPlayed > 5
                && playerStat.TotalOvers > 0
                && playerStat.TotalWickets > fMinimum;
        }
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

        public SeasonWickets(int minimum, bool yearCompare, PlayerName name)
        {
            YearCompare = yearCompare;
            fMinimum = minimum;
            Name = name;
        }

        public bool IncreaseStatScope()
        {
            fMinimum--;
            return fMinimum == 0;
        }
    }
}
