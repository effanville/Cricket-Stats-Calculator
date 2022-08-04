﻿namespace CricketStructures.Statistics
{
    /// <summary>
    /// The admissible types of statistic to calculate.
    /// </summary>
    public enum CricketStatTypes
    {
        // Statistics for the whole team.
        TeamRecord,
        TeamResultStats,
        ExtremeScores,
        LargestVictories,
        HeaviestDefeats,

        // Statistics for Partnerships.
        PartnershipStats,
        PlayerPartnershipStats,
        TeamPartnershipStats,

        // Player Collection Stats
        DetailedAllTimePlayerStatistics,
        DetailedAllTimeCareerStatistics,
        ClubCenturies,
        ClubHighScoreRecord,
        ClubCarryingOfBat,
        ClubSeasonRunsOver300,
        ClubSeasonRunsOver500,
        ClubSeasonAverageOver30,
        ClubInningsDismissals,
        ClubSeasonTwentyCatches,
        ClubSeasonTenStumpings,
        ClubOver5Wickets,
        ClubSeasonOver30Wickets,
        ClubSeasonAverageUnder15,
        ClubNumber5For,
        ClubLowEconomy,
        ClubLowStrikeRate,

        ClubCareerBatting,
        MostClubAppearances,
        MostClubRuns,
        HighestClubBattingAverage,
        MostClubWickets,
        ClubCareerBowling,
        ClubCareerFielding,

        // Player Individual Stats.
        PlayerBattingStats,
        PlayerBowlingStats,
        PlayerFieldingStats,
        PlayerAttendanceStats,
        SeasonAttendanceRecord,
        BattingRecord,
        CenturyScores,
        FiftyScores,
        ThirtyScores,
        HighScoreRecord,
        CarryingOfBat,
        SeasonBattingRecord,
        SeasonRunsOver300,
        SeasonRunsOver500,
        SeasonAverageOver30,
        InningsDismissals,
        SeasonTwentyCatches,
        SeasonTenStumpings,
        Over5Wickets,
        SeasonBowlingRecord,
        SeasonOver30Wickets,
        SeasonAverageUnder15,
        Number5For,
        LowEconomy,
        LowStrikeRate,
        SeasonFieldingRecord,
        CareerBowling,
        CareerFielding,
    }
}
