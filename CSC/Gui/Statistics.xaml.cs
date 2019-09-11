using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Cricket;
using System.IO;

namespace CricketStatsCalc
{
    public class DummyBattingStats
    {
        public string aName { get; set; }
        public int Tot_Runs { get; set; }
        public int Tot_innings { get; set; }
        public int Tot_not_out { get; set; }
        public string Bestbatting { get; set; }
        public double Batting_average {get; set; }


        public DummyBattingStats(string Name1, int Tot_Runs1, int Tot_innings1, int Tot_not_out1, string best, double Batting_average1)
        {
            aName = Name1;
            Tot_Runs = Tot_Runs1;
            Tot_innings = Tot_innings1;
            Tot_not_out = Tot_not_out1;
            Bestbatting = best;
            Batting_average = Batting_average1;
        }
    }

    class BattingCompare : IComparer<DummyBattingStats>
    {
        public int Compare(DummyBattingStats x, DummyBattingStats y)
        {

            if (x.Batting_average > y.Batting_average)
            {
                return -1;
            }
            if (x.Batting_average < y.Batting_average)
            {
                return 1;
            }
      
            return 0;
        }
    }

    public class DummyBowlingStats
    {
        public string name { get; set; }
        public int overs { get; set; }
        public int maidens { get; set; }
        public int runs_conceded { get; set; }
        public int wickets { get; set; }
        public double average {get; set; }
        public double economy { get; set; }
        public string bestbowl { get; set; }

        public DummyBowlingStats(string nm, int ovrs, int mdns, int rns, int wckts, double av, double econ, string bestbowling)
        {
            name = nm;
            overs = ovrs;
            maidens = mdns;
            runs_conceded = rns;
            wickets = wckts;
            average = av;
            economy = econ;
            bestbowl = bestbowling;
        }
    }

    class BowlingCompare : IComparer<DummyBowlingStats>
    {
        public int Compare(DummyBowlingStats x, DummyBowlingStats y)
        {
            if (double.IsNaN(x.average))
            {
                return 1;
            }
            if (double.IsNaN(y.average))
            {
                return -1;
            }
            if (x.average < y.average)
            {
                return -1;
            }
            if (x.average > y.average)
            {
                return 1;
            }

            return 0;
        }
    }

    public class DummyFieldingStats
    {
        public string name { get; set; }
        public int cat { get; set; }
        public int ro { get; set; }
        public int cat_w { get; set; }
        public int st_w { get; set; }
        public int total { get; set; }

        public DummyFieldingStats(string nm, int catches, int runouts, int catchew_wrns, int stumping, int totals)
        {
            name = nm;
            cat = catches;
            ro = runouts;
            cat_w = catchew_wrns;
            st_w = stumping;
            total = totals;
        }
    }

    class FieldingCompare : IComparer<DummyFieldingStats>
    {
        public int Compare(DummyFieldingStats x, DummyFieldingStats y)
        {
            if (x.total > y.total)
            {
                return -1;
            }
            if (x.total > y.total)
            {
                return 1;
            }

            return 0;
        }
    }

    public class DummyKeeperStats
    {
        public string name { get; set; }
        public int cat_w { get; set; }
        public int st_w { get; set; }
        public int total { get; set; }

        public DummyKeeperStats(string nm, int catchew_wrns, int stumping, int totals)
        {
            name = nm;
            cat_w = catchew_wrns;
            st_w = stumping;
            total = totals;
        }
    }

