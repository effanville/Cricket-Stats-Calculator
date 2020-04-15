using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CricketStatsCalc
{
    public class MatchViewData
    {
        public string OppositionName;

        public DateTime MatchDate;

        private string fToDisplay;

        public string ToDisplay
        {
            get { return fToDisplay; }
            set
            {
                fToDisplay = value;
            }
        }

        public MatchViewData(string input1, DateTime input2)
        {
            OppositionName = input1;

            MatchDate = input2;
            fToDisplay = OppositionName + " " + MatchDate.ToShortDateString();
        }
    }

    public class MatchDateCompare : IComparer<MatchViewData>
    {
        public int Compare(MatchViewData x, MatchViewData y)
        {
            return DateTime.Compare(x.MatchDate, y.MatchDate);
        }
    }
}
