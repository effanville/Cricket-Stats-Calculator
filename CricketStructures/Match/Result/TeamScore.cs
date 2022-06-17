using CricketStructures.Match.Innings;

namespace CricketStructures.Match.Result
{
    public sealed class TeamScore
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
