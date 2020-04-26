using Cricket.Player;
using System.Collections.Generic;
using System.Linq;

namespace Cricket.Match
{
    public class BattingInnings
    {
        /// <summary>
        /// list of players that play in this innings
        /// </summary>
        private List<BattingEntry> fBattingInfo = new List<BattingEntry>();
        public List<BattingEntry> BattingInfo
        {
            get
            {
                return fBattingInfo;
            }
            set
            {
                fBattingInfo = value;
            }
        }
        private int fExtras;

        public int Extras
        { 
            get { return fExtras; }
            set { fExtras = value; }
        }

        public bool SetScores(PlayerName player, BattingWicketLossType howOut, int runs, PlayerName fielder = null, PlayerName bowler = null)
        {
            var result = BattingInfo.Find(entry => entry.Name.Equals(player));
            if (result != null)
            {
                result.SetScores(howOut, runs, fielder, bowler);
                return true;
            }

            return false;
        }

        public void AddPlayer(PlayerName player)
        {
            BattingInfo.Add(new BattingEntry(player));
        }

        public bool PlayerListed(PlayerName player)
        {
            return BattingInfo.Any(card => card.Name.Equals(player));
        }

        public bool Remove(PlayerName player)
        {
            int removed = BattingInfo.RemoveAll(card => card.Name.Equals(player));
            return removed == 1;
        }

        public InningsScore Score()
        {
            int runs = Extras;
            int wickets = 0;
            foreach (var batsman in BattingInfo)
            {
                wickets += batsman.Out() ? 1 : 0;
                runs += batsman.RunsScored;
            }

            return new InningsScore(runs, wickets);
        }
        public BattingInnings Copy()
        {
            return new BattingInnings()
            {
                Extras = this.Extras,
                BattingInfo = new List<BattingEntry>(this.BattingInfo)
            };
        }

        public BattingInnings(List<PlayerName> playerNames)
        {
            foreach (var name in playerNames)
            {
                BattingInfo.Add(new BattingEntry(name));
            }

            Extras = 0;
        }

        public BattingInnings()
        {
        }
    }
}
