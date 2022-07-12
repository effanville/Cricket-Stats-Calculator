using System.Collections.Generic;
using System.Text;

using Common.Structure.ReportWriting;

using CricketStructures.Match;

namespace CricketStructures.Statistics.Implementation.Collection
{
    /// <summary>
    /// A <see cref="IStatCollection"/> that contains 
    /// </summary>
    public sealed class DetailedAllTimeStatistics : IStatCollection
    {
        private readonly CricketStatsCollection Stats;

        /// <inheritdoc/>
        public ICricketStat this[CricketStatTypes statisticType]
        {
            get => Stats[statisticType];
        }

        /// <inheritdoc/>
        public IReadOnlyList<CricketStatTypes> StatisticTypes => Stats.StatisticTypes;

        internal DetailedAllTimeStatistics(ICricketTeam team, MatchType[] matchTypes)
        {
            var stats = new List<CricketStatTypes>()
            {
                CricketStatTypes.TeamResultStats,
                CricketStatTypes.PartnershipStats,
                CricketStatTypes.DetailedAllTimePlayerStatistics
            };
            Stats = new CricketStatsCollection("Detailed AllTime Statistics", stats, team, matchTypes);
        }

        /// <inheritdoc/>
        public void ExportStats(ReportBuilder reportBuilder, DocumentElement headerElement)
        {
            Stats.ExportStats(reportBuilder, headerElement);
        }
    }
}
