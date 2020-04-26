using NUnit.Framework;
using Cricket.Match;
using Cricket.Player;

namespace MatchTests
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
    }
}
