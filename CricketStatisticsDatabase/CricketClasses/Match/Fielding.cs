using Cricket.Player;
using System.Collections.Generic;
using System.Linq;

namespace Cricket.Match
{
    public class Fielding
    {
        /// <summary>
        /// list of players that play in this innings
        /// </summary>
        private List<FieldingEntry> fFielding = new List<FieldingEntry>();
        public List<FieldingEntry> FieldingInfo
        {
            get { return fFielding; }
            set{ fFielding = value; }
        }

        public bool SetFielding(PlayerName player, int catches, int runOuts, int stumpings, int keeperCatches)
        {
            var result = FieldingInfo.Find(entry => entry.Name == player);
            if (result != null)
            {
                result.SetScores(catches, runOuts, stumpings, keeperCatches);
                return true;
            }

            return false;
        }

        public void AddPlayer(PlayerName player)
        {
            FieldingInfo.Add(new FieldingEntry(player));
        }

        public bool PlayerListed(PlayerName player)
        {
            return FieldingInfo.Any(card => card.Name.Equals(player));
        }

        public bool Remove(PlayerName player)
        {
            int removed = FieldingInfo.RemoveAll(card => card.Name.Equals(player));
            return removed == 1;
        }

        public Fielding(List<PlayerName> playerNames)
        {
            foreach (var name in playerNames)
            {
                FieldingInfo.Add(new FieldingEntry(name));
            }
        }
        public Fielding()
        {
        }
    }
}
