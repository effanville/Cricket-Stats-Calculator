using Cricket.Player;
using NUnit.Framework;

namespace PlayerTests
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
    }
}
