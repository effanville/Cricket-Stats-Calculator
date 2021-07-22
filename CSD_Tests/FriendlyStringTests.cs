using System;
using System.Collections.Generic;
using System.IO;
using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;
using NUnit.Framework;

namespace CricketStructures.Tests
{
    internal class FriendlyStringTests
    {
        [TestCase(0)]
        [TestCase(1)]
        public void DoStuff(int index)
        {
            var values = GetValues(index);
            var player = new PlayerName("Root", "Joe");
            var batting = new List<(int, Wicket)>(values.Item1);
            var bowling = new List<(int, int, int, int)>(values.Item2);
            var fielding = new List<(int, int, int, int)>(values.Item3);
            var season = TestCaseInstances.CreateTestSeason("Walkern", player, batting, bowling, fielding);
            var matchToTest = season.Matches[0];
            var friendlyString = matchToTest.SerializeToString();
            var stringThing = friendlyString.ToString();
            File.WriteAllText($"c:\\data\\source\\test{index}.txt", stringThing);
        }

        private Tuple<(int, Wicket)[], (int, int, int, int)[], (int, int, int, int)[]> GetValues(int valueIndex)
        {
            (int, Wicket)[] batting;
            (int, int, int, int)[] bowling;
            (int, int, int, int)[] fielding;
            switch (valueIndex)
            {
                case 0:
                default:
                    batting = new (int, Wicket)[] { (10, Wicket.NotOut), (20, Wicket.Bowled) };
                    bowling = new (int, int, int, int)[] { (4, 2, 20, 1), (6, 0, 20, 0) };
                    fielding = new (int, int, int, int)[] { (1, 0, 0, 0), (0, 1, 0, 0) };
                    return new Tuple<(int, Wicket)[], (int, int, int, int)[], (int, int, int, int)[]>(batting, bowling, fielding);
                case 1:
                    batting = new (int, Wicket)[] { (0, Wicket.NotOut), (20, Wicket.Bowled), (21, Wicket.Stumped) };
                    bowling = new (int, int, int, int)[] { (4, 2, 20, 1), (6, 0, 20, 0), (0, 0, 0, 0) };
                    fielding = new (int, int, int, int)[] { (1, 0, 0, 0), (0, 1, 0, 0), (2, 0, 0, 0) };
                    return new Tuple<(int, Wicket)[], (int, int, int, int)[], (int, int, int, int)[]>(batting, bowling, fielding);
            }
        }
    }
}
