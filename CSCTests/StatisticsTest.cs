using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Cricket;
using CricketStatsCalc;

namespace CSCTests
{
    /// <summary>
    /// Checks statistics are calculated correctly.
    /// Adds a single player, then creates several "matches" and then ensure that the statistics
    /// are the expected values.
    /// </summary>
    [TestFixture]
    public class StatisticsTests
    {
        // Create player and then add several matches
        Cricket_Player Matt = new Cricket_Player("Matt");

        private static List<string> PlayerNames = new List<string>() { "Matt", "", "", "", "", "", "", "", "", "", "" };


        Cricket_Match DummyMatch = new Cricket_Match("Sandon", new DateTime(2019, 5, 5), "Sandon",ResultType.Win, MatchType.League , "Matt", PlayerNames);

        Cricket_Match DummyMatch2 = new Cricket_Match("Aston", new DateTime(2019, 5, 5), "Aston", ResultType.Win, MatchType.League,"Matt",  PlayerNames);

        public void BattingStatsPrep_TwoMatch(int[] RS1, int[] MO1, int[] RS2, int[] MO2)
        {
            List<OutType> Method_Out1 = new List<OutType>(new OutType[11]);
            for (int i = 0; i < 11; ++i)
            {
                Method_Out1[i] = (OutType)Enum.Parse(typeof(OutType), MO1[i].ToString());
            }

            List<OutType> Method_Out2 = new List<OutType>(new OutType[11]);
            for (int i = 0; i<11;++i)
            {
                Method_Out2[i] = (OutType)Enum.Parse(typeof(OutType), MO2[i].ToString());
            }
            List<int> RunsScored1 = RS1.ToList();
            List<int> RunsScored2 = RS2.ToList();
            DummyMatch.FBatting.Set_Data(RunsScored1, Method_Out1, 0);
            DummyMatch2.FBatting.Set_Data(RunsScored2, Method_Out2, 0);

        }

        public void BowlingStatsPrep_TwoMatch(int[] O1, int[] M1, int[] RC1, int[] W1, int[] O2, int[] M2, int[] RC2, int[] W2 )
        { 
            int extras = 0;
            List<int> Ov1 = O1.ToList();
            List<int> Md1 = M1.ToList();
            List<int> R1 = RC1.ToList();
            List<int> Wc1 = W1.ToList();
            List<int> Ov2 = O2.ToList();
            List<int> Md2 = M2.ToList();
            List<int> R2 = RC2.ToList();
            List<int> Wc2 = W2.ToList();
            DummyMatch.FBowling.Add_Data(Ov1,Md1,R1,Wc1,0);
            DummyMatch2.FBowling.Add_Data(Ov2, Md2, R2, Wc2,0);
        }
        
        public void FieldingStatsPrep_TwoMatch(int[] Ca1, int[] ROu1, int[] STu1, int[] CWi1, int[] Ca2, int[] ROu2, int[] STu2, int[] CWi2)
        {
            List<int> C1 = Ca1.ToList();
            List<int> RO1 = ROu1.ToList();
            List<int> ST1 = STu1.ToList();
            List<int> CW1 = CWi1.ToList();
            List<int> C2 = Ca2.ToList();
            List<int> RO2 = ROu2.ToList();
            List<int> ST2 = STu2.ToList();
            List<int> CW2 = CWi2.ToList();
            DummyMatch.FFieldingStats.Add_Data(C1,RO1, ST1,CW1);
            DummyMatch2.FFieldingStats.Add_Data(C2, RO2, ST2, CW2);
        }

