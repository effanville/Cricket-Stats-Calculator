using Cricket.Player;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cricket.Match
{
    public class WicketKeeperStats
    {
        public PlayerName Name
        {
            get;
            private set;
        }

        public int Stumpings
        { get; set; }

        public int Catches
        { get; set; }

        public void SetScores( int stumpings, int catches)
        {
            Stumpings = stumpings;
            Catches = catches;
        }

        public WicketKeeperStats(PlayerName name)
        {
            Name = name;
        }
    }
}
