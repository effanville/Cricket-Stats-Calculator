namespace FinancialStructures.Reporting
{
    /// <summary>
    /// Report Logger contract. Allows for reporting using types or strings.
    /// </summary>
    public interface IReportLogger
    {
        bool Log(ReportSeverity severity, ReportType type, ReportLocation location, string message);

        bool LogUseful(ReportType type, ReportLocation location, string message);

        bool LogUsefulError(ReportLocation location, string message);

        bool LogWithStrings(string severity, string type, string location, string message);

        bool LogUsefulWithStrings(string type, string location, string message);

        bool LogUsefulErrorWithStrings(string location, string message);
    }
}
