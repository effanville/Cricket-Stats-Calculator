using System.IO;
using System.IO.Abstractions;

using Common.Structure.Reporting;
using Common.Structure.ReportWriting;

namespace CricketStructures.Statistics.Collection
{
    public static class StatCollectionExtensions
    {
        public static bool IsPlayerStat(this StatCollection stat)
        {
            return stat == StatCollection.PlayerBrief
                || stat == StatCollection.PlayerDetailed
                || stat == StatCollection.PlayerSeason;
        }

        public static bool IsSeasonStat(this StatCollection stat)
        {
            return stat == StatCollection.SeasonBrief
                || stat == StatCollection.PlayerSeason;
        }

        public static bool IsAllTimeStat(this StatCollection stat)
        {
            return stat == StatCollection.AllTimeBrief
                || stat == StatCollection.AllTimeDetailed;
        }

        /// <summary>
        /// Export the collection to a file.
        /// </summary>
        /// <param name="collection">The collection to export.</param>
        /// <param name="fileSystem"></param>
        /// <param name="filePath"></param>
        /// <param name="exportType"></param>
        public static void ExportStats(this IStatCollection collection, IFileSystem fileSystem, string filePath, DocumentType exportType, IReportLogger logger)
        {
            try
            {
                ReportBuilder rb = new ReportBuilder(exportType, new ReportSettings() { UseColours = true });
                _ = rb.WriteHeader(collection.Header);
                collection.ExportStats(rb, DocumentElement.h1);
                _ = rb.WriteFooter();

                using (Stream stream = fileSystem.FileStream.Create(filePath, FileMode.Create))
                using (StreamWriter fileWriter = new StreamWriter(stream))
                {
                    fileWriter.WriteLine(rb.ToString());
                }
            }
            catch (IOException exception)
            {
                _ = logger.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Saving, $"Error when creating stats: {exception.Message}.");
                return;
            }
        }
    }
}
