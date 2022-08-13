﻿using System;
using System.Collections.Generic;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;

namespace CricketStructures.Statistics.Implementation.Player.Batting
{
    public sealed class SeasonRunsRecord : ISeasonAggregateStat<PlayerBattingRecord>
    {
        private int fMinimum;
        public string Title => fMinimum == 0 ? "Yearly Batting Record" : $"Over {fMinimum} runs in a season";

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

            return playerStat.TotalRuns > fMinimum;
        }
        public Comparison<PlayerBattingRecord> Comparison
        {
            get
            {
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

        public SeasonRunsRecord(int minimum, PlayerName name)
        {
            fMinimum = minimum;
            Name = name;

        }

        public bool IncreaseStatScope()
        {
            fMinimum -= 5;
            return fMinimum <= 0;
        }
    }
}
