﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cricket
{
    public class Cricket_Match
    {

        private string fOpposition;
        public string FOpposition
        {
            get
            {
                return fOpposition;
            }
            set
            {
                fOpposition = value;
            }
        }

        private string date;
        public string Date
        { get { return date; }
            set { date = value; }
        }

        private string fPlace;
        public string FPlace
        {
            get { return fPlace; }
            set { fPlace = value; }
        }

        private ResultType fResult;
        public ResultType FResult
        {
            get { return fResult; }
            set { fResult = value; }
        }

        /// <summary>
        /// list of players that play in this match
        /// </summary>
        private List<Cricket_Player> fPlayers;
        public List<Cricket_Player> FPlayers
        {
            get { return fPlayers; }
            set { fPlayers = value; }
        }

        private Batting_Innings fBatting;
        public Batting_Innings FBatting
        {
            get { return fBatting; }
            set { fBatting = value; }
        }
        private BowlingInnings fBowling;
        public BowlingInnings FBowling
        {
            get { return fBowling; }
            set { fBowling = value; }
        }

        private Fielding fFieldingStats;
        public Fielding FFieldingStats
        {
            get { return fFieldingStats; }
            set { fFieldingStats = value; }
        }

        private Cricket_Player fMoM;
        public Cricket_Player FMoM
        {
            get { return fMoM; }
            set { fMoM = value; }
        }

        /// <summary>
        /// default generator of match
        /// </summary>
        /// <param name="oppos">Name of the opposition</param>
        /// <param name="Players">List of players that play</param>
        public Cricket_Match(string oppos, List<Cricket_Player> Players)
        {
            fOpposition = oppos;

            fPlayers = Players;

            fBatting = new Batting_Innings(Players);

            fBowling = new BowlingInnings(Players);

            fFieldingStats = new Fielding(Players);
        }

        public Cricket_Match(string oppos, string date1, string place, ResultType Result, List<Cricket_Player> Players)
        {
            fOpposition = oppos;

            fPlayers = Players;

            date = date1;

            fPlace = place;

            fResult = Result;

            fBatting = new Batting_Innings(Players);

            fBowling = new BowlingInnings(Players);

            fFieldingStats = new Fielding(Players);
        }

        public Cricket_Match()
        {
            string dummy = "";
            fOpposition = dummy;

            fPlayers = new List<Cricket_Player>();

            var fBatting = new Batting_Innings();

            var fBowling = new BowlingInnings();

            var fFieldingStats = new Fielding();
        }

        /// <summary>
        /// Query to determine whether 
        /// </summary>
        /// <param name="person">Person who querying whether they played or not</param>
        /// <param name="Player_index">Return of the index of the player if he plays (if not is -1)</param>
        /// <returns>True if play, false if doesn't play</returns>
        public bool PlayNotPlay(Cricket_Player person, out int Player_index)
        {
            bool played = false;
            int i = 0;
            for (i = 0; i < fPlayers.Count; ++i)
            {
                if (fPlayers[i] == person)
                {
                    played = true;
                    break;
                }
            }
            if (played == false)
            {
                Player_index = -1;
                return false;
            }

            Player_index = i;
            return true;
        }
    }

    public enum ResultType
    {
        Loss = 0,
        Draw = 1,
        Tie = 2,
        Win = 3,
    }
}