using Cricket.Player;
using CSD_Tests;
using NUnit.Framework;
using System.Collections.Generic;
using Validation;

namespace CricketClasses.PlayerTests
{
    [TestFixture]
    public sealed class PlayerNameTests
    {
        [Test]
        public void CanCreate()
        {
            string surname = "Bloggs";
            string forename = "Joe";

            var name = new PlayerName(surname, forename);

            Assert.AreEqual(forename, name.Forename);
            Assert.AreEqual(surname, name.Surname);
        }

        [Test]
        public void CanEdit()
        {
            string surname = "Bloggs";
            string forename = "Joe";

            var name = new PlayerName(surname, forename);

            Assert.AreEqual(forename, name.Forename);
            Assert.AreEqual(surname, name.Surname);

            name.EditName("Smith", "Steve");
            Assert.AreEqual("Steve", name.Forename);
            Assert.AreEqual("Smith", name.Surname);
        }

        [TestCase("Bloggs", "Joe", "Bloggs", "Joe", true)]
        [TestCase("Bloggs", "Joe", "Bloggs", "Mark", false)]
        [TestCase("Bloggs", "Joe", "Simon", "Joe", false)]
        [TestCase("Bloggs", "Joe", "Simth", "Alan", false)]
        [TestCase("Bloggs", "Joe", "Bloggs", null, false)]
        [TestCase("Bloggs", "Joe", null, "Joe", false)]
        [TestCase("Bloggs", "Joe", null, null, false)]
        [TestCase("Bloggs", null, "Bloggs", "Joe", false)]
        [TestCase(null, "Joe", "Bloggs", "Joe", false)]
        [TestCase(null, null, "Bloggs", "Joe", false)]
        [TestCase("Bloggs", null, "Bloggs", null, true)]
        [TestCase(null, "Joe", null, "Joe", true)]
        [TestCase(null, null, null, null, true)]
        public void EqualityCorrect(string surname, string forename, string testingSurname, string testingForename, bool expected)
        {
            var player = new PlayerName(surname, forename);
            Assert.AreEqual(expected, player.Equals(new PlayerName(testingSurname, testingForename)));
        }

        [Test]
        public void ToStringCorrect()
        {
            string surname = "Bloggs";
            string forename = "Joe";

            var name = new PlayerName(surname, forename);

            Assert.AreEqual(forename, name.Forename);
            Assert.AreEqual(surname, name.Surname);
            Assert.AreEqual(forename + " " + surname, name.ToString());
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
            var name = new PlayerName(surname, forename);
            var valid = name.Validate();
            Assert.AreEqual(isValid, valid);
        }

        [TestCase("Smith", "Steve", true, new string[] { })]
        [TestCase("Bloggs", "Joe", true, new string[] { })]
        [TestCase("Bloggs", "", false, new string[] { "Forename cannot be empty or null." })]
        [TestCase("", "Joe", false, new string[] { "Surname cannot be empty or null." })]
        [TestCase("Bloggs", null, false, new string[] { "Forename cannot be empty or null." })]
        [TestCase(null, "Joe", false, new string[] { "Surname cannot be empty or null." })]
        public void TestValidityMessage(string surname, string forename, bool isValid, string[] isValidMessage )
        {
            var name = new CricketPlayer(surname, forename);
            var valid = name.Validation();
            var expectedList = new List<ValidationResult>();
            if (!isValid)
            {
                var expected = new ValidationResult();
                expected.IsValid = isValid;
                expected.Messages.AddRange(isValidMessage);
                expectedList.Add(expected);
            }
            
            Assertions.ValidationListsEqual(expectedList, valid);
        }

        [Test]
        public void TestValidityMessageBothNamesNull()
        {
            var name = new CricketPlayer(null, null);
            var valid = name.Validation();
            var expectedList = new List<ValidationResult>();
            var expectedSurnameError = new ValidationResult();
            expectedSurnameError.IsValid = false;
            expectedSurnameError.Messages.AddRange(new string[] { "Surname cannot be empty or null." });
            expectedList.Add(expectedSurnameError);

            var expectedForenameError = new ValidationResult();
            expectedForenameError.IsValid = false;
            expectedForenameError.Messages.AddRange(new string[] { "Forename cannot be empty or null." });
            expectedList.Add(expectedForenameError);

            Assertions.ValidationListsEqual(expectedList, valid);
        }
    }

    [TestFixture]
    public sealed class PlayerNameToStringTests
    {
        [TestCase("Bloggs", "Joe")]
        [TestCase("Bloggs", "")]
        [TestCase("", "Joe")]
        [TestCase("Bloggs", null)]
        [TestCase(null, "Joe")]
        [TestCase(null, null)]
        public void ConvertCorrectly(string surname, string forename)
        {
            var name = new PlayerName(surname, forename);

            var converter = new PlayerNameToStringConverter();

            var converted = converter.Convert(name, null, null, null);
            Assert.AreEqual(name.ToString(), converted);
        }

        [TestCase("Bloggs", "Joe")]
        [TestCase("Bloggs", "")]
        [TestCase("", "Joe")]
        [TestCase("Bloggs", null)]
        [TestCase(null, "Joe")]
        [TestCase(null, null)]
        public void ConvertBackCorrectly(string surname, string forename)
        {
            var name = new PlayerName(surname, forename);

            var converter = new PlayerNameToStringConverter();

            var converted = converter.ConvertBack(name.ToString(), null, null, null);
            Assert.AreEqual(name, converted);
        }
    }
}
