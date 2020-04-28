﻿using Cricket.Interfaces;
using Cricket.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using Validation;

namespace Cricket.Match
{
    public class CricketMatch : ICricketMatch, IValidity
    {
        public override string ToString()
        {
            var date = MatchData.Date;
            return date.Year + "/" + date.Month + "/"+ date.Day + " " + MatchData.Opposition;
        }

        public bool SameMatch(DateTime date, string opposition)
        {
            if(MatchData.Date.Equals(date) )
            {
                if (string.IsNullOrEmpty(MatchData.Opposition))
                {
                    if (string.IsNullOrEmpty(opposition))
                    {
                        return true;
                    }

                    return false;
                }

                if (MatchData.Opposition.Equals(opposition))
                {
                    return true;
                }
            }

            return false;
        }

        private MatchInfo fMatchData = new MatchInfo();

        public MatchInfo MatchData
        { 
            get { return fMatchData; }
            set { fMatchData = value; }
        }

        private ResultType fResult;
        public ResultType Result
        {
            get { return fResult; }
            set { fResult = value; }
        }

        /// <summary>
        /// list of players that play in this match
        /// </summary>
        private List<PlayerName> fPlayerNames = new List<PlayerName>();
        public List<PlayerName> PlayerNames
        {
            get { return fPlayerNames; }
            set { fPlayerNames = value; }
        }

        private BattingInnings fBatting = new BattingInnings();
        public BattingInnings Batting
        {
            get { return fBatting; }
            set { fBatting = value; }
        }

        private BowlingInnings fBowling = new BowlingInnings();
        public BowlingInnings Bowling
        {
            get { return fBowling; }
            set { fBowling = value; }
        }

        private Fielding fFieldingStats = new Fielding();
        public Fielding FieldingStats
        {
            get { return fFieldingStats; }
            set { fFieldingStats = value; }
        }

        private PlayerName fMoM;
        public PlayerName ManOfMatch
        {
            get { return fMoM; }
            set { fMoM = value; }
        }

        /// <summary>
        /// default generator of match
        /// </summary>
        /// <param name="oppos">Name of the opposition</param>
        /// <param name="Players">List of players that play</param>
        public CricketMatch(string oppos, List<PlayerName> playerNames)
        {
            MatchData = new MatchInfo()
            {
                Opposition = oppos,
            };

            PlayerNames = playerNames;
            Batting = new BattingInnings(PlayerNames);
            Bowling = new BowlingInnings(PlayerNames);
            FieldingStats = new Fielding(PlayerNames);
        }

        public CricketMatch(string oppos, DateTime date1, string place, ResultType result, MatchType TypeofMatch, PlayerName moM, List<PlayerName> playerNames)
        {
            MatchData = new MatchInfo()
            {
                Opposition = oppos,
                Date = date1,
                Type = TypeofMatch,
                Place = place
            };

            PlayerNames = playerNames;
            Result = result;
            ManOfMatch = moM;
            Batting = new BattingInnings(PlayerNames);
            Bowling = new BowlingInnings(PlayerNames);
            FieldingStats = new Fielding(PlayerNames);
        }

        public CricketMatch(MatchInfo info)
        {
            MatchData = info;
        }

        public CricketMatch()
        {
        }

        /// <summary>
        /// Query to determine whether a player played or not.
        /// </summary>
        public bool PlayNotPlay(PlayerName person)
        {
            return PlayerNames.Contains(person);
        }


        public bool EditInfo(string opposition, DateTime date, string place, MatchType typeOfMatch, ResultType result)
        {
            return EditMatchInfo(opposition, date, place, typeOfMatch) & EditResult(result); 
        }

        public bool EditMatchInfo(string opposition, DateTime date, string place, MatchType typeOfMatch)
        {
            MatchData.Opposition = opposition;
            MatchData.Date = date;
            MatchData.Place = place;
            MatchData.Type = typeOfMatch;
            return true;
        }

        public bool EditResult(ResultType result) 
        {
            Result = result;
            return true;
        }

        public bool EditManOfMatch(PlayerName player) 
        { 
            ManOfMatch = player;
            return true;
        }

        public bool AddPlayer(PlayerName player)
        {
            if (!PlayNotPlay(player))
            {
                PlayerNames.Add(player);
                return true;
            }

            return false;
        }

        public void SetBatting(BattingInnings innings)
        {
            foreach (var entry in innings.BattingInfo)
            {
                if (!Batting.PlayerListed(entry.Name))
                {
                    AddBattingEntry(entry.Name, entry.MethodOut, entry.RunsScored, entry.Fielder, entry.Bowler);
                }
                else
                {
                    EditBattingEntry(entry.Name, entry.MethodOut, entry.RunsScored, entry.Fielder, entry.Bowler);
                }
            }
        }

        public bool AddBattingEntry(PlayerName player, Wicket howOut, int runs, PlayerName fielder = null, PlayerName bowler = null)
        {
            if (!Batting.PlayerListed(player))
            {
                if (!PlayNotPlay(player))
                {
                    PlayerNames.Add(player);
                }
                Batting.AddPlayer(player);
                Batting.SetScores(player, howOut, runs, fielder, bowler);
                return true;
            }

            return false;
        }

