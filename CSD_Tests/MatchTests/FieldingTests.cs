using Cricket.Match;
using Cricket.Player;
using NUnit.Framework;
using System.Collections.Generic;

namespace CricketClasses.MatchTests
{
    [TestFixture]
    class FieldingTests
    {
        [Test]
        public void CanCreate()
        {
            var player1 = new PlayerName("Bloggs", "Joe");
            var player2 = new PlayerName("Smith", "Steve");
            var playerNames = new List<PlayerName>() { player1, player2 };
            var innings = new Fielding(playerNames);

            Assert.AreEqual(2, innings.FieldingInfo.Count);
        }

        [Test]
        public void CanAddPlayer()
        {
            var player1 = new PlayerName("Bloggs", "Joe");
            var playerNames = new List<PlayerName>() { player1 };
            var innings = new Fielding(playerNames);

            Assert.AreEqual(1, innings.FieldingInfo.Count);

            var player2 = new PlayerName("Smith", "Steve");
            innings.AddPlayer(player2);

            Assert.AreEqual(2, innings.FieldingInfo.Count);
        }

        [Test]
        public void CanCheckPlayerListed()
        {
            var player1 = new PlayerName("Bloggs", "Joe");
            var player2 = new PlayerName("Smith", "Steve");
            var playerNames = new List<PlayerName>() { player1, player2 };
            var innings = new Fielding(playerNames);

            Assert.AreEqual(2, innings.FieldingInfo.Count);

            Assert.AreEqual(true, innings.PlayerListed(player1));
            Assert.AreEqual(false, innings.PlayerListed(new PlayerName("Wood", "Mark")));
        }

        [Test]
        public void CanRemovePlayer()
        {
            var player1 = new PlayerName("Bloggs", "Joe");
            var player2 = new PlayerName("Smith", "Steve");
            var playerNames = new List<PlayerName>() { player1, player2 };
            var innings = new Fielding(playerNames);

            Assert.AreEqual(2, innings.FieldingInfo.Count);

            innings.Remove(player2);
            Assert.AreEqual(1, innings.FieldingInfo.Count);
        }

        [Test]
        public void CanSetScores()
        {
            var player1 = new PlayerName("Bloggs", "Joe");
            var player2 = new PlayerName("Smith", "Steve");
            var playerNames = new List<PlayerName>() { player1, player2 };
            var innings = new Fielding(playerNames);

            Assert.AreEqual(2, innings.FieldingInfo.Count);
            innings.SetFielding(player1, 4, 2, 7, 5);

            Assert.AreEqual(4, innings.FieldingInfo[0].Catches);
            Assert.AreEqual(2, innings.FieldingInfo[0].RunOuts);
            Assert.AreEqual(7, innings.FieldingInfo[0].KeeperStumpings);
            Assert.AreEqual(5, innings.FieldingInfo[0].KeeperCatches);

            Assert.AreEqual(0, innings.FieldingInfo[1].Catches);
            Assert.AreEqual(0, innings.FieldingInfo[1].RunOuts);
            Assert.AreEqual(0, innings.FieldingInfo[1].KeeperStumpings);
            Assert.AreEqual(0, innings.FieldingInfo[1].KeeperCatches);
        }
    }
}
