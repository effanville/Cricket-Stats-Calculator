using System.Collections.Generic;
using System.Linq;

namespace FinancialStructures.Reporting
{
    /// <summary>
    /// Collection of ErrorReport with added query functionality to tell the user what is happening.
    /// </summary>
    public class ErrorReports
    {
        public override string ToString()
        {
            return $"These reports contain {Count()} entries: {GetReports(ReportType.Error)} errors,{GetReports(ReportType.Warning)} warnings and {GetReports(ReportType.Report)} reports.";
        }

        /// <summary>
        /// Creates an instance with an empty list of reports.
        /// </summary>
        public ErrorReports()
        {
            fReports = new List<ErrorReport>();
        }

        /// <summary>
        /// Number of reports in total.
        /// </summary>
        public int Count()
        {
            return fReports.Count();
        }

        /// <summary>
        /// Determines if there are any reports in the list.
        /// </summary>
        /// <returns></returns>
        public bool Any()
        {
            return fReports.Any();
        }

        private List<ErrorReport> fReports;

        /// <summary>
        /// Adds the reports from another repository of reports to this one.
        /// </summary>
        /// <param name="reports"></param>
        public void AddReports(ErrorReports reports)
        {
            fReports.AddRange(reports.GetReports());
        }

        /// <summary>
        /// Adds a report to the existing list 
        /// </summary>
        public void AddReportFromStrings(string severity, string type, string location, string message)
        {
            ReportSeverity.TryParse(severity, out ReportSeverity reportSeverity);
            ReportType.TryParse(type, out ReportType typeOfReport);
            ReportLocation.TryParse(location, out ReportLocation locationType);

            AddErrorReport(reportSeverity, typeOfReport, locationType, message);
        }

        /// <summary>
        /// Adds a report of any type to the existing list 
        /// </summary>
        public void AddErrorReport(ReportSeverity severity, ReportType type, ReportLocation location, string newReport)
        {
            fReports.Add(new ErrorReport(severity, type, location, newReport));
        }

        /// <summary>
        /// Adds a report of any type to the existing list 
        /// </summary>
        private void AddGeneralReport(ReportType type, ReportLocation location, string newReport)
        {
            fReports.Add(new ErrorReport(type, location, newReport));
        }

        /// <summary>
        /// Adds a report to the existing list 
        /// </summary>
        public void AddReport(string newReport, ReportLocation location = ReportLocation.Unknown)
        {
            AddGeneralReport(ReportType.Report, location, newReport);
        }

        /// <summary>
        /// Adds an Error report to the existing list 
        /// </summary>
        public void AddError(string newReport, ReportLocation location = ReportLocation.Unknown)
        {
            AddGeneralReport(ReportType.Error, location, newReport);
        }

        /// <summary>
        /// Adds a Warning report to the existing list 
        /// </summary>
        public void AddWarning(string newReport, ReportLocation location = ReportLocation.Unknown)
        {
            AddGeneralReport(ReportType.Warning, location, newReport);
        }

        /// <summary>
        /// Returns a copy of all reports held in the structure.
        /// </summary>
        public List<ErrorReport> GetReports()
        {
            var copiedReports = new List<ErrorReport>();
            copiedReports.AddRange(fReports);
            return copiedReports;
        }

        /// <summary>
        /// Returns all reports of a certain severity from the system.
        /// </summary>
        public List<ErrorReport> GetReports(ReportSeverity severity = ReportSeverity.Useful)
        {
            if (severity.Equals(ReportSeverity.Critical))
            {
                return fReports.Where(report => report.ErrorSeverity == severity).ToList();
            }
            if (severity.Equals(ReportSeverity.Useful))
            {
                return fReports.Where(report => report.ErrorSeverity == severity || report.ErrorSeverity == ReportSeverity.Critical).ToList();
            }

            return GetReports();
        }

        /// <summary>
        /// Returns a list of reports with location the same as the specified location.
        /// </summary>
        public List<ErrorReport> GetReports(ReportLocation location)
        {
            return fReports.Where(report => report.ErrorLocation == location).ToList();
        }

        /// <summary>
        /// Returns a list of reports with ReportType matching the desired type.
        /// </summary>
        public List<ErrorReport> GetReports(ReportType reportType)
        {
            return fReports.Where(report => report.ErrorType == reportType).ToList();
        }

        /// <summary>
        /// Removes element at index <param name="i"/>
        /// </summary>
        public void RemoveReport(int i)
        {
            if (i >= 0 && i < fReports.Count())
            {
                fReports.RemoveAt(i);
            }
        }

        /// <summary>
        /// Removes all reports held in the list.
        /// </summary>
        public void Clear()
        {
            fReports.Clear();
        }
    }
}
