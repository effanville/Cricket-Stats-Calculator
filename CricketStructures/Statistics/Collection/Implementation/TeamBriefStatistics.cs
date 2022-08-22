using System;
using System.Collections.Generic;
using System.Linq;
using CricketStructures.Season;
using CricketStructures.Player;
using Common.Structure.ReportWriting;
using CricketStructures.Statistics.Implementation.Player.Fielding;
using CricketStructures.Match.Innings;
using CricketStructures.Statistics.Implementation.Team;

namespace CricketStructures.Statistics.Collection.Implementation
{
    /// <summary>
    /// A default statistics collection with brief records for a team over all time or 
    /// for a single specific season.
    /// </summary>
    internal sealed class TeamBriefStatistics : IStatCollection
    {
        private readonly CricketStatsCollection Stats;
        public string Header
        {
            get;
            private set;
        }

        public DateTime? SeasonYear
        {
            get;
            set;
        }

        public ClubCareerBattingRecords BattingStats => Stats[CricketStatTypes.ClubCareerBatting] as ClubCareerBattingRecords;

        public ClubCareerBowlingRecords BowlingStats => Stats[CricketStatTypes.ClubCareerBowling] as ClubCareerBowlingRecords;
        public ClubCareerFieldingRecords FieldingStats => Stats.Statistics[CricketStatTypes.ClubCareerFielding] as ClubCareerFieldingRecords;

        private readonly ICricketStat TeamRecord;

        /// <inheritdoc/>
        public ICricketStat this[CricketStatTypes statisticType]
        {
            get => Stats[statisticType];
        }

        /// <inheritdoc/>
        public IReadOnlyList<CricketStatTypes> StatisticTypes => Stats.StatisticTypes.Union(new List<CricketStatTypes> { CricketStatTypes.TeamRecord }).ToList();

        internal TeamBriefStatistics()
        {
        }

        internal TeamBriefStatistics(ICricketTeam team, Match.MatchType[] matchTypes)
        {
            Header = $"Statistics for {team.TeamName}";
            var stats = new[]
            {
                CricketStatTypes.ClubCareerAttendance,
                CricketStatTypes.TeamPartnershipStats,
                CricketStatTypes.ClubCareerBatting,
                CricketStatTypes.ClubCareerBowling,
                CricketStatTypes.ClubCareerFielding,
            };
            Stats = new CricketStatsCollection(null, stats, team, matchTypes);
            TeamRecord = CricketStatsFactory.Generate(CricketStatTypes.TeamRecord, team, matchTypes);
        }

        internal TeamBriefStatistics(string teamName, ICricketSeason season, Match.MatchType[] matchTypes)
        {
            Header = $"Statistics for {teamName} for the year {season.Year.Year}";
            SeasonYear = season.Year;

            var stats = new[]
            {
                CricketStatTypes.ClubCareerAttendance,
                CricketStatTypes.ClubCareerBatting,
                CricketStatTypes.TeamPartnershipStats,
                CricketStatTypes.ClubCareerBowling,
                CricketStatTypes.ClubCareerFielding,
            };
            Stats = new CricketStatsCollection(null, stats, teamName, season, matchTypes);

            TeamRecord = CricketStatsFactory.Generate(CricketStatTypes.TeamRecord, teamName, season, matchTypes);
        }

        /// <inheritdoc/>
        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            var innerHeaderElement = headerElement.GetNext();

            _ = rb.WriteTitle(Header, headerElement);

            TeamRecord.ExportStats(rb, innerHeaderElement);

            (PlayerScore Best, PlayerName Name) bestBatting = BattingStats?.PlayerBatting.Select(player => (player.Value.Best, player.Value.Name))?.Max() ?? (null, null);
            _ = rb.WriteParagraph(new string[] { "Best Batting:", bestBatting.Name.ToString(), bestBatting.Best.ToString() });

            (BowlingPerformance BestFigures, PlayerName Name) = BowlingStats?.PlayerBowling.Select(player => (player.Value.BestFigures, player.Value.Name))?.Max() ?? (null, null);
            _ = rb.WriteParagraph(new string[] { "Best Bowling:", Name.ToString(), BestFigures.ToString() });

            List<PlayerFieldingRecord> fielding = FieldingStats?.PlayerFielding.Select(val => val.Value).ToList();
            int mostKeeper = fielding.Max(player => player.TotalKeeperDismissals);
            List<PlayerName> keepers = fielding.Where(player => player.TotalKeeperDismissals.Equals(mostKeeper)).Select(player => player.Name).ToList();
            _ = rb.WriteParagraph(new string[] { "Most Dismissals as keeper:", $"{mostKeeper}", string.Join(",", keepers) });

            Stats.ExportStats(rb, headerElement);
        }
    }
}
