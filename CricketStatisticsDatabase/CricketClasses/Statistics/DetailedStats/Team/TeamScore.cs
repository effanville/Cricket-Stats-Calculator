using Cricket.Match;

namespace Cricket.Statistics.DetailedStats
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
