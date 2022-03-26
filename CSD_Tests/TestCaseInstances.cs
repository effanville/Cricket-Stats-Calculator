using System;
using System.Collections.Generic;
using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;
using NUnit.Framework;

namespace CricketStructures.Tests
{
    internal class TestCaseInstances
    {
        private static Dictionary<string, ICricketMatch> fExampleMatches;

        public static Dictionary<string, ICricketMatch> ExampleMatches
        {
            get
            {
                if (fExampleMatches == null)
                {
                    fExampleMatches = new Dictionary<string, ICricketMatch>();
                    string example1Name = "HighestODIChase";
                    var firstExample = new CricketMatch();
                    firstExample.EditInfo("South Africa", "Australia", new DateTime(2006, 3, 12), "Joburg");
                    firstExample.SetBattingFirst(isHomeTeam: false);

                    var aussieInnings = firstExample.GetInnings("Australia", batting: true);
                    aussieInnings.DidNotBat("Lewis", "ML", 11);
                    aussieInnings.SetBatting("Gilchrist", "AC", Wicket.Caught, 55, 1, 1, 97, "Hall", "AJ", false, "Telemachus", "R");
                    aussieInnings.SetBatting("Katich", "SM", Wicket.Caught, 79, 2, 2, 216, "Telemachus", "R", false, "Ntini", "M");
                    aussieInnings.SetBatting("Ponting", "RT", Wicket.Caught, 164, 3, 4, 407, "Dippenaar", "HH", false, "Telemachus", "R");
                    aussieInnings.SetBatting("Hussey", "MEK", Wicket.Caught, 81, 4, 3, 374, "Ntini", "M", false, "Hall", "AJ");
                    aussieInnings.SetBatting("Symonds", "A", Wicket.NotOut, 27, 5, 0, 0);
                    aussieInnings.SetBatting("Lee", "B", Wicket.NotOut, 9, 6, 0, 0);
                    aussieInnings.DidNotBat("Martyn", "DR", 7);
                    aussieInnings.DidNotBat("Clarke", "MJ", 8);
                    aussieInnings.DidNotBat("Bracken", "NW", 9);
                    aussieInnings.DidNotBat("Clark", "SR", 10);
                    aussieInnings.SetExtras(0, 4, 5, 10);
                    aussieInnings.SetBowling("Ntini", "M", 9, 0, 80, 1, 1, 0);
                    aussieInnings.SetBowling("Hall", "AJ", 10, 0, 80, 1, 0, 2);
                    aussieInnings.SetBowling("van der Wath", "JJ", 10, 0, 76, 0, 1, 1);
                    aussieInnings.SetBowling("Telemachus", "R", 10, 1, 87, 2, 3, 7);
                    aussieInnings.SetBowling("Smith", "GC", 4, 0, 29, 0, 0, 0);
                    aussieInnings.SetBowling("Kallis", "JH", 6, 0, 70, 0, 0, 0);
                    aussieInnings.SetBowling("Kemp", "JM", 1, 0, 8, 0, 0, 0);

                    var sAInnings = firstExample.GetInnings("South Africa", batting: true);
                    sAInnings.SetBatting("Smith", "GC", Wicket.Caught, 90, 1, 2, 190, "Hussey", "MEK", false, "Clarke", "SR");
                    sAInnings.SetBatting("Dippenaar", "HH", Wicket.Bowled, 1, 2, 1, 3, null, null, false, "Bracken", "NW");
                    sAInnings.SetBatting("Gibbs", "HH", Wicket.Caught, 175, 3, 4, 299, "Lee", "B", false, "Symonds", "A");
                    sAInnings.SetBatting("de Villiers", "AB", Wicket.Caught, 14, 4, 3, 284, "Bracken", "NW", false, "Clarke", "MJ");
                    sAInnings.SetBatting("Kallis", "JH", Wicket.Caught, 20, 5, 5, 327, "Symonds", "A", false, "Symonds", "A");
                    sAInnings.SetBatting("Boucher", "MV", Wicket.NotOut, 50, 6, 0, 0);
                    sAInnings.SetBatting("Kemp", "JM", Wicket.Caught, 13, 7, 6, 355, "Martyn", "DR", false, "Bracken", "NW");
                    sAInnings.SetBatting("van der Wath", "JJ", Wicket.Caught, 35, 8, 7, 399, "Ponting", "RT", false, "Bracken", "NW");
                    sAInnings.SetBatting("Telemachus", "R", Wicket.Caught, 12, 9, 8, 423, "Hussey", "MEK", false, "Bracken", "NW");
                    sAInnings.SetBatting("Hall", "AJ", Wicket.Caught, 7, 10, 9, 433, "Clarke", "MJ", false, "Lee", "B");
                    sAInnings.SetBatting("Ntini", "M", Wicket.NotOut, 1, 11, 0, 0);
                    sAInnings.SetExtras(4, 8, 4, 4);
                    sAInnings.SetBowling("Lee", "B", (double)47 / 6, 0, 68, 1, 1, 3);
                    sAInnings.SetBowling("Bracken", "NW", 10, 0, 67, 5, 0, 0);
                    sAInnings.SetBowling("Clark", "SR", 6, 0, 54, 0, 0, 0);
                    sAInnings.SetBowling("Lewis", "ML", 10, 0, 113, 0, 1, 1);
                    sAInnings.SetBowling("Symonds", "A", 9, 0, 75, 2, 0, 0);
                    sAInnings.SetBowling("Clarke", "MJ", 7, 0, 49, 1, 0, 0);
                    fExampleMatches.Add(example1Name, firstExample);
                }

                return fExampleMatches;
            }
        }

