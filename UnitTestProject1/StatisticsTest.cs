using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cricket;
using CricketStatsCalc;

namespace CSCTests
{
    /// <summary>
    /// Checks statistics are calculated correctly.
    /// Adds a single player, then creates several "matches" and then ensure that the statistics
    /// are the expected values.
    /// </summary>
    [TestClass]
    public class StatisticsTests
    {
        // Create player and then add several matches
        Cricket_Player Matt = new Cricket_Player("Matt");

        private static List<string> PlayerNames = new List<string>() { "Matt", "", "", "", "", "", "", "", "", "", "" };

        Cricket_Match DummyMatch = new Cricket_Match("Sandon", PlayerNames);

        Cricket_Match DummyMatch2 = new Cricket_Match("Sandon", PlayerNames);



        [TestMethod]
        public void BattingTotalRunsTest()
        {
            List<int> RunsScored1 = new List<int>() { 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            List<OutType> Method_Out1 = new List<OutType>() { OutType.NotOut, OutType.DidNotBat, OutType.DidNotBat, OutType.DidNotBat, OutType.DidNotBat, OutType.DidNotBat, OutType.DidNotBat, OutType.DidNotBat, OutType.DidNotBat, OutType.DidNotBat, OutType.DidNotBat, OutType.DidNotBat };
            List<int> RunsScored2 = new List<int>() { 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            List<OutType> Method_Out2 = new List<OutType>() { OutType.Bowled, OutType.DidNotBat, OutType.DidNotBat, OutType.DidNotBat, OutType.DidNotBat, OutType.DidNotBat, OutType.DidNotBat, OutType.DidNotBat, OutType.DidNotBat, OutType.DidNotBat, OutType.DidNotBat, OutType.DidNotBat };
            DummyMatch.FBatting.Set_Data(RunsScored1,Method_Out1,0);
            DummyMatch2.FBatting.Set_Data(RunsScored2, Method_Out2, 0);
            Globals.Ardeley.Add(Matt);
            Globals.GamesPlayed.Add(DummyMatch);
            Globals.GamesPlayed.Add(DummyMatch2);

            Matt.set_statistics();
            var expected = 55;
            Assert.AreEqual(expected, Matt.Batting_average);
        }

        [TestMethod]
        public void BattingAverageTest()
        {

        }
    }
}
