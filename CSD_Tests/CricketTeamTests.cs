using System;
using CricketStructures.Player;
using CricketStructures;
using CricketStructures.Interfaces;
using NUnit.Framework;

namespace CricketStructures.Tests
{
    [TestFixture]
    public class CricketTeamTests
    {
        [Test]
        public void CanAddPlayer()
        {
            ICricketTeam team = new CricketTeam();
            PlayerName player = new PlayerName("Broad", "Stuart");

            Assert.IsTrue(team.AddPlayer(player));
            Assert.AreEqual(1, team.Players.Count);
        }

        [Test]
        public void CannotAddSecondSameNamePlayer()
        {
            CricketTeam team = new CricketTeam();
            PlayerName player = new PlayerName("Broad", "Stuart");

            Assert.IsTrue(team.AddPlayer(player));
            Assert.AreEqual(1, team.Players.Count);

            Assert.IsFalse(team.AddPlayer(player));
        }

        [TestCase("Broad", "Stuart", true)]
        [TestCase("White", "Stuart", false)]
        [TestCase("Broad", "Mark", false)]
        [TestCase(null, "Stuart", false)]
        [TestCase("Broad", null, false)]
        public void CanTestContainsPlayer(string surname, string forename, bool expectedContained)
        {
            CricketTeam team = new CricketTeam();
            PlayerName player = new PlayerName("Broad", "Stuart");
            team.AddPlayer(player);
            Assert.AreEqual(expectedContained, team.ContainsPlayer(new PlayerName(surname, forename)));
        }

        [TestCase("Broad", "Stuart", false)]
        [TestCase("White", "Stuart", false)]
        [TestCase("Broad", "Mark", false)]
        public void CanTestEmptyTeamContainsPlayer(string surname, string forename, bool expectedContained)
        {
            CricketTeam team = new CricketTeam();
            Assert.AreEqual(expectedContained, team.ContainsPlayer(new PlayerName(surname, forename)));
        }

        [TestCase("Broad", "Stuart", true)]
        [TestCase("White", "Stuart", false)]
        public void CanRemovePlayer(string surname, string forename, bool expectedRemoved)
        {
            CricketTeam team = new CricketTeam();
            PlayerName player = new PlayerName("Broad", "Stuart");

            team.AddPlayer(player);
            Assert.AreEqual(expectedRemoved, team.RemovePlayer(new PlayerName(surname, forename)));
            int number = expectedRemoved ? 0 : 1;
            Assert.AreEqual(number, team.Players.Count);
        }

        [Test]
        public void CanAddSeason()
        {
            CricketTeam team = new CricketTeam();
            DateTime date = new DateTime(2000, 1, 1);
            Assert.IsTrue(team.AddSeason(date, "Worst"));
            Assert.AreEqual(1, team.Seasons.Count);
        }

        [Test]
        public void CannotAddSecondSameNameSeason()
        {
            CricketTeam team = new CricketTeam();
            DateTime date = new DateTime(2000, 1, 1);
            Assert.IsTrue(team.AddSeason(date, "Worst"));
            Assert.AreEqual(1, team.Seasons.Count);

            Assert.IsFalse(team.AddSeason(date, "Worst"));
        }

        [TestCase("2000/1/1", "Worst", true)]
        [TestCase("2000/1/1", "Fish", false)]
        [TestCase("2001/1/1", "Worst", false)]
        [TestCase("2001/1/1", "Day", false)]
        [TestCase(null, "Day", false)]
        [TestCase("2001/1/1", null, false)]
        public void CanTestContainsSeason(DateTime dateToTest, string nameToTest, bool expectedContains)
        {
            CricketTeam team = new CricketTeam();
            DateTime date = new DateTime(2000, 1, 1);
            team.AddSeason(date, "Worst");
            Assert.AreEqual(expectedContains, team.ContainsSeason(dateToTest, nameToTest));
        }

        [TestCase("2000/1/1", "Worst", false)]
        [TestCase("2000/1/1", "Fish", false)]
        [TestCase("2001/1/1", "Worst", false)]
        public void CanTestEmptyTeamContainsSeason(DateTime dateToTest, string nameToTest, bool expectedContains)
        {
            CricketTeam team = new CricketTeam();
            Assert.AreEqual(expectedContains, team.ContainsSeason(dateToTest, nameToTest));
        }

        [TestCase("2000/1/1", "Worst", 1)]
        [TestCase("2000/1/1", "Fish", 0)]
        [TestCase("2001/1/1", "Worst", 0)]
        [TestCase("2001/1/1", "Day", 0)]
        public void CanRemoveSeason(DateTime dateToTest, string nameToTest, int expectedRemoved)
        {
            CricketTeam team = new CricketTeam();
            DateTime date = new DateTime(2000, 1, 1);
            _ = team.AddSeason(date, "Worst");
            Assert.AreEqual(1, team.Seasons.Count);

            Assert.AreEqual(expectedRemoved, team.RemoveSeason(dateToTest, nameToTest));
            int number = 1 - expectedRemoved;
            Assert.AreEqual(number, team.Seasons.Count);
        }
    }
}
