using System.Collections.Generic;
using Cricket.Match;
using Cricket.Player;
using CSD_Tests;
using NUnit.Framework;
using Common.Structure.Validation;

namespace CricketClasses.MatchTests
{
    [TestFixture]
    public sealed class BowlingEntryTests
    {
        [Test]
        public void CanCreate()
        {
            var name = new PlayerName("Bloggs", "Joe");
            var bowling = new BowlingEntry(name);

            Assert.AreEqual(name, bowling.Name);
        }

        [Test]
        public void CanSetStats()
        {
            var name = new PlayerName("Bloggs", "Joe");
            var bowling = new BowlingEntry(name);

            bowling.SetBowling(4, 1, 23, 1);
            Assert.AreEqual(4, bowling.OversBowled);
            Assert.AreEqual(1, bowling.Maidens);
            Assert.AreEqual(23, bowling.RunsConceded);
            Assert.AreEqual(1, bowling.Wickets);
        }

        [TestCase(1, 1, 1, 1, true)]
        [TestCase(5, 4, 3, 1, true)]
        [TestCase(5, 4, 3, 11, false)]
        [TestCase(-1, 4, 3, 1, false)]
        [TestCase(5, -1, 3, 1, false)]
        [TestCase(5, 4, -2, 1, false)]
        [TestCase(5, 4, 3, -3, false)]
        public void ValidityTests(int overs, int maidens, int runs, int wickets, bool isValid)
        {
            var name = new PlayerName("Bloggs", "Joe");
            var bowling = new BowlingEntry(name);
            bowling.SetBowling(overs, maidens, runs, wickets);

            var valid = bowling.Validate();
            Assert.AreEqual(isValid, valid);
        }

        [TestCase(1, 1, 1, 1, true, new string[] { })]
        [TestCase(5, 4, 3, 11, false, new string[] { "Wickets cannot take values above 10." })]
        [TestCase(-1, 4, 3, 1, false, new string[] { "OversBowled cannot take a negative value." })]
        [TestCase(5, -1, 3, 1, false, new string[] { "Maidens cannot take a negative value." })]
        [TestCase(5, 4, -2, 1, false, new string[] { "RunsConceded cannot take a negative value." })]
        [TestCase(5, 4, 3, -3, false, new string[] { "Wickets cannot take a negative value." })]
        public void ValidityMessageTests(int overs, int maidens, int runs, int wickets, bool isValid, string[] validMessages)
        {
            var name = new PlayerName("Bloggs", "Joe");
            var bowling = new BowlingEntry(name);
            bowling.SetBowling(overs, maidens, runs, wickets);

            var valid = bowling.Validation();
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
