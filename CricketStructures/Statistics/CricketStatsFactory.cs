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
using System.Collections.Generic;

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

                case CricketStatTypes.YearByYearRecord:
                    return new SeasonAggregateStatList<DatedRecord<TeamRecord>>(new YearByYearRecord());
                case CricketStatTypes.TeamAgainstRecord:
                    return new MatchAggregateStatList<Labell<string, TeamRecord>>(new TeamAgainstRecord());
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

                case CricketStatTypes.ClubCenturies:
                    return new MatchAggregateStatList<PlayerScore>(new HighScores(100, null));

                case CricketStatTypes.ClubHighScoreRecord:
                    return new HighScoreRecord();

                case CricketStatTypes.ClubCarryingOfBat:
                    return new MatchAggregateStatList<PlayerScore>(new CarryingBat(null));

                case CricketStatTypes.ClubSeasonRunsOver300:
                    return new SeasonAggregateStatList<PlayerBattingRecord>(new RunsRecord(false, 300, null));

                case CricketStatTypes.ClubSeasonRunsOver500:
                    return new SeasonAggregateStatList<PlayerBattingRecord>(new RunsRecord(false, 300, null));

                case CricketStatTypes.ClubSeasonAverageOver30:
                    return new SeasonAggregateStatList<PlayerBattingRecord>(new BattingAverage(false, 30, null));

                case CricketStatTypes.ClubInningsDismissals:
                    return new MatchAggregateStatList<InningsDismissals>(new MostInningsDismissals(3, null));

                case CricketStatTypes.ClubSeasonTwentyCatches:
                    return new SeasonAggregateStatList<NameDatedRecord<int>>(new SeasonCatchRecord(20, null));

                case CricketStatTypes.ClubSeasonTenStumpings:
                    return new SeasonAggregateStatList<NameDatedRecord<int>>(new SeasonStumpingRecord(10, null));

                case CricketStatTypes.ClubOver5Wickets:
                    return new MatchAggregateStatList<BowlingPerformance>(new BestBowlingRecord(5, null));

                case CricketStatTypes.ClubSeasonOver20Wickets:
                    return new SeasonAggregateStatList<PlayerBowlingRecord>(new WicketRecord(false, 20, yearCompare: false, null));

                case CricketStatTypes.ClubSeasonAverageUnder15:
                    return new SeasonAggregateStatList<PlayerBowlingRecord>(new BowlingAverage(false, 15, null));

                case CricketStatTypes.ClubNumber5For:
                    return new MatchAggregateStatList<NamedRecord<int>>(new HighWicketHauls(5, null));

                case CricketStatTypes.ClubLowEconomy:
                    return new SeasonAggregateStatList<PlayerBowlingRecord>(new LowEconomyStat(), 20);

                case CricketStatTypes.ClubLowStrikeRate:
                    return new LowStrikeRateStat();

                case CricketStatTypes.ClubCareerBatting:
                    return new ClubCareerBattingRecords();

                case CricketStatTypes.ClubCareerBowling:
                    return new ClubCareerBowlingRecords();

                case CricketStatTypes.ClubCareerFielding:
                    return new ClubCareerFieldingRecords();

                case CricketStatTypes.ClubCareerAttendance:
                    return new ClubCareerAttendanceRecords();

                case CricketStatTypes.MostClubAppearances:
                    return new MatchAggregateStatList<PlayerAttendanceRecord>(new AttendanceRecord(true, 0, null), 10);

                case CricketStatTypes.MostClubRuns:
                    return new MatchAggregateStatList<PlayerBattingRecord>(new RunsRecord(true, 0, null), 10);

                case CricketStatTypes.HighestClubBattingAverage:
                    return new MatchAggregateStatList<PlayerBattingRecord>(new BattingAverage(true, 0, null), 10);

                case CricketStatTypes.MostClubWickets:
                    return new MatchAggregateStatList<PlayerBowlingRecord>(new WicketRecord(true, 0, false, null), 10);

                case CricketStatTypes.LowestClubBowlingAverage:
                    return new MatchAggregateStatList<PlayerBowlingRecord>(new BowlingAverage(true, 50, null), 10);

                // Player Individual Stats.
                case CricketStatTypes.PlayerBattingRecord:
                    return new PlayerBattingRecord(playerName);

                case CricketStatTypes.PlayerBowlingRecord:
                    return new PlayerBowlingRecord(playerName);

                case CricketStatTypes.PlayerFieldingStats:
                    return new PlayerFieldingRecord(playerName);

                case CricketStatTypes.PlayerAttendanceRecord:
                    return new PlayerAttendanceRecord(playerName);

                case CricketStatTypes.SeasonAttendanceRecord:
                    return new SeasonAggregateStatList<PlayerAttendanceRecord>(new AttendanceRecord(false, 0, playerName));

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
                    return new SeasonAggregateStatList<PlayerBattingRecord>(new RunsRecord(false, 0, playerName));

                case CricketStatTypes.SeasonRunsOver300:
                    return new SeasonAggregateStatList<PlayerBattingRecord>(new RunsRecord(false, 300, playerName));

                case CricketStatTypes.SeasonRunsOver500:
                    return new SeasonAggregateStatList<PlayerBattingRecord>(new RunsRecord(false, 500, playerName));

                case CricketStatTypes.SeasonAverageOver30:
                    return new SeasonAggregateStatList<PlayerBattingRecord>(new BattingAverage(false, 30, playerName));

                case CricketStatTypes.InningsDismissals:
                    return new MatchAggregateStatList<InningsDismissals>(new MostInningsDismissals(3, playerName));

                case CricketStatTypes.SeasonTwentyCatches:
                    return new SeasonAggregateStatList<NameDatedRecord<int>>(new SeasonCatchRecord(20, playerName));

                case CricketStatTypes.SeasonTenStumpings:
                    return new SeasonAggregateStatList<NameDatedRecord<int>>(new SeasonStumpingRecord(10, playerName));

                case CricketStatTypes.Over5Wickets:
                    return new MatchAggregateStatList<BowlingPerformance>(new BestBowlingRecord(5, playerName));

                case CricketStatTypes.SeasonBowlingRecord:
                    return new SeasonAggregateStatList<PlayerBowlingRecord>(new WicketRecord(false, 0, yearCompare: true, playerName));

                case CricketStatTypes.SeasonOver20Wickets:
                    return new SeasonAggregateStatList<PlayerBowlingRecord>(new WicketRecord(false, 20, yearCompare: false, playerName));

                case CricketStatTypes.SeasonAverageUnder15:
                    return new SeasonAggregateStatList<PlayerBowlingRecord>(new BowlingAverage(false, 15, playerName));

                case CricketStatTypes.Number5For:
                    return new MatchAggregateStatList<NamedRecord<int>>(new HighWicketHauls(5, playerName));

                case CricketStatTypes.LowEconomy:
                    return new SeasonAggregateStatList<PlayerBowlingRecord>(new LowEconomyStat(playerName), 20);

                case CricketStatTypes.LowStrikeRate:
                    return new LowStrikeRateStat(playerName);

                case CricketStatTypes.SeasonFieldingRecord:
                    return new SeasonAggregateStatList<PlayerFieldingRecord>(new SeasonFieldingRecord(0, playerName));

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
        /// <param name="team">The team to calculate the values of the statistic.</param>
        /// <param name="matchTypes">The types of match to use.</param>
        /// <param name="playerName">An optional extra name of the player to calculate the statistic for.</param>
        /// <returns>The statistic.</returns>
        public static ICricketStat Generate(
            CricketStatTypes statName,
            string teamName,
            IReadOnlyList<ICricketSeason> seasons,
            MatchType[] matchTypes,
            PlayerName playerName = null)
        {
            var stats = Generate(statName, playerName);
            stats.CalculateStats(teamName, seasons, matchTypes);
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
