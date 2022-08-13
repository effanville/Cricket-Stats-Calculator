﻿using System;
using System.Collections.Generic;
using System.Linq;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;

namespace CricketStructures.Statistics.Implementation.Collection
{
    /// <summary>
    /// Contains a list of statistics.
    /// </summary>
    /// <typeparam name="T">The specific type of statistic in the list</typeparam>
    internal sealed class TeamAggregateStatList<T> : ICricketStat where T : class
    {
        private readonly string Header;
        private readonly IReadOnlyList<string> Headers;
        private readonly Func<T, string[]> OutputValueSelector;
        private readonly Func<PlayerName, string, ICricketSeason, MatchType[], T> StatGenerator;
        private readonly Func<T, bool> SelectorFunc;
        private readonly Comparison<T> Comparison;
        private readonly PlayerName Name;

        public List<T> Stats
        {
            get;
            set;
        } = new List<T>();

        public TeamAggregateStatList()
        {
        }

        public TeamAggregateStatList(
            string header,
            IReadOnlyList<string> headers,
            Func<T, string[]> outputValueSelector,
            Func<PlayerName, string, ICricketSeason, MatchType[], T> statGenerator,
            Func<T, bool> selectorFunc,
            Comparison<T> comparison)
            : this()
        {
            Header = header;
            Headers = headers;
            OutputValueSelector = outputValueSelector;
            StatGenerator = statGenerator;
            SelectorFunc = selectorFunc;
            Comparison = comparison;
        }

        public TeamAggregateStatList(
            string header,
            IReadOnlyList<string> headers,
            Func<T, string[]> outputValueSelector,
            PlayerName name,
            Func<PlayerName, string, ICricketSeason, MatchType[], T> statGenerator,
            Func<T, bool> selectorFunc,
            Comparison<T> comparison)
            : this(header, headers, outputValueSelector, statGenerator, selectorFunc, comparison)
        {
            Name = name;
        }

        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            CricketStatsHelpers.SeasonIterator(
                team.Seasons,
                season => CalculateStats(team.TeamName, season, matchTypes),
                preCycleAction: ResetStats,
                postCycleAction: Finalise);
        }

        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            var playerNames = Name == null ? season.Players(teamName, matchTypes) : new List<PlayerName>() { Name };
            List<T> stats = playerNames.Select(name => StatGenerator(name, teamName, season, matchTypes)).ToList();
            IEnumerable<T> filteredRecords = stats.Where(player => SelectorFunc(player));
            Stats.AddRange(filteredRecords);
        }

        public void ResetStats()
        {
            Stats.Clear();
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            throw new NotImplementedException();
        }

        public void Finalise()
        {
            Stats.Sort(Comparison);
        }

        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            if (Stats.Any())
            {
                var values = Stats
                        .Select(value =>
                        OutputValueSelector(value));
                _ = rb.WriteTitle(Header, headerElement)
                    .WriteTableFromEnumerable(Headers, values, headerFirstColumn: false);
            }
        }
    }
}
