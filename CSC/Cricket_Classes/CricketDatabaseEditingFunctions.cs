using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cricket;
using ReportingStructures;
using CricketStatsCalc;

/// <summary>
/// Namespace for functions to access the underlying database
/// </summary>
namespace CricketDatabaseEditing
{
    /// <summary>
    /// Contains all calls allowed outside of the cricket namespace. 
    /// </summary>
    public static class CricketDatabaseEditingFunctions
    {
        public static List<string> GetPlayers
        {
            get
            {
                if (Globals.Ardeley.Count == 0)
                {
                    return null;
                }

                List<string> Players = new List<string>();
                foreach (Cricket_Player person in Globals.Ardeley)
                {
                    Players.Add(person.Name);
                }

                return Players;
            }
        }

        public static bool PlayerExists(string name)
        {
            foreach (Cricket_Player person in Globals.Ardeley)
            {
                if (person.Name == name)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool AddPlayer(string name)
        {
            if (PlayerExists(name))
            {
                ErrorReports.AddError($"Player with name {name} already exists in database.");
                return false;
            }
            Cricket_Player newPlayer = new Cricket_Player(name);
            Globals.Ardeley.Add(newPlayer);

            return true;
        }

        public static bool AddMatch()
        {
            return false;
        }

        public static List<string> GetMatchesOppoDate()
        {
            List<MatchViewData> outputs = new List<MatchViewData>();
            foreach (Cricket_Match opposition in Globals.GamesPlayed)
            {
                MatchViewData Temp = new MatchViewData(opposition.FOpposition, opposition.Date);
                outputs.Add(Temp);
            }
            MatchDateCompare MDC = new MatchDateCompare();
            outputs.Sort(MDC);

            List<string> MatchDate = new List<string>();
            foreach (MatchViewData match in outputs)
            {
                MatchDate.Add(match.ToDisplay);
            }

            return MatchDate;
        }

    }
}
