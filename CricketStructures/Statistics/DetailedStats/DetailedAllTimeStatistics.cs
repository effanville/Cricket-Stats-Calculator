using System.IO;
using CricketStructures.Interfaces;
using StructureCommon.FileAccess;

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

        public void ExportStats(string filePath, ExportType exportType)
        {
            try
            {
                StreamWriter writer = new StreamWriter(filePath);

                if (exportType.Equals(ExportType.Html))
                {
                    FileWritingSupport.CreateHTMLHeader(writer, "Statistics for team", useColours: true);
                }

                FileWritingSupport.WriteTitle(writer, exportType, "Team Records");
                TeamAllTimeResults.ExportStats(writer, exportType);

                FileWritingSupport.WriteTitle(writer, exportType, "Partnership Records");
                PartnershipStatistics.ExportStats(writer, exportType);

                FileWritingSupport.WriteTitle(writer, exportType, "Individual Player Records");

                PlayerAllTimeDetailedStats.ExportStats(writer, exportType);

                if (exportType.Equals(ExportType.Html))
                {
                    FileWritingSupport.CreateHTMLFooter(writer);
                }
                writer.Close();
            }
            catch (IOException)
            {
            }
        }
    }
}
