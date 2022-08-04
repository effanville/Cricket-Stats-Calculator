using CricketStructures.Player;

using CricketStructures.Match;
using CricketStructures.Season;
using CricketStructures.Statistics.Implementation.Partnerships;
using CricketStructures.Statistics.Implementation.Player;
using CricketStructures.Statistics.Implementation.Team;
using CricketStructures.Statistics.Implementation.Player.Batting;
using CricketStructures.Statistics.Implementation.Player.Fielding;
using CricketStructures.Statistics.Implementation.Player.Bowling;
using CricketStructures.Statistics.Implementation.Player.Career;
using CricketStructures.Statistics.Implementation.Collection;

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
                    return new CenturyScores();
                case CricketStatTypes.ClubHighScoreRecord:
                    return new HighScoreRecord();
                case CricketStatTypes.ClubCarryingOfBat:
                    return new CarryingOfBat();
                case CricketStatTypes.ClubSeasonRunsOver500:
                    return new SeasonRunsOver500();
                case CricketStatTypes.ClubSeasonAverageOver30:
                    return new SeasonAverageOver30();
                case CricketStatTypes.ClubInningsDismissals:
                    return new SingleInningsDismissals();
                case CricketStatTypes.ClubSeasonTwentyCatches:
                    return new TwentyCatchesSeason();
                case CricketStatTypes.ClubSeasonTenStumpings:
                    return new TenStumpingsSeason();
                case CricketStatTypes.ClubOver5Wickets:
                    return new Over5Wickets();
                case CricketStatTypes.ClubSeasonOver30Wickets:
                    return new SeasonOver30Wickets();
                case CricketStatTypes.ClubSeasonAverageUnder15:
                    return new SeasonAverageUnder15();
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
                case CricketStatTypes.PlayerBattingStats:
                    return new PlayerBattingStatistics(playerName);
                case CricketStatTypes.PlayerBowlingStats:
                    return new PlayerBowlingStatistics(playerName);
                case CricketStatTypes.PlayerFieldingStats:
                    return new PlayerFieldingStatistics(playerName);
                case CricketStatTypes.PlayerAttendanceStats:
                    return new PlayerAttendanceStatistics(playerName);
                case CricketStatTypes.BattingRecord:
                    return new BattingRecord(playerName);
                case CricketStatTypes.CenturyScores:
                    return new CenturyScores(playerName);
                case CricketStatTypes.HighScoreRecord:
                    return new HighScoreRecord(playerName);
                case CricketStatTypes.CarryingOfBat:
                    return new CarryingOfBat(playerName);
                case CricketStatTypes.SeasonRunsOver500:
                    return new SeasonRunsOver500(playerName);
                case CricketStatTypes.SeasonAverageOver30:
                    return new SeasonAverageOver30(playerName);
                case CricketStatTypes.InningsDismissals:
                    return new SingleInningsDismissals(playerName);
                case CricketStatTypes.SeasonTwentyCatches:
                    return new TwentyCatchesSeason(playerName);
                case CricketStatTypes.SeasonTenStumpings:
                    return new TenStumpingsSeason(playerName);
                case CricketStatTypes.Over5Wickets:
                    return new Over5Wickets(playerName);
                case CricketStatTypes.SeasonOver30Wickets:
                    return new SeasonOver30Wickets(playerName);
                case CricketStatTypes.SeasonAverageUnder15:
                    return new SeasonAverageUnder15(playerName);
                case CricketStatTypes.Number5For:
                    return new Number5Fors(playerName);
                case CricketStatTypes.LowEconomy:
                    return new LowEconomyStat(playerName);
                case CricketStatTypes.LowStrikeRate:
                    return new LowStrikeRateStat(playerName);
                case CricketStatTypes.CareerBowling:
                    return new CareerBowlingRecord(playerName);
                case CricketStatTypes.DetailedAllTimeCareerStatistics:
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
