using Cricket.Interfaces;

namespace Cricket.Player
{
    /// <summary>
    /// Class containing all information about a player of cricket
    /// </summary>
    public class CricketPlayer : ICricketPlayer
    {
        public override string ToString()
        {
            return Name.ToString();
        }
        public CricketPlayer()
        {
        }

        public CricketPlayer(PlayerName name)
        {
            Name = name;
        }

        public CricketPlayer(string surname, string forename)
        {
            Name = new PlayerName(surname, forename);
        }


        public void EditName(string surname, string forename)
        {
            var newNames = new PlayerName(surname, forename);
            if (!Name.Equals(newNames))
            {
                Name = newNames;
            }
        }

        public PlayerName Name
        {
            get;
            set;
        }
    }
}