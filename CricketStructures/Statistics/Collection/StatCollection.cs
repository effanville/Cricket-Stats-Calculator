namespace CricketStructures.Statistics.Collection
{
    /// <summary>
    /// The types of a statistics collection.
    /// </summary>
    public enum StatCollection
    {
        /// <summary>
        /// A user specified collection of statistics.
        /// </summary>
        Custom,

        /// <summary>
        /// A collection for a single player with brief values.
        /// </summary>
        PlayerBrief,

        /// <summary>
        /// A collection for a single player with a detailed output.
        /// </summary>
        PlayerDetailed,

        /// <summary>
        /// A collection of statistics for a player over a season.
        /// </summary>
        PlayerSeason,

        /// <summary>
        /// A collection of statistics for a team for a single season.
        /// </summary>
        SeasonBrief,

        /// <summary>
        /// A collection of statistics for a team over all records held.
        /// </summary>
        AllTimeBrief,

        /// <summary>
        /// A collection of many statistics for a team over all records held.
        /// </summary>
        AllTimeDetailed
    }
}
