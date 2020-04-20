using StringFunctions;
using System;

namespace Cricket.Statistics
{
    public class BestBowling : IComparable
    {
        public double Wickets;
        public int Runs;
        public string Opposition;
        public DateTime Date;

        public int CompareTo(object obj)
        {
            if (obj is BestBowling otherBowling)
            {
                if (!Wickets.Equals(otherBowling.Wickets))
                {
                    return Wickets.CompareTo(otherBowling.Wickets);
                }
                if (!Runs.Equals(otherBowling.Runs))
                {
                    return Runs.CompareTo(otherBowling.Runs);
                }
            }

            return 0;
        }

        public override string ToString()
        {
            return Wickets + "-" + Runs + " vs " + Opposition + " on " + Date.ToUkDateString();
        }
    }
}
