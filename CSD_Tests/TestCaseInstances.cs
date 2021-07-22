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
