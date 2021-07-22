using System.Collections.Generic;
using System.Linq;
using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;
using NUnit.Framework;
using Common.Structure.Validation;

namespace CricketStructures.Tests.MatchTests
{
    [TestFixture]
    public class BattingInningsTests
    {
        [Test]
        public void CanCheckPlayerListed()
        {
            var player1 = new PlayerName("Bloggs", "Joe");
            var player2 = new PlayerName("Smith", "Steve");
            var playerNames = new List<PlayerName>() { player1, player2 };
            var innings = new CricketInnings("", "other");

            innings.SetBatting(player1, Wicket.Bowled, 5, 1, 2, 3);
            innings.SetBatting(player2, Wicket.Caught, 0, 0, 1, 3);
            Assert.AreEqual(2, innings.Batting.Count);

            Assert.AreEqual(true, innings.IsBattingPlayer(player1));
            Assert.AreEqual(false, innings.IsFieldingPlayer(player1));
            Assert.AreEqual(false, innings.IsBattingPlayer(new PlayerName("Wood", "Mark")));
        }

        [Test]
        public void CanRemovePlayer()
        {
            var player1 = new PlayerName("Bloggs", "Joe");
            var player2 = new PlayerName("Smith", "Steve");
            var playerNames = new List<PlayerName>() { player1, player2 };
            var innings = new CricketInnings("", "other");

            innings.SetBatting(player1, Wicket.Bowled, 5, 1, 2, 3);
            innings.SetBatting(player2, Wicket.Caught, 0, 0, 1, 3);
            Assert.AreEqual(2, innings.Batting.Count);

            _ = innings.DeleteBatting(player2);
            Assert.AreEqual(1, innings.Batting.Count);
        }

        [Test]
        public void CanSetScores()
        {
            var player1 = new PlayerName("Bloggs", "Joe");
            var player2 = new PlayerName("Smith", "Steve");
            var playerNames = new List<PlayerName>() { player1, player2 };
            var innings = new CricketInnings("", "other");

            innings.SetBatting(player1, Wicket.Bowled, 5, 1, 1, 0);
            innings.SetBatting(player2, Wicket.DidNotBat, 0, 0, 1, 3);
            Assert.AreEqual(2, innings.Batting.Count);

            var player1Batting = innings.GetBatting("", player1);
            Assert.AreEqual(5, player1Batting.RunsScored);
            Assert.AreEqual(Wicket.Bowled, player1Batting.MethodOut);
            var player2Batting = innings.GetBatting("", player2);
            Assert.AreEqual(0, player2Batting.RunsScored);
            Assert.AreEqual(Wicket.DidNotBat, player2Batting.MethodOut);
        }

        [TestCase(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new[] { Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut }, 0, 11, 0)]
        [TestCase(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new[] { Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut, Wicket.RunOut }, 5, 11, 5)]
        [TestCase(new int[] { 58, 73, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new[] { Wicket.NotOut, Wicket.NotOut, Wicket.DidNotBat, Wicket.DidNotBat, Wicket.DidNotBat, Wicket.DidNotBat, Wicket.DidNotBat, Wicket.DidNotBat, Wicket.DidNotBat, Wicket.DidNotBat, Wicket.DidNotBat }, 5, 0, 136)]
        [TestCase(new int[] { 58, 73, 8, 22, 5, 0, 0, 0, 0, 0, 0, 0 }, new[] { Wicket.Bowled, Wicket.Stumped, Wicket.NotOut, Wicket.Caught, Wicket.NotOut, Wicket.DidNotBat, Wicket.DidNotBat, Wicket.DidNotBat, Wicket.DidNotBat, Wicket.DidNotBat, Wicket.DidNotBat }, 5, 3, 171)]
        public void CanGetTotalMatchScore(int[] runs, Wicket[] wicketTypes, int extras, int expectedWickets, int expectedRuns)
        {
            var innings = new CricketInnings(null, "");
            for (int i = 0; i < 11; i++)
            {
                string surname = "Bloggs" + i;
                string forename = "Joe" + i;
                innings.SetBatting(new PlayerName(surname, forename), wicketTypes[i], runs[i], 1, 1, 0);
            }

            Assert.AreEqual(11, innings.Batting.Count);
            innings.SetExtras(extras, 0, 0, 0);

            var score = innings.BattingScore();

            Assert.AreEqual(expectedWickets, score.Wickets, "Wickets not correct");
            Assert.AreEqual(expectedRuns, score.Runs, "Runs not correct");
        }

        [TestCase(5, 5, true)]
        [TestCase(5, -5, false)]
        [TestCase(12, 0, false)]
        [TestCase(13, -5, false)]
        public void ValidityTests(int numberPlayers, int extras, bool isValid)
        {
            var innings = new CricketInnings();
            for (int i = 0; i < numberPlayers; i++)
            {
                innings.SetBatting(new PlayerName("Surname" + i, "forename"), Wicket.DidNotBat, 0, 0, 0, 0);
            }

            innings.InningsExtras = new Extras(extras, 0, 0, 0);

            var result = innings.Validate();

            Assert.AreEqual(isValid, result);
        }

        [TestCase(5, 5, true, new string[] { })]
        [TestCase(5, -5, false, new string[] { "Byes cannot take a negative value." })]
        [TestCase(12, 0, false, new string[] { "Batting cannot take values above 11." })]
        public void ValidityMessageTests(int numberPlayers, int extras, bool isValid, string[] messages)
        {
            var innings = new CricketInnings();
            for (int i = 0; i < numberPlayers; i++)
            {
                innings.SetBatting(new PlayerName("Surname" + i, "forename"), Wicket.DidNotBat, 0, 0, 0, 0);
            }

            innings.InningsExtras = new Extras(extras, 0, 0, 0);

            var valid = innings.Validation();

            var expectedList = new List<ValidationResult>();
            if (!isValid)
            {
                var expected = new ValidationResult
                {
                    IsValid = isValid
                };
                expected.Messages.AddRange(messages);
                expectedList.Add(expected);
            }

            Assertions.ValidationListsEqual(expectedList, valid);
        }
    }
}
