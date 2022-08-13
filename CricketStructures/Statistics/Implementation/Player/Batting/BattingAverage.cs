using System;
using System.Collections.Generic;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;

namespace CricketStructures.Statistics.Implementation.Player.Batting
{
    internal sealed class BattingAverage : ISeasonAggregateStat<PlayerBattingRecord>
    {
        private int fMinimum;

        public string Title => $"Average over {fMinimum} in a season";

        public PlayerName Name
        {
            get; private set;
        }

        IReadOnlyList<string> ISeasonAggregateStat<PlayerBattingRecord>.Headers
        {
            get
            {
                if (Name != null)
                {
                    return PlayerBattingRecord.PlayerHeaders;
                }
                else
                {
                    return PlayerBattingRecord.Headers;
                }
            }
        }

        public Func<PlayerBattingRecord, string[]> OutputValueSelector
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

        public Func<PlayerName, string, ICricketSeason, MatchType[], PlayerBattingRecord> StatGenerator =>
        (name, teamName, season, matchTypes) => CricketStatsFactory.Generate(CricketStatTypes.PlayerBattingRecord, teamName, season, matchTypes, name) as PlayerBattingRecord;

        public Func<PlayerBattingRecord, bool> SelectorFunc => Selector;
        bool Selector(PlayerBattingRecord playerStat)
        {

            return playerStat.MatchesPlayed > 5
                && playerStat.Average > fMinimum;
        }

        public Comparison<PlayerBattingRecord> Comparison => (a, b) => b.Average.CompareTo(a.Average);

        public BattingAverage(int minimum, PlayerName name)
        {
            fMinimum = minimum;
            Name = name;
        }

        public bool IncreaseStatScope()
        {
            fMinimum -= 4;
            return fMinimum <= 0;
        }
    }
}
