using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;

namespace CricketStructures.Statistics.Implementation.Collection
{
    /// <summary>
    /// A generic container for a collection of statistics.
    /// </summary>
    internal sealed class CricketStatsCollection : ICricketStat, IStatCollection
    {
        /// <summary>
        /// The header as an overall description of the statistics.
        /// </summary>
        private readonly string Header;

        /// <summary>
        /// The statistics contained in this collection.
        /// </summary>
        public IDictionary<CricketStatTypes, ICricketStat> Statistics
        {
            get;
            set;
        } = new Dictionary<CricketStatTypes, ICricketStat>();

        /// <inheritdoc/>
        public IReadOnlyList<CricketStatTypes> StatisticTypes => Statistics.Keys.ToList();

        /// <inheritdoc/>
        public ICricketStat this[CricketStatTypes index]
        {
            get
            {
                if (Statistics.TryGetValue(index, out ICricketStat stat))
                {
                    return stat;
                }

                return null;
            }
        }

        /// <summary>
        /// Construct an empty instance.
        /// </summary>
        public CricketStatsCollection()
        {
        }

        public CricketStatsCollection(string header)
        {
            Header = header;
        }

        public CricketStatsCollection(
            string header,
            IList<CricketStatTypes> statsToGenerate,
            ICricketTeam team,
            MatchType[] matchTypes,
            PlayerName playerName = null)
        {
            Header = header;
            foreach (CricketStatTypes statName in statsToGenerate)
            {
                var stat = CricketStatsFactory.Generate(statName, team, matchTypes, playerName);
                Statistics.Add(statName, stat);
            }
        }

        public CricketStatsCollection(
            string header,
            IList<CricketStatTypes> statsToGenerate,
            string teamName,
            ICricketSeason season,
            MatchType[] matchTypes,
            PlayerName playerName = null)
        {
            Header = header;
            foreach (CricketStatTypes statName in statsToGenerate)
            {
                var stat = CricketStatsFactory.Generate(statName, teamName, season, matchTypes, playerName);
                Statistics.Add(statName, stat);
            }
        }

        public CricketStatsCollection(
            string header,
            IList<CricketStatTypes> statsToGenerate,
            PlayerName playerName = null)
        {
            Header = header;
            foreach (CricketStatTypes statName in statsToGenerate)
            {
                var stat = CricketStatsFactory.Generate(statName, playerName);
                Statistics.Add(statName, stat);
            }
        }

        /// <inheritdoc/>
        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            foreach (var stat in Statistics)
            {
                stat.Value.CalculateStats(team, matchTypes);
            }
        }

        /// <inheritdoc/>
        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            foreach (var stat in Statistics)
            {
                stat.Value.CalculateStats(teamName, season, matchTypes);
            }
        }

        /// <inheritdoc/>
        public void UpdateStats(string teamName, ICricketMatch match)
        {
            foreach (var stat in Statistics)
            {
                stat.Value.UpdateStats(teamName, match);
            }
        }

        /// <inheritdoc/>
        public void ResetStats()
        {
            foreach (var stat in Statistics)
            {
                stat.Value.ResetStats();
            }
        }

        /// <inheritdoc/>
        public StringBuilder ExportStats(DocumentType exportType, DocumentElement headerElement)
        {
            StringBuilder sb = new StringBuilder();

            TextWriting.WriteHeader(sb, exportType, Header, useColours: true);

            var statsTypes = Statistics.Keys;
            foreach (CricketStatTypes statName in statsTypes)
            {
                var stats = Statistics[statName];
                _ = sb.Append(stats.ExportStats(exportType, headerElement));
            }

            TextWriting.WriteFooter(sb, exportType);
            return sb;
        }
    }
}
