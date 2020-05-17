using Cricket.Match;
using Cricket.Player;
using CSD_Tests;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using StructureCommon.Validation;

namespace CricketClasses.MatchTests
{
    [TestFixture]
    public sealed class CricketMatchTests
    {
        [Test]
        public void CanCreate()
        {
            var matchInfo = new MatchInfo
            {
                Opposition = "Evil"
            };
            var match = new CricketMatch(matchInfo);

            Assert.AreEqual("Evil", match.MatchData.Opposition);
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
            var matchInfo1 = new MatchInfo(oppo1, date1, null, MatchType.League);
            var match = new CricketMatch(matchInfo1);

            Assert.AreEqual(expectedResult, match.SameMatch(date2, oppo2));
        }

        [TestCase("Smith", "Steve", true)]
        [TestCase("Smith", "steve", false)]
        [TestCase("Smith", "Jobs", false)]
        public void DidHePlay(string surname, string forename, bool expected)
        {
            var players = new List<PlayerName>() { new PlayerName("Smith", "Steve"), new PlayerName("Root", "Joe") };
            var match = new CricketMatch("Sandon", players);

            Assert.AreEqual(expected, match.PlayNotPlay(new PlayerName(surname, forename)));
        }

        [Test]
        public void CanEditMatchInfo()
        {
            var matchInfo = new MatchInfo
            {
                Opposition = "Evil"
            };
            var match = new CricketMatch(matchInfo);

            Assert.AreEqual("Evil", match.MatchData.Opposition);

            match.EditInfo("Sandon", new DateTime(2010, 2, 3), "Spain", MatchType.Evening, ResultType.Draw);

            Assert.AreEqual("Sandon", match.MatchData.Opposition);
            Assert.AreEqual(new DateTime(2010, 2, 3), match.MatchData.Date);
            Assert.AreEqual("Spain", match.MatchData.Place);
            Assert.AreEqual(MatchType.Evening, match.MatchData.Type);
            Assert.AreEqual(ResultType.Draw, match.Result);
        }

        [Test]
        public void CanEditMOM()
        {
            var matchInfo = new MatchInfo
            {
                Opposition = "Evil"
            };
            var match = new CricketMatch(matchInfo);
            var player = new PlayerName("Root", "Joe");
            match.EditManOfMatch(player);
            Assert.AreEqual(player, match.ManOfMatch);
        }

        [TestCase("Smith", "Steve", false)]
        [TestCase("Smith", "steve", true)]
        [TestCase("Smith", "Jobs", true)]
        public void CanAddPlayer(string surname, string forename, bool added)
        {
            var players = new List<PlayerName>() { new PlayerName("Smith", "Steve"), new PlayerName("Root", "Joe") };
            var match = new CricketMatch("Sandon", players);
            int length = players.Count + (added ? 1 : 0);
            bool result = match.AddPlayer(new PlayerName(surname, forename));
            Assert.AreEqual(added, result);
            Assert.AreEqual(length, match.PlayerNames.Count);
        }

        [Test]
        public void CanSetBatting()
        {
            var players = new List<PlayerName>() { new PlayerName("Smith", "Steve"), new PlayerName("Root", "Joe") };

            var player1 = new PlayerName("Root", "Joe");
            var player2 = new PlayerName("Smith", "Jobs");
            var playerNames = new List<PlayerName>() { player1, player2 };
            var innings = new BattingInnings(null, playerNames);

            innings.SetScores(player1, Wicket.Bowled, 5, 1, 1, 0);
            innings.SetScores(player2, Wicket.RunOut, 9, 1, 1, 0);
            var match = new CricketMatch("Sandon", players);

            match.SetBatting(innings);

            Assert.AreEqual(3, match.PlayerNames.Count);
            Assert.AreEqual(3, match.Batting.BattingInfo.Count);
            var player1Scores = match.GetBatting(player1);
            Assert.AreEqual(Wicket.Bowled, player1Scores.MethodOut);
            Assert.AreEqual(5, player1Scores.RunsScored);
            Assert.AreEqual(null, player1Scores.Fielder);
            Assert.AreEqual(null, player1Scores.Bowler);

            var player2Scores = match.GetBatting(player2);
            Assert.AreEqual(Wicket.RunOut, player2Scores.MethodOut);
            Assert.AreEqual(9, player2Scores.RunsScored);
            Assert.AreEqual(null, player2Scores.Fielder);
            Assert.AreEqual(null, player2Scores.Bowler);

            var playerScores = match.GetBatting(new PlayerName("Smith", "Steve"));
            Assert.AreEqual(Wicket.DidNotBat, playerScores.MethodOut);
            Assert.AreEqual(0, playerScores.RunsScored);
            Assert.AreEqual(null, playerScores.Fielder);
            Assert.AreEqual(null, playerScores.Bowler);
        }

