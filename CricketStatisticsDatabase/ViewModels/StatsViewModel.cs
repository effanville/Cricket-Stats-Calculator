using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Windows.Input;

using Common.Structure.DisplayClasses;
using Common.Structure.Extensions;
using Common.Structure.ReportWriting;
using Common.UI.Commands;
using Common.UI.Services;
using Common.UI.ViewModelBases;

using CricketStructures;
using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;
using CricketStructures.Statistics;
using CricketStructures.Statistics.DetailedStats;

namespace CSD.ViewModels
{
    public class StatsViewModel : ViewModelBase<ICricketTeam>
    {
        private readonly IFileInteractionService fFileService;

        public List<StatisticsType> StatisticTypes => Enum.GetValues(typeof(StatisticsType)).Cast<StatisticsType>().ToList();

        private StatisticsType fSelectedStatsType;
        public StatisticsType SelectedStatsType
        {
            get => fSelectedStatsType;
            set
            {
                SelectedStats = null;
                fSelectedStatsType = value;
                OnPropertyChanged(nameof(SelectedStatsType));
                OnPropertyChanged(nameof(SeasonStatsSelected));

                var matchTypesToUse = MatchTypeNames.Where(name => name.Selected).Select(name => name.Instance).ToArray();

                if (value == StatisticsType.AllTimeBrief)
                {
                    SelectedStats = new TeamBriefStatistics(DataStore, matchTypesToUse);
                }
                if (value == StatisticsType.AllTimeDetailed)
                {
                    SelectedStats = new DetailedAllTimeStatistics(DataStore);
                }
                if (value == StatisticsType.SeasonBrief && SelectedSeason != null)
                {
                    SelectedStats = new TeamBriefStatistics(DataStore.TeamName, SelectedSeason, matchTypesToUse);
                }
            }
        }

        private List<Selectable<CricketStructures.Match.MatchType>> fMatchTypeNames = new List<Selectable<CricketStructures.Match.MatchType>>();

        public List<Selectable<CricketStructures.Match.MatchType>> MatchTypeNames
        {
            get => fMatchTypeNames;
            set
            {
                fMatchTypeNames = value;
                OnPropertyChanged();
            }
        }

        public bool SeasonStatsSelected => SelectedStatsType == StatisticsType.SeasonBrief;


        private bool fSeasonStatsSet;
        public bool SeasonStatsSet
        {
            get => fSeasonStatsSet;
            set
            {
                fSeasonStatsSet = value;
                OnPropertyChanged(nameof(SeasonStatsSet));
            }
        }

        private object fSelectedStats;
        public object SelectedStats
        {
            get => fSelectedStats;
            set
            {
                fSelectedStats = value;
                OnPropertyChanged(nameof(SelectedStats));
            }
        }

        private ICricketSeason fSelectedSeason;
        public ICricketSeason SelectedSeason
        {
            get => fSelectedSeason;
            set
            {
                fSelectedSeason = value;
                OnPropertyChanged(nameof(SelectedSeason));
                SelectedStats = new TeamBriefStatistics(DataStore.TeamName, value);
            }
        }

        private PlayerName fSelectedPlayer;
        public PlayerName SelectedPlayer
        {
            get => fSelectedPlayer;
            set
            {
                fSelectedPlayer = value;
                OnPropertyChanged(nameof(SelectedPlayer));
            }
        }

        private bool fPlayerStatsSet;
        public bool PlayerStatsSet
        {
            get => fPlayerStatsSet;
            set
            {
                fPlayerStatsSet = value;
                OnPropertyChanged(nameof(PlayerStatsSet));
            }
        }
        private PlayerBriefStatistics fSelectedPlayerStats;
        public PlayerBriefStatistics SelectedPlayerStats
        {
            get => fSelectedPlayerStats;
            set
            {
                fSelectedPlayerStats = value;
                OnPropertyChanged(nameof(SelectedPlayerStats));
                PlayerStatsSet = value == null ? false : true;
            }
        }

