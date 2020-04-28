using Cricket.Match;
using Cricket.Player;
using CSD_Tests;
using NUnit.Framework;
using System.Collections.Generic;
using Validation;

namespace CricketClasses.MatchTests
{
    public class BowlingInningsTests
    { 
        [Test]
        public void CanCreate()
        {
            var player1 = new PlayerName("Bloggs", "Joe");
            var player2 = new PlayerName("Smith", "Steve");
            var playerNames = new List<PlayerName>() { player1, player2 };
            var innings = new BowlingInnings(playerNames);

            Assert.AreEqual(2, innings.BowlingInfo.Count);
            Assert.AreEqual(0, innings.ByesLegByes);
        }

        [Test]
        public void CanAddPlayer()
        {
            var player1 = new PlayerName("Bloggs", "Joe");
            var playerNames = new List<PlayerName>() { player1 };
            var innings = new BowlingInnings(playerNames);

            Assert.AreEqual(1, innings.BowlingInfo.Count);

            var player2 = new PlayerName("Smith", "Steve");
            innings.AddPlayer(player2);

            Assert.AreEqual(2, innings.BowlingInfo.Count);
        }

        [Test]
        public void CanCheckPlayerListed()
        {
            var player1 = new PlayerName("Bloggs", "Joe");
            var player2 = new PlayerName("Smith", "Steve");
            var playerNames = new List<PlayerName>() { player1, player2 };
            var innings = new BowlingInnings(playerNames);

            Assert.AreEqual(2, innings.BowlingInfo.Count);

            Assert.AreEqual(true, innings.PlayerListed(player1));
            Assert.AreEqual(false, innings.PlayerListed(new PlayerName("Wood", "Mark")));
        }

        [Test]
        public void CanRemovePlayer()
        {
            var player1 = new PlayerName("Bloggs", "Joe");
            var player2 = new PlayerName("Smith", "Steve");
            var playerNames = new List<PlayerName>() { player1, player2 };
            var innings = new BowlingInnings(playerNames);

            Assert.AreEqual(2, innings.BowlingInfo.Count);

            innings.Remove(player2);
            Assert.AreEqual(1, innings.BowlingInfo.Count);
        }

        [Test]
        public void CanSetScores()
        {
            var player1 = new PlayerName("Bloggs", "Joe");
            var player2 = new PlayerName("Smith", "Steve");
            var playerNames = new List<PlayerName>() { player1, player2 };
            var innings = new BowlingInnings(playerNames);

            Assert.AreEqual(2, innings.BowlingInfo.Count);
            innings.SetScores(player1, 4,2,7, 5);

            Assert.AreEqual(4, innings.BowlingInfo[0].OversBowled);
            Assert.AreEqual(2, innings.BowlingInfo[0].Maidens);
            Assert.AreEqual(7, innings.BowlingInfo[0].RunsConceded);
            Assert.AreEqual(5, innings.BowlingInfo[0].Wickets);

            Assert.AreEqual(0, innings.BowlingInfo[1].OversBowled);
            Assert.AreEqual(0, innings.BowlingInfo[1].Maidens);
            Assert.AreEqual(0, innings.BowlingInfo[1].Wickets);
            Assert.AreEqual(0, innings.BowlingInfo[1].RunsConceded);
        }

        [TestCase(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 0, 0, 0)]
        [TestCase(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new[] { 1, 1, 5, 2, 2, 0, 0, 0, 0, 0, 0, 0 }, 5, 11, 5)]
        [TestCase(new int[] { 58, 73, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 5, 0, 136)]
        [TestCase(new int[] { 58, 73, 8, 22, 5, 0, 0, 0, 0, 0, 0, 0 }, new[] { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, 5, 5, 171)]
        public void CanGetTotalMatchScore(int[] runs, int[] wicketsTaken, int byes, int expectedWickets, int expectedRuns)
        {
            var playerNames = new List<PlayerName>();
            for (int i = 0; i < 11; i++)
            {
                string surname = "Bloggs" + i;
                string forename = "Joe" + i;
                playerNames.Add(new PlayerName(surname, forename));
            }
            var innings = new BowlingInnings(playerNames);

            Assert.AreEqual(11, innings.BowlingInfo.Count);
            Assert.AreEqual(0, innings.ByesLegByes);
            innings.ByesLegByes = byes;

            for (int i = 0; i < 11; i++)
            {
                innings.SetScores(playerNames[i], 0,0, runs[i], wicketsTaken[i]);
            }
            var score = innings.Score();

            Assert.AreEqual(expectedWickets, score.Wickets, "Wickets not correct");
            Assert.AreEqual(expectedRuns, score.Runs, "Runs not correct");
        }

        [TestCase(5, 5,0, true)]
        [TestCase(5, -5,0, false)]
        [TestCase(12, 0,0, false)]
        [TestCase(13, -5,0, false)]
        [TestCase(9, 0, 12, false)]
        public void ValidityTests(int numberPlayers, int extras, int wicketsTaken, bool isValid)
        {

            var innings = new BowlingInnings();
            for (int i = 0; i < numberPlayers; i++)
            {
                innings.AddPlayer(new PlayerName("Surname" + i, "forename"));
                if (i == 0|| i==1)
                {
                    innings.SetScores(new PlayerName("Surname" + i, "forename"), 4, 0, 5, wicketsTaken/2);
                }
            }


            innings.ByesLegByes = extras;

            var result = innings.Validate();

            Assert.AreEqual(isValid, result);
        }

        [TestCase(5, 5, true, new string[] { })]
        [TestCase(5, -5, false, new string[] { "ByesLegByes cannot take a negative value." })]
        [TestCase(12, 0, false, new string[] { "BowlingInfo cannot take values above 11." })]
        public void ValidityMessageTests(int numberPlayers, int extras, bool isValid, string[] messages)
        {
            var innings = new BowlingInnings();
            for (int i = 0; i < numberPlayers; i++)
            {
                innings.AddPlayer(new PlayerName("Surname" + i, "forename"));
            }

            innings.ByesLegByes = extras;

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
