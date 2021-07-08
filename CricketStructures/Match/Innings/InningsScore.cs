using System;

namespace CricketStructures.Match.Innings
{
    public class InningsScore : IComparable, IEquatable<InningsScore>
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

        public bool Equals(InningsScore other)
        {
            return Runs.Equals(other.Runs) && Wickets.Equals(other.Wickets);
        }

        public int Runs
        {
            get; set;
        }

        public int Wickets
        {
            get; set;
        }

        public InningsScore()
        {
        }

        public InningsScore(int runs, int wickets)
        {
            Runs = runs;
            Wickets = wickets;
        }
    }
}
