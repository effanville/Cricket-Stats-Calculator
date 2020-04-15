namespace FinancialStructures.Reporting
{
    /// <summary>
    /// Report logger that does nothing, but declares successfully reported.
    /// </summary>
    public class NothingReportLogger : IReportLogger
    {
        public bool Log(ReportSeverity severity, ReportType type, ReportLocation location, string message)
        {
            return true;
        }

        public bool LogUseful(ReportType type, ReportLocation location, string message)
        {
            return true;
        }

        public bool LogUsefulError(ReportLocation location, string message)
        {
            return true;
        }

        public bool LogUsefulErrorWithStrings(string location, string message)
        {
            return true;
        }

        public bool LogUsefulWithStrings(string type, string location, string message)
        {
            return true;
        }

        public bool LogWithStrings(string severity, string type, string location, string message)
        {
            return true;
        }
    }
}
