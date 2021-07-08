using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Player;

namespace CricketStructures.Statistics.PlayerStats
{
    public class BowlingPerformance
    {
        public PlayerName Name
        {
            get;
            set;
        }

        public double Overs
        {
            get;
            set;
        }

        public int Maidens
        {
            get;
            set;
        }

        public int RunsConceded
        {
            get;
            set;
        }

        public int Wickets
        {
            get;
            set;
        }

        public MatchInfo MatchData
        {
            get;
            set;
        }

        public BowlingPerformance()
        {
        }

        public BowlingPerformance(BowlingEntry bowlingEntry, MatchInfo matchData)
        {
            Name = bowlingEntry.Name;
            MatchData = matchData;
            Overs = bowlingEntry.OversBowled;
            Maidens = bowlingEntry.Maidens;
            RunsConceded = bowlingEntry.RunsConceded;
            Wickets = bowlingEntry.Wickets;
        }
    }
}
