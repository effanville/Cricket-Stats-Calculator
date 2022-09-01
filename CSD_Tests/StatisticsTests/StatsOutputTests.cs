using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Statistics.Collection;

using NUnit.Framework;

namespace CricketStructures.Tests.StatisticsTests
{
    [TestFixture]
    public sealed class StatsOutputTests
    {
        [TestCase(StatCollection.SeasonBrief, "Example1.xml", "example1-season-brief.html")]
        [TestCase(StatCollection.AllTimeBrief, "Example1.xml", "example1-alltime-brief.html")]
        public void TeamStats(StatCollection statType, string teamFileName, string expectedOutputFile)
        {
            CricketTeam team = ExampleFileHelpers.GetExampleFromXmlFile<CricketTeam>(teamFileName);
            var seasons = team.Seasons;
            var season = seasons[0];
            var stats = StatsCollectionBuilder.StandardStat(statType, MatchHelpers.AllMatchTypes, team, "TeamName", season, null);

            var expectedFile = ExampleFileHelpers.GetLazyCachedExampleFile(expectedOutputFile);
            var rb = new ReportBuilder(DocumentType.Html, new ReportSettings(useColours: false, useDefaultStyle: true, useScripts: false));
            stats.ExportStats(rb, DocumentElement.h1);
            var outputString = rb.ToString();
            Assert.AreEqual(expectedFile, outputString);
        }

        [TestCase(StatCollection.PlayerSeason, "Example1.xml", "example1-playerseason-brief.html")]
        [TestCase(StatCollection.PlayerBrief, "Example1.xml", "example1-playeralltime-brief.html")]
        public void PlayerStats(StatCollection statType, string teamFileName, string expectedOutputFile)
        {
            CricketTeam team = ExampleFileHelpers.GetExampleFromXmlFile<CricketTeam>(teamFileName);
            var seasons = team.Seasons;
            var season = seasons[0];
            var playerName = new PlayerName("Chief", "Master");
            var stats = StatsCollectionBuilder.StandardStat(statType, MatchHelpers.AllMatchTypes, team, "TeamName", season, playerName);
            var rb = new ReportBuilder(DocumentType.Html, new ReportSettings(useColours: false, useDefaultStyle: true, useScripts: false));
            stats.ExportStats(rb, DocumentElement.h1);
            var outputString = rb.ToString();
            var expectedFile = ExampleFileHelpers.GetLazyCachedExampleFile(expectedOutputFile);


            Assert.AreEqual(expectedFile, outputString);
        }
    }
}
