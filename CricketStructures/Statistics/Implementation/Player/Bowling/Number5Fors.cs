﻿using System;
using System.Collections.Generic;
using System.Linq;

using Common.Structure.NamingStructures;
using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;
using CricketStructures.Season;

namespace CricketStructures.Statistics.Implementation.Player.Bowling
{
    public sealed class HighWicketHauls : IMatchAggregateStat<NamedRecord<int>>
    {
        private int fMinimum;
        public string Title => $"Individual Wicket hauls over {fMinimum} wickets.";

        public PlayerName Name
        {
            get; private set;
        }

        public IReadOnlyList<string> Headers => new string[] { "Name", "Number Hauls" };

        public Func<NamedRecord<int>, string[]> OutputValueSelector => value => value.ArrayValues();

        public Action<PlayerName, string, ICricketMatch, List<NamedRecord<int>>> AddStatsAction => Create;
        void Create(PlayerName name, string teamName, ICricketMatch match, List<NamedRecord<int>> stats)
        {
            CricketStatsHelpers.BowlingIterator(
                match,
                teamName,
                UpdateFiveFors);
            void UpdateFiveFors(BowlingEntry bowlingEntry)
            {
                if (Name == null || bowlingEntry.Name.Equals(Name))
                {
                    if (bowlingEntry.Wickets >= 5)
                    {
                        if (stats.Any(entry => entry.Name.Equals(bowlingEntry.Name)))
                        {
                            var value = stats.First(entry => entry.Name.Equals(bowlingEntry.Name));
                            value.UpdateValue(1);
                        }
                        else
                        {
                            stats.Add(new NamedRecord<int>("NumberFiveFor", bowlingEntry.Name, 1, Update));
                        }
                    }
                }

                int Update(int a, int b)
                {
                    return a + b;
                }
            }
        }

        public Comparison<NamedRecord<int>> Comparison => NamedRecordComparers.ValueCompare<int>();

        public HighWicketHauls(int minimum, PlayerName name)
        {
            fMinimum = minimum;
            Name = name;
        }

        public bool IncreaseStatScope()
        {
            return true;
        }
    }

    internal class Number5Fors : ICricketStat
    {
        private readonly PlayerName Name;
        public List<NamedRecord<int>> NumberFiveFors
        {
            get;
            set;
        } = new List<NamedRecord<int>>();

        public Number5Fors()
        {
        }

        public Number5Fors(PlayerName name)
        {
            Name = name;
        }

        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            CricketStatsHelpers.SeasonIterator(
                team.Seasons,
                season => CalculateStats(team.TeamName, season, matchTypes));
        }

        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            CricketStatsHelpers.MatchIterator(
                season,
                matchTypes,
                match => UpdateStats(teamName, match),
                postCycleAction: () => NumberFiveFors.Sort((a, b) => b.Value.CompareTo(a.Value)));
        }

        public void ResetStats()
        {
            NumberFiveFors.Clear();
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            CricketStatsHelpers.BowlingIterator(
                match,
                teamName,
                UpdateFiveFors);
            void UpdateFiveFors(BowlingEntry bowlingEntry)
            {
                if (Name == null || bowlingEntry.Name.Equals(Name))
                {
                    if (bowlingEntry.Wickets >= 5)
                    {
                        if (NumberFiveFors.Any(entry => entry.Name.Equals(bowlingEntry.Name)))
                        {
                            var value = NumberFiveFors.First(entry => entry.Name.Equals(bowlingEntry.Name));
                            value.UpdateValue(1);
                        }
                        else
                        {
                            NumberFiveFors.Add(new NamedRecord<int>("NumberFiveFor", bowlingEntry.Name, 1, Update));
                        }
                    }
                }

                int Update(int a, int b)
                {
                    return a + b;
                }
            }
        }

        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            if (NumberFiveFors.Any())
            {
                _ = rb.WriteTitle("Number Five Fors", headerElement)
                    .WriteTableFromEnumerable(new string[] { "Name", "NumberFiveFor" }, NumberFiveFors.Select(value => new string[] { value.Name.ToString(), value.Value.ToString() }), headerFirstColumn: false);
            }
        }
    }
}
