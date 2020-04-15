using System;
using StringFunctions;

namespace FinancialStructures.Reporting
{
    /// <summary>
    /// Collection of standard reporting mechanisms.
    /// </summary>
    public class LogReporter : IReportLogger
    {
        private readonly Action<ReportSeverity, ReportType, ReportLocation, string> fLoggingAction;

        /// <summary>
        /// Log an arbitrary message.
        /// </summary>
        public bool LogWithStrings(string severity, string type, string location, string message)
        {
            if (fLoggingAction != null)
            {
                fLoggingAction(severity.ToEnum<ReportSeverity>(), type.ToEnum<ReportType>(), location.ToEnum<ReportLocation>(), message);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Log an arbitrary message.
        /// </summary>
        public bool Log(ReportSeverity severity, ReportType type, ReportLocation location, string message)
        {
            if (fLoggingAction != null)
            {
                fLoggingAction(severity, type, location, message);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Log an arbitrary message.
        /// </summary>
        public bool LogUsefulWithStrings(string type, string location, string message)
        {
            if (fLoggingAction != null)
            {
                fLoggingAction(ReportSeverity.Useful, type.ToEnum<ReportType>(), location.ToEnum<ReportLocation>(), message);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Log an arbitrary message.
        /// </summary>
        public bool LogUseful(ReportType type, ReportLocation location, string message)
        {
            if (fLoggingAction != null)
            {
                fLoggingAction(ReportSeverity.Useful, type, location, message);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Log an arbitrary message.
        /// </summary>
        public bool LogUsefulError(ReportLocation location, string message)
        {
            if (fLoggingAction != null)
            {
                fLoggingAction(ReportSeverity.Useful, ReportType.Error, location, message);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Log an error message.
        /// </summary>
        public bool LogUsefulErrorWithStrings(string location, string message)
        {
            if (fLoggingAction != null)
            {
                fLoggingAction("Useful".ToEnum<ReportSeverity>(), "Error".ToEnum<ReportType>(), location.ToEnum<ReportLocation>(), message);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Constructor for reporting mechanisms. Parameter addReport is the report callback mechanism.
        /// </summary>
        public LogReporter(Action<string, string, string, string> addReport)
        {
            fLoggingAction = (detailLevel, type, location, message) => addReport(detailLevel.ToString(), type.ToString(), location.ToString(), message);
        }

        /// <summary>
        /// Constructor for reporting mechanisms. Parameter addReport is the report callback mechanism.
        /// </summary>
        public LogReporter(Action<ReportSeverity, ReportType, ReportLocation, string> addReport)
        {
            fLoggingAction = addReport;
        }
    }
}
