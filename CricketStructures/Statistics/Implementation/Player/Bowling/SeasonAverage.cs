using System;
using System.Collections.Generic;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;

namespace CricketStructures.Statistics.Implementation.Player.Bowling
{
    public sealed class SeasonAverage : ISeasonAggregateStat<PlayerBowlingRecord>
    {
        private readonly double fInitialMaximum;
        private double fMaximum;
        public string Title => $"Average under {fMaximum} in a season";

        public PlayerName Name
        {
            get;
            private set;
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
                && playerStat.TotalWickets > 15
                && playerStat.Average < fMaximum;
        }
        public Comparison<PlayerBowlingRecord> Comparison => (a, b) => b.Average.CompareTo(a.Average);

        public SeasonAverage(double maximum, PlayerName name)
        {
            fInitialMaximum = maximum;
            fMaximum = maximum;
            Name = name;
        }

        public bool IncreaseStatScope()
        {
            fMaximum++;
            return fMaximum >= fInitialMaximum + 30.0;
        }
    }
}
