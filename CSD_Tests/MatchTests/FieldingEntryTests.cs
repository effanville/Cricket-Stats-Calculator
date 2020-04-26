using Cricket.Match;
using Cricket.Player;
using NUnit.Framework;

namespace MatchTests
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
    }
}
