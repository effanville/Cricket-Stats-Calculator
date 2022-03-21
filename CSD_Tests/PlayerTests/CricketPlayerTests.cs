using System.Collections.Generic;
using Cricket.Player;
using CSD_Tests;
using NUnit.Framework;
using Common.Structure.Validation;

namespace CricketClasses.PlayerTests
{
    [TestFixture]
    public sealed class CricketPlayerTests
    {
        [Test]
        public void CanCreate()
        {
            string surname = "Bloggs";
            string forename = "Joe";

            var player = new CricketPlayer(surname, forename);

            Assert.AreEqual(forename, player.Name.Forename);
            Assert.AreEqual(surname, player.Name.Surname);
        }

        [Test]
        public void CanEdit()
        {
            string surname = "Bloggs";
            string forename = "Joe";

            var player = new CricketPlayer(surname, forename);

            Assert.AreEqual(forename, player.Name.Forename);
            Assert.AreEqual(surname, player.Name.Surname);

            player.EditName("Smith", "Steve");
            Assert.AreEqual("Steve", player.Name.Forename);
            Assert.AreEqual("Smith", player.Name.Surname);
        }

        [Test]
        public void ToStringCorrect()
        {
            string surname = "Bloggs";
            string forename = "Joe";

            var player = new CricketPlayer(surname, forename);

            Assert.AreEqual(forename, player.Name.Forename);
            Assert.AreEqual(surname, player.Name.Surname);
            Assert.AreEqual(forename + " " + surname, player.ToString());
        }

        [TestCase("Smith", "Steve", true)]
        [TestCase("Bloggs", "Joe", true)]
        [TestCase("Bloggs", "", false)]
        [TestCase("", "Joe", false)]
        [TestCase("Bloggs", null, false)]
        [TestCase(null, "Joe", false)]
        [TestCase(null, null, false)]
        public void TestValidity(string surname, string forename, bool isValid)
        {
            var name = new CricketPlayer(surname, forename);
            var valid = name.Validate();
            Assert.AreEqual(isValid, valid);
        }

        [TestCase("Bloggs", "Joe", true, new string[] { })]
        [TestCase("Bloggs", "", false, new string[] { "SecondaryName cannot be empty or null." })]
        [TestCase("", "Joe", false, new string[] { "PrimaryName cannot be empty or null." })]
        [TestCase("Bloggs", null, false, new string[] { "SecondaryName cannot be empty or null." })]
        [TestCase(null, "Joe", false, new string[] { "PrimaryName cannot be empty or null." })]
        public void TestValidityMessage(string surname, string forename, bool isValid, string[] isValidMessage)
        {
            var name = new CricketPlayer(surname, forename);
            var valid = name.Validation();
            var expected = new ValidationResult();
            if (!isValid)
            {
                expected.IsValid = isValid;
                expected.Messages.AddRange(isValidMessage);
                var expectedList = new List<ValidationResult>
                {
                    expected
                };
                Assertions.ValidationListsEqual(expectedList, valid);
            }
            else
            {
                Assertions.ValidationListsEqual(new List<ValidationResult>(), valid);
            }
        }

        [Test]
        public void TestValidityMessages()
        {
            var name = new CricketPlayer("", "");
            var valid = name.Validation();
            var expected1 = new ValidationResult
            {
                IsValid = false
            };
            expected1.Messages.Add("PrimaryName cannot be empty or null.");
            var expected2 = new ValidationResult
            {
                IsValid = false
            };
            expected2.Messages.Add("SecondaryName cannot be empty or null.");
            var expectedList = new List<ValidationResult>
            {
                expected1,
                expected2
            };
            Assertions.ValidationListsEqual(expectedList, valid);
        }
    }
}
