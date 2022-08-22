using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Player;

using System.Collections.Generic;
using System.Linq;

namespace CricketStructures.Statistics.Collection.Implementation
{
    internal sealed class DetailedAllTimePlayerStatistics : IStatCollection
    {
        private readonly CricketStatsCollection BattingStats;
        private readonly CricketStatsCollection BowlingStats;
        private readonly CricketStatsCollection FieldingStats;
        private readonly CricketStatsCollection CareerStats;

        public string Header => "Individual Player Records";

        public IReadOnlyList<CricketStatTypes> StatisticTypes
        {
            get
            {
                return BattingStats.StatisticTypes
                    .Union(BowlingStats.StatisticTypes)
                    .Union(FieldingStats.StatisticTypes)
                    .Union(CareerStats.StatisticTypes)
                    .ToList();
            }
        }

        public ICricketStat this[CricketStatTypes statisticType] { get => throw new System.NotImplementedException(); }

        public DetailedAllTimePlayerStatistics(ICricketTeam team, MatchType[] matchTypes)
        {
            var battingStats = new[]
            {
                CricketStatTypes.ClubCenturies,
                CricketStatTypes.ClubHighScoreRecord,
                CricketStatTypes.ClubCarryingOfBat,
                CricketStatTypes.ClubSeasonRunsOver500,
                CricketStatTypes.ClubSeasonAverageOver30,
            };
            BattingStats = new CricketStatsCollection("Batting Performances", battingStats, team, matchTypes);
            var bowlingStats = new[]
            {
                CricketStatTypes.ClubOver5Wickets,
                CricketStatTypes.ClubSeasonOver20Wickets,
                CricketStatTypes.ClubSeasonAverageUnder15,
                CricketStatTypes.ClubNumber5For,
                CricketStatTypes.ClubLowEconomy,
                CricketStatTypes.ClubLowStrikeRate,
            };
            BowlingStats = new CricketStatsCollection("Bowling Performances", bowlingStats, team, matchTypes);
            var fieldingStats = new[]
            {
                CricketStatTypes.ClubInningsDismissals,
                CricketStatTypes.ClubSeasonTwentyCatches,
                CricketStatTypes.ClubSeasonTenStumpings,
            };
            FieldingStats = new CricketStatsCollection("Fielding Performances", fieldingStats, team, matchTypes);

            var stats = new[] {
                CricketStatTypes.MostClubAppearances,
                CricketStatTypes.MostClubRuns,
                CricketStatTypes.HighestClubBattingAverage,
                CricketStatTypes.MostClubWickets,
                CricketStatTypes.ClubCareerBatting,
                CricketStatTypes.ClubCareerBowling,
                CricketStatTypes.ClubCareerFielding,
            };
            CareerStats = new CricketStatsCollection("Leading Career Records", stats, team, matchTypes);
        }

        public DetailedAllTimePlayerStatistics(PlayerName playerName, ICricketTeam team, MatchType[] matchTypes)
        {
            var battingStats = new[]
            {
                CricketStatTypes.CenturyScores,
                CricketStatTypes.HighScoreRecord,
                CricketStatTypes.CarryingOfBat,
                CricketStatTypes.SeasonRunsOver500,
                CricketStatTypes.SeasonAverageOver30,
            };
            BattingStats = new CricketStatsCollection("Batting Performances", battingStats, team, matchTypes, playerName);
            var bowlingStats = new[]
            {
                CricketStatTypes.Over5Wickets,
                CricketStatTypes.SeasonOver20Wickets,
                CricketStatTypes.SeasonAverageUnder15,
                CricketStatTypes.Number5For,
                CricketStatTypes.LowEconomy,
                CricketStatTypes.LowStrikeRate,
            };
            BowlingStats = new CricketStatsCollection("Bowling Performances", bowlingStats, team, matchTypes, playerName);
            var fieldingStats = new[]
            {
                CricketStatTypes.InningsDismissals,
                CricketStatTypes.SeasonTwentyCatches,
                CricketStatTypes.SeasonTenStumpings,
            };
            FieldingStats = new CricketStatsCollection("Fielding Performances", fieldingStats, team, matchTypes, playerName);
            var stats = new[] {
                CricketStatTypes.PlayerBattingRecord,
                CricketStatTypes.PlayerBowlingRecord,
                CricketStatTypes.PlayerFieldingStats,
            };
            CareerStats = new CricketStatsCollection("Leading Career Records", stats, team, matchTypes, playerName);
        }

        /// <inheritdoc/>
        public void ExportStats(ReportBuilder reportBuilder, DocumentElement headerElement)
        {
            _ = reportBuilder.WriteTitle(Header, headerElement);
            BattingStats.ExportStats(reportBuilder, headerElement.GetNext());
            BowlingStats.ExportStats(reportBuilder, headerElement.GetNext());
            FieldingStats.ExportStats(reportBuilder, headerElement.GetNext());
            CareerStats.ExportStats(reportBuilder, headerElement.GetNext());
        }
    }
}