        [TestCase("Smith", "Steve", "Bowled", 5, true, true, false)]
        [TestCase("Smith", "steve", "Bowled", 7, true, true, true)]
        [TestCase("Smith", "Jobs", "Bowled", 5, true, true, true)]
        [TestCase("Smith", "Jobs", "Bowled", 7, false, false, true)]
        [TestCase("Smith", "Jobs", "Stumped", 9, false, false, true)]
        [TestCase("Smith", "Jobs", "Bowled", 5, true, false, true)]
        public void CanAddBattingEntry(string surname, string forename, Wicket howOut, int runs, bool fielderAdd, bool bowlerAdd, bool added)
        {
            var players = new List<PlayerName>() { new PlayerName("Smith", "Steve"), new PlayerName("Root", "Joe") };
            var match = new CricketMatch("Sandon", players);
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

            var didAdd = match.AddBattingEntry(new PlayerName(surname, forename), howOut, runs, 1, 1, 0, fielder, bowler);
            Assert.AreEqual(added, didAdd);

            if (added)
            {
                var player = match.GetBatting(new PlayerName(surname, forename));
                Assert.AreEqual(howOut, player.MethodOut);
                Assert.AreEqual(runs, player.RunsScored);
                Assert.AreEqual(fielder, player.Fielder);
                Assert.AreEqual(bowler, player.Bowler);
            }
        }

        [TestCase("Smith", "Steve", "Bowled", 5, true, true, true)]
        [TestCase("Smith", "steve", "Bowled", 7, true, true, false)]
        [TestCase("Smith", "Jobs", "Bowled", 5, true, true, false)]
        [TestCase("Smith", "Steve", "Bowled", 7, false, false, true)]
        [TestCase("Smith", "Steve", "Stumped", 9, false, false, true)]
        [TestCase("Smith", "Steve", "Bowled", 5, true, false, true)]
        public void CanEditBattingEntry(string surname, string forename, Wicket howOut, int runs, bool fielderAdd, bool bowlerAdd, bool edited)
        {
            var players = new List<PlayerName>() { new PlayerName("Smith", "Steve"), new PlayerName("Root", "Joe") };
            var match = new CricketMatch("Sandon", players);
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

            var didEdit = match.EditBattingEntry(new PlayerName(surname, forename), howOut, runs, 1, 1, 0, fielder, bowler);
            Assert.AreEqual(edited, didEdit);

            if (edited)
            {
                var player = match.GetBatting(new PlayerName(surname, forename));
                Assert.AreEqual(howOut, player.MethodOut);
                Assert.AreEqual(runs, player.RunsScored);
                Assert.AreEqual(fielder, player.Fielder);
                Assert.AreEqual(bowler, player.Bowler);
            }
        }

        [TestCase("Smith", "Steve", true)]
        [TestCase("Smith", "steve", false)]
        [TestCase("Smith", "Jobs", false)]
        public void CanDeleteBattingEntry(string surname, string forename, bool deleted)
        {
            var players = new List<PlayerName>() { new PlayerName("Smith", "Steve"), new PlayerName("Root", "Joe") };
            var match = new CricketMatch("Sandon", players);

            var didDelete = match.DeleteBattingEntry(new PlayerName(surname, forename));
            Assert.AreEqual(deleted, didDelete);
        }

