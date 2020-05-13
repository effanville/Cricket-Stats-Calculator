using Cricket;
using Cricket.Match;
using NUnit.Framework;
using System;

namespace CricketClasses.SeasonTests
{
    [TestFixture]
    public class CricketSeasonTests
    {
        [Test]
        public void CanCreate()
        {
            var year = new DateTime(2010, 1, 1);
            var name = "Why";
            var season = new CricketSeason(year, name);
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
            var season = new CricketSeason(year, name);
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
            var season = new CricketSeason(year, name);
            Assert.AreEqual(expected, season.SameSeason(testingYear, testingName));
        }

        [Test]
        public void CanEditNames()
        {
            var year = new DateTime(2010, 1, 1);
            var name = "Why";
            var season = new CricketSeason(year, name);
            Assert.AreEqual(year, season.Year);
            Assert.AreEqual(name, season.Name);

            season.EditSeasonName(new DateTime(2002, 1, 1), "Fish");
            Assert.AreEqual(new DateTime(2002, 1, 1), season.Year);
            Assert.AreEqual("Fish", season.Name);
        }

        [Test]
        public void CanAddMatch()
        {
            var year = new DateTime(2010, 1, 1);
            var name = "Why";
            var season = new CricketSeason(year, name);

            var matchInfo = new MatchInfo
            {
                Date = new DateTime(2010, 4, 3),
                Opposition = "Sandon"
            };
            season.AddMatch(matchInfo);

            Assert.AreEqual(1, season.Matches.Count);
        }

        [TestCase("2010/4/3", "Sandon", true)]
        [TestCase("2010/4/3", "Wasps", false)]
        [TestCase("2010/3/3", "Sandon", false)]
        [TestCase("2010/2/3", "Maryland", false)]
        public void ContainsMatchTests(DateTime date, string opposition, bool expectedContains)
        {
            var year = new DateTime(2010, 1, 1);
            var name = "Why";
            var season = new CricketSeason(year, name);
            var matchInfo = new MatchInfo
            {
                Date = new DateTime(2010, 4, 3),
                Opposition = "Sandon"
            };
            season.AddMatch(matchInfo);

            var contains = season.ContainsMatch(date, opposition);
            Assert.AreEqual(expectedContains, contains);
        }

        [TestCase("2010/4/3", "Sandon", true)]
        [TestCase("2010/4/3", "Wasps", false)]
        [TestCase("2010/3/3", "Sandon", false)]
        [TestCase("2010/2/3", "Maryland", false)]
        public void CanRemoveMatch(DateTime date, string opposition, bool expectedRemoval)
        {
            var year = new DateTime(2010, 1, 1);
            var name = "Why";
            var season = new CricketSeason(year, name);
            var matchInfo = new MatchInfo
            {
                Date = new DateTime(2010, 4, 3),
                Opposition = "Sandon"
            };
            season.AddMatch(matchInfo);

            var removed = season.RemoveMatch(date, opposition);

            Assert.AreEqual(expectedRemoval, removed);
            int number = expectedRemoval ? 0 : 1;
            Assert.AreEqual(number, season.Matches.Count);
        }
    }
}
