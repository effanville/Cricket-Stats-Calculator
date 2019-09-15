using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ReportingStructures
{
    /// <summary>
    /// Structure 
    /// </summary>
    public static class ErrorReports
    {
        public static bool Configure()
        {
            ok = true;
            fReports = new List<string>();
            fWarnings = new List<string>();
            fErrors = new List<string>();
            return true;
        }

        /// <summary>
        /// member to say whether should continue or not. 
        /// if false then stops program to continue
        /// </summary>
        private static bool ok; 

        private static List<string> fReports;

        private static List<string> fWarnings;

        private static List<string> fErrors;

        public static void NotOk()
        {
            ok = false;
        }

        public static void AllOk()
        {
            ok = true;
        }

        public static bool OkNotOk()
        {
            return ok;
        }

        /// <summary>
        /// Adds a report to the existing list 
        /// </summary>
        /// <param name="newReport">Report one wants to add to held reports</param>
        public static void AddReport(string newReport)
        {
            fReports.Add(newReport);
        }

        /// <summary>
        /// Adds a warning to the existing list 
        /// </summary>
        /// <param name="newWarning">Warning one wants to add to held reports</param>
        public static void AddWarning(string newWarning)
        {
            fWarnings.Add(newWarning);
        }

        /// <summary>
        /// Adds a report to the existing list 
        /// </summary>
        /// <param name="newError">Report one wants to add to held reports</param>
        public static void AddError(string newError)
        {
            fErrors.Add(newError);
            NotOk();
        }

        /// <summary>
        /// Function to obtain 
        /// </summary>
        /// <returns>Currently held reports</returns>
        public static List<string> GetReport()
        {
            return fReports;
        }

        public static List<string> GetWarnings()
        {
            return fWarnings;
        }

        public static List<string> GetErrors()
        {
            return fErrors;
        }

        public static void Clear()
        {
            fErrors.Clear();
            fWarnings.Clear();
            fReports.Clear();
            ok = true;
        }
    }

    public enum ReportType
    {
        Report = 0,
        Warning = 1,
        Error = 2,
    }
}
