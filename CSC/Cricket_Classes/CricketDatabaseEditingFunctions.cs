using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cricket;
using ReportingStructures;

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

            return true;
        }

        public static bool AddMatch()

    }
}