        [Test]
        public void CanSetBowling()
        {
            var players = new List<PlayerName>() { new PlayerName("Smith", "Steve"), new PlayerName("Root", "Joe") };

            var player1 = new PlayerName("Root", "Joe");
            var player2 = new PlayerName("Smith", "Jobs");
            var playerNames = new List<PlayerName>() { player1, player2 };
            var innings = new BowlingInnings(null, playerNames);

            innings.SetScores(player1, 4, 2, 23, 1);
            innings.SetScores(player2, 5, 1, 19, 4);
            var match = new CricketMatch("Sandon", players);

            match.SetBowling(innings);

            Assert.AreEqual(3, match.PlayerNames.Count);
            Assert.AreEqual(3, match.Bowling.BowlingInfo.Count);

            var player1Scores = match.GetBowling(player1);
            Assert.AreEqual(4, player1Scores.OversBowled);
            Assert.AreEqual(2, player1Scores.Maidens);
            Assert.AreEqual(23, player1Scores.RunsConceded);
            Assert.AreEqual(1, player1Scores.Wickets);

            var player2Scores = match.GetBowling(player2);
            Assert.AreEqual(5, player2Scores.OversBowled);
            Assert.AreEqual(1, player2Scores.Maidens);
            Assert.AreEqual(19, player2Scores.RunsConceded);
            Assert.AreEqual(4, player2Scores.Wickets);

            var playerScores = match.GetBowling(new PlayerName("Smith", "Steve"));
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
            var players = new List<PlayerName>() { new PlayerName("Smith", "Steve"), new PlayerName("Root", "Joe") };
            var match = new CricketMatch("Sandon", players);

            var didAdd = match.AddBowlingEntry(new PlayerName(surname, forename), overs, maidens, runs, wickets);
            Assert.AreEqual(added, didAdd);

            if (added)
            {
                var player = match.GetBowling(new PlayerName(surname, forename));
                Assert.AreEqual(overs, player.OversBowled);
                Assert.AreEqual(maidens, player.Maidens);
                Assert.AreEqual(runs, player.RunsConceded);
                Assert.AreEqual(wickets, player.Wickets);
            }
        }

        [TestCase("Smith", "Steve", 4, 4, 23, 1, true)]
        [TestCase("Smith", "steve", 4, 4, 23, 1, false)]
        [TestCase("Smith", "Jobs", 4, 4, 23, 2, false)]
        [TestCase("Smith", "Steve", 5, 4, 23, 1, true)]
        public void CanEditBowlingEntry(string surname, string forename, int overs, int maidens, int runs, int wickets, bool edited)
        {
            var players = new List<PlayerName>() { new PlayerName("Smith", "Steve"), new PlayerName("Root", "Joe") };
            var match = new CricketMatch("Sandon", players);

            var didEdit = match.EditBowlingEntry(new PlayerName(surname, forename), overs, maidens, runs, wickets);
            Assert.AreEqual(edited, didEdit);

            if (edited)
            {
                var player = match.GetBowling(new PlayerName(surname, forename));
                Assert.AreEqual(overs, player.OversBowled);
                Assert.AreEqual(maidens, player.Maidens);
                Assert.AreEqual(runs, player.RunsConceded);
                Assert.AreEqual(wickets, player.Wickets);
            }
        }

        [TestCase("Smith", "Steve", true)]
        [TestCase("Smith", "steve", false)]
        [TestCase("Smith", "Jobs", false)]
        public void CanDeleteBowlingEntry(string surname, string forename, bool deleted)
        {
            var players = new List<PlayerName>() { new PlayerName("Smith", "Steve"), new PlayerName("Root", "Joe") };
            var match = new CricketMatch("Sandon", players);

            var didDelete = match.DeleteBowlingEntry(new PlayerName(surname, forename));
            Assert.AreEqual(deleted, didDelete);
        }


