using Cricket.Match;
using Cricket.Player;
using CSD_Tests;
using NUnit.Framework;
using System.Collections.Generic;
using StructureCommon.Validation;

namespace CricketClasses.MatchTests
{
    [TestFixture]
    public sealed class BattingEntryTests
    {
        [Test]
        public void CanCreate()
        {
            var name = new PlayerName("Bloggs", "Joe");
            var batting = new BattingEntry(name);

            Assert.AreEqual(name, batting.Name);
        }

        [Test]
        public void CanSetStats()
        {
            var name = new PlayerName("Bloggs", "Joe");
            var batting = new BattingEntry(name);
            var bowler = new PlayerName("Fish", "Simon");
            batting.SetScores(Wicket.Bowled, 23, 1, 1, 0, name, bowler);
            Assert.AreEqual(Wicket.Bowled, batting.MethodOut);
            Assert.AreEqual(23, batting.RunsScored);
            Assert.AreEqual(name, batting.Fielder);
            Assert.AreEqual(bowler, batting.Bowler);
        }

        [TestCase(Wicket.Bowled, true)]
        [TestCase(Wicket.Caught, true)]
        [TestCase(Wicket.DidNotBat, false)]
        [TestCase(Wicket.HitWicket, true)]
        [TestCase(Wicket.LBW, true)]
        [TestCase(Wicket.NotOut, false)]
        [TestCase(Wicket.RunOut, true)]
        [TestCase(Wicket.Stumped, true)]
        public void WasHeOut(Wicket howOut, bool outOrNot)
        {
            var name = new PlayerName("Bloggs", "Joe");
            var batting = new BattingEntry(name);
            batting.SetScores(howOut, 23, 1, 1, 0);
            Assert.AreEqual(outOrNot, batting.Out());
        }

        [TestCase(Wicket.DidNotBat, 0, false, false, true)]
        [TestCase(Wicket.DidNotBat, 3, false, false, false)]
        [TestCase(Wicket.DidNotBat, 0, false, true, false)]
        [TestCase(Wicket.DidNotBat, 0, true, false, false)]
        [TestCase(Wicket.NotOut, 1, false, false, true)]
        [TestCase(Wicket.NotOut, 1, true, false, false)]
        [TestCase(Wicket.NotOut, 1, false, true, false)]
        [TestCase(Wicket.Bowled, 3, false, false, false)]
        [TestCase(Wicket.Bowled, 3, true, false, true)]
        [TestCase(Wicket.Bowled, -1, true, false, false)]
        [TestCase(Wicket.Bowled, 1, true, true, false)]
        [TestCase(Wicket.Caught, 3, true, true, true)]
        [TestCase(Wicket.Caught, 3, false, false, false)]
        [TestCase(Wicket.Caught, 3, true, false, false)]
        [TestCase(Wicket.Caught, 3, false, true, false)]
        [TestCase(Wicket.LBW, 3, true, false, true)]
        [TestCase(Wicket.LBW, 3, false, true, false)]
        [TestCase(Wicket.LBW, 3, false, false, false)]
        [TestCase(Wicket.Stumped, 3, true, true, true)]
        [TestCase(Wicket.Stumped, 3, true, false, false)]
        [TestCase(Wicket.Stumped, 3, false, true, false)]
        [TestCase(Wicket.Stumped, 3, false, false, false)]
        [TestCase(Wicket.RunOut, 3, false, false, false)]
        [TestCase(Wicket.RunOut, 3, false, true, true)]
        [TestCase(Wicket.RunOut, 3, true, true, false)]
        [TestCase(Wicket.RunOut, 3, true, false, false)]
        [TestCase(Wicket.HitWicket, 3, true, false, true)]
        [TestCase(Wicket.HitWicket, 3, false, true, false)]
        [TestCase(Wicket.HitWicket, 3, true, true, false)]
        public void ValidityTests(Wicket howOut, int runs, bool bowlerInc, bool fielderInc, bool isValid)
        {
            PlayerName bowler = null;
            PlayerName fielder = null;
            if (bowlerInc)
            {
                bowler = new PlayerName("Steyn", "Dale");
            }
            if (fielderInc)
            {
                fielder = new PlayerName("Rhodes", "Jonty");
            }
            var name = new PlayerName("Bloggs", "Joe");
            var batting = new BattingEntry(name);
            batting.SetScores(howOut, runs, 1, 1, 0, fielder, bowler);

            var valid = batting.Validate();
            Assert.AreEqual(isValid, valid);
        }

