using System;

namespace Cricket.Match
{
    public class InningsScore : IComparable
    {
        public override string ToString()
        {
            return Runs + " - " + Wickets;
        }

        public int CompareTo(object obj)
        {
            if (obj is InningsScore otherScore)
            {
                if (otherScore.Runs.Equals(Runs))
                {
                    return Wickets.CompareTo(otherScore.Wickets);
                }

                return Runs.CompareTo(otherScore.Runs);
            }

            return 0;
        }

        public int Runs
        { get; set; }

        public int Wickets
        { get; set; }

        public InningsScore()
        { }

        public InningsScore(int runs, int wickets)
        {
            Runs = runs;
            Wickets = wickets;
        }
    }
}