        [Test]
        public void CanSetFielding()
        {
            var players = new List<PlayerName>() { new PlayerName("Smith", "Steve"), new PlayerName("Root", "Joe") };

            var player1 = new PlayerName("Root", "Joe");
            var player2 = new PlayerName("Smith", "Jobs");
            var playerNames = new List<PlayerName>() { player1, player2 };
            var innings = new Fielding(null, playerNames);

            innings.SetFielding(player1, 4, 2, 23, 1);
            innings.SetFielding(player2, 5, 1, 19, 4);
            var match = new CricketMatch("Sandon", players);

            match.SetFielding(innings);

            Assert.AreEqual(3, match.PlayerNames.Count);
            Assert.AreEqual(3, match.FieldingStats.FieldingInfo.Count);

            var player1Scores = match.GetFielding(player1);
            Assert.AreEqual(4, player1Scores.Catches);
            Assert.AreEqual(2, player1Scores.RunOuts);
            Assert.AreEqual(23, player1Scores.KeeperStumpings);
            Assert.AreEqual(1, player1Scores.KeeperCatches);

            var player2Scores = match.GetFielding(player2);
            Assert.AreEqual(5, player2Scores.Catches);
            Assert.AreEqual(1, player2Scores.RunOuts);
            Assert.AreEqual(19, player2Scores.KeeperStumpings);
            Assert.AreEqual(4, player2Scores.KeeperCatches);

            var playerScores = match.GetFielding(new PlayerName("Smith", "Steve"));
            Assert.AreEqual(0, playerScores.Catches);
            Assert.AreEqual(0, playerScores.RunOuts);
            Assert.AreEqual(0, playerScores.KeeperStumpings);
            Assert.AreEqual(0, playerScores.KeeperCatches);
        }

        [TestCase("Smith", "Steve", 4, 4, 23, 1, false)]
        [TestCase("Smith", "steve", 4, 4, 23, 1, true)]
        [TestCase("Smith", "Jobs", 4, 4, 23, 1, true)]
        public void CanAddFieldingEntry(string surname, string forename, int overs, int maidens, int runs, int wickets, bool added)
        {
            var players = new List<PlayerName>() { new PlayerName("Smith", "Steve"), new PlayerName("Root", "Joe") };
            var match = new CricketMatch("Sandon", players);

            var didAdd = match.AddFieldingEntry(new PlayerName(surname, forename), overs, maidens, runs, wickets);
            Assert.AreEqual(added, didAdd);

            if (added)
            {
                var player = match.GetFielding(new PlayerName(surname, forename));
                Assert.AreEqual(overs, player.Catches);
                Assert.AreEqual(maidens, player.RunOuts);
                Assert.AreEqual(runs, player.KeeperStumpings);
                Assert.AreEqual(wickets, player.KeeperCatches);
            }
        }

        [TestCase("Smith", "Steve", 4, 4, 23, 1, true)]
        [TestCase("Smith", "steve", 4, 4, 23, 1, false)]
        [TestCase("Smith", "Jobs", 4, 4, 23, 2, false)]
        [TestCase("Smith", "Steve", 5, 4, 23, 1, true)]
        public void CanEditFieldingEntry(string surname, string forename, int overs, int maidens, int runs, int wickets, bool edited)
        {
            var players = new List<PlayerName>() { new PlayerName("Smith", "Steve"), new PlayerName("Root", "Joe") };
            var match = new CricketMatch("Sandon", players);

            var didEdit = match.EditFieldingEntry(new PlayerName(surname, forename), overs, maidens, runs, wickets);
            Assert.AreEqual(edited, didEdit);

            if (edited)
            {
                var player = match.GetFielding(new PlayerName(surname, forename));
                Assert.AreEqual(overs, player.Catches);
                Assert.AreEqual(maidens, player.RunOuts);
                Assert.AreEqual(runs, player.KeeperStumpings);
                Assert.AreEqual(wickets, player.KeeperCatches);
            }
        }

        [TestCase("Smith", "Steve", true)]
        [TestCase("Smith", "steve", false)]
        [TestCase("Smith", "Jobs", false)]
        public void CanDeleteFieldingEntry(string surname, string forename, bool deleted)
        {
            var players = new List<PlayerName>() { new PlayerName("Smith", "Steve"), new PlayerName("Root", "Joe") };
            var match = new CricketMatch("Sandon", players);

            var didDelete = match.DeleteFieldingEntry(new PlayerName(surname, forename));
            Assert.AreEqual(deleted, didDelete);
        }

        [TestCase("", false)]
        [TestCase(null, false)]
        [TestCase("Sam", true)]
        public void ValidityTests(string opposition, bool isValid)
        {
            var info = new MatchInfo
            {
                Opposition = opposition
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
                Opposition = opposition
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