        [TestCase(Wicket.DidNotBat, 0, false, false, true, new string[] { })]
        [TestCase(Wicket.DidNotBat, 3, false, false, false, new string[] { "RunsScored was expected to be equal to 0." })]
        [TestCase(Wicket.DidNotBat, 0, false, true, false, new string[] { "Fielder cannot be set with DidnotBat." })]
        [TestCase(Wicket.DidNotBat, 0, true, false, false, new string[] { "Bowler cannot be set with DidnotBat." })]
        [TestCase(Wicket.NotOut, 1, false, false, true, new string[] { })]
        [TestCase(Wicket.NotOut, 1, true, false, false, new string[] { "Bowler should not be set with NotOut." })]
        [TestCase(Wicket.NotOut, 1, false, true, false, new string[] { "Fielder should not be set with NotOut." })]
        [TestCase(Wicket.Bowled, 3, false, false, false, new string[] { "Bowler should be set with Bowled." })]
        [TestCase(Wicket.Bowled, 3, true, false, true, new string[] { })]
        [TestCase(Wicket.Bowled, -1, true, false, false, new string[] { "RunsScored cannot take a negative value." })]
        [TestCase(Wicket.Bowled, 1, true, true, false, new string[] { "Fielder should not be set with Bowled." })]
        [TestCase(Wicket.Caught, 3, true, true, true, new string[] { })]
        [TestCase(Wicket.Caught, 3, true, false, false, new string[] { "Fielder should be set with Caught." })]
        [TestCase(Wicket.Caught, 3, false, true, false, new string[] { "Bowler should be set with Caught." })]
        [TestCase(Wicket.LBW, 3, true, false, true, new string[] { })]
        [TestCase(Wicket.LBW, 3, false, false, false, new string[] { "Bowler should be set with LBW." })]
        [TestCase(Wicket.LBW, 3, true, true, false, new string[] { "Fielder should not be set with LBW." })]
        [TestCase(Wicket.Stumped, 3, true, true, true, new string[] { })]
        [TestCase(Wicket.Stumped, 3, true, false, false, new string[] { "Fielder should be set with Stumped." })]
        [TestCase(Wicket.Stumped, 3, false, true, false, new string[] { "Bowler should be set with Stumped." })]
        [TestCase(Wicket.RunOut, 3, false, false, false, new string[] { "Fielder should be set with RunOut." })]
        [TestCase(Wicket.RunOut, 3, false, true, true, new string[] { })]
        [TestCase(Wicket.RunOut, 3, true, true, false, new string[] { "Bowler should not be set with RunOut." })]
        [TestCase(Wicket.HitWicket, 3, true, false, true, new string[] { })]
        [TestCase(Wicket.HitWicket, 3, true, true, false, new string[] { "Fielder should not be set with HitWicket." })]
        public void ValidityMessageTests(Wicket howOut, int runs, bool bowlerInc, bool fielderInc, bool isValid, string[] validMessages)
        {
            PlayerName bowler = null;
            PlayerName fielder = null;
            if (bowlerInc)
            {
                bowler = new PlayerName("Steyn", "Dale");
            }
            if (fielderInc)
            {
                fielder = new PlayerName("Rhodes", "Jonty");
            }
            var name = new PlayerName("Bloggs", "Joe");
            var batting = new BattingEntry(name);
            batting.SetScores(howOut, runs, 1, 1, 0, fielder, bowler);

            var valid = batting.Validation();
            int number = isValid ? 0 : 1;
            Assert.AreEqual(number, valid.Count);

            var expectedList = new List<ValidationResult>();
            if (!isValid)
            {
                var expected = new ValidationResult
                {
                    IsValid = isValid
                };
                expected.Messages.AddRange(validMessages);
                expectedList.Add(expected);
            }

            Assertions.ValidationListsEqual(expectedList, valid);
        }
    }
}
