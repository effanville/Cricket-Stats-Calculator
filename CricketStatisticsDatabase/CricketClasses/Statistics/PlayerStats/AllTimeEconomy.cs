using Cricket.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cricket.Statistics.PlayerStats
{
    public class AllTimeEconomy
    {
        public PlayerName Name
        {
            get;
            set;
        }

        public int Wickets
        {
            get;
            set;
        }

        public double Economy
        {
            get;
            set;
        }

        public AllTimeEconomy()
        {
        }

        public AllTimeEconomy(PlayerName name, int wickets, double average)
        {
            Name = name;
            Wickets = wickets;
            Economy = average;
        }

        public string ToCSVLine()
        {
            return Name.ToString() + "," + Wickets + "," + Economy;
        }
    }
}
