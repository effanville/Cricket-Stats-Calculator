﻿namespace Cricket.Match
{
    public class InningsScore
    {
        public override string ToString()
        {
            return Runs + " - " + Wickets;
        }
        public int Runs
        { get; private set; }

        public int Wickets
        { get; private set; }

        public InningsScore()
        { }

        public InningsScore(int runs, int wickets)
        {
            Runs = runs;
            Wickets = wickets;
        }
    }
}
