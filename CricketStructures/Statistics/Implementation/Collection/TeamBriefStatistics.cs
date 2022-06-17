using System;
using System.Collections.Generic;
using System.Linq;
using CricketStructures.Season;
using CricketStructures.Player;
using CricketStructures.Player.Interfaces;
using Common.Structure.ReportWriting;
using System.Text;
using CricketStructures.Statistics.Implementation.Player;
using CricketStructures.Statistics.Implementation.Player.Batting;
using CricketStructures.Statistics.Implementation.Player.Fielding;
using CricketStructures.Statistics.Implementation.Player.Model;

namespace CricketStructures.Statistics.Implementation.Collection
{
    /// <summary>
    /// A default statistics collection with brief records for a team over all time or 
    /// for a single specific season.
    /// </summary>
    public sealed class TeamBriefStatistics : IStatCollection
    {
        public DateTime? SeasonYear
        {
            get;
            set;
        }

        public List<PlayerBriefStatistics> SeasonPlayerStats
        {
            get;
            set;
        } = new List<PlayerBriefStatistics>();

        private readonly ICricketStat Partnerships;

        private readonly ICricketStat TeamRecord;

        /// <inheritdoc/>
        public ICricketStat this[CricketStatTypes statisticType]
        {
            get
            {
                switch (statisticType)
                {
                    case CricketStatTypes.TeamPartnershipStats:
                        return Partnerships;
                    case CricketStatTypes.TeamRecord:
                        return TeamRecord;
                    default:
                        return null;
                }
            }
        }

        /// <inheritdoc/>
        public IReadOnlyList<CricketStatTypes> StatisticTypes => new List<CricketStatTypes>() { CricketStatTypes.TeamPartnershipStats, CricketStatTypes.TeamRecord };

        internal TeamBriefStatistics()
        {
        }

        internal TeamBriefStatistics(ICricketTeam team, Match.MatchType[] matchTypes)
        {
            CalculatePlayerStats(team, matchTypes);
            Partnerships = CricketStatsFactory.Generate(CricketStatTypes.TeamPartnershipStats, team, matchTypes);
            TeamRecord = CricketStatsFactory.Generate(CricketStatTypes.TeamRecord, team, matchTypes);
        }

        internal TeamBriefStatistics(string teamName, ICricketSeason season, Match.MatchType[] matchTypes)
        {
            SeasonYear = season.Year;
            CalculatePlayerStats(teamName, season, matchTypes);
            Partnerships = CricketStatsFactory.Generate(CricketStatTypes.TeamPartnershipStats, teamName, season, matchTypes);
            TeamRecord = CricketStatsFactory.Generate(CricketStatTypes.TeamRecord, teamName, season, matchTypes);
        }

        public void CalculatePlayerStats(ICricketTeam team, Match.MatchType[] matchTypes)
        {
            foreach (ICricketPlayer player in team.Players())
            {
                PlayerBriefStatistics playerStats = new PlayerBriefStatistics(player.Name, team, matchTypes);
                SeasonPlayerStats.Add(playerStats);
            }
        }

        public void CalculatePlayerStats(string teamName, ICricketSeason season, Match.MatchType[] matchTypes)
        {
            if (!season.Year.Equals(SeasonYear))
            {
                return;
            }

            foreach (PlayerName player in season.Players(teamName))
            {
                PlayerBriefStatistics playerStats = new PlayerBriefStatistics(teamName, player, season, matchTypes);
                SeasonPlayerStats.Add(playerStats);
            }
        }

        /// <inheritdoc/>
        public StringBuilder ExportStats(DocumentType exportType, DocumentElement headerElement)
        {
            StringBuilder sb = new StringBuilder();
            TextWriting.WriteHeader(sb, exportType, "Statistics for team", useColours: true);
            var innerHeaderElement = headerElement++;
            if (!SeasonYear.HasValue)
            {
                TextWriting.WriteTitle(sb, exportType, "All time Brief Statistics", headerElement);
            }
            else
            {
                TextWriting.WriteTitle(sb, exportType, $"For season {SeasonYear.Value.Year}", headerElement);
            }

            _ = sb.Append(TeamRecord.ExportStats(exportType, innerHeaderElement));

            (BestBatting Best, PlayerName Name) bestBatting = SeasonPlayerStats.Select(player => (player.BattingStats.Best, player.Name)).Max();
            TextWriting.WriteParagraph(sb, exportType, new string[] { "Best Batting:", bestBatting.Name.ToString(), bestBatting.Best.ToString() });

            (BestBowling BestFigures, PlayerName Name) = SeasonPlayerStats.Select(player => (player.BowlingStats.BestFigures, player.Name)).Max();
            TextWriting.WriteParagraph(sb, exportType, new string[] { "Best Bowling:", Name.ToString(), BestFigures.ToString() });

            List<PlayerFieldingStatistics> fielding = SeasonPlayerStats.Select(player => player.FieldingStats).ToList();
            int mostKeeper = fielding.Max(player => player.TotalKeeperDismissals);
            List<PlayerName> keepers = fielding.Where(player => player.TotalKeeperDismissals.Equals(mostKeeper)).Select(player => player.Name).ToList();
            TextWriting.WriteParagraph(sb, exportType, new string[] { "Most Dismissals as keeper:", $"{mostKeeper}", string.Join(",", keepers) });

            TextWriting.WriteTitle(sb, exportType, "Appearances", innerHeaderElement);

            List<PlayerAttendanceStatistics> played = SeasonPlayerStats.Select(player => player.Played as PlayerAttendanceStatistics).ToList();
            played.Sort((x, y) => y.TotalGamesPlayed.CompareTo(x.TotalGamesPlayed));
            TableWriting.WriteTable(sb, exportType, played, headerFirstColumn: false);

            TextWriting.WriteTitle(sb, exportType, "Batting Stats", innerHeaderElement);
            List<PlayerBattingStatistics> batting = SeasonPlayerStats.Select(player => player.BattingStats).ToList();
            _ = batting.RemoveAll(bat => bat.TotalInnings.Equals(0));
            batting.Sort((x, y) => y.TotalRuns.CompareTo(x.TotalRuns));
            TableWriting.WriteTable(sb, exportType, batting, headerFirstColumn: false);

            _ = sb.Append(Partnerships.ExportStats(exportType, innerHeaderElement));

            TextWriting.WriteTitle(sb, exportType, "Bowling Stats", innerHeaderElement);
            List<PlayerBowlingStatistics> bowling = SeasonPlayerStats.Select(player => player.BowlingStats).ToList();
            _ = bowling.RemoveAll(bowl => bowl.TotalOvers.Equals(0));
            bowling.Sort((x, y) => y.TotalWickets.CompareTo(x.TotalWickets));
            TableWriting.WriteTable(sb, exportType, bowling, headerFirstColumn: false);

            TextWriting.WriteTitle(sb, exportType, "Fielding Stats", innerHeaderElement);
            _ = fielding.RemoveAll(field => field.TotalDismissals.Equals(0));
            fielding.Sort((x, y) => y.TotalDismissals.CompareTo(x.TotalDismissals));
            TableWriting.WriteTable(sb, exportType, fielding, headerFirstColumn: false);

            TextWriting.WriteFooter(sb, exportType);

            return sb;
        }
    }
}
