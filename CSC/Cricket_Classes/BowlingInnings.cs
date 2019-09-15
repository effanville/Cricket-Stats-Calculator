using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cricket
{
    public class BowlingInnings
    {
        /// <summary>
        /// list of players that play in this innings
        /// </summary>
        private List<string> fPlayerNames;
        private List<string> FPlayerNames
        {
            get
            {
                return fPlayerNames;
            }
            set
            {
                fPlayerNames = value;
            }
        }

        private List<int> fOvers_Bowled;
        public List<int> FOvers_Bowled
        {
            get { return fOvers_Bowled; }
            set { fOvers_Bowled = value; }
        }

        private List<int> fMaidens;
        public List<int> FMaidens
        {
            get { return fMaidens; }
            set { fMaidens = value; }
        }

        private List<int> fRuns_Conceded;
        public List<int> FRuncs_Conceded
        {
            get { return fRuns_Conceded; }
            set { fRuns_Conceded = value; }
        }

        private List<int> fWickets;
        public List<int> FWickets
        {
            get { return fWickets; }
            set { fWickets = value; }
        }


        private int byesLegByes;
        public int ByesLegByes
        {
            get { return byesLegByes; }
            set { byesLegByes = value; }
        }

        public void Add_Data(List<int> Overs, List<int> Maidens, List<int> Runs, List<int> Wickets, int BLB)
        {
            fOvers_Bowled = Overs;
            fMaidens = Maidens;
            fRuns_Conceded = Runs;
            fWickets = Wickets;
            byesLegByes = BLB;
        }

        public BowlingInnings(List<string> PlayerNames, List<int> Overs, List<int> Maidens, List<int> Runs, List<int> Wickets)
        {
            fPlayerNames = PlayerNames;
            fOvers_Bowled = Overs;
            fMaidens = Maidens;
            fRuns_Conceded = Runs;
            fWickets = Wickets;
        }

        public BowlingInnings(List<string> PlayerNames)
        {
            fPlayerNames = PlayerNames;
        }

        /// <summary>
        /// Standard default generator
        /// </summary>
        public BowlingInnings()
        {
        }
    }
}