        public StatsViewModel(ICricketTeam team, IFileInteractionService fileService)
            : base("Statistics", team)
        {
            fFileService = fileService;
            MatchTypeNames = new List<Selectable<CricketStructures.Match.MatchType>>();
            foreach (var name in MatchHelpers.AllMatchTypes)
            {
                MatchTypeNames.Add(new Selectable<CricketStructures.Match.MatchType>(name, false));
            }

            ExportPlayerStatsCommand = new RelayCommand(ExecuteExportPlayerStatsCommand);
            ExportStatsCommand = new RelayCommand(ExecuteExportStatsCommand);
            ExportAllStatsCommand = new RelayCommand(ExecuteExportAllStatsCommand);
            ExportDetailedAllStatsCommand = new RelayCommand(ExecuteExportDetailedAllStatsCommand);
        }

        public ICommand ExportPlayerStatsCommand
        {
            get;
        }

        private void ExecuteExportPlayerStatsCommand()
        {
            FileInteractionResult gotFile = fFileService.SaveFile("html", "", filter: "Html Files|*.html|CSV Files|*.csv|All Files|*.*");
            if (gotFile.Success)
            {
                PlayerBriefStatistics playerStats = new PlayerBriefStatistics(DataStore.TeamName, SelectedPlayer, SelectedSeason, MatchHelpers.AllMatchTypes);
                string extension = Path.GetExtension(gotFile.FilePath).Trim('.');
                DocumentType type = extension.ToEnum<DocumentType>();
                playerStats.ExportStats(new FileSystem(), gotFile.FilePath, type);
            }
        }

        public ICommand ExportStatsCommand
        {
            get;
        }

        private void ExecuteExportStatsCommand()
        {
            FileInteractionResult gotFile = fFileService.SaveFile("html", "", filter: "Html Files|*.html|CSV Files|*.csv|All Files|*.*");
            if (gotFile.Success)
            {
                TeamBriefStatistics allTimeStats = new TeamBriefStatistics(DataStore.TeamName, SelectedSeason, MatchHelpers.AllMatchTypes);
                string extension = Path.GetExtension(gotFile.FilePath).Trim('.');
                DocumentType type = extension.ToEnum<DocumentType>();
                allTimeStats.ExportStats(new FileSystem(), gotFile.FilePath, type);
            }
        }

        public ICommand ExportAllStatsCommand
        {
            get;
        }

        private void ExecuteExportAllStatsCommand()
        {
            FileInteractionResult gotFile = fFileService.SaveFile("html", "", filter: "Html Files|*.html|CSV Files|*.csv|All Files|*.*");
            if (gotFile.Success)
            {
                var matchTypesToUse = MatchTypeNames.Where(name => name.Selected).Select(name => name.Instance).ToArray();
                string extension = Path.GetExtension(gotFile.FilePath).Trim('.');
                DocumentType type = extension.ToEnum<DocumentType>();

                TeamBriefStatistics allTimeStatsNew = new TeamBriefStatistics(DataStore, MatchHelpers.AllMatchTypes);
                allTimeStatsNew.ExportStats(new FileSystem(), gotFile.FilePath, type);
            }
        }

        public ICommand ExportDetailedAllStatsCommand
        {
            get;
        }

        private void ExecuteExportDetailedAllStatsCommand()
        {
            FileInteractionResult gotFile = fFileService.SaveFile("html", "", filter: "Html Files|*.html|CSV Files|*.csv|All Files|*.*");
            if (gotFile.Success)
            {
                string extension = Path.GetExtension(gotFile.FilePath).Trim('.');
                DocumentType type = extension.ToEnum<DocumentType>();
                DetailedAllTimeStatistics allTimeStatsNew = new DetailedAllTimeStatistics(DataStore);
                allTimeStatsNew.ExportStats(new FileSystem(), gotFile.FilePath, type);
            }
        }

        public override void UpdateData(ICricketTeam cricketTeam)
        {
            base.UpdateData(cricketTeam);
        }
    }
}
