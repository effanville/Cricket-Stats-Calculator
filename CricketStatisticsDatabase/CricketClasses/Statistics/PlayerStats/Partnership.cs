using Cricket.Player;
using System;

namespace Cricket.Statistics
{
    public sealed class Partnership : IComparable
    {
        public int CompareTo(object obj)
        {
            if (obj is Partnership ship)
            {
                if (PlayerOne.Equals(ship.PlayerOne) && PlayerTwo.Equals(ship.PlayerTwo) && Wicket.Equals(ship.Wicket))
                {
                    return Runs.CompareTo(ship.Runs);
                }
            }

            return 0;
        }

        public bool ContainsPlayer(PlayerName player)
        {
            if (PlayerOne.Equals(player))
            {
                return true;
            }
            if (PlayerTwo.Equals(player))
            {
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return Wicket + "," + PlayerOne.ToString() + "," + PlayerTwo.ToString() + "," + Runs;
        }

        public PlayerName PlayerOne
        {
            get;
            set;
        }

        public PlayerName PlayerTwo
        {
            get;
            set;
        }

        public int Wicket
        {
            get;
            set;
        }

        public int Runs
        {
            get;
            set;
        }

        public Partnership(PlayerName playerOne, PlayerName playerTwo)
        {
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;
        }

        public Partnership(PlayerName playerOne, PlayerName playerTwo, int wicket, int runs)
        {
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;
            SetScores(wicket, runs);
        }

        public Partnership()
        { }

        public void SetScores(int wicket, int runs)
        {
            Wicket = wicket;
            Runs = runs;
        }
    }
}