    class KeeperCompare : IComparer<DummyKeeperStats>
    {
        public int Compare(DummyKeeperStats x, DummyKeeperStats y)
        {
            if (x.total > y.total)
            {
                return -1;
            }
            if (x.total > y.total)
            {
                return 1;
            }

            return 0;
        }
    }

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class StatisticsWindow: Window
    {
        public StatisticsWindow()
        {
            InitializeComponent();

            List<DummyBattingStats> DummyBatStats = new List<DummyBattingStats>();
            List<DummyBowlingStats> DummyBowlStats = new List<DummyBowlingStats>();
            List<DummyFieldingStats> DummyFieldStats = new List<DummyFieldingStats>();
            List<DummyKeeperStats> DummyKeepStats = new List<DummyKeeperStats>();

            StreamWriter swBat = new StreamWriter("Batting.csv");
            StreamWriter swBowl = new StreamWriter("Bowling.csv");
            StreamWriter swField = new StreamWriter("Fielding.csv");

            swBat.WriteLine("Name, Total Runs, Total innings, Total not out, Batting average, best batting");
            swBowl.WriteLine("Name,  Total overs, Total mdns, Total runsconceded, Total wickets, Average, Economy, Best bowling");
            swField.WriteLine("Name, total catches, total runouts, total catches keeper, total stump keeper, total fielding, total keep");
            foreach (Cricket_Player person in Globals.Ardeley)
            {
                //create mock stats for batting
                string Name1 = person.Name;
                int Tot_Runs1 = person.Total_runs;
                int Tot_innings1 = person.Total_innings;
                int Tot_not_out1 = person.Total_not_out;
                string best = person.Bestbatting;
                double Batting_average1 = person.Batting_average;
                DummyBattingStats NextPlayer = new DummyBattingStats(Name1, Tot_Runs1, Tot_innings1, Tot_not_out1, best, Math.Round(Batting_average1,2));
                DummyBatStats.Add(NextPlayer);

                //create mock stats for bowling
                int Tot_Ovrs = person.Total_overs;
                int Tot_mdns = person.Total_maidens;
                int Tot_runs_conc = person.Total_runs_conceded;
                int Tot_wckts = person.Total_wickets;
                double BAv = person.Bowling_average;
                double Becon = person.Bowling_economy;
                string bowlbest = person.Best_bowl_figures;
                DummyBowlingStats NextBowlPlayer = new DummyBowlingStats(Name1, Tot_Ovrs, Tot_mdns, Tot_runs_conc,Tot_wckts,Math.Round(BAv,2), Math.Round(Becon,2), bowlbest);
                DummyBowlStats.Add(NextBowlPlayer);

                // create mock stats for fielding
                int tot_catches = person.Total_catches;
                int tot_runouts = person.Total_run_out;
                int tot_catches_w = person.Total_catches_w;
                int tot_stump_w = person.Total_stumpings_w;
                int tot_fielding = person.Total_fielding_dismissals;
                DummyFieldingStats NextfieldPlayer = new DummyFieldingStats(Name1, tot_catches, tot_runouts, tot_catches_w, tot_stump_w, tot_fielding);
                DummyFieldStats.Add(NextfieldPlayer);

                int tot_keep = person.Total_keeper_dismissals;
                if (person.Total_keeper_dismissals > 0)
                {
                    
                    DummyKeeperStats NextKeeperPlayer = new DummyKeeperStats(Name1, tot_catches_w, tot_stump_w, tot_keep);
                    DummyKeepStats.Add(NextKeeperPlayer);
                }

                string lineBat = person.Name + "," + person.Total_runs.ToString() + "," + person.Total_innings.ToString() + "," + person.Total_not_out.ToString() + "," + person.Batting_average.ToString() + " , " + best.ToString();
                string lineBowl = person.Name + "," + person.Total_overs.ToString() + "," + Tot_mdns.ToString() + "," + Tot_runs_conc.ToString() + "," + Tot_wckts.ToString() + "," + BAv.ToString() + "," + Becon.ToString() + " , " + bowlbest.ToString();
                string lineField = person.Name + " , " + tot_catches.ToString() + " , " + tot_runouts.ToString() + " , " + tot_catches_w.ToString() + " , " + tot_stump_w.ToString() + " , " + tot_fielding.ToString()+ " , " + tot_keep.ToString();

                swBat.WriteLine(lineBat);
                swBowl.WriteLine(lineBowl);
                swField.WriteLine(lineField);
            }
            BattingCompare BC = new BattingCompare();
            DummyBatStats.Sort(BC);
            BattingStats.ItemsSource = DummyBatStats;
            BattingStats.AutoGenerateColumns = true;


            swBat.Close();
            swBowl.Close();
            swField.Close();


            BowlingCompare BowlC = new BowlingCompare();
            DummyBowlStats.Sort(BowlC);
            BowlingStats.ItemsSource = DummyBowlStats;
            BowlingStats.AutoGenerateColumns = true;


            FieldingCompare FieldC = new FieldingCompare();
            DummyFieldStats.Sort(FieldC);
            FieldingStats.ItemsSource = DummyFieldStats ;



            KeeperCompare KeepC = new KeeperCompare();
            DummyKeepStats.Sort(KeepC);
        }
    }
}
