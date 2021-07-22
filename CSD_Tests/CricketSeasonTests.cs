using System;
using CricketStructures.Match;
using CricketStructures.Season;
using NUnit.Framework;

namespace CricketStructures.Tests
{
    [TestFixture]
    public class CricketSeasonTests
    {
        [Test]
        public void CanCreate()
        {
            DateTime year = new DateTime(2010, 1, 1);
            string name = "Why";
            CricketSeason season = new CricketSeason(year, name);
            Assert.AreEqual(year, season.Year);
            Assert.AreEqual(name, season.Name);
        }

        [TestCase("Bloggs", "2000/1/1", "Bloggs", "2000/1/1", true)]
        [TestCase("Bloggs", "2000/1/1", "Bloggs", "2001/1/1", false)]
        [TestCase("Bloggs", "2000/1/1", "Simon", "2000/1/1", false)]
        [TestCase("Bloggs", "2000/1/1", "Simth", "2001/1/1", false)]
        [TestCase("Bloggs", "2000/1/1", "Bloggs", null, false)]
        [TestCase("Bloggs", "2000/1/1", null, "2000/1/1", false)]
        [TestCase("Bloggs", "2000/1/1", null, null, false)]
        [TestCase("Bloggs", null, "Bloggs", "2000/1/1", false)]
        [TestCase(null, "2000/1/1", "Bloggs", "2000/1/1", false)]
        [TestCase(null, null, "Bloggs", "2000/1/1", false)]
        [TestCase("Bloggs", null, "Bloggs", null, true)]
        [TestCase(null, "2000/1/1", null, "2000/1/1", true)]
        [TestCase(null, null, null, null, true)]
        public void EqualityCorrect(string name, DateTime year, string testingName, DateTime testingYear, bool expected)
        {
            CricketSeason season = new CricketSeason(year, name);
            Assert.AreEqual(expected, season.Equals(new CricketSeason(testingYear, testingName)));
        }

        [TestCase("Bloggs", "2000/1/1", "Bloggs", "2000/1/1", true)]
        [TestCase("Bloggs", "2000/1/1", "Bloggs", "2001/1/1", false)]
        [TestCase("Bloggs", "2000/1/1", "Simon", "2000/1/1", false)]
        [TestCase("Bloggs", "2000/1/1", "Simth", "2001/1/1", false)]
        [TestCase("Bloggs", "2000/1/1", "Bloggs", null, false)]
        [TestCase("Bloggs", "2000/1/1", null, "2000/1/1", false)]
        [TestCase("Bloggs", "2000/1/1", null, null, false)]
        [TestCase("Bloggs", null, "Bloggs", "2000/1/1", false)]
        [TestCase(null, "2000/1/1", "Bloggs", "2000/1/1", false)]
        [TestCase(null, null, "Bloggs", "2000/1/1", false)]
        [TestCase("Bloggs", null, "Bloggs", null, true)]
        [TestCase(null, "2000/1/1", null, "2000/1/1", true)]
        [TestCase(null, null, null, null, true)]
        public void SameSeasonCorrect(string name, DateTime year, string testingName, DateTime testingYear, bool expected)
        {
            CricketSeason season = new CricketSeason(year, name);
            Assert.AreEqual(expected, season.SameSeason(testingYear, testingName));
        }

        [Test]
        public void CanEditNames()
        {
            DateTime year = new DateTime(2010, 1, 1);
            string name = "Why";
            CricketSeason season = new CricketSeason(year, name);
            Assert.AreEqual(year, season.Year);
            Assert.AreEqual(name, season.Name);

            season.EditSeasonName(new DateTime(2002, 1, 1), "Fish");
            Assert.AreEqual(new DateTime(2002, 1, 1), season.Year);
            Assert.AreEqual("Fish", season.Name);
        }

        [Test]
        public void CanAddMatch()
        {
            DateTime year = new DateTime(2010, 1, 1);
            string name = "Why";
            CricketSeason season = new CricketSeason(year, name);

            MatchInfo matchInfo = new MatchInfo
            {
                Date = new DateTime(2010, 4, 3),
                HomeTeam = "Sandon"
            };
            _ = season.AddMatch(matchInfo);

            Assert.AreEqual(1, season.Matches.Count);
        }

        [TestCase("2010/4/3", "Sandon", true)]
        [TestCase("2010/4/3", "Wasps", false)]
        [TestCase("2010/3/3", "Sandon", false)]
        [TestCase("2010/2/3", "Maryland", false)]
        public void ContainsMatchTests(DateTime date, string opposition, bool expectedContains)
        {
            DateTime year = new DateTime(2010, 1, 1);
            string name = "Why";
            CricketSeason season = new CricketSeason(year, name);
            MatchInfo matchInfo = new MatchInfo
            {
                Date = new DateTime(2010, 4, 3),
                HomeTeam = "Sandon",
                AwayTeam = "Walkern"
            };
            _ = season.AddMatch(matchInfo);

            bool contains = season.ContainsMatch(date, opposition, "Walkern");
            Assert.AreEqual(expectedContains, contains);
        }

        [TestCase("2010/4/3", "Sandon", true)]
        [TestCase("2010/4/3", "Wasps", false)]
        [TestCase("2010/3/3", "Sandon", false)]
        [TestCase("2010/2/3", "Maryland", false)]
        public void CanRemoveMatch(DateTime date, string opposition, bool expectedRemoval)
        {
            DateTime year = new DateTime(2010, 1, 1);
            string name = "Why";
            CricketSeason season = new CricketSeason(year, name);
            MatchInfo matchInfo = new MatchInfo
            {
                Date = new DateTime(2010, 4, 3),
                HomeTeam = "Sandon",
                AwayTeam = "Walkern"
            };
            _ = season.AddMatch(matchInfo);

            bool removed = season.RemoveMatch(date, opposition, "Walkern");

            Assert.AreEqual(expectedRemoval, removed);
            int number = expectedRemoval ? 0 : 1;
            Assert.AreEqual(number, season.Matches.Count);
        }
    }
}
