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
        // collections of statistics to set for each player

        private bool fCalculated;

        public bool Calculated
        {
            get { return fCalculated; }
            set { fCalculated = value; }
        }

        // match wide statistics

        private int total_MoM;
        public int Total_Mom
        { get { return total_MoM; }
            set { total_MoM = value; }
        }

        // batting statistics
        private int fTotalInnings;
        public int TotalInnings
        {
            get
            {
                return fTotalInnings;
            }
            set
            {
                fTotalInnings = value;
            }
        }

        private int total_not_out;
        public int Total_not_out
        {
            get
            {
                return total_not_out;
            }
            set
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
            set
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
            set
            {
                batting_average = value;
            }
        }

        private string bestbatting;
        public string Bestbatting
        {
            get { return bestbatting; }
            set { bestbatting = value; }
        }
        // still need statistic holder for types of dismissal

        // bowling statistics
        private int total_overs;
        public int Total_overs
        { get { return total_overs; }
            set { total_overs = value; }
        }

        private int total_wickets;
        public int Total_wickets
        {
            get { return total_wickets; }
            set { total_wickets = value; }
        }

        private int total_runs_conceded;
        public int Total_runs_conceded
        {
            get { return total_runs_conceded; }
            set { total_runs_conceded = value; }
        }

        private int total_maidens;
        public int Total_maidens
        {
            get { return total_maidens; }
            set { total_maidens = value; }
        }
        private double bowling_average;
        public double Bowling_average
        {
            get { return bowling_average; }
            set
            {
                bowling_average = value;
            }
        }

        private double bowling_economy;
        public double Bowling_economy
        {
            get { return bowling_economy; }
            set { bowling_economy = value; }
        }

        private string best_bowl_figures;
        public string Best_bowl_figures
        {
            get { return best_bowl_figures;}
            set { best_bowl_figures = value; }
        }

        // statistic for types of dismissal as well

        // fielding statistics
        private int total_catches;
        public int Total_catches
        {
            get { return total_catches; }
            set { total_catches = value; }
         }

        private int total_run_out;
        public int Total_run_out
        { get { return total_run_out; }
            set { total_run_out = value;  }
        }

        // wicketkeeper_statistics

        private int total_catches_w;
        public int Total_catches_w
        { get { return total_catches_w; }
            set { total_catches_w = value; } }

        private int total_stumpings_w;
        public int Total_stumpings_w
        { get { return total_stumpings_w; }
            set { total_stumpings_w = value; }
        }

        private int total_fielding_dismissals;
        public int Total_fielding_dismissals
        { get { return total_fielding_dismissals; }
            set { total_fielding_dismissals = value; }
        }

        private int total_keeper_dismissals;
        public int Total_keeper_dismissals
        {
            get { return total_keeper_dismissals; }
            set { total_keeper_dismissals = value; }
        }

        // functions producing statistics 
    }
}