        [Test]
        [TestCase(new int[] { 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] {0,7,7,7,7,7,7,7,7,7,7 }, new int[] { 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 4, 7,7,7,7,7,7,7,7,7,7 }, new double[] { 55, 2, 1, 55 }, TestName = "Batting_1Nout_Out") ]
        [TestCase(new int[] { 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 0, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7 }, new int[] { 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 0, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7 }, new double[] { 50, 2, 2, 0 }, TestName = "Batting_Not_Out")]
        [TestCase(new int[] { 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 3, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7 }, new int[] { 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 4, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7 }, new double[] { 30, 2, 0, 15 }, TestName = "Batting_2Out")]
        [TestCase(new int[] { 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 3, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7 }, new int[] { 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 4, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7 }, new double[] { 27, 2, 0, 13.5 }, TestName = "Batting_double_av")]
        public void BattingTotalRunsTest(int[] RunsScored1, int[] MO1, int[] RunsScored2, int[] MO2, double[] expectedValue)
        {
            BattingStatsPrep_TwoMatch(RunsScored1, MO1, RunsScored2, MO2);

            // we test batting here, but still must add values to bowling to avoid errors
            int[] DV = new int[11] { 0,0,0,0,0,0,0,0,0,0,0};

            BowlingStatsPrep_TwoMatch(DV, DV, DV, DV, DV, DV, DV, DV);

            FieldingStatsPrep_TwoMatch(DV, DV, DV, DV, DV, DV, DV, DV);

            // load data into the database
            Globals.Ardeley.Add(Matt);
            Globals.GamesPlayed.Add(DummyMatch);
            Globals.GamesPlayed.Add(DummyMatch2);

            Matt.set_statistics();

            Assert.AreEqual(expectedValue[0], Matt.Total_runs,"Expected Runs Not Same");
            Assert.AreEqual(expectedValue[1], Matt.Total_innings, "Expected Total Innings Not Same");
            Assert.AreEqual(expectedValue[2], Matt.Total_not_out,"Expected Not Out Not same");
            Assert.AreEqual(expectedValue[3], Matt.Batting_average, "Expected Average Not Same");

            // clear database for future tests
            Globals.Ardeley.Remove(Matt);
            Matt.Calculated = false;
            Matt.Batting_average = 0;
            Matt.Total_not_out = 0;
            Matt.Total_innings = 0;
            Globals.GamesPlayed.Remove(DummyMatch);
            Globals.GamesPlayed.Remove(DummyMatch2);
        }

        /// <summary>
        /// Test to ensure
        /// </summary>
        /// <param name="O1"></param>
        /// <param name="M1"></param>
        /// <param name="R1"></param>
        /// <param name="W1"></param>
        /// <param name="O2"></param>
        /// <param name="M2"></param>
        /// <param name="R2"></param>
        /// <param name="W2"></param>
        /// <param name="ExpectedVals"></param>
        [Test]
        [TestCase(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new double[] { 0, 0, 0, 0, double.NaN , double.NaN }, TestName = "Bowling_Nothing")]
        [TestCase(new int[] { 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 33, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new double[] { 10, 2, 40, 0, double.NaN, 4 }, TestName = "Bowling_NoWickets")]
        [TestCase(new int[] { 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new double[] { 4, 0, 20, 1, 20, 5 }, TestName = "Bowling_1Wicket")]
        [TestCase(new int[] { 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 17, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new int[] { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new double[] { 9, 2, 27, 3, 9, 3 }, TestName = "Bowling_Wickets")]
        public void BowlingStatsTest(int [] O1, int[] M1, int[] R1, int[] W1, int[] O2, int[] M2, int[] R2, int[] W2, double[] ExpectedVals)
        {
            // we test bowling here, but still must add values to batting to avoid errors
            int[] DV = new int[11] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            BattingStatsPrep_TwoMatch(DV, DV, DV, DV);

            BowlingStatsPrep_TwoMatch(O1, M1, R1,  W1,  O2,  M2, R2, W2);

            FieldingStatsPrep_TwoMatch(DV, DV, DV, DV, DV, DV, DV, DV);

            // load data into the database
            Globals.Ardeley.Add(Matt);
            Globals.GamesPlayed.Add(DummyMatch);
            Globals.GamesPlayed.Add(DummyMatch2);

            Matt.set_statistics();

            Assert.AreEqual(ExpectedVals[0], Matt.Total_overs, "Expected Overs Not Same");
            Assert.AreEqual(ExpectedVals[1], Matt.Total_maidens, "Expected Maidens Not Same");
            Assert.AreEqual(ExpectedVals[2], Matt.Total_runs_conceded, "Expected Runs Conceded Not Same");
            Assert.AreEqual(ExpectedVals[3], Matt.Total_wickets, "Expected Wickets Not Same");
            Assert.AreEqual(ExpectedVals[4], Matt.Bowling_average, "Expected Average Not Same");
            Assert.AreEqual(ExpectedVals[5], Matt.Bowling_economy, "Expected Econ Not Same");
            // clear database for future tests
            Globals.Ardeley.Remove(Matt);
            Matt.Calculated = false;
            Matt.Batting_average = 0;
            Matt.Total_not_out = 0;
            Matt.Total_innings = 0;
            Globals.GamesPlayed.Remove(DummyMatch);
            Globals.GamesPlayed.Remove(DummyMatch2);
        }
    }
}
