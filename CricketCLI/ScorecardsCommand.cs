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
    public sealed class ScorecardsCommand : ICommand
    {
        public string Name => "scorecard";

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

        public ScorecardsCommand(IFileSystem fileSystem, IReportLogger logger, string smtpAuthUser, string smtpAuthPassword)
        {
            SubCommands.Add(new ExportScorecardsCommand(fileSystem, logger, smtpAuthUser, smtpAuthPassword));
            SubCommands.Add(new ScorecardAddCommand(fileSystem, logger));
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
            return CommandExtensions.Execute(this, console, args);
        }
    }
}