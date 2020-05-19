using Cricket.Interfaces;
using System.IO;

namespace Cricket.Statistics.DetailedStats
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

        public void ExportStats(string filePath)
        {
            try
            {
                StreamWriter streamWriter = new StreamWriter(filePath);

                streamWriter.WriteLine($"Exporting Team {1 + 1}");

                streamWriter.WriteLine("");
                streamWriter.WriteLine("Team Records");
                streamWriter.WriteLine("");
                TeamAllTimeResults.ExportStats(streamWriter);

                streamWriter.WriteLine("");
                streamWriter.WriteLine("Partnership Records");
                streamWriter.WriteLine("");
                PartnershipStatistics.ExportStats(streamWriter);

                streamWriter.WriteLine("");
                streamWriter.WriteLine("Individual Player Records");
                streamWriter.WriteLine("");
                PlayerAllTimeDetailedStats.ExportStats(streamWriter);

                streamWriter.Close();
            }
            catch (IOException)
            {
            }
        }
    }
}
