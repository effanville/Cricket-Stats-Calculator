using Cricket.Interfaces;
using Cricket.Statistics.PlayerStats;
using ExportHelpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            foreach (var season in team.Seasons)
            {
                CalculateStats(season);
            }
        }

        public void CalculateStats(ICricketSeason season)
        {
            var seasonStats = new TeamBriefStatistics(season);
            var manyCatches = seasonStats.SeasonPlayerStats.Where(player => player.FieldingStats.Catches > 20);
            TwentyCatchesSeason.AddRange(manyCatches.Select(catches => new SeasonCatches() { Name = catches.Name, Year = season.Year, SeasonDismissals = catches.FieldingStats.Catches }));

            var manyStumpings = seasonStats.SeasonPlayerStats.Where(player => player.FieldingStats.KeeperStumpings > 10);
            TenStumpingsSeason.AddRange(manyCatches.Select(catches => new SeasonCatches() { Name = catches.Name, Year = season.Year, SeasonDismissals = catches.FieldingStats.KeeperStumpings }));

            foreach (var match in season.Matches)
            {
                UpdateStats(match);
            }

            DismissalsInOneInnings.Sort((a, b) => b.Dismissals.CompareTo(a.Dismissals));
            TwentyCatchesSeason.Sort((a, b) => b.SeasonDismissals.CompareTo(a.SeasonDismissals));
            TenStumpingsSeason.Sort((a, b) => b.SeasonDismissals.CompareTo(a.SeasonDismissals));
        }

        public void UpdateStats(ICricketMatch match)
        {
            foreach (var field in match.FieldingStats.FieldingInfo)
            {
                if (field.TotalDismissals() > 4)
                {
                    DismissalsInOneInnings.Add(new InningsDismissals(field, match.MatchData));
                }
            }
        }

        public void ExportStats(StreamWriter writer)
        {
            if (DismissalsInOneInnings.Any())
            {
                writer.WriteLine("");
                writer.WriteLine("Overall Batting Performance");
                writer.WriteLine(GenericHeaderWriter.TableHeader(new InningsDismissals(), ","));
                foreach (var record in DismissalsInOneInnings)
                {
                    writer.WriteLine(record.ToCSVLine());
                }
            }

            if (TwentyCatchesSeason.Any())
            {
                writer.WriteLine("");
                writer.WriteLine("Overall Batting Performance");
                writer.WriteLine(GenericHeaderWriter.TableHeader(new SeasonCatches(), ","));
                foreach (var record in TwentyCatchesSeason)
                {
                    writer.WriteLine(record.ToCSVLine());
                }
            }

            if (TenStumpingsSeason.Any())
            {
                writer.WriteLine("");
                writer.WriteLine("Overall Batting Performance");
                writer.WriteLine(GenericHeaderWriter.TableHeader(new SeasonCatches(), ","));
                foreach (var record in TenStumpingsSeason)
                {
                    writer.WriteLine(record.ToCSVLine());
                }
            }
        }
    }
}
