﻿using System;
using CricketStructures.Player;

namespace CricketStructures.Match.Innings
{
    public sealed class Partnership : IComparable<Partnership>
    {
        public int Runs
        {
            get;
            set;
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

        public MatchInfo MatchData
        {
            get;
            set;
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

        public bool SamePair(PlayerName playerOne, PlayerName playerTwo)
        {
            if (PlayerOne.Equals(playerOne) && PlayerTwo.Equals(playerTwo))
            {
                return true;
            }
            if (PlayerTwo.Equals(playerOne) && PlayerOne.Equals(playerTwo))
            {
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return Wicket + "," + PlayerOne.ToString() + "," + PlayerTwo.ToString() + "," + Runs + "," + MatchData?.ToString();
        }

        public Partnership(PlayerName playerOne, PlayerName playerTwo, MatchInfo matchData = null)
        {
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;
            MatchData = matchData;
        }

        public Partnership(PlayerName playerOne, PlayerName playerTwo, int wicket, int runs)
        {
            PlayerOne = playerOne;
            PlayerTwo = playerTwo;

            SetScores(wicket, runs);
        }

        public Partnership()
        {
        }

        public void SetScores(int wicket, int runs)
        {
            Wicket = wicket;
            Runs = runs;
        }

        public int CompareTo(object obj)
        {
            if (obj is Partnership other)
            {
                return CompareTo(other);
            }

            return 0;
        }

        public int CompareTo(Partnership other)
        {
            if (Wicket.Equals(other.Wicket))
            {
                // this is only sensible to be this way around
                // even if more null checks are needed elsewhere
                return Runs.CompareTo(other.Runs);
            }

            // partnerships for different wickets are not comparable.
            return 0;
        }
    }
}