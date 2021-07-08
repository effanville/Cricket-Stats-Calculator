using CricketStructures.Match;
using CricketStructures.Match.Innings;

namespace CricketStructures.Statistics.DetailedStats
{
    public class TeamScore
    {
        public InningsScore Score
        {
            get;
            set;
        }

        public MatchInfo Info
        {
            get;
            set;
        }

        public TeamScore()
        {
        }

        public TeamScore(InningsScore score, MatchInfo info)
        {
            Score = score;
            Info = info;
        }
    }
}
