﻿using System;
using System.Collections.Generic;
using System.Linq;
using Cricket.Interfaces;
using Cricket.Player;
using Cricket.Statistics;
using StructureCommon.Extensions;
using StructureCommon.Validation;

namespace Cricket.Match
{
    public sealed class CricketMatch : ICricketMatch, IValidity
    {
        public event EventHandler PlayerAdded;

        private void OnPlayerAdded(PlayerName newPlayerName)
        {
            PlayerAdded?.Invoke(newPlayerName, new EventArgs());
        }

        public override string ToString()
        {
            return MatchData.ToString();
        }

        public void EditPlayerName(PlayerName oldName, PlayerName newName)
        {
            foreach (var entry in Batting.BattingInfo)
            {
                if (entry.Name.Equals(oldName))
                {
                    entry.Name = newName;
                }
            }
            foreach (var entry in Bowling.BowlingInfo)
            {
                if (entry.Name.Equals(oldName))
                {
                    entry.Name = newName;
                }
            }
            foreach (var entry in FieldingStats.FieldingInfo)
            {
                if (entry.Name.Equals(oldName))
                {
                    entry.Name = newName;
                    entry.keeperFielding.Name = newName;
                }
            }
        }

        public bool SameMatch(DateTime date, string opposition)
        {
            if (MatchData.Date.Equals(date))
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

        public MatchInfo MatchData
        {
            get;
            set;
        }

        public ResultType Result
        {
            get;
            set;
        }

        public TeamInnings BattingFirstOrSecond
        {
            get;
            set;
        }

        /// <inheritdoc/>
        public List<PlayerName> Players()
        {
            var players = new HashSet<PlayerName>();
            players.UnionWith(Batting.Players());
            players.UnionWith(Bowling.Players());
            players.UnionWith(FieldingStats.Players());
            return players.ToList();
        }

        public BattingInnings Batting
        {
            get;
            set;
        } = new BattingInnings();

        public BowlingInnings Bowling
        {
            get;
            set;
        } = new BowlingInnings();

        public Fielding FieldingStats
        {
            get;
            set;
        } = new Fielding();

        public PlayerName ManOfMatch
        {
            get;
            set;
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

            Batting = new BattingInnings(MatchData, playerNames);
            Bowling = new BowlingInnings(MatchData, playerNames);
            FieldingStats = new Fielding(MatchData, playerNames);
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
            return Players().Contains(person);
        }

        public bool EditInfo(string opposition, DateTime date, string place, Location homeOrAway, MatchType typeOfMatch, ResultType result, TeamInnings firstOrSecond)
        {
            return EditMatchInfo(opposition, date, place, homeOrAway, typeOfMatch) & EditResult(result) & EditInningsPlace(firstOrSecond);
        }

        public bool EditMatchInfo(string opposition, DateTime date, string place, Location homeOrAway, MatchType typeOfMatch)
        {
            MatchData.Opposition = opposition;
            MatchData.Date = date;
            MatchData.Place = place;
            MatchData.HomeOrAway = homeOrAway;
            MatchData.Type = typeOfMatch;
            Bowling.MatchData = MatchData;
            Batting.MatchData = MatchData;
            FieldingStats.MatchData = MatchData;
            return true;
        }

        public bool EditResult(ResultType result)
        {
            Result = result;
            return true;
        }

        public bool EditInningsPlace(TeamInnings result)
        {
            BattingFirstOrSecond = result;
            return true;
        }

        public bool EditManOfMatch(PlayerName player)
        {
            ManOfMatch = player;
            return true;
        }

        public void SetBatting(BattingInnings innings)
        {
            foreach (BattingEntry entry in innings.BattingInfo)
            {
                if (!Batting.PlayerListed(entry.Name))
                {
                    OnPlayerAdded(entry.Name);
                    AddBattingEntry(entry.Name, entry.MethodOut, entry.RunsScored, entry.Order, entry.WicketFellAt, entry.TeamScoreAtWicket, entry.Fielder, entry.Bowler);
                }
                else
                {
                    EditBattingEntry(entry.Name, entry.MethodOut, entry.RunsScored, entry.Order, entry.WicketFellAt, entry.TeamScoreAtWicket, entry.Fielder, entry.Bowler);
                }
            }

            Batting.Extras = innings.Extras;
        }

        public bool AddBattingEntry(PlayerName player, Wicket howOut, int runs, int order, int wicketFellAt, int teamScoreAtWicket, PlayerName fielder = null, PlayerName bowler = null)
        {
            if (!Batting.PlayerListed(player))
            {
                Batting.AddPlayer(player);
                Batting.SetScores(player, howOut, runs, order, wicketFellAt, teamScoreAtWicket, fielder, bowler);
                return true;
            }

            return false;
        }

        public bool EditBattingEntry(PlayerName player, Wicket howOut, int runs, int order, int wicketFellAt, int teamScoreAtWicket, PlayerName fielder = null, PlayerName bowler = null)
        {
            if (Batting.PlayerListed(player))
            {
                Batting.SetScores(player, howOut, runs, order, wicketFellAt, teamScoreAtWicket, fielder, bowler);
                return true;
            }

            return false;
        }

        public bool DeleteBattingEntry(PlayerName player)
        {
            if (Batting.PlayerListed(player))
            {
                return Batting.Remove(player);
            }

            return false;
        }

        public void SetBowling(BowlingInnings innings)
        {
            foreach (BowlingEntry entry in innings.BowlingInfo)
            {
                if (!Bowling.PlayerListed(entry.Name))
                {
                    OnPlayerAdded(entry.Name);
                    AddBowlingEntry(entry.Name, entry.OversBowled, entry.Maidens, entry.RunsConceded, entry.Wickets);
                }
                else
                {
                    EditBowlingEntry(entry.Name, entry.OversBowled, entry.Maidens, entry.RunsConceded, entry.Wickets);
                }
            }

            Bowling.ByesLegByes = innings.ByesLegByes;
        }

        public bool AddBowlingEntry(PlayerName player, double overs, int maidens, int runsConceded, int wickets)
        {
            if (!Bowling.PlayerListed(player))
            {
                Bowling.AddPlayer(player);
                Bowling.SetScores(player, overs, maidens, runsConceded, wickets);
                return true;
            }

            return false;
        }

        public bool EditBowlingEntry(PlayerName player, double overs, int maidens, int runsConceded, int wickets)
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
                return Bowling.Remove(player);
            }
            return false;
        }

        public void SetFielding(Fielding innings)
        {
            foreach (FieldingEntry entry in innings.FieldingInfo)
            {
                if (!FieldingStats.PlayerListed(entry.Name))
                {
                    OnPlayerAdded(entry.Name);
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
            List<ValidationResult> results = new List<ValidationResult>();
            results.AddValidations(MatchData.Validation(), ToString());
            results.AddValidations(Batting.Validation(), ToString());
            results.AddValidations(Bowling.Validation(), ToString());
            results.AddValidations(FieldingStats.Validation(), ToString());

            return results;
        }

        public List<Partnership> Partnerships()
        {
            List<Partnership> partnerships = Batting.Partnerships();
            foreach (Partnership ship in partnerships)
            {
                if (ship != null && ship.MatchData == null)
                {
                    ship.MatchData = MatchData;
                }
            }
            return partnerships;
        }
    }
}
