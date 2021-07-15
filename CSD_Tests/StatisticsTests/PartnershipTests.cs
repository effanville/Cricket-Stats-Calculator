using CricketStructures.Match.Innings;
using CricketStructures.Player;
using NUnit.Framework;

namespace CricketStructures.Tests.StatisticsTests
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
            var best = new Partnership(playerOne, playerTwo)
            {
                Runs = runs,
                Wicket = wicket
            };

            var otherBest = new Partnership(playerOne, playerTwo)
            {
                Runs = otherRuns,
                Wicket = otherWicket
            };

            int comparison = best.CompareTo(otherBest);
            Assert.AreEqual(expected, comparison);
        }
    }
}
