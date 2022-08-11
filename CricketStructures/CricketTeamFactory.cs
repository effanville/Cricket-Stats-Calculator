using System.IO.Abstractions;

using Common.Structure.FileAccess;

using CricketStructures.Migration;

namespace CricketStructures
{
    public static class CricketTeamFactory
    {
        public static ICricketTeam Create()
        {
            return new CricketTeam();
        }

        public static ICricketTeam CreateFromFile(IFileSystem fileSystem, string filePath, out string error)
        {
            CricketTeam database = XmlFileAccess.ReadFromXmlFile<CricketTeam>(fileSystem, filePath, out error);
            if (string.IsNullOrEmpty(error))
            {
                database.SetupEventListening();
            }

            return database;
        }

        public static ICricketTeam CreateFromOldStyleFile(IFileSystem fileSystem, string filePath, out string error)
        {
            Cricket.Team.CricketTeam database = XmlFileAccess.ReadFromXmlFile<Cricket.Team.CricketTeam>(fileSystem, filePath, out error);
            CricketTeam newStyle = TeamConverter.Conversion(database);
            if (string.IsNullOrEmpty(error))
            {
                newStyle.SetupEventListening();
            }

            return newStyle;
        }
    }
}
