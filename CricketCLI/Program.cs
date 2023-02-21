using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;

using Common.Console;
using Common.Console.Commands;
using Common.Console.Options;
using Common.Structure.Extensions;
using Common.Structure.Reporting;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace CricketCLI
{
    public static class Program
    {
        private static void WriteLine(string text) 
        {
            string message = $"[{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}] -{text}";
            Console.WriteLine(message);
        }
        private static void WriteError(string text)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            WriteLine(text);
            Console.ForegroundColor = color;
        }
        
        // Create the logger.
        private static void ReportAction(ReportSeverity severity, ReportType reportType, ReportLocation location, string text)
        {
            if (reportType == ReportType.Error)
            {
                WriteError(text);
            }
            else
            {
                WriteLine(text);
            }
        }

        public static int Main(string[] args)
        {
            // Build a config object, using env vars and JSON providers.
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var smtpAuthUser = config.GetValue<string>("SmtpAuthUser");
            var smtpAuthPassword = config.GetValue<string>("SmtpAuthPassword");
            IReportLogger logger = new LogReporter(ReportAction);
            IConsole console = new ConsoleInstance(WriteError, WriteLine);
            
            IFileSystem fileSystem = new FileSystem();

            // Define the acceptable commands for this program.
            var validCommands = new List<ICommand>()
            {
                new CreateStatisticsCommand(fileSystem, logger, smtpAuthUser, smtpAuthPassword),
                new ScorecardsCommand(fileSystem, logger, smtpAuthUser, smtpAuthPassword),
            };
            
            Assembly assembly = Assembly.GetExecutingAssembly();
            string informationVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            _ = logger.Log(ReportSeverity.Useful, ReportType.Information, ReportLocation.Execution, $"CricketCLI - version {informationVersion}");

            // Generate the context, validate the arguments and execute.
            ConsoleContext.SetAndExecute(args, console, logger, validCommands);
            var directorySep = fileSystem.Path.DirectorySeparatorChar;
            string logPath = $"{fileSystem.Directory.GetCurrentDirectory()}{directorySep}{DateTime.Now.FileSuitableDateTimeValue()}-consoleLog.log";
            logger.WriteReportsToFile(logPath);
            return 0;
        }
    }
}