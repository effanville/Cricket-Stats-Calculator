using System;
using System.Collections.Generic;
using System.Linq;
using CricketStructures.Season;
using CricketStructures.Player;
using CricketStructures.Player.Interfaces;
using Common.Structure.ReportWriting;
using CricketStructures.Statistics.Implementation.Player;
using CricketStructures.Statistics.Implementation.Player.Model;
using CricketStructures.Statistics.Implementation.Player.Fielding;
using CricketStructures.Match.Innings;

namespace CricketStructures.Statistics.Implementation.Collection
{
    /// <summary>
    /// A default statistics collection with brief records for a team over all time or 
    /// for a single specific season.
    /// </summary>
    public sealed class TeamBriefStatistics : IStatCollection
    {
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
            Header = $"Statistics for {team.TeamName}";
            CalculatePlayerStats(team, matchTypes);
            Partnerships = CricketStatsFactory.Generate(CricketStatTypes.TeamPartnershipStats, team, matchTypes);
            TeamRecord = CricketStatsFactory.Generate(CricketStatTypes.TeamRecord, team, matchTypes);
        }

        internal TeamBriefStatistics(string teamName, ICricketSeason season, Match.MatchType[] matchTypes)
        {
            Header = $"Statistics for {teamName} for the year {season.Year.Year}";
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

            foreach (PlayerName player in season.Players(teamName, matchTypes))
            {
                PlayerBriefStatistics playerStats = new PlayerBriefStatistics(teamName, player, season, matchTypes);
                SeasonPlayerStats.Add(playerStats);
            }
        }

        /// <inheritdoc/>
        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            var innerHeaderElement = headerElement.GetNext();

                _ = rb.WriteTitle(Header, headerElement);

            TeamRecord.ExportStats(rb, innerHeaderElement);

            (PlayerScore Best, PlayerName Name) bestBatting = SeasonPlayerStats?.Select(player => (player.BattingStats.Best, player.Name))?.Max() ?? (null, null);
            _ = rb.WriteParagraph(new string[] { "Best Batting:", bestBatting.Name.ToString(), bestBatting.Best.ToString() });

            (BowlingPerformance BestFigures, PlayerName Name) = SeasonPlayerStats.Select(player => (player.BowlingStats.BestFigures, player.Name))?.Max() ?? (null, null);
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
            List<PlayerBattingRecord> batting = SeasonPlayerStats.Select(player => player.BattingStats).ToList();
            _ = batting.RemoveAll(bat => bat.TotalInnings.Equals(0));
            batting.Sort((x, y) => y.TotalRuns.CompareTo(x.TotalRuns));


            var headers = new string[]
            {
                "Name",
                "Innings",
                "Not Out",
                "Runs",
                "Average",
                "Centuries",
                "Fifties",
                "Best"
            };
            var fields = batting.Select(stat => new string[]
            {
                stat.Name.ToString(),
                stat.TotalInnings.ToString(),
                stat.TotalNotOut.ToString(),
                stat.TotalRuns.ToString(),
                stat.Average.ToString(),
                stat.Centuries.ToString(),
                stat.Fifties.ToString(),
                stat.Best.ToString()
            });
            _ = rb.WriteTableFromEnumerable(headers, fields, headerFirstColumn: false);

            Partnerships.ExportStats(rb, innerHeaderElement);

            _ = rb.WriteTitle("Bowling Stats", innerHeaderElement);
            List<PlayerBowlingRecord> bowling = SeasonPlayerStats.Select(player => player.BowlingStats).ToList();
            _ = bowling.RemoveAll(bowl => bowl.TotalOvers.Equals(0));
            bowling.Sort((x, y) => y.TotalWickets.CompareTo(x.TotalWickets));

            var bowlingHeaders = new string[]
    {
                    "Name",
                    "Overs",
                    "Maidens",
                    "Runs Conceded",
                    "Wickets",
                    "Average",
                    "Economy",
                    "Strike Rate",
                    "Best Figures"
    };
            var bowlingFields = bowling.Select(stat => new string[]
            {
                stat.Name.ToString(),
                stat.TotalOvers.ToString(),
                stat.TotalMaidens.ToString(),
                stat.TotalRunsConceded.ToString(),
                stat.TotalWickets.ToString(),
                stat.Average.ToString(),
                stat.Economy.ToString(),
                stat.StrikeRate.ToString(),
                stat.BestFigures.ToString(),
            });
            _ = rb.WriteTableFromEnumerable(bowlingHeaders, bowlingFields, headerFirstColumn: false);

            _ = rb.WriteTitle("Fielding Stats", innerHeaderElement);
            _ = fielding.RemoveAll(field => field.TotalDismissals.Equals(0));
            fielding.Sort((x, y) => y.TotalDismissals.CompareTo(x.TotalDismissals));
            var fieldingHeaders = new string[]
            {
                    "Name",
                    "Catches",
                    "Run Outs",
                    "Stumpings",
                    "KeeperCatches",
                    "Total"
            };
            var fieldingFields = fielding.Select(stat => new string[]
            {
                stat.Name.ToString(),
                stat.Catches.ToString(),
                stat.RunOuts.ToString(),
                stat.KeeperStumpings.ToString(),
                stat.KeeperCatches.ToString(),
                stat.TotalDismissals.ToString()
            });
            _ = rb.WriteTableFromEnumerable(fieldingHeaders, fieldingFields, headerFirstColumn: false);
        }
    }
}
