using System.IO;
using System.IO.Abstractions;
using System.Text;
using Common.Structure.FileAccess;
using Common.Structure.ReportWriting;

namespace CricketStructures.Statistics.DetailedStats
{
    public class DetailedAllTimeStatistics
    {
        public TeamResultStats TeamAllTimeResults
        {
            get;
            set;
        } = new TeamResultStats();

        public PartnershipStats PartnershipStatistics
        {
            get;
            set;
        } = new PartnershipStats();

        public DetailedAllTimePlayerStatistics PlayerAllTimeDetailedStats
        {
            get;
            set;
        } = new DetailedAllTimePlayerStatistics();

        public DetailedAllTimeStatistics()
        {
        }

        public DetailedAllTimeStatistics(ICricketTeam team)
        {
            GenerateAllTimeStats(team);
        }

        public void GenerateAllTimeStats(ICricketTeam team)
        {
            PartnershipStatistics.CalculateStats(team);
            TeamAllTimeResults.CalculateStats(team);
            PlayerAllTimeDetailedStats.CalculateStats(team);
        }

        public void ExportStats(IFileSystem fileSystem, string filePath, ExportType exportType)
        {
            try
            {
                StringBuilder sb = ExportString(exportType);

                using (Stream stream = fileSystem.FileStream.Create(filePath, FileMode.Create))
                using (StreamWriter fileWriter = new StreamWriter(stream))
                {
                    fileWriter.WriteLine(sb.ToString());
                }
            }
            catch (IOException)
            {
                return;
            }

            StringBuilder ExportString(ExportType exportType)
            {
                StringBuilder sb = new StringBuilder();

                if (exportType.Equals(ExportType.Html))
                {
                    TextWriting.CreateHTMLHeader(sb, "Statistics for team", useColours: true);
                }

                TextWriting.WriteTitle(sb, exportType, "Team Records");
                TeamAllTimeResults.ExportStats(sb, exportType);

                TextWriting.WriteTitle(sb, exportType, "Partnership Records");
                PartnershipStatistics.ExportStats(sb, exportType);

                TextWriting.WriteTitle(sb, exportType, "Individual Player Records");

                PlayerAllTimeDetailedStats.ExportStats(sb, exportType);

                if (exportType.Equals(ExportType.Html))
                {
                    TextWriting.CreateHTMLFooter(sb);
                }

                return sb;
            }
        }
    }
}
