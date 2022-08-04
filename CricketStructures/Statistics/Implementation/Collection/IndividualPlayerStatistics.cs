using System;
using System.Collections.Generic;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Player;

namespace CricketStructures.Statistics.Implementation.Collection
{
    internal sealed class IndividualPlayerStatistics : IStatCollection
    {
        private readonly PlayerName PlayerName;
        private readonly CricketStatsCollection Stats;
        private readonly CricketStatsCollection AttendanceStats;
        private readonly CricketStatsCollection BattingStats;
        private readonly CricketStatsCollection BowlingStats;
        private readonly CricketStatsCollection FieldingStats;
        public ICricketStat this[CricketStatTypes statisticType] { get => throw new NotImplementedException(); }

        public IReadOnlyList<CricketStatTypes> StatisticTypes => throw new NotImplementedException();

        public string Header => $"Individual Player statistics for {PlayerName}";

        public IndividualPlayerStatistics(ICricketTeam team, PlayerName playerName, MatchType[] matchTypes)
        {
            PlayerName = playerName;
            var stats = new[]
            {
                CricketStatTypes.PlayerAttendanceStats,
                CricketStatTypes.PlayerBattingStats,
                CricketStatTypes.PlayerPartnershipStats,
                CricketStatTypes.PlayerBowlingStats,
                CricketStatTypes.PlayerFieldingStats
            };
            Stats = new CricketStatsCollection("Overall Player Records", stats, team, matchTypes, playerName);

            var attendanceStats = new[] { CricketStatTypes.SeasonAttendanceRecord };
            AttendanceStats = new CricketStatsCollection("Attendance Record", attendanceStats, team, matchTypes, playerName);

            var battingStats = new[]
            {
                CricketStatTypes.HighScoreRecord,
                CricketStatTypes.ThirtyScores,
                CricketStatTypes.SeasonBattingRecord,
            };
            BattingStats = new CricketStatsCollection("Batting Record", battingStats, team, matchTypes, playerName);


            var bowlingStats = new[]
            {
                CricketStatTypes.SeasonBowlingRecord,
                CricketStatTypes.Over5Wickets,
            };
            BowlingStats = new CricketStatsCollection("Bowling Record", bowlingStats, team, matchTypes, playerName);

            var fieldingStats = new[]
            {
                CricketStatTypes.SeasonFieldingRecord
            };
            FieldingStats = new CricketStatsCollection("Fielding Record", fieldingStats, team, matchTypes, playerName);
        }

        public void ExportStats(ReportBuilder reportBuilder, DocumentElement headerElement)
        {
            Stats.ExportStats(reportBuilder, headerElement);
            AttendanceStats.ExportStats(reportBuilder, headerElement);
            BattingStats.ExportStats(reportBuilder, headerElement);
            BowlingStats.ExportStats(reportBuilder, headerElement);
            FieldingStats.ExportStats(reportBuilder, headerElement);
        }
    }
}
