using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cricket
{
    /// <summary>
    /// Class containing all information about a player of cricket
    /// </summary>
    public class Cricket_Player
    {
        public Cricket_Player()
        {
        }

        public Cricket_Player(string nameCandidate)
        {
            Name = nameCandidate;
        }

        public string Name
        {
            get { return name; }
            private set { name = value; }
        }

        public void EditName(string newName)
        {
            if (newName != Name)
            {
                Name = newName;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string name;

        // collections of statistics to set for each player

        private bool calculated;

        public bool Calculated
        {
            get { return calculated; }
            set { calculated = value; }
        }

        // match wide statistics

        private int total_MoM;
        public int Total_Mom
        { get { return total_MoM; }
            set { total_MoM = value; }
        }

        // batting statistics
        private int total_innings;
        public int Total_innings
        {
            get
            {
                return total_innings;
            }
            private set
            {
                total_innings = value;
            }
        }

        private int total_not_out;
        public int Total_not_out
        {
            get
            {
                return total_not_out;
            }
            private set
            {
                total_not_out = value;
            }
        }

        private int total_runs;
        public int Total_runs
        {
            get
            {
                return total_runs;
            }
            private set
            {
                total_runs = value;
            }
        }

        private double batting_average;
        public double Batting_average
        {
            get
            {
                return batting_average;
            }
            private set
            {
                batting_average = value;
            }
        }

        private string bestbatting;
        public string Bestbatting
        {
            get { return bestbatting; }
            private set { bestbatting = value; }
        }
        // still need statistic holder for types of dismissal

        // bowling statistics
        private int total_overs;
        public int Total_overs
        { get { return total_overs; }
            private set { total_overs = value; }
        }

        private int total_wickets;
        public int Total_wickets
        {
            get { return total_wickets; }
            private set { total_wickets = value; }
        }

        private int total_runs_conceded;
        public int Total_runs_conceded
        {
            get { return total_runs_conceded; }
            private set { total_runs_conceded = value; }
        }

        private int total_maidens;
        public int Total_maidens
        {
            get { return total_maidens; }
            private set { total_maidens = value; }
        }
        private double bowling_average;
        public double Bowling_average
        {
            get { return bowling_average; }
            private set
            {
                bowling_average = value;
            }
        }

        private double bowling_economy;
        public double Bowling_economy
        {
            get { return bowling_economy; }
            private set { bowling_economy = value; }
        }

        private string best_bowl_figures;
        public string Best_bowl_figures
        {
            get { return best_bowl_figures;}
            private set { best_bowl_figures = value; }
        }

        // statistic for types of dismissal as well

        // fielding statistics
        private int total_catches;
        public int Total_catches
        {
            get { return total_catches; }
            private set { total_catches = value; }
         }

        private int total_run_out;
        public int Total_run_out
        { get { return total_run_out; }
            private set { total_run_out = value;  }
        }

        // wicketkeeper_statistics

        private int total_catches_w;
        public int Total_catches_w
        { get { return total_catches_w; }
            private set { total_catches_w = value; } }

        private int total_stumpings_w;
        public int Total_stumpings_w
        { get { return total_stumpings_w; }
            private set { total_stumpings_w = value; }
        }

        private int total_fielding_dismissals;
        public int Total_fielding_dismissals
        { get { return total_fielding_dismissals; }
            private set { total_fielding_dismissals = value; }
        }

        private int total_keeper_dismissals;
        public int Total_keeper_dismissals
        {
            get { return total_keeper_dismissals; }
            private set { total_keeper_dismissals = value; }
        }

        // functions producing statistics 

        /// <summary>
        /// Function that sets all statistics for the player
        /// </summary>
        public bool set_statistics()
        {
            if (!Calculated)
            {
                total_runs = 0;
                total_innings = 0;
                total_not_out = 0;
                total_overs = 0;
                total_maidens = 0;
                total_runs_conceded = 0;
                total_wickets = 0;
                total_catches = 0;
                total_run_out = 0;
                total_catches_w = 0;
                total_stumpings_w = 0;

                int best_bowl_wckts = 0;
                int best_bowl_runs = 0;
                string best_bowl_oppo = "";
                string best_bowl_date = "";
                int bestbat = 0;
                string bestbat_oppo = "";
                string best_bat_date = "";
                foreach (Cricket_Match game in Globals.GamesPlayed)
                {
                    int playerIndex = 0;
                    if (game.PlayNotPlay(this, out playerIndex))
                    {
                        if (game.FType == MatchType.League)
                        {
                            // first update batting information and statistics
                            total_runs = total_runs + game.FBatting.FRuns_Scored[playerIndex];

                            OutType test = OutType.DidNotBat;
                            // if the player batted, then out type is not didnotbat
                            if (!game.FBatting.FMethod_Out[playerIndex].Equals(test))
                            {
                                total_innings += 1;

                                // if batsman was not out, then add to this value
                                OutType test2 = OutType.NotOut;
                                if (game.FBatting.FMethod_Out[playerIndex].Equals(test2))
                                {
                                    total_not_out += 1;
                                }
                            }

                            if (total_innings - total_not_out != 0)
                            {
                                Batting_average = (double)total_runs / ((double)total_innings - (double)total_not_out);
                            }

                            if (game.FBatting.FRuns_Scored[playerIndex] > bestbat)
                            {
                                bestbat = game.FBatting.FRuns_Scored[playerIndex];
                                bestbat_oppo = game.FOpposition;
                                best_bat_date = game.Date.ToShortDateString();
                            }

                            // next update bowling information and statistics

                            total_overs += game.FBowling.FOvers_Bowled[playerIndex];
                            total_maidens += game.FBowling.FMaidens[playerIndex];
                            total_runs_conceded += game.FBowling.FRuncs_Conceded[playerIndex];
                            total_wickets += game.FBowling.FWickets[playerIndex];

                            if (total_wickets != 0)
                            {
                                bowling_average = (double)total_runs_conceded / (double)total_wickets;

                            }
                            else { bowling_average = double.NaN; }

                            if (total_overs != 0)
                            { bowling_economy = (double)total_runs_conceded / (double)total_overs; }
                            else { bowling_economy = double.NaN; }

                            // now produce best bowling figures
                            // best figures are the most wickets for the fewest runs

                            // if this game has more wickets than before, then these are the best figures so far
                            if (game.FBowling.FWickets[playerIndex] > best_bowl_wckts)
                            {
                                best_bowl_wckts = game.FBowling.FWickets[playerIndex];
                                best_bowl_runs = game.FBowling.FRuncs_Conceded[playerIndex];
                                best_bowl_oppo = game.FOpposition;
                                best_bowl_date = game.Date.ToShortDateString();
                            }

                            // if the number of wickets is the same, but have fewer runs, then these are the best figures
                            if (game.FBowling.FWickets[playerIndex] == best_bowl_wckts)
                            {
                                if (game.FBowling.FMaidens[playerIndex] < best_bowl_runs)
                                {
                                    best_bowl_wckts = game.FBowling.FWickets[playerIndex];
                                    best_bowl_runs = game.FBowling.FRuncs_Conceded[playerIndex];
                                    best_bowl_oppo = game.FOpposition;
                                    best_bowl_date = game.Date.ToShortDateString();
                                }
                            }

                            // finally deal with fielding statistics
                            total_catches += game.FFieldingStats.FCatches[playerIndex];
                            total_run_out += game.FFieldingStats.FRunOuts[playerIndex];

                            total_catches_w += game.FFieldingStats.FCatchesKeeper[playerIndex];
                            total_stumpings_w += game.FFieldingStats.FStumpings[playerIndex];
                        }
                    }
                }

                total_fielding_dismissals = total_catches + total_run_out + total_catches_w + total_stumpings_w;
                total_keeper_dismissals = total_catches_w + total_stumpings_w;
                Bestbatting = bestbat.ToString() + " v " + bestbat_oppo + " " + best_bat_date;
                best_bowl_figures = best_bowl_wckts.ToString() + "-" + best_bowl_runs.ToString() + " v " + best_bowl_oppo + " " + best_bowl_date;
            }

            return true;
        }
    }
}