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
    public sealed class ScorecardAddCommand : ICommand
    {
        private readonly IFileSystem fFileSystem;
        private readonly IReportLogger fLogger;
        private readonly CommandOption<string> fFilepathOption;
        private readonly CommandOption<string> fScorecardLocation;
        public string Name => "add";

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
        public ScorecardAddCommand(IFileSystem fileSystem, IReportLogger logger)
        {            
            fFileSystem = fileSystem;
            fLogger = logger;
            Func<string, bool> fileValidator = filepath => fileSystem.File.Exists(filepath);
            fFilepathOption = new CommandOption<string>("filepath", "The path to the team database.", required: true, fileValidator);
            Options.Add(fFilepathOption);
            fScorecardLocation = new CommandOption<string>("newScorecard", "The path/directory to add the scorecards from.", required: true, fileValidator);
            Options.Add(fExportDirectory);
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


            var scorecardLocation = fScorecardLocation.Value;
            string extension = fFileSystem.Path.GetExtension(scorecardLocation).Trim('.');
            if(!Enum.TryParse<DocumentType>(extension, out var docType))
            {
                _ = fLogger.LogUseful(ReportType.Information, ReportLocation.Loading, $"[Command {Name}] - Scorecard not of recognised type.");
            }
                
            CricketMatch newMatch = CricketMatch.CreateFromScorecard(docType, scorecardLocation);
            if(!team.AddMatch(newMatch))
            {
                _ = fLogger.LogUseful(ReportType.Information, ReportLocation.Loading, $"[Command {Name}] - Match with data already exists.");
            }

            _ = fLogger.LogUseful(ReportType.Information, ReportLocation.Loading, $"[Command {Name}] - Completed execution.");
            return 0;
        }
    }
}