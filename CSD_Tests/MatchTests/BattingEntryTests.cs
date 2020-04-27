using NUnit.Framework;
using Cricket.Match;
using Cricket.Player;
using System.Collections.Generic;
using Validation;
using CSD_Tests;

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
            batting.SetScores(BattingWicketLossType.Bowled, 23, name, bowler);
            Assert.AreEqual(BattingWicketLossType.Bowled, batting.MethodOut);
            Assert.AreEqual(23, batting.RunsScored);
            Assert.AreEqual(name, batting.Fielder);
            Assert.AreEqual(bowler, batting.Bowler);
        }

        [TestCase(BattingWicketLossType.Bowled, true)]
        [TestCase(BattingWicketLossType.Caught, true)]
        [TestCase(BattingWicketLossType.DidNotBat, false)]
        [TestCase(BattingWicketLossType.HitWicket, true)]
        [TestCase(BattingWicketLossType.LBW, true)]
        [TestCase(BattingWicketLossType.NotOut, false)]
        [TestCase(BattingWicketLossType.RunOut, true)]
        [TestCase(BattingWicketLossType.Stumped, true)]
        public void WasHeOut(BattingWicketLossType howOut, bool outOrNot)
        {
            var name = new PlayerName("Bloggs", "Joe");
            var batting = new BattingEntry(name);
            batting.SetScores(howOut, 23);
            Assert.AreEqual(outOrNot, batting.Out());
        }

        [TestCase(BattingWicketLossType.DidNotBat, 0, false, false, true)]
        [TestCase(BattingWicketLossType.DidNotBat, 3, false, false, false)]
        [TestCase(BattingWicketLossType.DidNotBat, 0, false, true, false)]
        [TestCase(BattingWicketLossType.DidNotBat, 0, true, false, false)]
        [TestCase(BattingWicketLossType.NotOut, 1,  false, false, true)]
        [TestCase(BattingWicketLossType.NotOut, 1, true, false, false)]
        [TestCase(BattingWicketLossType.NotOut, 1, false, true, false)]
        [TestCase(BattingWicketLossType.Bowled, 3,  false, false, false)]
        [TestCase(BattingWicketLossType.Bowled, 3, true, false, true)]
        [TestCase(BattingWicketLossType.Bowled, -1, true, false, false)]
        [TestCase(BattingWicketLossType.Bowled, 1, true, true, false)]
        [TestCase(BattingWicketLossType.Caught, 3, true, true, true)]
        [TestCase(BattingWicketLossType.Caught, 3, false, false, false)]
        [TestCase(BattingWicketLossType.Caught, 3, true, false, false)]
        [TestCase(BattingWicketLossType.Caught, 3, false, true, false)]
        [TestCase(BattingWicketLossType.LBW, 3, true, false, true)]
        [TestCase(BattingWicketLossType.LBW, 3, false, true, false)]
        [TestCase(BattingWicketLossType.LBW, 3, false, false, false)]
        [TestCase(BattingWicketLossType.Stumped, 3, true, true, true)]
        [TestCase(BattingWicketLossType.Stumped, 3, true, false, false)]
        [TestCase(BattingWicketLossType.Stumped, 3, false, true, false)]
        [TestCase(BattingWicketLossType.Stumped, 3, false, false, false)]
        [TestCase(BattingWicketLossType.RunOut, 3,  false, false, false)]
        [TestCase(BattingWicketLossType.RunOut, 3, false, true, true)]
        [TestCase(BattingWicketLossType.RunOut, 3, true, true, false)]
        [TestCase(BattingWicketLossType.RunOut, 3, true, false, false)]
        [TestCase(BattingWicketLossType.HitWicket, 3, true, false, true)]
        [TestCase(BattingWicketLossType.HitWicket, 3, false, true, false)]
        [TestCase(BattingWicketLossType.HitWicket, 3, true, true, false)]
        public void ValidityTests(BattingWicketLossType howOut, int runs, bool bowlerInc, bool fielderInc, bool isValid)
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
            batting.SetScores(howOut, runs, fielder, bowler);

            var valid = batting.Validate();
            Assert.AreEqual(isValid, valid);
        }

        [TestCase(BattingWicketLossType.DidNotBat, 0, false, false, true, new string[] { })]
        [TestCase(BattingWicketLossType.DidNotBat, 3, false, false, false, new string[] { "RunsScored was expected to be equal to 0." })]
        [TestCase(BattingWicketLossType.DidNotBat, 0, false, true, false, new string[] { "Fielder cannot be set with DidnotBat." })]
        [TestCase(BattingWicketLossType.DidNotBat, 0, true, false, false, new string[] { "Bowler cannot be set with DidnotBat." })]
        [TestCase(BattingWicketLossType.NotOut, 1, false, false, true, new string[] { })]
        [TestCase(BattingWicketLossType.NotOut, 1, true, false, false, new string[] { "Bowler should not be set with NotOut." })]
        [TestCase(BattingWicketLossType.NotOut, 1, false, true, false, new string[] { "Fielder should not be set with NotOut." })]
        [TestCase(BattingWicketLossType.Bowled, 3, false, false, false, new string[] { "Bowler should be set with Bowled." })]
        [TestCase(BattingWicketLossType.Bowled, 3, true, false, true, new string[] { })]
        [TestCase(BattingWicketLossType.Bowled, -1, true, false, false, new string[] { "RunsScored cannot take a negative value." })]
        [TestCase(BattingWicketLossType.Bowled, 1, true, true, false, new string[] { "Fielder should not be set with Bowled." })]
        [TestCase(BattingWicketLossType.Caught, 3, true, true, true, new string[] { })]
        [TestCase(BattingWicketLossType.Caught, 3, true, false, false, new string[] { "Fielder should be set with Caught." })]
        [TestCase(BattingWicketLossType.Caught, 3, false, true, false, new string[] { "Bowler should be set with Caught." })]
        [TestCase(BattingWicketLossType.LBW, 3, true, false, true, new string[] { })]
        [TestCase(BattingWicketLossType.LBW, 3, false, false, false, new string[] { "Bowler should be set with LBW." })]
        [TestCase(BattingWicketLossType.LBW, 3, true, true, false, new string[] { "Fielder should not be set with LBW." })]
        [TestCase(BattingWicketLossType.Stumped, 3, true, true, true, new string[] { })]
        [TestCase(BattingWicketLossType.Stumped, 3, true, false, false, new string[] { "Fielder should be set with Stumped." })]
        [TestCase(BattingWicketLossType.Stumped, 3, false, true, false, new string[] { "Bowler should be set with Stumped." })]
        [TestCase(BattingWicketLossType.RunOut, 3, false, false, false, new string[] { "Fielder should be set with RunOut." })]
        [TestCase(BattingWicketLossType.RunOut, 3, false, true, true, new string[] { })]
        [TestCase(BattingWicketLossType.RunOut, 3, true, true, false, new string[] { "Bowler should not be set with RunOut." })]
        [TestCase(BattingWicketLossType.HitWicket, 3, true, false, true, new string[] { })]
        [TestCase(BattingWicketLossType.HitWicket, 3, true, true, false, new string[] { "Fielder should not be set with HitWicket." })]
        public void ValidityMessageTests(BattingWicketLossType howOut, int runs, bool bowlerInc, bool fielderInc, bool isValid, string[] validMessages)
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
            batting.SetScores(howOut, runs, fielder, bowler);

            var valid = batting.Validation();
            int number = isValid ? 0 : 1;
            Assert.AreEqual(number, valid.Count);

            var expectedList = new List<ValidationResult>();
            if (!isValid)
            {
                var expected = new ValidationResult();
                expected.IsValid = isValid;
                expected.Messages.AddRange(validMessages);
                expectedList.Add(expected);
            }

            Assertions.AreEqualResults(expectedList, valid);
        }
    }
}
