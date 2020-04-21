using Cricket.Player;
using NUnit.Framework;

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

        [TestCase("Bloggs", "Joe", true)]
        [TestCase("Bloggs", "", false)]
        [TestCase("", "Joe", false)]
        public void TestValidity(string surname, string forename, bool isValid)
        {
            var name = new PlayerName(surname, forename);
            var valid = name.Validate();
            Assert.AreEqual(isValid, valid);
        }
    }

    [TestFixture]
    public sealed class PlayerNameToStringTests
    {
        [TestCase("Bloggs", "Joe")]
        [TestCase("Bloggs", "")]
        [TestCase("", "Joe")]
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
        public void ConvertBackCorrectly(string surname, string forename)
        {
            var name = new PlayerName(surname, forename);

            var converter = new PlayerNameToStringConverter();

            var converted = converter.ConvertBack(name.ToString(), null, null, null);
            Assert.AreEqual(name, converted);
        }
    }
}
