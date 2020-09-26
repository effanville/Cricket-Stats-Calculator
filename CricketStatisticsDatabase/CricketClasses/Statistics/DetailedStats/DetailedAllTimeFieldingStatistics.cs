using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cricket.Interfaces;
using Cricket.Statistics.PlayerStats;
using StructureCommon.FileAccess;

namespace Cricket.Statistics.DetailedStats
{
    public class DetailedAllTimeFieldingStatistics
    {
        public List<InningsDismissals> DismissalsInOneInnings
        {
            get;
            set;
        } = new List<InningsDismissals>();

        public List<SeasonCatches> TwentyCatchesSeason
        {
            get;
            set;
        } = new List<SeasonCatches>();

        public List<SeasonCatches> TenStumpingsSeason
        {
            get;
            set;
        } = new List<SeasonCatches>();

        public void CalculateStats(ICricketTeam team)
        {
            foreach (ICricketSeason season in team.Seasons)
            {
                CalculateStats(season);
            }
        }

        public void CalculateStats(ICricketSeason season)
        {
            TeamBriefStatistics seasonStats = new TeamBriefStatistics(season);
            IEnumerable<PlayerBriefStatistics> manyCatches = seasonStats.SeasonPlayerStats.Where(player => player.FieldingStats.Catches > 20);
            TwentyCatchesSeason.AddRange(manyCatches.Select(catches => new SeasonCatches() { Name = catches.Name, Year = season.Year, SeasonDismissals = catches.FieldingStats.Catches }));

            IEnumerable<PlayerBriefStatistics> manyStumpings = seasonStats.SeasonPlayerStats.Where(player => player.FieldingStats.KeeperStumpings > 10);
            TenStumpingsSeason.AddRange(manyCatches.Select(catches => new SeasonCatches() { Name = catches.Name, Year = season.Year, SeasonDismissals = catches.FieldingStats.KeeperStumpings }));

            foreach (ICricketMatch match in season.Matches)
            {
                UpdateStats(match);
            }

            DismissalsInOneInnings.Sort((a, b) => b.Dismissals.CompareTo(a.Dismissals));
            TwentyCatchesSeason.Sort((a, b) => b.SeasonDismissals.CompareTo(a.SeasonDismissals));
            TenStumpingsSeason.Sort((a, b) => b.SeasonDismissals.CompareTo(a.SeasonDismissals));
        }

        public void UpdateStats(ICricketMatch match)
        {
            foreach (Match.FieldingEntry field in match.FieldingStats.FieldingInfo)
            {
                if (field.TotalDismissals() > 4)
                {
                    DismissalsInOneInnings.Add(new InningsDismissals(field, match.MatchData));
                }
            }
        }

        public void ExportStats(StreamWriter writer, ExportType exportType)
        {
            if (DismissalsInOneInnings.Any())
            {
                FileWritingSupport.WriteTitle(writer, exportType, "Most Dismissals in on Innings", HtmlTag.h3);
                FileWritingSupport.WriteTable(writer, exportType, DismissalsInOneInnings, headerFirstColumn: false);
            }

            if (TwentyCatchesSeason.Any())
            {
                FileWritingSupport.WriteTitle(writer, exportType, "Twenty cathces in one season", HtmlTag.h3);
                FileWritingSupport.WriteTable(writer, exportType, TwentyCatchesSeason, headerFirstColumn: false);
            }

            if (TenStumpingsSeason.Any())
            {
                FileWritingSupport.WriteTitle(writer, exportType, "Ten Stumpings in one season", HtmlTag.h3);
                FileWritingSupport.WriteTable(writer, exportType, TenStumpingsSeason, headerFirstColumn: false);
            }
        }
    }
}
