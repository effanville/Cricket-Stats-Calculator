using System.Collections.Generic;
using CricketStructures.Match.Innings;
using CricketStructures.Player;
using NUnit.Framework;
using Common.Structure.Validation;

namespace CricketStructures.Tests.MatchTests
{
    public class BowlingInningsTests
    {
        [Test]
        public void CanCheckPlayerListed()
        {
            var player1 = new PlayerName("Bloggs", "Joe");
            var player2 = new PlayerName("Smith", "Steve");
            var innings = new CricketInnings("", "other");

            innings.SetBowling(player2, 0, 0, 0, 0);
            innings.SetBowling(player1, 4, 2, 7, 5);

            Assert.AreEqual(2, innings.Bowling.Count);

            Assert.AreEqual(false, innings.IsBattingPlayer(player1));
            Assert.AreEqual(true, innings.IsFieldingPlayer(player1));
            Assert.AreEqual(false, innings.IsFieldingPlayer(new PlayerName("Wood", "Mark")));
        }

        [Test]
        public void CanRemovePlayer()
        {
            var player1 = new PlayerName("Bloggs", "Joe");
            var player2 = new PlayerName("Smith", "Steve");
            var innings = new CricketInnings("", "other");

            innings.SetBowling(player2, 0, 0, 0, 0);
            innings.SetBowling(player1, 4, 2, 7, 5);

            Assert.AreEqual(2, innings.Bowling.Count);

            _ = innings.DeleteBowling(player2);
            Assert.AreEqual(1, innings.Bowling.Count);
        }

        [Test]
        public void CanSetScores()
        {
            var player1 = new PlayerName("Bloggs", "Joe");
            var player2 = new PlayerName("Smith", "Steve");

            var innings = new CricketInnings("", "other");

            innings.SetBowling(player2, 0, 0, 0, 0);
            innings.SetBowling(player1, 4, 2, 7, 5);

            Assert.AreEqual(2, innings.Bowling.Count);

            var bowling = innings.GetBowling("other", player1);
            Assert.AreEqual(4, bowling.OversBowled);
            Assert.AreEqual(2, bowling.Maidens);
            Assert.AreEqual(7, bowling.RunsConceded);
            Assert.AreEqual(5, bowling.Wickets);


            var bowling1 = innings.GetBowling("other", player2);
            Assert.AreEqual(0, bowling1.OversBowled);
            Assert.AreEqual(0, bowling1.Maidens);
            Assert.AreEqual(0, bowling1.Wickets);
            Assert.AreEqual(0, bowling1.RunsConceded);
        }

        [TestCase(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 0, 0, 0)]
        [TestCase(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new[] { 1, 1, 5, 2, 2, 0, 0, 0, 0, 0, 0, 0 }, 5, 11, 5)]
        [TestCase(new int[] { 58, 73, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 5, 0, 136)]
        [TestCase(new int[] { 58, 73, 8, 22, 5, 0, 0, 0, 0, 0, 0, 0 }, new[] { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, 5, 5, 171)]
        public void CanGetTotalMatchScore(int[] runs, int[] wicketsTaken, int byes, int expectedWickets, int expectedRuns)
        {
            var innings = new CricketInnings(null, "");
            for (int i = 0; i < 11; i++)
            {
                string surname = "Bloggs" + i;
                string forename = "Joe" + i;
                innings.SetBowling(new PlayerName(surname, forename), 0, 0, 0, 0);
            }

            Assert.AreEqual(11, innings.Bowling.Count);
            innings.SetExtras(byes, 0, 0, 0);

            for (int i = 0; i < 11; i++)
            {
                string surname = "Bloggs" + i;
                string forename = "Joe" + i;
                innings.SetBowling(new PlayerName(surname, forename), 0, 0, runs[i], wicketsTaken[i]);
            }
            var score = innings.BowlingScore();

            Assert.AreEqual(expectedWickets, score.Wickets, "Wickets not correct");
            Assert.AreEqual(expectedRuns, score.Runs, "Runs not correct");
        }

        [TestCase(5, 5, 0, true)]
        [TestCase(5, -5, 0, false)]
        [TestCase(12, 0, 0, false)]
        [TestCase(13, -5, 0, false)]
        [TestCase(9, 0, 12, false)]
        public void ValidityTests(int numberPlayers, int extras, int wicketsTaken, bool isValid)
        {
            var innings = new CricketInnings();
            for (int i = 0; i < numberPlayers; i++)
            {
                innings.SetBowling(new PlayerName("Surname" + i, "forename"), 4, 0, 5, wicketsTaken / 2);

            }

            innings.InningsExtras = new Extras(extras, 0, 0, 0);

            bool result = innings.Validate();

            Assert.AreEqual(isValid, result);
        }

        [TestCase(5, 5, true, new string[] { })]
        [TestCase(5, -5, false, new string[] { "Byes cannot take a negative value." })]
        [TestCase(12, 0, false, new string[] { "Bowling cannot take values above 11." })]
        public void ValidityMessageTests(int numberPlayers, int extras, bool isValid, string[] messages)
        {
            var innings = new CricketInnings();
            for (int i = 0; i < numberPlayers; i++)
            {
                innings.SetBowling(new PlayerName("Surname" + i, "forename"), 0, 0, 0, 0);
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
