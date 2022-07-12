using System;
using System.Collections.Generic;
using System.Linq;
using CricketStructures.Season;
using CricketStructures.Player;
using CricketStructures.Player.Interfaces;
using Common.Structure.ReportWriting;
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
        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            _ = rb.WriteHeader("Statistics for team");
            var innerHeaderElement = headerElement.GetNext();
            if (!SeasonYear.HasValue)
            {
                _ = rb.WriteTitle("All time Brief Statistics", headerElement);
            }
            else
            {
                _ = rb.WriteTitle($"For season {SeasonYear.Value.Year}", headerElement);
            }

            TeamRecord.ExportStats(rb, innerHeaderElement);

            (BestBatting Best, PlayerName Name) bestBatting = SeasonPlayerStats.Select(player => (player.BattingStats.Best, player.Name)).Max();
            _ = rb.WriteParagraph(new string[] { "Best Batting:", bestBatting.Name.ToString(), bestBatting.Best.ToString() });

            (BestBowling BestFigures, PlayerName Name) = SeasonPlayerStats.Select(player => (player.BowlingStats.BestFigures, player.Name)).Max();
            _ = rb.WriteParagraph(new string[] { "Best Bowling:", Name.ToString(), BestFigures.ToString() });

            List<PlayerFieldingStatistics> fielding = SeasonPlayerStats.Select(player => player.FieldingStats).ToList();
            int mostKeeper = fielding.Max(player => player.TotalKeeperDismissals);
            List<PlayerName> keepers = fielding.Where(player => player.TotalKeeperDismissals.Equals(mostKeeper)).Select(player => player.Name).ToList();
            _ = rb.WriteParagraph(new string[] { "Most Dismissals as keeper:", $"{mostKeeper}", string.Join(",", keepers) });

            _ = rb.WriteTitle("Appearances", innerHeaderElement);

            List<PlayerAttendanceStatistics> played = SeasonPlayerStats.Select(player => player.Played as PlayerAttendanceStatistics).ToList();
            played.Sort((x, y) => y.TotalGamesPlayed.CompareTo(x.TotalGamesPlayed));
            _ = rb.WriteTable(played, headerFirstColumn: false);

            _ = rb.WriteTitle("Batting Stats", innerHeaderElement);
            List<PlayerBattingStatistics> batting = SeasonPlayerStats.Select(player => player.BattingStats).ToList();
            _ = batting.RemoveAll(bat => bat.TotalInnings.Equals(0));
            batting.Sort((x, y) => y.TotalRuns.CompareTo(x.TotalRuns));
            _ = rb.WriteTable(batting, headerFirstColumn: false);

            Partnerships.ExportStats(rb, innerHeaderElement);

            _ = rb.WriteTitle("Bowling Stats", innerHeaderElement);
            List<PlayerBowlingStatistics> bowling = SeasonPlayerStats.Select(player => player.BowlingStats).ToList();
            _ = bowling.RemoveAll(bowl => bowl.TotalOvers.Equals(0));
            bowling.Sort((x, y) => y.TotalWickets.CompareTo(x.TotalWickets));
            _ = rb.WriteTable(bowling, headerFirstColumn: false);

            _ = rb.WriteTitle("Fielding Stats", innerHeaderElement);
            _ = fielding.RemoveAll(field => field.TotalDismissals.Equals(0));
            fielding.Sort((x, y) => y.TotalDismissals.CompareTo(x.TotalDismissals));
            _ = rb.WriteTable(fielding, headerFirstColumn: false);

            _ = rb.WriteFooter();
        }
    }
}
