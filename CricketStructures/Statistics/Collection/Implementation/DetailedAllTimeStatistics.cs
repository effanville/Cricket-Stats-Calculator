using System.Collections.Generic;

using Common.Structure.ReportWriting;

using CricketStructures.Match;

namespace CricketStructures.Statistics.Collection.Implementation
{
    /// <summary>
    /// A <see cref="IStatCollection"/> that contains 
    /// </summary>
    internal sealed class DetailedAllTimeStatistics : IStatCollection
    {
        private readonly CricketStatsCollection TeamResultStats;
        private readonly CricketStatsCollection PartnershipStats;
        private readonly DetailedAllTimePlayerStatistics PlayerStats;

        /// <inheritdoc/>
        public ICricketStat this[CricketStatTypes statisticType]
        {
            get => PartnershipStats[statisticType];
        }

        /// <inheritdoc/>
        public IReadOnlyList<CricketStatTypes> StatisticTypes => PartnershipStats.StatisticTypes;

        public string Header
        {
            get;
            private set;
        }

        internal DetailedAllTimeStatistics(ICricketTeam team, MatchType[] matchTypes)
        {
            Header = $"Detailed Statistics for {team.TeamName}";
            var stats = new List<CricketStatTypes>()
            {
                CricketStatTypes.PartnershipStats
            };
            PartnershipStats = new CricketStatsCollection(Header, stats, team, matchTypes);

            var teamResultStats = new List<CricketStatTypes>()
            {
                CricketStatTypes.YearByYearRecord,
                CricketStatTypes.TeamRunsRecord,
                CricketStatTypes.TeamWicketsRecord,
                CricketStatTypes.TeamAgainstRecord,
                CricketStatTypes.ExtremeScores,
                CricketStatTypes.LargestVictories,
                CricketStatTypes.HeaviestDefeats,
            };

            TeamResultStats = new CricketStatsCollection($"Team Records for {team.TeamName}", teamResultStats, team, matchTypes);
            PlayerStats = new DetailedAllTimePlayerStatistics(team, matchTypes);
        }

        /// <inheritdoc/>
        public void ExportStats(ReportBuilder reportBuilder, DocumentElement headerElement)
        {
            TeamResultStats.ExportStats(reportBuilder, headerElement);
            PartnershipStats.ExportStats(reportBuilder, headerElement);
            PlayerStats.ExportStats(reportBuilder, headerElement);
        }
    }
}
