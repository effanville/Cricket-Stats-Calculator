using System.Collections.Generic;
using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;
using NUnit.Framework;

namespace CricketStructures.Tests.MatchTests
{
    public sealed class MatchPartnerShipTests
    {
        private (CricketInnings, List<Partnership>) GenerateInnings(int index)
        {
            var innings = new CricketInnings();
            List<Partnership> ships = new List<Partnership>();
            switch (index)
            {
                case 0:
                {
                    innings.SetBatting(new PlayerName("Lyth", "A"), Wicket.Caught, 55, 1, 1, 116);
                    innings.SetBatting(new PlayerName("Frame", "WAR"), Wicket.Caught, 106, 2, 4, 205);
                    innings.SetBatting(new PlayerName("Ballance", "G"), Wicket.Caught, 23, 3, 2, 187);
                    innings.SetBatting(new PlayerName("Cadmore", "T"), Wicket.Bowled, 5, 4, 3, 201);
                    innings.SetBatting(new PlayerName("Leaning", "J"), Wicket.Caught, 0, 5, 5, 205);
                    innings.SetBatting(new PlayerName("Tattersall", "J"), Wicket.Bowled, 11, 6, 8, 250);
                    innings.SetBatting(new PlayerName("Willey", "D"), Wicket.Caught, 19, 7, 6, 231);
                    innings.SetBatting(new PlayerName("Maharaj", "K"), Wicket.Bowled, 0, 8, 7, 231);
                    innings.SetBatting(new PlayerName("Patterson", "S"), Wicket.Caught, 46, 9, 10, 327);
                    innings.SetBatting(new PlayerName("Coad", "B"), Wicket.Caught, 25, 10, 9, 295);
                    innings.SetBatting(new PlayerName("Olivier", "D"), Wicket.NotOut, 11, 11, 11, 327);
                    ships.Add(new Partnership(new PlayerName("Lyth", "A"), new PlayerName("Frame", "WAR"), 1, 116));
                    ships.Add(new Partnership(new PlayerName("Frame", "WAR"), new PlayerName("Ballance", "G"), 2, 71));
                    ships.Add(new Partnership(new PlayerName("Frame", "WAR"), new PlayerName("Cadmore", "T"), 3, 14));
                    ships.Add(new Partnership(new PlayerName("Frame", "WAR"), new PlayerName("Leaning", "J"), 4, 4));
                    ships.Add(new Partnership(new PlayerName("Leaning", "J"), new PlayerName("Tattersall", "J"), 5, 0));
                    ships.Add(new Partnership(new PlayerName("Tattersall", "J"), new PlayerName("Willey", "D"), 6, 26));
                    ships.Add(new Partnership(new PlayerName("Tattersall", "J"), new PlayerName("Maharaj", "K"), 7, 0));
                    ships.Add(new Partnership(new PlayerName("Tattersall", "J"), new PlayerName("Patterson", "S"), 8, 19));
                    ships.Add(new Partnership(new PlayerName("Patterson", "S"), new PlayerName("Coad", "B"), 9, 45));
                    ships.Add(new Partnership(new PlayerName("Patterson", "S"), new PlayerName("Olivier", "D"), 10, 32));
                    break;
                }
            }

            return (innings, ships);

        }

        [TestCase(0)]
        public void CanCalculatePartnerships(int inningsIndex)
        {
            var matchInfo = new MatchInfo
            {
                AwayTeam = "Evil"
            };
            var match = new CricketMatch(matchInfo);
            var data = GenerateInnings(inningsIndex);
            var innings = data.Item1;
            innings.BattingTeam = "Evil";
            match.SetInnings(data.Item1, true);
            var ships = match.Partnerships("Evil");
            Assertions.PartnershipsEqual(data.Item2, ships);
        }
    }
}
