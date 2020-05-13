using Cricket.Match;
using Cricket.Player;
using CSD_Tests;
using NUnit.Framework;
using System.Collections.Generic;
using StructureCommon.Validation;

namespace CricketClasses.MatchTests
{
    [TestFixture]
    public sealed class FieldingEntryTests
    {
        [Test]
        public void CanCreate()
        {
            var name = new PlayerName("Bloggs", "Joe");
            var fielding = new FieldingEntry(name);

            Assert.AreEqual(name, fielding.Name);
        }

        [Test]
        public void CanSetStats()
        {
            var name = new PlayerName("Bloggs", "Joe");
            var fielding = new FieldingEntry(name);
            fielding.SetScores(5, 4, 3, 1);
            Assert.AreEqual(5, fielding.Catches);
            Assert.AreEqual(4, fielding.RunOuts);
            Assert.AreEqual(3, fielding.KeeperStumpings);
            Assert.AreEqual(1, fielding.KeeperCatches);
        }

        [TestCase(1, 1, 1, 1, true)]
        [TestCase(5, 4, 3, 1, false)]
        [TestCase(-1, 4, 3, 1, false)]
        [TestCase(5, -1, 3, 1, false)]
        [TestCase(5, 4, -2, 1, false)]
        [TestCase(5, 4, 3, -3, false)]
        public void ValidityTests(int catches, int runOuts, int keepCatches, int keepStumpings, bool isValid)
        {
            var name = new PlayerName("Bloggs", "Joe");
            var fielding = new FieldingEntry(name);
            fielding.SetScores(catches, runOuts, keepCatches, keepStumpings);

            var valid = fielding.Validate();
            Assert.AreEqual(isValid, valid);
        }

        [TestCase(1, 1, 1, 1, true, new string[] { })]
        [TestCase(5, 4, 3, 1, false, new string[] { "FieldingEntry cannot take values above 10." })]
        [TestCase(-1, 4, 3, 1, false, new string[] { "Catches cannot take a negative value." })]
        [TestCase(5, -1, 3, 1, false, new string[] { "RunOuts cannot take a negative value." })]
        [TestCase(5, 4, -2, 1, false, new string[] { "KeeperStumpings cannot take a negative value." })]
        [TestCase(5, 4, 3, -3, false, new string[] { "KeeperCatches cannot take a negative value." })]
        public void ValidityMessageTests(int catches, int runOuts, int keepCatches, int keepStumpings, bool isValid, string[] validMessages)
        {
            var name = new PlayerName("Bloggs", "Joe");
            var fielding = new FieldingEntry(name);
            fielding.SetScores(catches, runOuts, keepCatches, keepStumpings);

            var valid = fielding.Validation();
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

        [TestCase(1, 1, 1, 1, true, new string[] { }, true, new string[] { })]
        [TestCase(5, 4, 3, -1, false, new string[] { "KeeperCatches cannot take a negative value." }, false, new string[] { "FieldingEntry cannot take values above 10." })]
        [TestCase(-1, -1, 3, 1, false, new string[] { "Catches cannot take a negative value." }, false, new string[] { "RunOuts cannot take a negative value." })]
        [TestCase(5, -1, 3, -1, false, new string[] { "RunOuts cannot take a negative value." }, false, new string[] { "KeeperCatches cannot take a negative value." })]
        [TestCase(5, 4, -2, -1, false, new string[] { "KeeperStumpings cannot take a negative value." }, false, new string[] { "KeeperCatches cannot take a negative value." })]
        public void ValidityMultipleMessageTests(int catches, int runOuts, int keepCatches, int keepStumpings, bool isValid, string[] validMessages, bool isValid2, string[] validMessages2)
        {
            var name = new PlayerName("Bloggs", "Joe");
            var fielding = new FieldingEntry(name);
            fielding.SetScores(catches, runOuts, keepCatches, keepStumpings);

            var valid = fielding.Validation();
            int number = isValid ? 0 : 1;
            if (!isValid2)
            {
                number++;
            }
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
            if (!isValid)
            {
                var expected = new ValidationResult
                {
                    IsValid = isValid2
                };
                expected.Messages.AddRange(validMessages2);
                expectedList.Add(expected);
            }

            Assertions.ValidationListsEqual(expectedList, valid);
        }
    }
}
