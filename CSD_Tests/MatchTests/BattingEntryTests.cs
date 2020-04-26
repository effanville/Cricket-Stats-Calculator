using NUnit.Framework;
using Cricket.Match;
using Cricket.Player;

namespace MatchTests
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
    }
}