        public static ICricketSeason CreateTestSeason(string TeamName, PlayerName name, List<(int, Wicket)> battingValues, List<(int, int, int, int)> bowlingValues, List<(int, int, int, int)> fieldingValues)
        {
            Assert.AreEqual(battingValues.Count, bowlingValues.Count);
            Assert.AreEqual(battingValues.Count, fieldingValues.Count);
            ICricketSeason season = new CricketSeason();

            for (int i = 0; i < battingValues.Count; i++)
            {
                string oppo = "oppo" + i;
                DateTime date = new DateTime(2000, 1, 1);
                season.AddMatch(new MatchInfo(TeamName, oppo, null, date, MatchType.League));
                var match = season.GetMatch(date, TeamName, oppo);
                match.SetBattingFirst(isHomeTeam: true);
                match.SetBatting(TeamName, name, battingValues[i].Item2, battingValues[i].Item1, 1, 1, 0);
                match.SetBowling(TeamName, name, bowlingValues[i].Item1, bowlingValues[i].Item2, bowlingValues[i].Item3, bowlingValues[i].Item4);
                for (int j = 0; j < fieldingValues[i].Item1; j++)
                {
                    //catches
                    match.SetBatting(oppo, new PlayerName("other", $"this{j}"), Wicket.Caught, 0, 0, 0, 0, name);
                }

                for (int j = 0; j < fieldingValues[i].Item2; j++)
                {
                    // run outs
                    match.SetBatting(oppo, new PlayerName("other", $"this{fieldingValues[i].Item1 + j}"), Wicket.RunOut, 0, 0, 0, 0, name);
                }
                for (int j = 0; j < fieldingValues[i].Item3; j++)
                {
                    // stumping
                    match.SetBatting(oppo, new PlayerName("other", $"this{fieldingValues[i].Item1 + fieldingValues[i].Item2 + j}"), Wicket.Stumped, 0, 0, 0, 0, name, true);
                }
                for (int j = 0; j < fieldingValues[i].Item4; j++)
                {
                    match.SetBatting(oppo, new PlayerName("other", $"this{fieldingValues[i].Item1 + fieldingValues[i].Item2 + fieldingValues[i].Item3 + j}"), Wicket.Caught, 0, 0, 0, 0, name, true);
                }
            }

            return season;
        }
    }
}
