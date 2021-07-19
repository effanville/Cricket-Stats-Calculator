using System;
using System.Collections.Generic;
using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;
using NUnit.Framework;
using Common.Structure.Validation;

namespace CricketStructures.Tests.MatchTests
{
    [TestFixture]
    public sealed class CricketMatchTests
    {
        [Test]
        public void CanCreate()
        {
            var matchInfo = new MatchInfo
            {
                AwayTeam = "Evil"
            };
            var match = new CricketMatch(matchInfo);

            Assert.AreEqual("Evil", match.MatchData.AwayTeam);
        }

        [TestCase("2000/1/1", "Sandon", "2000/1/1", "Sandon", true)]
        [TestCase("2000/1/1", "Sandon", "2000/2/1", "Sandon", false)]
        [TestCase(null, "Sandon", "2000/2/1", "Sandon", false)]
        [TestCase(null, "Sandon", null, "Sandon", true)]
        [TestCase(null, "Sandon", "2000/2/1", "Aspenden", false)]
        [TestCase(null, "Sandon", "2000/2/1", null, false)]
        [TestCase(null, null, "2000/2/1", "Sandon", false)]
        [TestCase("2000/2/1", null, "2000/2/1", "Sandon", false)]
        public void EqualMatches(DateTime date1, string oppo1, DateTime date2, string oppo2, bool expectedResult)
        {
            var matchInfo1 = new MatchInfo(oppo1, "Walkern", oppo1, date1, MatchType.League);
            var match = new CricketMatch(matchInfo1);

            Assert.AreEqual(expectedResult, match.SameMatch(date2, oppo2, "Walkern"));
        }

        [Test]
        public void CanEditMatchInfo()
        {
            var matchInfo = new MatchInfo
            {
                AwayTeam = "Evil"
            };
            var match = new CricketMatch(matchInfo);

            Assert.AreEqual("Evil", match.MatchData.AwayTeam);

            match.EditInfo("Walkern", "Sandon", new DateTime(2010, 2, 3), "Walkern", MatchType.Evening, ResultType.Draw);

            Assert.AreEqual("Sandon", match.MatchData.AwayTeam);
            Assert.AreEqual(new DateTime(2010, 2, 3), match.MatchData.Date);
            Assert.AreEqual("Walkern", match.MatchData.Location);
            Assert.AreEqual(MatchType.Evening, match.MatchData.Type);
            Assert.AreEqual(ResultType.Draw, match.Result);
        }

        [Test]
        public void CanEditMOM()
        {
            var matchInfo = new MatchInfo
            {
                AwayTeam = "Evil"
            };
            var match = new CricketMatch(matchInfo);
            var player = new PlayerName("Root", "Joe");
            _ = match.EditManOfMatch(new[] { player });
            Assert.AreEqual(player, match.MenOfMatch);
        }

        [SetUp]
        public void Setup()
        {
            MatchToTest = new CricketMatch("Sandon", "Walkern", new DateTime(), MatchType.League, false);
        }

        public CricketMatch MatchToTest;

        [Test]
        public void CanSetBatting()
        {
            var player1 = new PlayerName("Root", "Joe");
            var player2 = new PlayerName("Smith", "Jobs");
            var innings = new CricketInnings("Sandon", "Walkern");

            innings.SetBatting(player1, Wicket.Bowled, 5, 1, 1, 0);
            innings.SetBatting(player2, Wicket.RunOut, 9, 1, 1, 0);

            MatchToTest.SetInnings(innings, first: true);

            Assert.AreEqual(3, MatchToTest.FirstInnings.Batting.Count);
            var player1Scores = MatchToTest.GetBatting("Sandon", player1);
            Assert.AreEqual(Wicket.Bowled, player1Scores.MethodOut);
            Assert.AreEqual(5, player1Scores.RunsScored);
            Assert.AreEqual(null, player1Scores.Fielder);
            Assert.AreEqual(null, player1Scores.Bowler);

            var player2Scores = MatchToTest.GetBatting("Sandon", player2);
            Assert.AreEqual(Wicket.RunOut, player2Scores.MethodOut);
            Assert.AreEqual(9, player2Scores.RunsScored);
            Assert.AreEqual(null, player2Scores.Fielder);
            Assert.AreEqual(null, player2Scores.Bowler);

            var playerScores = MatchToTest.GetBatting("Sandon", new PlayerName("Smith", "Steve"));
            Assert.AreEqual(Wicket.DidNotBat, playerScores.MethodOut);
            Assert.AreEqual(0, playerScores.RunsScored);
            Assert.AreEqual(null, playerScores.Fielder);
            Assert.AreEqual(null, playerScores.Bowler);
        }

