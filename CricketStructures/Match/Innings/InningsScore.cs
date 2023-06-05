using System;

namespace CricketStructures.Match.Innings
{
    public class InningsScore : IComparable, IEquatable<InningsScore>
    {
        public int Runs
        {
            get; set;
        }

        public int Wickets
        {
            get; set;
        }

        public Over Overs
        {
            get;
            private set;
        }

        public InningsScore()
        {
        }

        public InningsScore(int runs, int wickets)
            : this(runs, wickets, Over.Unknown())
        {
        }

        public InningsScore(int runs, int wickets, Over overs)
        {
            Runs = runs;
            Wickets = wickets;
            Overs = overs;
        }

        /// <summary>
        /// Combines the two scores, taking the values from the first if larger.
        /// </summary>
        public static InningsScore Combine(InningsScore first, InningsScore second)
        {
            int comparison = first.CompareTo(second);
            int numberRuns = comparison < 0 ? second.Runs : first.Runs;
            int numberWickets = comparison < 0 ? second.Wickets : first.Wickets;
            Over numberOvers = first.Overs != Over.Unknown() ? first.Overs : second.Overs; 
            return new InningsScore(numberRuns, numberWickets, numberOvers);
        }

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

        public override bool Equals(object obj)
        {
            return Equals(obj as InningsScore);
        }

        public override int GetHashCode()
        {
            int hashcode = 11 + Runs.GetHashCode();
            return hashcode * Wickets.GetHashCode();
        }
    }
}
