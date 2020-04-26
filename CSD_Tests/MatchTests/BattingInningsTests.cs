using Cricket.Match;
using Cricket.Player;
using NUnit.Framework;
using System.Collections.Generic;

namespace MatchTests
{
    [TestFixture]
    public class BattingInningsTests
    {
        [Test]
        public void CanCreate()
        {
            var player1 = new PlayerName("Bloggs", "Joe");
            var player2 = new PlayerName("Smith", "Steve");
            var playerNames = new List<PlayerName>() { player1, player2 };
            var innings = new BattingInnings(playerNames);

            Assert.AreEqual(2, innings.BattingInfo.Count);
            Assert.AreEqual(0, innings.Extras);
        }

        [Test]
        public void CanAddPlayer()
        {
            var player1 = new PlayerName("Bloggs", "Joe");
            var playerNames = new List<PlayerName>() { player1 };
            var innings = new BattingInnings(playerNames);

            Assert.AreEqual(1, innings.BattingInfo.Count);
            Assert.AreEqual(0, innings.Extras);

            var player2 = new PlayerName("Smith", "Steve");
            innings.AddPlayer(player2);

            Assert.AreEqual(2, innings.BattingInfo.Count);
        }

        [Test]
        public void CanCheckPlayerListed()
        {
            var player1 = new PlayerName("Bloggs", "Joe");
            var player2 = new PlayerName("Smith", "Steve");
            var playerNames = new List<PlayerName>() { player1, player2 };
            var innings = new BattingInnings(playerNames);

            Assert.AreEqual(2, innings.BattingInfo.Count);
            Assert.AreEqual(0, innings.Extras);

            Assert.AreEqual(true, innings.PlayerListed(player1));
            Assert.AreEqual(false, innings.PlayerListed(new PlayerName("Wood", "Mark")));
        }

        [Test]
        public void CanRemovePlayer()
        {
            var player1 = new PlayerName("Bloggs", "Joe");
            var player2 = new PlayerName("Smith", "Steve");
            var playerNames = new List<PlayerName>() { player1, player2 };
            var innings = new BattingInnings(playerNames);

            Assert.AreEqual(2, innings.BattingInfo.Count);
            Assert.AreEqual(0, innings.Extras);

            innings.Remove(player2);
            Assert.AreEqual(1, innings.BattingInfo.Count);
        }

        [Test]
        public void CanSetScores()
        {
            var player1 = new PlayerName("Bloggs", "Joe");
            var player2 = new PlayerName("Smith", "Steve");
            var playerNames = new List<PlayerName>() { player1, player2 };
            var innings = new BattingInnings(playerNames);

            Assert.AreEqual(2, innings.BattingInfo.Count);
            Assert.AreEqual(0, innings.Extras);
            innings.SetScores(player1, BattingWicketLossType.Bowled, 5);

            Assert.AreEqual(5, innings.BattingInfo[0].RunsScored);
            Assert.AreEqual(BattingWicketLossType.Bowled, innings.BattingInfo[0].MethodOut);

            Assert.AreEqual(0, innings.BattingInfo[1].RunsScored);
            Assert.AreEqual(BattingWicketLossType.DidNotBat, innings.BattingInfo[1].MethodOut);
        }

        [TestCase(new int[] { 0,0,0,0,0,0,0,0,0,0,0,0}, new[] { BattingWicketLossType.RunOut, BattingWicketLossType.RunOut, BattingWicketLossType.RunOut, BattingWicketLossType.RunOut, BattingWicketLossType.RunOut, BattingWicketLossType.RunOut, BattingWicketLossType.RunOut, BattingWicketLossType.RunOut, BattingWicketLossType.RunOut, BattingWicketLossType.RunOut, BattingWicketLossType.RunOut },0, 11,0)]
        [TestCase(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new[] { BattingWicketLossType.RunOut, BattingWicketLossType.RunOut, BattingWicketLossType.RunOut, BattingWicketLossType.RunOut, BattingWicketLossType.RunOut, BattingWicketLossType.RunOut, BattingWicketLossType.RunOut, BattingWicketLossType.RunOut, BattingWicketLossType.RunOut, BattingWicketLossType.RunOut, BattingWicketLossType.RunOut }, 5, 11, 5)]
        [TestCase(new int[] { 58, 73, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new[] { BattingWicketLossType.NotOut, BattingWicketLossType.NotOut, BattingWicketLossType.DidNotBat, BattingWicketLossType.DidNotBat, BattingWicketLossType.DidNotBat, BattingWicketLossType.DidNotBat, BattingWicketLossType.DidNotBat, BattingWicketLossType.DidNotBat, BattingWicketLossType.DidNotBat, BattingWicketLossType.DidNotBat, BattingWicketLossType.DidNotBat }, 5, 0, 136)]
        [TestCase(new int[] { 58, 73, 8, 22, 5, 0, 0, 0, 0, 0, 0, 0 }, new[] { BattingWicketLossType.Bowled, BattingWicketLossType.Stumped, BattingWicketLossType.NotOut, BattingWicketLossType.Caught, BattingWicketLossType.NotOut, BattingWicketLossType.DidNotBat, BattingWicketLossType.DidNotBat, BattingWicketLossType.DidNotBat, BattingWicketLossType.DidNotBat, BattingWicketLossType.DidNotBat, BattingWicketLossType.DidNotBat }, 5, 3, 171)]
        public void CanGetTotalMatchScore(int[] runs, BattingWicketLossType[] wicketTypes,int extras, int expectedWickets, int expectedRuns)
        {
            var playerNames = new List<PlayerName>();
            for (int i = 0; i < 11; i++)
            {
                string surname = "Bloggs" + i;
                string forename = "Joe" + i;
                playerNames.Add(new PlayerName(surname, forename));
            }
            var innings = new BattingInnings(playerNames);

            Assert.AreEqual(11, innings.BattingInfo.Count);
            Assert.AreEqual(0, innings.Extras);
            innings.Extras = extras;

            for (int i = 0; i < 11; i++)
            {
                innings.SetScores(playerNames[i], wicketTypes[i], runs[i]);
            }
            var score = innings.Score();

            Assert.AreEqual(expectedWickets, score.Wickets, "Wickets not correct");
            Assert.AreEqual(expectedRuns, score.Runs, "Runs not correct");
        }
    }
}