        [TestCase("Smith", "Steve", "Bowled", 5, true, true)]
        [TestCase("Smith", "steve", "Bowled", 7, true, true)]
        [TestCase("Smith", "Jobs", "Bowled", 5, true, true)]
        [TestCase("Smith", "Jobs", "Bowled", 7, false, false)]
        [TestCase("Smith", "Jobs", "Stumped", 9, false, false)]
        [TestCase("Smith", "Jobs", "Bowled", 5, true, false)]
        public void CanAddBattingEntry(string surname, string forename, Wicket howOut, int runs, bool fielderAdd, bool bowlerAdd)
        {
            PlayerName fielder = null;
            if (fielderAdd)
            {
                fielder = new PlayerName("Rhodes", "Jonty");
            }
            PlayerName bowler = null;
            if (bowlerAdd)
            {
                bowler = new PlayerName("Steyn", "Dale");
            }

            MatchToTest.SetBatting("Sandon", new PlayerName(surname, forename), howOut, runs, 1, 1, 0, fielder, false, bowler);

            var player = MatchToTest.GetBatting("Sandon", new PlayerName(surname, forename));
            Assert.AreEqual(howOut, player.MethodOut);
            Assert.AreEqual(runs, player.RunsScored);
            Assert.AreEqual(fielder, player.Fielder);
            Assert.AreEqual(bowler, player.Bowler);
        }

        [TestCase("Smith", "Steve", "Bowled", 5, true, true, true)]
        [TestCase("Smith", "steve", "Bowled", 7, true, true, false)]
        [TestCase("Smith", "Jobs", "Bowled", 5, true, true, false)]
        [TestCase("Smith", "Steve", "Bowled", 7, false, false, true)]
        [TestCase("Smith", "Steve", "Stumped", 9, false, false, true)]
        [TestCase("Smith", "Steve", "Bowled", 5, true, false, true)]
        public void CanEditBattingEntry(string surname, string forename, Wicket howOut, int runs, bool fielderAdd, bool bowlerAdd, bool edited)
        {
            PlayerName fielder = null;
            if (fielderAdd)
            {
                fielder = new PlayerName("Rhodes", "Jonty");
            }
            PlayerName bowler = null;
            if (bowlerAdd)
            {
                bowler = new PlayerName("Steyn", "Dale");
            }

            MatchToTest.SetBatting("Sandon", new PlayerName(surname, forename), howOut, runs, 1, 1, 0, fielder, false, bowler);

            var player = MatchToTest.GetBatting("Sandon", new PlayerName(surname, forename));
            Assert.AreEqual(howOut, player.MethodOut);
            Assert.AreEqual(runs, player.RunsScored);
            Assert.AreEqual(fielder, player.Fielder);
            Assert.AreEqual(bowler, player.Bowler);
        }

        [TestCase("Smith", "Steve", true)]
        [TestCase("Smith", "steve", false)]
        [TestCase("Smith", "Jobs", false)]
        public void CanDeleteBattingEntry(string surname, string forename, bool deleted)
        {
            var player1 = new PlayerName("Smith", "Steve");
            var player2 = new PlayerName("Root", "Joe");
            MatchToTest.SetBatting("Sandon", player1, Wicket.Bowled, 0, 0, 0, 0);
            MatchToTest.SetBatting("Sandon", player2, Wicket.Bowled, 0, 0, 0, 0);
            bool didDelete = MatchToTest.DeleteBattingEntry("Sandon", new PlayerName(surname, forename));
            Assert.AreEqual(deleted, didDelete);
        }

