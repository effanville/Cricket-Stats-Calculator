namespace CricketStructures.Statistics
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
        ClubSeasonOver20Wickets,
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
        PlayerBattingRecord,
        PlayerBowlingRecord,
        PlayerFieldingStats,
        PlayerAttendanceStats,
        SeasonAttendanceRecord,
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
        SeasonOver20Wickets,
        SeasonAverageUnder15,
        Number5For,
        LowEconomy,
        LowStrikeRate,
        SeasonFieldingRecord,
        CareerFielding,
    }
}
