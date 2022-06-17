using System;
using CricketStructures.Season;
using CricketStructures.Player;
using Common.Structure.ReportWriting;
using System.Text;
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
            Stats = new CricketStatsCollection("Player Brief Statistics", stats, teamName, season, matchTypes, name);
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
            Stats = new CricketStatsCollection("Player Brief Statistics", stats, team, matchTypes, name);
        }

        /// <inheritdoc/>
        public StringBuilder ExportStats(DocumentType exportType, DocumentElement headerElement)
        {
            StringBuilder sb = new StringBuilder();
            var innerHeaderElement = headerElement++;
            TextWriting.WriteHeader(sb, exportType, $"Statistics for Player {Name}", useColours: true);

            TextWriting.WriteTitle(sb, exportType, $"Brief Statistics for player {Name}", headerElement);

            TextWriting.WriteTitle(sb, exportType, "Player Overall", innerHeaderElement);
            var played = Stats[CricketStatTypes.PlayerAttendanceStats] as PlayerAttendanceStatistics;
            TextWriting.WriteParagraph(sb, exportType, new string[] { "Games Played:", $"{played.TotalGamesPlayed}" });
            TextWriting.WriteParagraph(sb, exportType, new string[] { "Wins:", $"{played.TotalGamesWon}" });
            TextWriting.WriteParagraph(sb, exportType, new string[] { "Losses:", $"{played.TotalGamesLost}" });
            var battingStats = Stats[CricketStatTypes.PlayerBattingStats] as PlayerBattingStatistics;
            if (battingStats.Best != null)
            {
                TextWriting.WriteParagraph(sb, exportType, new string[] { "Best Batting:", battingStats.Best.ToString() });
            }

            var bowlingStats = Stats[CricketStatTypes.PlayerBowlingStats] as PlayerBowlingStatistics;
            if (bowlingStats.BestFigures != null)
            {
                TextWriting.WriteParagraph(sb, exportType, new string[] { "Best Bowling:", bowlingStats.BestFigures.ToString() });
            }

            _ = sb.Append(Stats.ExportStats(exportType, innerHeaderElement));

            TextWriting.WriteFooter(sb, exportType);

            return sb;
        }
    }
}