        [Test]
        public void CanSetBowling()
        {
            var player1 = new PlayerName("Root", "Joe");
            var player2 = new PlayerName("Smith", "Jobs");
            var innings = new CricketInnings("Walkern", "Sandon");

            innings.SetBowling(player1, 4, 2, 23, 1);
            innings.SetBowling(player2, 5, 1, 19, 4);

            MatchToTest.SetInnings(innings, true);

            Assert.AreEqual(3, MatchToTest.FirstInnings.Bowling.Count);

            var player1Scores = MatchToTest.GetBowling("Walkern", player1);
            Assert.AreEqual(4, player1Scores.OversBowled);
            Assert.AreEqual(2, player1Scores.Maidens);
            Assert.AreEqual(23, player1Scores.RunsConceded);
            Assert.AreEqual(1, player1Scores.Wickets);

            var player2Scores = MatchToTest.GetBowling("Walkern", player2);
            Assert.AreEqual(5, player2Scores.OversBowled);
            Assert.AreEqual(1, player2Scores.Maidens);
            Assert.AreEqual(19, player2Scores.RunsConceded);
            Assert.AreEqual(4, player2Scores.Wickets);

            var playerScores = MatchToTest.GetBowling("Walkern", new PlayerName("Smith", "Steve"));
            Assert.AreEqual(0, playerScores.OversBowled);
            Assert.AreEqual(0, playerScores.Maidens);
            Assert.AreEqual(0, playerScores.RunsConceded);
            Assert.AreEqual(0, playerScores.Wickets);
        }

        [TestCase("Smith", "Steve", 4, 4, 23, 1, false)]
        [TestCase("Smith", "steve", 4, 4, 23, 1, true)]
        [TestCase("Smith", "Jobs", 4, 4, 23, 1, true)]
        public void CanAddBowlingEntry(string surname, string forename, int overs, int maidens, int runs, int wickets, bool added)
        {
            MatchToTest.SetBowling("Sandon", new PlayerName(surname, forename), overs, maidens, runs, wickets);

            var player = MatchToTest.GetBowling("Sandon", new PlayerName(surname, forename));
            Assert.AreEqual(overs, player.OversBowled);
            Assert.AreEqual(maidens, player.Maidens);
            Assert.AreEqual(runs, player.RunsConceded);
            Assert.AreEqual(wickets, player.Wickets);

        }

        [TestCase("Smith", "Steve", 4, 4, 23, 1, true)]
        [TestCase("Smith", "steve", 4, 4, 23, 1, false)]
        [TestCase("Smith", "Jobs", 4, 4, 23, 2, false)]
        [TestCase("Smith", "Steve", 5, 4, 23, 1, true)]
        public void CanEditBowlingEntry(string surname, string forename, int overs, int maidens, int runs, int wickets, bool edited)
        {
            MatchToTest.SetBowling("Sandon", new PlayerName(surname, forename), overs, maidens, runs, wickets);

            var player = MatchToTest.GetBowling("Sandon", new PlayerName(surname, forename));
            Assert.AreEqual(overs, player.OversBowled);
            Assert.AreEqual(maidens, player.Maidens);
            Assert.AreEqual(runs, player.RunsConceded);
            Assert.AreEqual(wickets, player.Wickets);
        }

        [TestCase("Smith", "Steve", true)]
        [TestCase("Smith", "steve", false)]
        [TestCase("Smith", "Jobs", false)]
        public void CanDeleteBowlingEntry(string surname, string forename, bool deleted)
        {
            MatchToTest.SetBowling("Sandon", new PlayerName("Smith", "Steve"), 0, 0, 0, 0);
            MatchToTest.SetBowling("Sandon", new PlayerName("Root", "Joe"), 0, 0, 0, 0);
            var didDelete = MatchToTest.DeleteBowlingEntry("Sandon", new PlayerName(surname, forename));
            Assert.AreEqual(deleted, didDelete);
        }

        [TestCase("", false)]
        [TestCase(null, false)]
        [TestCase("Sam", true)]
        public void ValidityTests(string opposition, bool isValid)
        {
            var info = new MatchInfo
            {
                HomeTeam = opposition
            };
            var match = new CricketMatch(info);
            var valid = match.Validate();
            Assert.AreEqual(isValid, valid);
        }

        [TestCase("", false, new string[] { "Opposition cannot be empty or null." })]
        [TestCase(null, false, new string[] { "Opposition cannot be empty or null." })]
        [TestCase("Sam", true, new string[] { })]
        public void ValidityMessageTests(string opposition, bool isValid, string[] messages)
        {
            var info = new MatchInfo
            {
                AwayTeam = opposition
            };

            var match = new CricketMatch(info);
            var valid = match.Validation();

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
