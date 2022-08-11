using CricketStructures.Match.Innings;

using NUnit.Framework;

namespace CricketStructures.Tests.MatchTests
{
    [TestFixture]
    internal sealed class OverTests
    {
        [TestCase(1, 2, 1, 2, true)]
        [TestCase(1, 2, 1, 3, false)]
        [TestCase(1, 2, 2, 2, false)]
        public void AreEqual(int overOvers1, int overBalls1, int overOvers2, int overBalls2, bool expectedResult)
        {
            var over = new Over(overOvers1, overBalls1);
            var otherOver = new Over(overOvers2, overBalls2);
            Assert.That(expectedResult, Is.EqualTo(over.Equals(otherOver)));
        }

        [TestCase("1.2", 1, 2)]
        [TestCase("3.5", 3, 5)]
        [TestCase("3", 3, 0)]
        [TestCase("0.4", 0, 4)]
        public void FromString(string inputString, int overOvers1, int overBalls1)
        {
            var over = new Over(overOvers1, overBalls1);
            var otherOver = Over.FromString(inputString);
            Assert.That(over, Is.EqualTo(otherOver));
        }

        [TestCase("1.2", 1, 2)]
        [TestCase("3.5", 3, 5)]
        [TestCase("3", 3, 0)]
        [TestCase("0.4", 0, 4)]
        public void ToString(string expectedString, int overOvers1, int overBalls1)
        {
            var over = new Over(overOvers1, overBalls1);
            var overString = over.ToString();
            Assert.That(overString, Is.EqualTo(expectedString));
        }
    }
}
