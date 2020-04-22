using Cricket.Player;
using System.Collections.Generic;
using System.Linq;

namespace Cricket.Match
{
    public class BowlingInnings
    {
        /// <summary>
        /// list of players that play in this innings
        /// </summary>
        private List<BowlingEntry> fBowlingInfo = new List<BowlingEntry>();
        public List<BowlingEntry> BowlingInfo
        {
            get
            {
                return fBowlingInfo;
            }
            set
            {
                fBowlingInfo = value;
            }
        }

        private int byesLegByes;
        public int ByesLegByes
        {
            get { return byesLegByes; }
            set { byesLegByes = value; }
        }

        public bool SetScores(PlayerName player, int overs, int maidens, int runsConceded, int wickets)
        {
            var result = BowlingInfo.Find(entry => entry.Name == player);
            if (result != null)
            {
                result.SetBowling(overs, maidens, runsConceded, wickets);
                return true;
            }

            return false;
        }
        public void AddPlayer(PlayerName player)
        {
            BowlingInfo.Add(new BowlingEntry(player));
        }
        public bool PlayerListed(PlayerName player)
        {
            return BowlingInfo.Any(card => card.Name.Equals(player));
        }

        public bool Remove(PlayerName player)
        {
            int removed = BowlingInfo.RemoveAll(card => card.Name.Equals(player));
            return removed == 1;
        }

        public InningsScore Score()
        {
            int runs = ByesLegByes;
            int wickets = 0;
            foreach (var bowler in BowlingInfo)
            {
                wickets += bowler.Wickets;
                runs += bowler.RunsConceded;
            }

            return new InningsScore(runs, wickets);
        }

        public BowlingInnings(List<PlayerName> playerNames)
        {
            foreach (var name in playerNames)
            {
                BowlingInfo.Add(new BowlingEntry(name));
            }
        }

        public BowlingInnings Copy()
        {
            return new BowlingInnings()
            {
                ByesLegByes = this.ByesLegByes,
                BowlingInfo = new List<BowlingEntry>(this.BowlingInfo)
            };
        }

        /// <summary>
        /// Standard default generator
        /// </summary>
        public BowlingInnings()
        {
        }
    }
}
