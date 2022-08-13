﻿using CricketStructures.Season;
using Common.Structure.ReportWriting;
using CricketStructures.Match;
using CricketStructures.Player;

namespace CricketStructures.Statistics.Implementation.Collection
{
    internal sealed class DetailedAllTimePlayerStatistics : ICricketStat
    {
        private readonly CricketStatsCollection BattingStats;
        private readonly CricketStatsCollection BowlingStats;
        private readonly CricketStatsCollection FieldingStats;
        private readonly CricketStatsCollection CareerStats;

        public DetailedAllTimePlayerStatistics()
        {
            var battingStats = new[]
            {
                CricketStatTypes.ClubCenturies,
                CricketStatTypes.ClubHighScoreRecord,
                CricketStatTypes.ClubCarryingOfBat,
                CricketStatTypes.ClubSeasonRunsOver500,
                CricketStatTypes.ClubSeasonAverageOver30,
            };
            BattingStats = new CricketStatsCollection("Batting Performances", battingStats);
            var bowlingStats = new[]
            {
                CricketStatTypes.ClubOver5Wickets,
                CricketStatTypes.ClubSeasonOver20Wickets,
                CricketStatTypes.ClubSeasonAverageUnder15,
                CricketStatTypes.ClubNumber5For,
                CricketStatTypes.ClubLowEconomy,
                CricketStatTypes.ClubLowStrikeRate,
            };
            BowlingStats = new CricketStatsCollection("Bowling Performances", bowlingStats);
            var fieldingStats = new[]
            {
                CricketStatTypes.ClubInningsDismissals,
                CricketStatTypes.ClubSeasonTwentyCatches,
                CricketStatTypes.ClubSeasonTenStumpings,
            };
            FieldingStats = new CricketStatsCollection("Fielding Performances", fieldingStats);
            var stats = new[] {
                CricketStatTypes.MostClubAppearances,
                CricketStatTypes.MostClubRuns,
                CricketStatTypes.HighestClubBattingAverage,
                CricketStatTypes.MostClubWickets,
                CricketStatTypes.ClubCareerBatting,
                CricketStatTypes.ClubCareerBowling,
            };
            CareerStats = new CricketStatsCollection("Leading Career Records", stats);
        }

        public DetailedAllTimePlayerStatistics(PlayerName playerName)
        {
            var battingStats = new[]
            {
                CricketStatTypes.CenturyScores,
                CricketStatTypes.HighScoreRecord,
                CricketStatTypes.CarryingOfBat,
                CricketStatTypes.SeasonRunsOver500,
                CricketStatTypes.SeasonAverageOver30,
            };
            BattingStats = new CricketStatsCollection("Batting Performances", battingStats, playerName);
            var bowlingStats = new[]
            {
                CricketStatTypes.Over5Wickets,
                CricketStatTypes.SeasonOver20Wickets,
                CricketStatTypes.SeasonAverageUnder15,
                CricketStatTypes.Number5For,
                CricketStatTypes.LowEconomy,
                CricketStatTypes.LowStrikeRate,
            };
            BowlingStats = new CricketStatsCollection("Bowling Performances", bowlingStats, playerName);
            var fieldingStats = new[]
            {
                CricketStatTypes.InningsDismissals,
                CricketStatTypes.SeasonTwentyCatches,
                CricketStatTypes.SeasonTenStumpings,
            };
            FieldingStats = new CricketStatsCollection("Fielding Performances", fieldingStats, playerName);
            var stats = new[] {
                CricketStatTypes.PlayerBattingRecord,
                CricketStatTypes.PlayerBowlingRecord,
            };
            CareerStats = new CricketStatsCollection("Leading Career Records", stats);
        }

        /// <inheritdoc/>
        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            BattingStats.CalculateStats(team, matchTypes);
            BowlingStats.CalculateStats(team, matchTypes);
            FieldingStats.CalculateStats(team, matchTypes);
            CareerStats.CalculateStats(team, matchTypes);
        }

        /// <inheritdoc/>
        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            BattingStats.CalculateStats(teamName, season, matchTypes);
            BowlingStats.CalculateStats(teamName, season, matchTypes);
            FieldingStats.CalculateStats(teamName, season, matchTypes);
            CareerStats.CalculateStats(teamName, season, matchTypes);
        }

        /// <inheritdoc/>
        public void ResetStats()
        {
            BattingStats.ResetStats();
            BowlingStats.ResetStats();
            FieldingStats.ResetStats();
            CareerStats.ResetStats();
        }

        /// <inheritdoc/>
        public void Finalise()
        {
            BattingStats.Finalise();
            BowlingStats.Finalise();
            FieldingStats.Finalise();
            CareerStats.Finalise();
        }

        /// <inheritdoc/>
        public void UpdateStats(string teamName, ICricketMatch match)
        {
            BattingStats.UpdateStats(teamName, match);
            BowlingStats.UpdateStats(teamName, match);
            FieldingStats.UpdateStats(teamName, match);
            CareerStats.UpdateStats(teamName, match);
        }

        /// <inheritdoc/>
        public void ExportStats(ReportBuilder reportBuilder, DocumentElement headerElement)
        {
            _ = reportBuilder.WriteTitle("Individual Player Records", headerElement);
            BattingStats.ExportStats(reportBuilder, headerElement);
            BowlingStats.ExportStats(reportBuilder, headerElement);
            FieldingStats.ExportStats(reportBuilder, headerElement);
            CareerStats.ExportStats(reportBuilder, headerElement);
        }
    }
}
