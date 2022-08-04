using System.Collections.Generic;

using Common.Structure.ReportWriting;

namespace CricketStructures.Statistics
{
    /// <summary>
    /// Provides the ability to export a statistics object.
    /// </summary>
    public interface IStatCollection : IGenericStatCollection<CricketStatTypes, ICricketStat>
    {
    }

    /// <summary>
    /// Provides the ability to export a statistics object.
    /// </summary>
    public interface IGenericStatCollection<S, T>
        where S : System.Enum
        where T : class
    {
        /// <summary>
        /// The header title for the stats collection.
        /// </summary>
        string Header
        {
            get;
        }

        /// <summary>
        /// Retrieve all statistics held in this collection.
        /// </summary>
        IReadOnlyList<S> StatisticTypes
        {
            get;
        }

        /// <summary>
        /// Retrieve the statistic of the specific type.
        /// </summary>
        /// <param name="statisticType">The type to query for.</param>
        /// <returns>The statistic of the particular type.</returns>
        T this[S statisticType]
        {
            get;
        }

        /// <summary>
        /// Export the stats into a string format.
        /// </summary>
        /// <param name="exportType">The type of file to export to</param>
        /// <param name="headerElement">The base header value to use for the export.</param>
        /// <returns>A stringbuilder containing the exported stats.</returns>
        void ExportStats(ReportBuilder reportBuilder, DocumentElement headerElement);
    }
}
