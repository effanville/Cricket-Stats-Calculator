using System;
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
    internal sealed class SeasonAggregateStatList<T> : ICricketStat where T : class
    {
        private readonly int MinimumNumberOfStatsToDisplay = 10;

        private readonly ISeasonAggregateStat<T> StatGeneration;
        private string Header => StatGeneration.Title;
        private IReadOnlyList<string> Headers => StatGeneration.Headers;
        private Func<T, IReadOnlyList<string>> OutputValueSelector => StatGeneration.OutputValueSelector;
        private Func<PlayerName, string, ICricketSeason, MatchType[], T> StatGenerator => StatGeneration.StatGenerator;
        private Func<T, bool> SelectorFunc => StatGeneration.SelectorFunc;
        private Comparison<T> Comparison => StatGeneration.Comparison;
        private readonly int? OutputNumber = null;
        private readonly PlayerName Name;

        public List<T> Stats
        {
            get;
            set;
        } = new List<T>();

        public SeasonAggregateStatList(ISeasonAggregateStat<T> statGeneration, int? numberToOutput = null)
        {
            OutputNumber = numberToOutput;
            StatGeneration = statGeneration;
            Name = StatGeneration.Name;
        }

        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            int numberEntries = 0;
            bool reachedMin = false;
            do
            {
                CricketStatsHelpers.SeasonIterator(
                team.Seasons,
                season => CalculateStats(team.TeamName, season, matchTypes),
                preCycleAction: ResetStats,
                postCycleAction: Finalise);
                numberEntries = Stats.Count;
                if (numberEntries < MinimumNumberOfStatsToDisplay)
                {
                    reachedMin = StatGeneration.IncreaseStatScope();
                }
            }
            while (numberEntries < MinimumNumberOfStatsToDisplay && !reachedMin);
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
            if (OutputNumber.HasValue)
            {
                Stats = Stats.Take(OutputNumber.Value).ToList();
            }
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
