using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cricket
{
    public class Fielding
    {
        /// <summary>
        /// list of players that play in this innings
        /// </summary>
        private List<Cricket_Player> fPlayers;
        public List<Cricket_Player> FPlayers
        {
            get {return fPlayers;}
            set{fPlayers = value;}
        }

        private List<int> fCatches;
        public List<int> FCatches
        {
            get { return fCatches; }
            set { fCatches=value; }
        }

        private List<int> fRunOuts;
        public List<int> FRunOuts
        {
            get { return fRunOuts; }
            set { fRunOuts = value; }
        }

        private List<int> fStumpings;
        public List<int> FStumpings
        {
            get { return fStumpings; }
            set { fStumpings = value; }
        }

        private List<int> fCatchesKeeper;
        public List<int> FCatchesKeeper
        {
            get { return fCatchesKeeper; }
            set { fCatchesKeeper = value; }
        }

        public void Add_Data(List<int> catches, List<int> ro, List<int> st, List<int> keepcat)
        {
            fCatches = catches;
            fRunOuts = ro;
            fStumpings = st;
            fCatchesKeeper = keepcat;
        }

        public Fielding(List<Cricket_Player> Players)
        {
            fPlayers = Players;
        }
        public Fielding()
        {
            fPlayers = new List < Cricket_Player >();
        }
    }
}
