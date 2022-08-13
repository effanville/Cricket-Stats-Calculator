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
    internal sealed class MatchAggregateStatList<T> : ICricketStat where T : class
    {
        private readonly int MinimumNumberOfStatsToDisplay = 10;

        private readonly IMatchAggregateStat<T> _matchAggregateStat;
        private string Header => _matchAggregateStat.Title;
        private IReadOnlyList<string> Headers => _matchAggregateStat.Headers;
        private Func<T, string[]> OutputValueSelector => _matchAggregateStat.OutputValueSelector;
        private Action<PlayerName, string, ICricketMatch, List<T>> StatGenerator => _matchAggregateStat.AddStatsAction;
        private Comparison<T> Comparison => _matchAggregateStat.Comparison;
        private readonly PlayerName Name;

        public List<T> Stats
        {
            get;
            set;
        } = new List<T>();

        public MatchAggregateStatList(IMatchAggregateStat<T> matchStat)
        {
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
                           postCycleAction: Finalise);
        }

        public void ResetStats()
        {
            Stats.Clear();
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            StatGenerator(Name, teamName, match, Stats);
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
