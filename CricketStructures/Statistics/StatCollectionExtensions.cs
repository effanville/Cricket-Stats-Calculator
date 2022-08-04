using System.IO;
using System.IO.Abstractions;

using Common.Structure.ReportWriting;

namespace CricketStructures.Statistics
{
    public static class StatCollectionExtensions
    {
        /// <summary>
        /// Export the collection to a file.
        /// </summary>
        /// <param name="collection">The collection to export.</param>
        /// <param name="fileSystem"></param>
        /// <param name="filePath"></param>
        /// <param name="exportType"></param>
        public static void ExportStats(this IStatCollection collection, IFileSystem fileSystem, string filePath, DocumentType exportType)
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
            catch (IOException)
            {
                return;
            }
        }
    }
}
