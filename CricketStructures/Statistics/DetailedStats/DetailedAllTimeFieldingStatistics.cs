using System.Collections.Generic;
using System.Linq;
using CricketStructures.Season;
using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Statistics.PlayerStats;
using Common.Structure.FileAccess;
using Common.Structure.ReportWriting;
using System.Text;

namespace CricketStructures.Statistics.DetailedStats
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
                CalculateStats(team.TeamName, season);
            }
        }

        public void CalculateStats(string teamName, ICricketSeason season)
        {
            TeamBriefStatistics seasonStats = new TeamBriefStatistics(teamName, season);
            IEnumerable<PlayerBriefStatistics> manyCatches = seasonStats.SeasonPlayerStats.Where(player => player.FieldingStats.Catches > 20);
            TwentyCatchesSeason.AddRange(manyCatches.Select(catches => new SeasonCatches() { Name = catches.Name, Year = season.Year, SeasonDismissals = catches.FieldingStats.Catches }));

            IEnumerable<PlayerBriefStatistics> manyStumpings = seasonStats.SeasonPlayerStats.Where(player => player.FieldingStats.KeeperStumpings > 10);
            TenStumpingsSeason.AddRange(manyCatches.Select(catches => new SeasonCatches() { Name = catches.Name, Year = season.Year, SeasonDismissals = catches.FieldingStats.KeeperStumpings }));

            foreach (ICricketMatch match in season.Matches)
            {
                UpdateStats(teamName, match);
            }

            DismissalsInOneInnings.Sort((a, b) => b.Dismissals.CompareTo(a.Dismissals));
            TwentyCatchesSeason.Sort((a, b) => b.SeasonDismissals.CompareTo(a.SeasonDismissals));
            TenStumpingsSeason.Sort((a, b) => b.SeasonDismissals.CompareTo(a.SeasonDismissals));
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            foreach (FieldingEntry field in match.GetAllFielding(teamName))
            {
                if (field.TotalDismissals() > 4)
                {
                    DismissalsInOneInnings.Add(new InningsDismissals(teamName, field, match.MatchData));
                }
            }
        }

        public void ExportStats(StringBuilder writer, DocumentType exportType)
        {
            if (DismissalsInOneInnings.Any())
            {
                TextWriting.WriteTitle(writer, exportType, "Most Dismissals in on Innings", DocumentElement.h3);
                TableWriting.WriteTable(writer, exportType, DismissalsInOneInnings, headerFirstColumn: false);
            }

            if (TwentyCatchesSeason.Any())
            {
                TextWriting.WriteTitle(writer, exportType, "Twenty cathces in one season", DocumentElement.h3);
                TableWriting.WriteTable(writer, exportType, TwentyCatchesSeason, headerFirstColumn: false);
            }

            if (TenStumpingsSeason.Any())
            {
                TextWriting.WriteTitle(writer, exportType, "Ten Stumpings in one season", DocumentElement.h3);
                TableWriting.WriteTable(writer, exportType, TenStumpingsSeason, headerFirstColumn: false);
            }
        }
    }
}
