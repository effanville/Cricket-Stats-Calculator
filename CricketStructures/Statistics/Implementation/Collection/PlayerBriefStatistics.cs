using System;
using CricketStructures.Season;
using CricketStructures.Player;
using Common.Structure.ReportWriting;
using CricketStructures.Statistics.Implementation.Player;
using CricketStructures.Statistics.Implementation.Player.Batting;
using CricketStructures.Statistics.Implementation.Player.Fielding;
using System.Collections.Generic;

namespace CricketStructures.Statistics.Implementation.Collection
{
    public sealed class PlayerBriefStatistics : IStatCollection
    {
        private readonly CricketStatsCollection Stats;

        public PlayerName Name
        {
            get;
            set;
        }

        public string SeasonName
        {
            get;
        }

        public DateTime SeasonYear
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public ICricketStat this[CricketStatTypes statisticType]
        {
            get => Stats[statisticType];
        }

        /// <inheritdoc/>
        public IReadOnlyList<CricketStatTypes> StatisticTypes => Stats.StatisticTypes;

        public PlayerBattingStatistics BattingStats => Stats[CricketStatTypes.PlayerBattingStats] as PlayerBattingStatistics;

        public PlayerBowlingStatistics BowlingStats => Stats[CricketStatTypes.PlayerBowlingStats] as PlayerBowlingStatistics;
        public PlayerFieldingStatistics FieldingStats => Stats[CricketStatTypes.PlayerFieldingStats] as PlayerFieldingStatistics;

        public PlayerAttendanceStatistics Played => Stats[CricketStatTypes.PlayerAttendanceStats] as PlayerAttendanceStatistics;

        internal PlayerBriefStatistics()
        {
        }

        internal PlayerBriefStatistics(string teamName, PlayerName name, ICricketSeason season, Match.MatchType[] matchTypes)
        {
            Name = name;
            SeasonName = season.Name;
            SeasonYear = season.Year;
            var stats = new[]
            {
                CricketStatTypes.PlayerAttendanceStats,
                CricketStatTypes.PlayerBattingStats,
                CricketStatTypes.PlayerPartnershipStats,
                CricketStatTypes.PlayerBowlingStats,
                CricketStatTypes.PlayerFieldingStats
            };
            Stats = new CricketStatsCollection($"Brief Statistics for player {name}", stats, teamName, season, matchTypes, name);
        }

        internal PlayerBriefStatistics(PlayerName name, ICricketTeam team, Match.MatchType[] matchTypes)
        {
            Name = name;
            var stats = new[]
{
                CricketStatTypes.PlayerAttendanceStats,
                CricketStatTypes.PlayerBattingStats,
                CricketStatTypes.PlayerPartnershipStats,
                CricketStatTypes.PlayerBowlingStats,
                CricketStatTypes.PlayerFieldingStats
            };
            Stats = new CricketStatsCollection($"Brief Statistics for player {name}", stats, team, matchTypes, name);
        }

        /// <inheritdoc/>
        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            var innerHeaderElement = headerElement.GetNext();
            _ = rb.WriteHeader($"Statistics for Player {Name}")
                .WriteTitle($"Brief Statistics for player {Name}", headerElement)
                .WriteTitle("Player Overall", innerHeaderElement);

            var played = Stats[CricketStatTypes.PlayerAttendanceStats] as PlayerAttendanceStatistics;
            _ = rb.WriteParagraph(new string[] { "Games Played:", $"{played.TotalGamesPlayed}" })
                .WriteParagraph(new string[] { "Wins:", $"{played.TotalGamesWon}" })
                .WriteParagraph(new string[] { "Losses:", $"{played.TotalGamesLost}" });

            var battingStats = Stats[CricketStatTypes.PlayerBattingStats] as PlayerBattingStatistics;
            if (battingStats.Best != null)
            {
                _ = rb.WriteParagraph(new string[] { "Best Batting:", battingStats.Best.ToString() });
            }

            var bowlingStats = Stats[CricketStatTypes.PlayerBowlingStats] as PlayerBowlingStatistics;
            if (bowlingStats.BestFigures != null)
            {
                _ = rb.WriteParagraph(new string[] { "Best Bowling:", bowlingStats.BestFigures.ToString() });
            }

            Stats.ExportStats(rb, innerHeaderElement);

            _ = rb.WriteFooter();
        }
    }
}
