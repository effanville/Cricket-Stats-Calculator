using CricketStructures.Player;
using CricketStructures.Match;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Partnerships;
using CricketStructures.Statistics.Implementation.Player;
using CricketStructures.Statistics.Implementation.Team;
using CricketStructures.Statistics.Implementation.Player.Batting;
using CricketStructures.Statistics.Implementation.Player.Bowling;
using CricketStructures.Statistics.Implementation.Collection;
using CricketStructures.Match.Innings;
using Common.Structure.NamingStructures;
using CricketStructures.Statistics.Implementation.Player.Fielding;

namespace CricketStructures.Statistics
{
    /// <summary>
    /// Helpers for calculating cricket statistics.
    /// </summary>
    public sealed class CricketStatsFactory
    {
        /// <summary>
        /// Create a statistic
        /// </summary>
        /// <param name="statName">The type of statistic to create.</param>
        /// <param name="playerName">An optional extra name of the player to calculate the statistic for.</param>
        /// <returns>The statistic.</returns>
        public static ICricketStat Generate(CricketStatTypes statName, PlayerName playerName = null)
        {
            switch (statName)
            {
                // Statistics for the whole team.
                case CricketStatTypes.TeamRecord:
                    return new TeamRecord();
                case CricketStatTypes.TeamResultStats:
                    return new TeamResultStats();
                case CricketStatTypes.ExtremeScores:
                    return new ExtremeScores();
                case CricketStatTypes.LargestVictories:
                    return new LargestVictories();
                case CricketStatTypes.HeaviestDefeats:
                    return new HeaviestDefeats();

                // Statistics for Partnerships.
                case CricketStatTypes.PartnershipStats:
                    return new FullPartnershipStats();
                case CricketStatTypes.PlayerPartnershipStats:
                    return new PlayerPartnershipStats(playerName);
                case CricketStatTypes.TeamPartnershipStats:
                    return new PlayerPartnershipStats(null);

                // Player Collection Stats
                case CricketStatTypes.DetailedAllTimePlayerStatistics:
                    return new DetailedAllTimePlayerStatistics();

                case CricketStatTypes.ClubCenturies:
                    return new MatchAggregateStatList<PlayerScore>(new HighScores(100, null));

                case CricketStatTypes.ClubHighScoreRecord:
                    return new HighScoreRecord();

                case CricketStatTypes.ClubCarryingOfBat:
                    return new MatchAggregateStatList<PlayerScore>(new CarryingBat(null));

                case CricketStatTypes.ClubSeasonRunsOver300:
                    return new SeasonAggregateStatList<PlayerBattingRecord>(new SeasonRunsRecord(300, null));

                case CricketStatTypes.ClubSeasonRunsOver500:
                    return new SeasonAggregateStatList<PlayerBattingRecord>(new SeasonRunsRecord(300, null));

                case CricketStatTypes.ClubSeasonAverageOver30:
                    return new SeasonAggregateStatList<PlayerBattingRecord>(new BattingAverage(30, null));

                case CricketStatTypes.ClubInningsDismissals:
                    return new MatchAggregateStatList<InningsDismissals>(new MostInningsDismissals(3, null));

                case CricketStatTypes.ClubSeasonTwentyCatches:
                    return new SeasonAggregateStatList<NameDatedRecord<int>>(new SeasonCatches(20, null));

                case CricketStatTypes.ClubSeasonTenStumpings:
                    return new SeasonAggregateStatList<NameDatedRecord<int>>(new SeasonStumpings(10, null));

                case CricketStatTypes.ClubOver5Wickets:
                    return new BestBowlingRecord();

                case CricketStatTypes.ClubSeasonOver20Wickets:
                    return new SeasonAggregateStatList<PlayerBowlingRecord>(new SeasonWickets(20, yearCompare: false, null));

                case CricketStatTypes.ClubSeasonAverageUnder15:
                    return new SeasonAggregateStatList<PlayerBowlingRecord>(new SeasonAverage(15, null));

                case CricketStatTypes.ClubNumber5For:
                    return new Number5Fors();

                case CricketStatTypes.ClubLowEconomy:
                    return new LowEconomyStat();
                case CricketStatTypes.ClubLowStrikeRate:
                    return new LowStrikeRateStat();

                case CricketStatTypes.ClubCareerBatting:
                    return new ClubCareerBattingRecords();
                case CricketStatTypes.ClubCareerBowling:
                    return new ClubCareerBowlingRecords();
                case CricketStatTypes.MostClubAppearances:
                    return new MostClubAppearances();
                case CricketStatTypes.MostClubRuns:
                    return new MostClubRuns();
                case CricketStatTypes.HighestClubBattingAverage:
                    return new HighestClubBattingAverage();
                case CricketStatTypes.MostClubWickets:
                    return new MostClubWickets();

                // Player Individual Stats.
                case CricketStatTypes.PlayerBattingRecord:
                    return new PlayerBattingRecord(playerName);
                case CricketStatTypes.PlayerBowlingRecord:
                    return new PlayerBowlingRecord(playerName);
                case CricketStatTypes.PlayerFieldingStats:
                    return new PlayerFieldingStatistics(playerName);
                case CricketStatTypes.PlayerAttendanceStats:
                    return new PlayerAttendanceStatistics(playerName);
                case CricketStatTypes.SeasonAttendanceRecord:
                    return new SeasonAttendanceRecord(playerName);

                case CricketStatTypes.CenturyScores:
                    return new MatchAggregateStatList<PlayerScore>(new HighScores(100, playerName));

                case CricketStatTypes.FiftyScores:
                    return new MatchAggregateStatList<PlayerScore>(new HighScores(50, playerName));

                case CricketStatTypes.ThirtyScores:
                    return new MatchAggregateStatList<PlayerScore>(new HighScores(30, playerName));

                case CricketStatTypes.HighScoreRecord:
                    return new HighScoreRecord(playerName);

                case CricketStatTypes.CarryingOfBat:
                    return new MatchAggregateStatList<PlayerScore>(new CarryingBat(playerName));

                case CricketStatTypes.SeasonBattingRecord:
                    return new SeasonAggregateStatList<PlayerBattingRecord>(new SeasonRunsRecord(0, playerName));

                case CricketStatTypes.SeasonRunsOver300:
                    return new SeasonAggregateStatList<PlayerBattingRecord>(new SeasonRunsRecord(300, playerName));

                case CricketStatTypes.SeasonRunsOver500:
                    return new SeasonAggregateStatList<PlayerBattingRecord>(new SeasonRunsRecord(500, playerName));

                case CricketStatTypes.SeasonAverageOver30:
                    return new SeasonAggregateStatList<PlayerBattingRecord>(new BattingAverage(30, playerName));

                case CricketStatTypes.InningsDismissals:
                    return new MatchAggregateStatList<InningsDismissals>(new MostInningsDismissals(3, playerName));

                case CricketStatTypes.SeasonTwentyCatches:
                    return new SeasonAggregateStatList<NameDatedRecord<int>>(new SeasonCatches(20, playerName));

                case CricketStatTypes.SeasonTenStumpings:
                    return new SeasonAggregateStatList<NameDatedRecord<int>>(new SeasonStumpings(10, playerName));

                case CricketStatTypes.Over5Wickets:
                    return new BestBowlingRecord(5, playerName);

                case CricketStatTypes.SeasonBowlingRecord:
                    return new SeasonAggregateStatList<PlayerBowlingRecord>(new SeasonWickets(0, yearCompare: true, playerName));

                case CricketStatTypes.SeasonOver20Wickets:
                    return new SeasonAggregateStatList<PlayerBowlingRecord>(new SeasonWickets(20, yearCompare: false, playerName));

                case CricketStatTypes.SeasonAverageUnder15:
                    return new SeasonAggregateStatList<PlayerBowlingRecord>(new SeasonAverage(15, playerName));

                case CricketStatTypes.Number5For:
                    return new Number5Fors(playerName);
                case CricketStatTypes.LowEconomy:
                    return new LowEconomyStat(playerName);
                case CricketStatTypes.LowStrikeRate:
                    return new LowStrikeRateStat(playerName);

                case CricketStatTypes.SeasonFieldingRecord:
                    return new SeasonAggregateStatList<PlayerFieldingStatistics>(new SeasonFielding(0, playerName));

                case CricketStatTypes.CareerFielding:
                case CricketStatTypes.ClubCareerFielding:
                default:
                    return null;
            }
        }

