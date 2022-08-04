using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Collection;

namespace CricketStructures.Statistics
{
    /// <summary>
    /// Builder for a statis collection.
    /// </summary>
    public sealed class StatsCollectionBuilder
    {
        private CricketStatsCollection fCollection;

        /// <summary>
        /// Construct an instance of a <see cref="StatsCollectionBuilder"/>
        /// </summary>
        public StatsCollectionBuilder()
        {
        }

        /// <summary>
        /// Instantiate the collection with a header.
        /// </summary>
        public StatsCollectionBuilder WithCollection(string header)
        {
            fCollection = new CricketStatsCollection(header);
            return this;
        }

        /// <summary>
        /// Add a statistic for the team.
        /// </summary>
        public StatsCollectionBuilder WithTeamStatistic(CricketStatTypes statistic)
        {
            var stat = CricketStatsFactory.Generate(statistic);
            fCollection.Statistics.Add(statistic, stat);
            return this;
        }

        /// <summary>
        /// Add a statistic for the player specified.
        /// </summary>
        public StatsCollectionBuilder WithPlayerStatistic(CricketStatTypes statistic, PlayerName player)
        {
            var stat = CricketStatsFactory.Generate(statistic, player);
            fCollection.Statistics.Add(statistic, stat);
            return this;
        }

        /// <summary>
        /// Calculate all statistics from the team given for the types of match specified.
        /// </summary>
        public StatsCollectionBuilder CalculateFromTeam(ICricketTeam team, MatchType[] matchTypes)
        {
            fCollection.CalculateStats(team, matchTypes);
            return this;
        }

        /// <summary>
        /// Calculate all statistics from the team given for the types of match specified in the specific season.
        /// </summary>
        public StatsCollectionBuilder CalculateFromSeason(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            fCollection.CalculateStats(teamName, season, matchTypes);
            return this;
        }

        /// <summary>
        /// Return the collection created.
        /// </summary>
        public IStatCollection GetCollection()
        {
            return fCollection;
        }

        /// <summary>
        /// Export a standard statistics collection output.
        /// </summary>
        /// <param name="statType">The type of the collection to create.</param>
        /// <param name="matchTypes">The type of matches to export.</param>
        /// <param name="team">The team to create the stats for.</param>
        /// <param name="teamName">The name of the team.</param>
        /// <param name="season">The season to create stats for.</param>
        /// <param name="playerName"></param>
        /// <returns>An <see cref="IStatCollection"/> object to export the stats with.</returns>
        public static IStatCollection StandardStat(
            StatCollection statType,
            MatchType[] matchTypes,
            ICricketTeam team = null,
            string teamName = null,
            ICricketSeason season = null,
            PlayerName playerName = null)
        {
            switch (statType)
            {
                case StatCollection.PlayerBrief:
                    return new PlayerBriefStatistics(playerName, team, matchTypes);
                case StatCollection.PlayerSeason:
                    return new PlayerBriefStatistics(teamName, playerName, season, matchTypes);
                case StatCollection.SeasonBrief:
                    return new TeamBriefStatistics(teamName, season, matchTypes);
                case StatCollection.AllTimeBrief:
                    return new TeamBriefStatistics(team, matchTypes);
                case StatCollection.AllTimeDetailed:
                    return new DetailedAllTimeStatistics(team, matchTypes);
                case StatCollection.PlayerDetailed:
                    return new IndividualPlayerStatistics(team, playerName, matchTypes);
                case StatCollection.Custom:
                default:
                    return null;
            }
        }
    }
}
