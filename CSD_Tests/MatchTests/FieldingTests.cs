using System.Collections.Generic;
using NUnit.Framework;
using Common.Structure.Validation;
using CricketStructures.Player;
using CricketStructures.Match.Innings;
using CricketStructures.Match;

namespace CricketStructures.Tests.MatchTests
{
    [TestFixture]
    internal class FieldingTests
    {
        [Test]
        public void CanSetScores()
        {
            var player1 = new PlayerName("Bloggs", "Joe");
            var player2 = new PlayerName("Smith", "Steve");
            var batters = new List<PlayerName>();
            for (int i = 0; i < 11; i++)
            {
                batters.Add(new PlayerName("Surname" + i, "forename"));
            }

            var innings = new CricketInnings("", "other");

            innings.SetBatting(batters[0], Wicket.Caught, 0, 0, 0, 0, player1);
            innings.SetBatting(batters[1], Wicket.RunOut, 0, 0, 0, 0, player1);
            innings.SetBatting(batters[2], Wicket.RunOut, 0, 0, 0, 0, player1);
            innings.SetBatting(batters[3], Wicket.Stumped, 0, 0, 0, 0, player1, wasKeeper: true);
            innings.SetBatting(batters[4], Wicket.Stumped, 0, 0, 0, 0, player1, wasKeeper: true);
            innings.SetBatting(batters[5], Wicket.Stumped, 0, 0, 0, 0, player1, wasKeeper: true);
            innings.SetBatting(batters[6], Wicket.Caught, 0, 0, 0, 0, player1, wasKeeper: true);
            innings.SetBatting(batters[7], Wicket.Caught, 0, 0, 0, 0, player1, wasKeeper: true);
            innings.SetBatting(batters[8], Wicket.Caught, 0, 0, 0, 0, player1, wasKeeper: true);
            innings.SetBatting(batters[9], Wicket.Caught, 0, 0, 0, 0, player1, wasKeeper: true);


            var fielding1 = innings.GetFielding("other", player1);
            Assert.AreEqual(1, fielding1.Catches);
            Assert.AreEqual(2, fielding1.RunOuts);
            Assert.AreEqual(3, fielding1.KeeperStumpings);
            Assert.AreEqual(4, fielding1.KeeperCatches);


            var fielding2 = innings.GetFielding("other", player2);
            Assert.AreEqual(0, fielding2.Catches);
            Assert.AreEqual(0, fielding2.RunOuts);
            Assert.AreEqual(0, fielding2.KeeperStumpings);
            Assert.AreEqual(0, fielding2.KeeperCatches);
        }

        [TestCase(5, 5, true)]
        [TestCase(12, 0, false)]
        [TestCase(13, -5, false)]
        public void ValidityTests(int numberPlayers, int taken, bool isValid)
        {
            var innings = new CricketInnings();
            for (int i = 0; i < numberPlayers; i++)
            {
                innings.SetBowling(new PlayerName("Surname" + i, "forename"), 0, 0, 0, 0);
            }

            var result = innings.Validate();

            Assert.AreEqual(isValid, result);
        }

        [TestCase(5, true, new string[] { })]
        [TestCase(12, false, new string[] { "Bowling cannot take values above 11." })]
        public void ValidityMessageTests(int numberPlayers, bool isValid, string[] messages)
        {
            var innings = new CricketInnings();
            for (int i = 0; i < numberPlayers; i++)
            {
                innings.SetBowling(new PlayerName("Surname" + i, "forename"), 0, 0, 0, 0);
            }

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