        /// <summary>
        /// Create a statistic and calculate the value from the team
        /// </summary>
        /// <param name="statName">The type of statistic to create.</param>
        /// <param name="team">The team to calculate the values of the statistic.</param>
        /// <param name="matchTypes">The types of match to use.</param>
        /// <param name="playerName">An optional extra name of the player to calculate the statistic for.</param>
        /// <returns>The statistic.</returns>
        public static ICricketStat Generate(
            CricketStatTypes statName,
            ICricketTeam team,
            MatchType[] matchTypes,
            PlayerName playerName = null)
        {
            var stats = Generate(statName, playerName);
            stats.CalculateStats(team, matchTypes);
            return stats;
        }

        /// <summary>
        /// Create a statistic and calculate the value from the team
        /// </summary>
        /// <param name="statName">The type of statistic to create.</param>
        /// <param name="teamName">The teamName.</param>
        /// <param name="season">The season to calculate stats for.</param>
        /// <param name="matchTypes">The types of match to use.</param>
        /// <param name="playerName">An optional extra name of the player to calculate the statistic for.</param>
        /// <returns>The statistic.</returns>
        public static ICricketStat Generate(
            CricketStatTypes statName,
            string teamName,
            ICricketSeason season,
            MatchType[] matchTypes,
            PlayerName playerName = null)
        {
            var stats = Generate(statName, playerName);
            stats.CalculateStats(teamName, season, matchTypes);
            return stats;
        }
    }
}
