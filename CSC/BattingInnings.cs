using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cricket
{
    public class Batting_Innings
    {
        /// <summary>
        /// list of players that play in this innings
        /// </summary>
        private List<Cricket_Player> fPlayers;
        public List<Cricket_Player> FPlayers
        {
            get
            {
                return fPlayers;
            }
            set
            {
                fPlayers = value;
            }
        }

        /// <summary>
        /// information about how many runs scored by each player
        /// enumeration of this list is the same as the enumeartion of the fPlayers list
        /// </summary>
        private List<int> fRuns_Scored;
        public List<int> FRuns_Scored
        {
            get
            {
                return fRuns_Scored;
            }
            set
            {
                fRuns_Scored = value;
            }
        }

        private List<OutType> fMethod_Out;
        public List<OutType> FMethod_Out
        { get { return fMethod_Out; }
            set { fMethod_Out = value; }
        }

        public int fExtras;

        public void Set_Data(List<int> Runs_Scored, List<OutType> Method_Out, int Extras)
        {
            fRuns_Scored = Runs_Scored;
            fMethod_Out = Method_Out;
            fExtras = Extras;
        }

        /// <summary>
        /// provides generator for a innings for batting side
        /// takes in player names from the overlying match class
        /// inputs for the runs scored and how out are dealt with either from a WPF or from command line
        /// </summary>
        public Batting_Innings(List<Cricket_Player> Players, List<int> Runs_Scored,   List<OutType> Method_Out,int Extras)
        {
            fPlayers = Players;
            fRuns_Scored = Runs_Scored;
            fMethod_Out = Method_Out;
            fExtras = Extras;
        }

        public Batting_Innings(List<Cricket_Player> Players)
        {
            fPlayers = Players;
            fRuns_Scored = new List<int>();
            fMethod_Out = new List<OutType>();
            fExtras = 0;
            // now run some pop-out to allow user to add the rest of the data.
        }
        public Batting_Innings()
        {
        }
    }

    /// <summary>
    /// enumeration listing type of wicket
    /// </summary>
    public enum OutType
    {
        NotOut = 0,
        Bowled = 1,
        Caught = 2,
        LBW = 3,
        Stumped = 4,
        RunOut = 5,
        HitWicket = 6,
        DidNotBat =7
    }
}
