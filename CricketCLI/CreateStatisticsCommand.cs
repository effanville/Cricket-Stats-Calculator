using System;
using System.Collections.Generic;
using System.IO.Abstractions;
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
    public sealed class CreateStatisticsCommand : ICommand
    {
        private readonly IFileSystem fFileSystem;
        private readonly IReportLogger fLogger;
        private readonly CommandOption<string> fFilepathOption;
        private readonly CommandOption<string> fStatsOutputPath;
        private readonly CommandOption<StatCollection> fStatCollection;

        public string Name => "stats";

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

        public CreateStatisticsCommand(IFileSystem fileSystem, IReportLogger logger)
        {
            fFileSystem = fileSystem;
            fLogger = logger;
            Func<string, bool> fileValidator = filepath => fileSystem.File.Exists(filepath);
            fFilepathOption = new CommandOption<string>("filepath", "The path to the stats database.", required: true, fileValidator);
            Options.Add(fFilepathOption);
            fStatsOutputPath = new CommandOption<string>("statsOutputPath", "The path to output the stats to.", required: true, null);
            Options.Add(fStatsOutputPath);
            fStatCollection = new CommandOption<StatCollection>("statType", "The type of the collection to export.", null);
            Options.Add(fStatCollection);
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

            string exportFilePath = fStatsOutputPath.Value;
            if (fStatCollection.Value.IsPlayerStat())
            {
                string playerBaseFilePath = fFileSystem.Path.Combine(exportFilePath, "Players");
                fFileSystem.Directory.CreateDirectory(playerBaseFilePath);
                SaveAllPlayerStats(playerBaseFilePath, fStatCollection.Value, team, null);
            }
            else
            {
                string allTimeFilePath = fFileSystem.Path.Combine(exportFilePath, "allTimeStats.html");
                SaveAllTimeStats(allTimeFilePath, fStatCollection.Value, team, null);
            }

            _ = fLogger.LogUseful(ReportType.Information, ReportLocation.Loading, $"[Command {Name}] - Completed execution.");
            return 0;
        }

        private void SaveAllTimeStats(string allTimeFilePath, StatCollection statCollection, ICricketTeam team, ICricketSeason season)
        {
            var allTimeStats = StatsCollectionBuilder.StandardStat(
                        statCollection,
                        MatchHelpers.AllMatchTypes,
                        team: team,
                        teamName: team.TeamName,
                        season: season);
            string extension = fFileSystem.Path.GetExtension(allTimeFilePath).Trim('.');
            DocumentType type = extension.ToEnum<DocumentType>();
            allTimeStats.ExportStats(fFileSystem, allTimeFilePath, type, fLogger);
            _ = fLogger.LogUseful(ReportType.Information, ReportLocation.StatisticsPage, $"[Command {Name}] - Completed saving {statCollection} stats."); 
        }

        private void SaveAllPlayerStats(string baseFilePath, StatCollection selectedStatType, ICricketTeam team, ICricketSeason season)
        {
            string extension = fFileSystem.Path.GetExtension(baseFilePath).Trim('.');
            string location = fFileSystem.Path.GetDirectoryName(baseFilePath);
            var matchTypes = MatchHelpers.AllMatchTypes;
            IReadOnlyList<PlayerName> players = selectedStatType == StatCollection.PlayerBrief || selectedStatType == StatCollection.PlayerDetailed
                ? team.Players().Select(player => player.Name).ToList()
                : selectedStatType == StatCollection.PlayerSeason
                    ? season.Players(team.TeamName, matchTypes)
                    : new List<PlayerName>();

            DocumentType type = DocumentType.Html;
            foreach (var playerName in players)                
            {
                var folderSep = fFileSystem.Path.DirectorySeparatorChar;
                string filePath = selectedStatType.IsSeasonStat() 
                    ? $"{baseFilePath}{folderSep}{playerName}-{season.Year.Year}.{type}" 
                    : $"{baseFilePath}{folderSep}{playerName}.{type}";
                var allTimeStats = StatsCollectionBuilder.StandardStat(
                    selectedStatType,
                    matchTypes,
                    team: team,
                    teamName: team.TeamName,
                    season: season,
                    playerName: playerName);
                allTimeStats.ExportStats(fFileSystem, filePath, type, fLogger);
            }

            _ = fLogger.LogUseful(ReportType.Information, ReportLocation.StatisticsPage, $"[Command {Name}] - Completed saving {selectedStatType} stats.");
        }
    }
}