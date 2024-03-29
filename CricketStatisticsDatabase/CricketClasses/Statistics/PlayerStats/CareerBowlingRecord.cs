﻿using System;
using Cricket.Interfaces;
using Cricket.Player;

namespace Cricket.Statistics.PlayerStats
{
    public class CareerBowlingRecord
    {
        public PlayerName Name
        {
            get;
            set;
        }

        public int StartYear
        {
            get;
            set;
        }

        public int EndYear
        {
            get;
            set;
        }

        public double Overs
        {
            get;
            set;
        }

        public int Maidens
        {
            get;
            set;
        }

        public int RunsConceded
        {
            get;
            set;
        }

        public int Wickets
        {
            get;
            set;
        }

        public double Average
        {
            get;
            set;
        }

        public BestBowling BestFigures
        {
            get;
            set;
        }

        public int Catches
        {
            get;
            set;
        }

        public int KeeperDismissals
        {
            get;
            set;
        }

        public CareerBowlingRecord()
        {
        }

        public CareerBowlingRecord(PlayerName name, ICricketTeam team)
        {
            Name = name;
            Overs = 0;
            Maidens = 0;
            RunsConceded = 0;
            Wickets = 0;
            BestFigures = new BestBowling();
            StartYear = DateTime.Today.Year;
            EndYear = new DateTime().Year;

            Catches = 0;
            KeeperDismissals = 0;
            foreach (ICricketSeason season in team.Seasons)
            {
                foreach (ICricketMatch match in season.Matches)
                {
                    if (match.PlayNotPlay(Name))
                    {
                        if (match.MatchData.Date.Year < StartYear)
                        {
                            StartYear = match.MatchData.Date.Year;
                        }
                        if (match.MatchData.Date.Year > EndYear)
                        {
                            EndYear = match.MatchData.Date.Year;
                        }

                        Match.BowlingEntry bowling = match.GetBowling(Name);
                        if (bowling != null)
                        {
                            Overs += bowling.OversBowled;
                            Maidens += bowling.Maidens;
                            RunsConceded += bowling.RunsConceded;
                            Wickets += bowling.Wickets;

                            BestBowling possibleBest = new BestBowling()
                            {
                                Wickets = bowling.Wickets,
                                Runs = bowling.RunsConceded,
                                Opposition = match.MatchData.Opposition,
                                Date = match.MatchData.Date
                            };

                            if (possibleBest.CompareTo(BestFigures) > 0)
                            {
                                BestFigures = possibleBest;
                            }
                        }

                        Match.FieldingEntry fielding = match.GetFielding(Name);
                        if (fielding != null)
                        {
                            Catches += fielding.Catches;
                            KeeperDismissals += fielding.KeeperStumpings + fielding.KeeperCatches;
                        }
                    }
                }
            }

            if (Wickets != 0)
            {
                Average = RunsConceded / (double)Wickets;
            }
            else
            {
                Average = double.NaN;
            }
        }
    }
}
