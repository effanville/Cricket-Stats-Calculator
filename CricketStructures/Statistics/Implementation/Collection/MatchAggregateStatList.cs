using System;
using System.Collections.Generic;
using System.Linq;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Season;

namespace CricketStructures.Statistics.Implementation.Collection
{
    /// <summary>
    /// Contains a list of statistics.
    /// </summary>
    /// <typeparam name="T">The specific type of statistic in the list</typeparam>
    internal sealed class MatchAggregateStatList<T> : ICricketStat where T : class
    {
        private readonly int MinimumNumberOfStatsToDisplay = 10;

        private readonly IMatchAggregateStat<T> _matchAggregateStat;
        private string Header => _matchAggregateStat.Title;
        private IReadOnlyList<string> Headers => _matchAggregateStat.Headers;
        private Func<T, IReadOnlyList<string>> OutputValueSelector => _matchAggregateStat.OutputValueSelector;
        private Action<string, ICricketMatch, List<T>> StatGenerator => _matchAggregateStat.AddStatsAction;
        private Func<T, bool> SelectorFunc => _matchAggregateStat.SelectorFunc;
        private Comparison<T> Comparison => _matchAggregateStat.Comparison;
        private readonly int? OutputNumber = null;

        public List<T> Stats
        {
            get;
            set;
        } = new List<T>();

        public MatchAggregateStatList(IMatchAggregateStat<T> matchStat, int? numberToOutput = null)
        {
            OutputNumber = numberToOutput;
            _matchAggregateStat = matchStat;
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
                    reachedMin = _matchAggregateStat.IncreaseStatScope();
                }
            }
            while (numberEntries < MinimumNumberOfStatsToDisplay && !reachedMin);
        }

        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            CricketStatsHelpers.MatchIterator(
                           season,
                           matchTypes,
                           match => UpdateStats(teamName, match),
                           postCycleAction: AfterMatchFinalise);
        }

        public void ResetStats()
        {
            Stats.Clear();
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            StatGenerator(teamName, match, Stats);
        }

        public void AfterMatchFinalise()
        {
            if (Stats.Any() && Stats[0] is ICricketStat)
            {
                foreach (ICricketStat stat in Stats)
                {
                    stat.Finalise();
                }
            }

            Stats.Sort(Comparison);
        }

        public void Finalise()
        {
            if (Stats.Any() && Stats[0] is ICricketStat)
            {
                foreach (ICricketStat stat in Stats)
                {
                    stat.Finalise();
                }
            }

            Stats = Stats.Where(SelectorFunc).ToList();
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
