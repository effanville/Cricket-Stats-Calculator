using StringFunctions;
using System;

namespace Cricket.Statistics
{
    public class BestBowling : IComparable
    {
        public int Wickets;
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
                    // inverted as the lower the runs the better.
                    return otherBowling.Runs.CompareTo(Runs);
                }
            }

            return 0;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Opposition))
            {
                return Wickets + "-" + Runs + " vs unknown opposition";
            }
            return Wickets + "-" + Runs + " vs " + Opposition + " on " + Date.ToUkDateString();
        }
    }
}
