using Cricket.Player;
using Cricket.Statistics;
using NUnit.Framework;

namespace StatisticsTests
{
    public sealed class PartnershipTests
    {
        [TestCase(10, 0, 10, 0, 0)]
        [TestCase(10, 0, 5, 0, 1)]
        [TestCase(10, 1, 10, 2, 0)]
        [TestCase(5, 0, 10, 0, -1)]
        [TestCase(10, 3, 10, 1, 0)]
        [TestCase(20, 1, 10, 1, 1)]
        [TestCase(0, 0, 0, 0, 0)]
        [TestCase(10, 3, 10, 4, 0)]
        [TestCase(10, 3, 10, 3, 0)]
        [TestCase(10, 4, 10, 2, 0)]
        public void ComparisonTests(int runs, int wicket, int otherRuns, int otherWicket, int expected)
        {
            var playerOne = new PlayerName("player", "one");
            var playerTwo = new PlayerName("player", "two");
            var best = new Partnership(playerOne, playerTwo);
            best.Runs = runs;
            best.Wicket = wicket;

            var otherBest = new Partnership(playerOne, playerTwo);
            otherBest.Runs = otherRuns;
            otherBest.Wicket = otherWicket;

            int comparison = best.CompareTo(otherBest);
            Assert.AreEqual(expected, comparison);
        }
    }
}
