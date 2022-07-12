using System.Text;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Season;

namespace CricketStructures.Statistics
{
    /// <summary>
    /// A cricket statistic enabling calculation and exporting.
    /// </summary>
    public interface ICricketStat
    {
        /// <summary>
        /// Calculate the stats for an entire team for specific match types.
        /// </summary>
        /// <param name="team"></param>
        /// <param name="matchTypes"></param>
        void CalculateStats(ICricketTeam team, MatchType[] matchTypes);

        /// <summary>
        /// Calculate the stats for a specific season for a collection of match types.
        /// </summary>
        /// <param name="season"></param>
        /// <param name="matchTypes"></param>
        void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes);

        /// <summary>
        /// Update the stored statistics from the existing stats as well as
        /// this match.
        /// </summary>
        /// <param name="teamName">The name of the team.</param>
        /// <param name="match">The match to add statistics for.</param>
        void UpdateStats(string teamName, ICricketMatch match);

        /// <summary>
        /// Resets the statistics held to their default values. 
        /// </summary>
        void ResetStats();

        /// <summary>
        /// Export the stats into a RebortBuilder format.
        /// </summary>
        /// <param name="rb">The reportbuilder to export to.</param>
        /// <param name="headerElement">The base header value to use for the export.</param>
        void ExportStats(ReportBuilder rb, DocumentElement headerElement);
    }
}
