using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Compression;
using System.Linq;

using Common.Console;
using Common.Console.Commands;
using Common.Console.Options;

using Common.Structure.DisplayClasses;
using Common.Structure.Extensions;
using Common.Structure.Reporting;
using Common.Structure.ReportWriting;

using CricketStructures;
using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics.Collection;

namespace CricketCLI
{
    public sealed class ExportScorecardsCommand : ICommand
    {
        private readonly string fSmtpAuthUser;
        private readonly string fSmtpAuthPassword;
        private readonly IFileSystem fFileSystem;
        private readonly IReportLogger fLogger;
        private readonly CommandOption<string> fFilepathOption;
        private readonly CommandOption<string> fExportDirectory;
        private readonly CommandOption<DocumentType> fDocType;

        public string Name => "export";

        /// <inheritdoc/>
        public IList<CommandOption> Options
        {
            get;
        } = new List<CommandOption>();

        /// <inheritdoc/>
        public IList<ICommand> SubCommands
        {
            get;
        } = new List<ICommand>();

        public ExportScorecardsCommand(IFileSystem fileSystem, IReportLogger logger, string smtpAuthUser, string smtpAuthPassword)
        {
            fSmtpAuthUser = smtpAuthUser;
            fSmtpAuthPassword = smtpAuthPassword;
            fFileSystem = fileSystem;
            fLogger = logger;
            Func<string, bool> fileValidator = filepath => fileSystem.File.Exists(filepath);
            fFilepathOption = new CommandOption<string>("filepath", "The path to the team database.", required: true, fileValidator);
            Options.Add(fFilepathOption);
            fExportDirectory = new CommandOption<string>("exportPath", "The path to export the scorecards to.", required: true, null);
            Options.Add(fExportDirectory);
            fDocType = new CommandOption<DocumentType>("reportType", "The type of the export.", null);
            Options.Add(fDocType);
        }

        /// <inheritdoc/>
        public void WriteHelp(IConsole console)
        {
            CommandExtensions.WriteHelp(this, console);
        }

        /// <inheritdoc/>
        public bool Validate(IConsole console, string[] args)
        { 
            return CommandExtensions.Validate(this, args, console);
        }

        /// <inheritdoc/>
        public int Execute(IConsole console, string[] args = null)
        {
            _ = fLogger.LogUseful(ReportType.Information, ReportLocation.Loading, $"[Command {Name}] - Beginning execution.");
            var teamFilePath = fFilepathOption.Value;
            ICricketTeam team = CricketTeamFactory.CreateFromFile(fFileSystem, teamFilePath, out string error);

            if(!string.IsNullOrEmpty(error))
            {
                _ = fLogger.LogUsefulError(ReportLocation.Loading, $"[Command {Name}] - Error when creating cricket team from file: {error}");
                return 1;
            }

            string exportFolder = fExportDirectory.Value;
            string scorecardFolder = fFileSystem.Path.Combine(exportFolder, "Scorecards");
            DocumentType docType = fDocType.Value;
            _ = fFileSystem.Directory.CreateDirectory(scorecardFolder);
            foreach (ICricketSeason season in team.Seasons)
            {
                var seasonDirectory = fFileSystem.Path.Combine(scorecardFolder, season.Year.Year.ToString());
                _ = fFileSystem.Directory.CreateDirectory(seasonDirectory);
                foreach (ICricketMatch match in season.Matches)
                {
                    var serializedMatch = match.SerializeToString(docType);
                    var matchFileName = $"{match.MatchData.ToFileNameString()}.{docType}";
                    var matchLocation = fFileSystem.Path.Combine(seasonDirectory, matchFileName);
                    fFileSystem.File.WriteAllText(matchLocation, serializedMatch.ToString());
                }
            }
        
            string zipFile = fFileSystem.Path.Combine(exportFolder, "scorecards.zip");
            if(fFileSystem.File.Exists(zipFile))
            {
                fFileSystem.File.Delete(zipFile);
            }
            ZipFile.CreateFromDirectory(scorecardFolder, zipFile);

            _ = fLogger.LogUseful(ReportType.Information, ReportLocation.Loading, $"[Command {Name}] - Completed execution.");
            Email.WriteEmail(fFileSystem, fSmtpAuthUser, fSmtpAuthPassword, "Cricket Scorecards", new List<string> { fSmtpAuthUser }, new List<string>{ zipFile });
            return 0;
        }
    }
}