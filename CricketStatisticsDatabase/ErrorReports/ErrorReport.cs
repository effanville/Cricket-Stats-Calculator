using System;

namespace FinancialStructures.Reporting
{
    /// <summary>
    /// A report structure containing information about a possible problem (or just info) happening in the program.
    /// </summary>
    public class ErrorReport : IComparable
    {
        /// <summary>
        /// Output of error as a string. This does not include the severity of the report.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ErrorType.ToString() + " - " + ErrorLocation.ToString() + " - " + Message;

        }

        /// <summary>
        /// Method of comparison
        /// </summary>
        public int CompareTo(object obj)
        {
            if (obj is ErrorReport value)
            {
                if (value.ErrorType.Equals(ErrorType))
                {
                    if (value.ErrorLocation.Equals(ErrorLocation))
                    {
                        return Message.CompareTo(value.Message);
                    }

                    return value.ErrorLocation.CompareTo(ErrorLocation);
                }

                return ErrorType.CompareTo(value.ErrorType);
            }

            return 0;
        }

        private ReportSeverity fErrorSeverity;

        /// <summary>
        /// How serious the report is, enabling a grading of the reports based on seriousness.
        /// </summary>
        public ReportSeverity ErrorSeverity
        {
            get { return fErrorSeverity; }
            set { fErrorSeverity = value; }
        }

        private ReportType fErrorType;

        /// <summary>
        /// The type of the report (is this an error or a warning etc).
        /// </summary>
        public ReportType ErrorType
        {
            get { return fErrorType; }
            set { fErrorType = value; }
        }

        private ReportLocation fErrorLocation;

        /// <summary>
        /// Where is this a report from.
        /// </summary>
        public ReportLocation ErrorLocation
        {
            get { return fErrorLocation; }
            set { fErrorLocation = value; }
        }

        private string fMessage;

        /// <summary>
        /// Any extra text needed to aid info to the report.
        /// </summary>
        public string Message
        {
            get { return fMessage; }
            set { fMessage = value; }
        }

        public ErrorReport()
        {
        }

        public ErrorReport(ReportType type, ReportLocation errorLocation, string msg)
        {
            ErrorType = type;
            ErrorLocation = errorLocation;
            Message = msg;
        }

        public ErrorReport(ReportSeverity severity, ReportType type, ReportLocation errorLocation, string msg)
        {
            ErrorSeverity = severity;
            ErrorType = type;
            ErrorLocation = errorLocation;
            Message = msg;
        }
    }
}
