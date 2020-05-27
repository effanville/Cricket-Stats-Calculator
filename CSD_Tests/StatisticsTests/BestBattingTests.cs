using System;
using Cricket.Match;
using Cricket.Statistics;
using NUnit.Framework;

namespace CricketClasses.StatisticsTests
{
    [TestFixture]
    public sealed class BestBattingTests
    {
        [TestCase(10, 0, 10, 0, 0)]
        [TestCase(10, 0, 5, 0, 1)]
        [TestCase(10, 1, 10, 2, 1)]
        [TestCase(5, 0, 10, 0, -1)]
        [TestCase(10, 3, 10, 1, -1)]
        [TestCase(20, 1, 10, 1, 1)]
        [TestCase(0, 0, 0, 0, 0)]
        [TestCase(10, 3, 10, 4, 0)]
        public void ComparisonTests(int runs, Wicket howOut, int otherRuns, Wicket otherHowOut, int expected)
        {
            var best = new BestBatting
            {
                Opposition = "Sandon",
                Runs = runs,
                HowOut = howOut
            };

            var otherBest = new BestBatting
            {
                Runs = otherRuns,
                HowOut = otherHowOut,
                Opposition = "Aston"
            };

            int comparison = best.CompareTo(otherBest);
            Assert.AreEqual(expected, comparison);
        }

        [TestCase(10, 1, "Sandon", "2010/1/1", "10 not out vs Sandon on 1/1/2010")]
        [TestCase(10, 2, "Sandon", "2010/1/1", "10 vs Sandon on 1/1/2010")]
        [TestCase(10, 1, null, null, "10 not out vs unknown opposition")]
        [TestCase(10, 2, null, null, "10 vs unknown opposition")]
        public void ToStringTests(int runs, Wicket howOut, string opposition, DateTime date, string expected)
        {
            var best = new BestBatting
            {
                Runs = runs,
                HowOut = howOut,
                Opposition = opposition,
                Date = date
            };
            string value = best.ToString();
            Assert.AreEqual(expected, value);
        }
    }
}
