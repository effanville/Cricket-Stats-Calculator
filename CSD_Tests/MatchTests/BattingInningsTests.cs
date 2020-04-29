using Cricket.Match;
using Cricket.Player;
using CSD_Tests;
using NUnit.Framework;
using System.Collections.Generic;
using Validation;

namespace CricketClasses.MatchTests
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
            var innings = new BattingInnings(null, playerNames);

            Assert.AreEqual(2, innings.BattingInfo.Count);
            Assert.AreEqual(0, innings.Extras);
        }

        [Test]
        public void CanAddPlayer()
        {
            var player1 = new PlayerName("Bloggs", "Joe");
            var playerNames = new List<PlayerName>() { player1 };
            var innings = new BattingInnings(null, playerNames);

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
            var innings = new BattingInnings(null, playerNames);

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
            var innings = new BattingInnings(null, playerNames);

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
            var innings = new BattingInnings(null, playerNames);

            Assert.AreEqual(2, innings.BattingInfo.Count);
            Assert.AreEqual(0, innings.Extras);
            innings.SetScores(player1, Wicket.Bowled, 5);

            Assert.AreEqual(5, innings.BattingInfo[0].RunsScored);
            Assert.AreEqual(Wicket.Bowled, innings.BattingInfo[0].MethodOut);

            Assert.AreEqual(0, innings.BattingInfo[1].RunsScored);
            Assert.AreEqual(Wicket.DidNotBat, innings.BattingInfo[1].MethodOut);
        }

        [TestCase(new int[] { 0,0,0,0,0,0,0,0,0,0,0,0}, new[] { Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut },0, 11,0)]
        [TestCase(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new[] { Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut }, 5, 11, 5)]
        [TestCase(new int[] { 58, 73, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new[] { Wicket.NotOut, Wicket.NotOut, Wicket.DidNotBat, Wicket.DidNotBat, Wicket.DidNotBat, Wicket.DidNotBat, Wicket.DidNotBat, Wicket.DidNotBat, Wicket.DidNotBat, Wicket.DidNotBat, Wicket.DidNotBat }, 5, 0, 136)]
        [TestCase(new int[] { 58, 73, 8, 22, 5, 0, 0, 0, 0, 0, 0, 0 }, new[] { Wicket.Bowled, Wicket.Stumped, Wicket.NotOut, Wicket.Caught, Wicket.NotOut, Wicket.DidNotBat, Wicket.DidNotBat, Wicket.DidNotBat, Wicket.DidNotBat, Wicket.DidNotBat, Wicket.DidNotBat }, 5, 3, 171)]
        public void CanGetTotalMatchScore(int[] runs, Wicket[] wicketTypes,int extras, int expectedWickets, int expectedRuns)
        {
            var playerNames = new List<PlayerName>();
            for (int i = 0; i < 11; i++)
            {
                string surname = "Bloggs" + i;
                string forename = "Joe" + i;
                playerNames.Add(new PlayerName(surname, forename));
            }
            var innings = new BattingInnings(null, playerNames);

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

        [TestCase(5, 5, true)]
        [TestCase(5, -5, false)]
        [TestCase(12, 0, false)]
        [TestCase(13, -5, false)]
        public void ValidityTests(int numberPlayers, int extras, bool isValid)
        {
            
            var innings = new BattingInnings();
            for (int i = 0; i < numberPlayers; i++)
            {
                innings.AddPlayer(new PlayerName("Surname" + i, "forename"));
            }

            innings.Extras = extras;

            var result = innings.Validate();

            Assert.AreEqual(isValid, result);
        }

        [TestCase(5, 5, true, new string[] { })]
        [TestCase(5, -5, false, new string[] { "Extras cannot take a negative value." })]
        [TestCase(12, 0, false, new string[] { "BattingInfo cannot take values above 11." })]
        public void ValidityMessageTests(int numberPlayers, int extras, bool isValid, string[] messages)
        {
            var innings = new BattingInnings();
            for (int i = 0; i < numberPlayers; i++)
            {
                innings.AddPlayer(new PlayerName("Surname" + i, "forename"));
            }

            innings.Extras = extras;

            var valid = innings.Validation();

            var expectedList = new List<ValidationResult>();
            if (!isValid)
            {
                var expected = new ValidationResult();
                expected.IsValid = isValid;
                expected.Messages.AddRange(messages);
                expectedList.Add(expected);
            }

            Assertions.AreEqualResults(expectedList, valid);
        }
    }
}
