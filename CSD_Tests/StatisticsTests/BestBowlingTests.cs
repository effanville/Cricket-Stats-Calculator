using System;
using Cricket.Statistics;
using NUnit.Framework;

namespace CricketClasses.StatisticsTests
{
    [TestFixture]
    public sealed class BestBowlingTests
    {
        [TestCase(10, 0, 10, 0, 0)]
        [TestCase(10, 0, 5, 0, -1)]
        [TestCase(10, 1, 10, 2, -1)]
        [TestCase(5, 0, 10, 0, 1)]
        [TestCase(10, 3, 10, 1, 1)]
        [TestCase(20, 1, 10, 1, -1)]
        [TestCase(0, 0, 0, 0, 0)]
        [TestCase(10, 3, 10, 4, -1)]
        [TestCase(10, 3, 10, 3, 0)]
        [TestCase(10, 4, 10, 2, 1)]
        public void ComparisonTests(int runs, int wickets, int otherRuns, int otherWickets, int expected)
        {
            var best = new BestBowling
            {
                Runs = runs,
                Wickets = wickets
            };

            var otherBest = new BestBowling
            {
                Runs = otherRuns,
                Wickets = otherWickets
            };

            int comparison = best.CompareTo(otherBest);
            Assert.AreEqual(expected, comparison);
        }

        [TestCase(10, 1, "Sandon", "2010/1/1", "1-10 vs Sandon on 1/1/2010")]
        [TestCase(10, 2, "Sandon", "2010/1/1", "2-10 vs Sandon on 1/1/2010")]
        [TestCase(10, 1, null, null, "1-10 vs unknown opposition")]
        [TestCase(10, 2, null, null, "2-10 vs unknown opposition")]
        public void ToStringTests(int runs, int wickets, string opposition, DateTime date, string expected)
        {
            var best = new BestBowling
            {
                Runs = runs,
                Wickets = wickets,
                Opposition = opposition,
                Date = date
            };
            string value = best.ToString();
            Assert.AreEqual(expected, value);
        }
    }
}
