using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cricket;

namespace CSCv01.Cricket_Classes
{
    public class Cricket_Team
    {
        private List<Cricket_Player> FPlayers
        {
            get;
            set;
        }

        public Cricket_Team()
        {

        }
        public void Add_Player(Cricket_Player newPlayer)
        {
            if (FPlayers != null)
            {
                FPlayers.Add(newPlayer);
            }
        }
    }
}
