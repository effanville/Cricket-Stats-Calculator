﻿using Cricket.Match;
using System;
using StringFunctions;

namespace Cricket.Statistics
{
    public class BestBatting : IComparable
    {
        public int Runs;
        public Wicket HowOut;
        public string Opposition;
        public DateTime Date;

        public int CompareTo(object obj)
        {
            if (obj is BestBatting otherBest)
            {
                if (Runs.Equals(otherBest.Runs))
                {
                    if (HowOut == Wicket.NotOut && otherBest.HowOut == Wicket.NotOut)
                    {
                        return 0;
                    }

                    if (HowOut == Wicket.NotOut)
                    {
                        return 1;
                    }

                    if (otherBest.HowOut == Wicket.NotOut)
                    {
                        return -1;
                    }
                }

                return Runs.CompareTo(otherBest.Runs);
            }

            return 0;
        }

        public override string ToString()
        {
            var outname = HowOut == Wicket.NotOut ? " not out" : "";

            if (string.IsNullOrEmpty(Opposition))
            {
                return Runs + outname + " vs unknown opposition";
            }

            return Runs + outname + " vs " + Opposition + " on " + Date.ToUkDateString();
        }
    }
}