        public bool EditBattingEntry(PlayerName player, Wicket howOut, int runs, PlayerName fielder = null, PlayerName bowler = null) 
        {
            if (Batting.PlayerListed(player))
            {
                Batting.SetScores(player, howOut, runs, fielder, bowler);
                return true;
            }

            return false;
        }

        public bool DeleteBattingEntry(PlayerName player) 
        {
            if (Batting.PlayerListed(player))
            {
                if (!Bowling.PlayerListed(player) && !FieldingStats.PlayerListed(player))
                {
                    PlayerNames.Remove(player);
                }
                return Batting.Remove(player);
            }

            return false;
        }

        public void SetBowling(BowlingInnings innings)
        {
            foreach (var entry in innings.BowlingInfo)
            {
                if (!Bowling.PlayerListed(entry.Name))
                {
                    AddBowlingEntry(entry.Name, entry.OversBowled, entry.Maidens, entry.RunsConceded, entry.Wickets);
                }
                else 
                {
                    EditBowlingEntry(entry.Name, entry.OversBowled, entry.Maidens, entry.RunsConceded, entry.Wickets);
                }
            }
        }

        public bool AddBowlingEntry(PlayerName player, int overs, int maidens, int runsConceded, int wickets) 
        {
            if (!Bowling.PlayerListed(player))
            {
                if (!PlayNotPlay(player))
                {
                    PlayerNames.Add(player);
                }

                Bowling.AddPlayer(player);
                Bowling.SetScores(player, overs, maidens, runsConceded, wickets);
                return true;
            }

            return false;
        }

        public bool EditBowlingEntry(PlayerName player, int overs, int maidens, int runsConceded, int wickets)
        {
            if (Bowling.PlayerListed(player))
            {
                Bowling.SetScores(player, overs, maidens, runsConceded, wickets);
                return true;
            }

            return false;
        }

        public bool DeleteBowlingEntry(PlayerName player)
        {
            if (Bowling.PlayerListed(player))
            {
                if (!Batting.PlayerListed(player) && !FieldingStats.PlayerListed(player))
                {
                    PlayerNames.Remove(player);
                }
                return Bowling.Remove(player);
            }
            return false;
        }

        public void SetFielding(Fielding innings)
        {
            foreach (var entry in innings.FieldingInfo)
            {
                if (!FieldingStats.PlayerListed(entry.Name))
                {
                    AddFieldingEntry(entry.Name, entry.Catches, entry.RunOuts, entry.keeperFielding.Stumpings, entry.keeperFielding.Catches);
                }
                else
                {
                    EditFieldingEntry(entry.Name, entry.Catches, entry.RunOuts, entry.keeperFielding.Stumpings, entry.keeperFielding.Catches);
                }
            }
        }

        public bool AddFieldingEntry(PlayerName player, int catches, int runOuts, int stumpings, int keeperCatches) 
        {
            if (!FieldingStats.PlayerListed(player))
            {
                if (!PlayerNames.Contains(player))
                {
                    PlayerNames.Add(player);
                }
                FieldingStats.AddPlayer(player);
                FieldingStats.SetFielding(player, catches, runOuts, stumpings, keeperCatches);
                return true;
            }

            return false;
        }

        public bool EditFieldingEntry(PlayerName player, int catches, int runOuts, int stumpings, int keeperCatches) 
        {
            if (FieldingStats.PlayerListed(player))
            {
                FieldingStats.SetFielding(player, catches, runOuts, stumpings, keeperCatches);
                return true;
            }

            return false;
        }

        public bool DeleteFieldingEntry(PlayerName player)
        {
            if (FieldingStats.PlayerListed(player))
            {
                if (!Batting.PlayerListed(player) && !Bowling.PlayerListed(player))
                {
                    PlayerNames.Remove(player);
                }
                return FieldingStats.Remove(player);
            }

            return false;
        }

        public BattingEntry GetBatting(PlayerName player)
        {
            if (Batting.PlayerListed(player))
            {
                return Batting.BattingInfo.First(batsman => batsman.Name.Equals(player));
            }

            return null;
        }

        public BowlingEntry GetBowling(PlayerName player)
        {
            if (Bowling.PlayerListed(player))
            {
                return Bowling.BowlingInfo.First(bowler => bowler.Name.Equals(player));
            }

            return null;
        }

        public FieldingEntry GetFielding(PlayerName player)
        {
            if (FieldingStats.PlayerListed(player))
            {
                return FieldingStats.FieldingInfo.First(fielder => fielder.Name.Equals(player));
            }

            return null;
        }

        public bool Validate()
        {
            return !Validation().Any(validation => !validation.IsValid);
        }

        public List<ValidationResult> Validation()
        {
            var results = new List<ValidationResult>();
            results.AddRange(MatchData.Validation());
            results.AddRange(Batting.Validation());
            results.AddRange(Bowling.Validation());
            results.AddRange(FieldingStats.Validation());

            return results;
        }
    }

    public enum ResultType
    {
        Loss = 0,
        Draw = 1,
        Tie = 2,
        Win = 3,
    }

    public enum MatchType
    {
        League = 0,
        Friendly = 1,
        Evening = 2,
    }
}
