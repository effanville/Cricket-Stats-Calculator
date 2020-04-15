namespace FinancialStructures.Reporting
{
    public enum ReportType
    {
        Error = 0,
        Warning = 1,
        Report = 2
    }

    public enum ReportLocation
    {
        Unknown,
        Downloading,
        Saving,
        Loading,
        AddingData,
        EditingData,
        DeletingData,
        Parsing,
        StatisticsPage,
        DatabaseAccess,
        Help
    }

    /// <summary>
    /// How serious the report is.
    /// </summary>
    public enum ReportSeverity
    {
        /// <summary>
        /// The most serious level, always to be returned.
        /// </summary>
        Critical,

        /// <summary>
        /// Middle seriousness.
        /// </summary>
        Useful,

        /// <summary>
        /// Low seriousness, only output if everything is to be recorded.
        /// </summary>
        Detailed
    }
}
