using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Cricket.Interfaces;
using Cricket.Player;
using Cricket.Statistics;
using Cricket.Statistics.DetailedStats;
using StructureCommon.Extensions;
using StructureCommon.FileAccess;
using UICommon.Commands;
using UICommon.Services;
using UICommon.ViewModelBases;

namespace GUI.ViewModels
{
    public class StatsViewModel : ViewModelBase<ICricketTeam>
    {
        private readonly IFileInteractionService fFileService;
        private readonly IDialogCreationService fDialogService;
        private readonly Action<Action<ICricketTeam>> UpdateTeam;

        public List<StatisticsType> StatisticTypes
        {
            get
            {
                return Enum.GetValues(typeof(StatisticsType)).Cast<StatisticsType>().ToList();
            }
        }

        private StatisticsType fSelectedStatsType;
        public StatisticsType SelectedStatsType
        {
            get
            {
                return fSelectedStatsType;
            }
            set
            {
                SelectedStats = null;
                fSelectedStatsType = value;
                OnPropertyChanged(nameof(SelectedStatsType));
                OnPropertyChanged(nameof(SeasonStatsSelected));

                if (value == StatisticsType.AllTimeBrief)
                {
                    SelectedStats = new TeamBriefStatistics(Team);
                }
                if (value == StatisticsType.AllTimeDetailed)
                {
                    SelectedStats = new DetailedAllTimeStatistics(Team);
                }
                if (value == StatisticsType.SeasonBrief && SelectedSeason != null)
                {
                    SelectedStats = new TeamBriefStatistics(SelectedSeason);
                }
            }
        }

        public bool SeasonStatsSelected
        {
            get
            {
                return SelectedStatsType == StatisticsType.SeasonBrief;
            }
        }

        public ICricketTeam Team
        {
            get;
            set;
        }

        private bool fSeasonStatsSet;
        public bool SeasonStatsSet
        {
            get
            {
                return fSeasonStatsSet;
            }
            set
            {
                fSeasonStatsSet = value;
                OnPropertyChanged(nameof(SeasonStatsSet));
            }
        }

        private object fSelectedStats;
        public object SelectedStats
        {
            get
            {
                return fSelectedStats;
            }
            set
            {
                fSelectedStats = value;
                OnPropertyChanged(nameof(SelectedStats));
            }
        }

        private ICricketSeason fSelectedSeason;
        public ICricketSeason SelectedSeason
        {
            get
            {
                return fSelectedSeason;
            }
            set
            {
                fSelectedSeason = value;
                OnPropertyChanged(nameof(SelectedSeason));
                SelectedStats = new TeamBriefStatistics(value);
            }
        }

        private PlayerName fSelectedPlayer;
        public PlayerName SelectedPlayer
        {
            get
            {
                return fSelectedPlayer;
            }
            set
            {
                fSelectedPlayer = value;
                OnPropertyChanged(nameof(SelectedPlayer));
            }
        }

        private bool fPlayerStatsSet;
        public bool PlayerStatsSet
        {
            get
            {
                return fPlayerStatsSet;
            }
            set
            {
                fPlayerStatsSet = value;
                OnPropertyChanged(nameof(PlayerStatsSet));
            }
        }
        private PlayerBriefStatistics fSelectedPlayerStats;
        public PlayerBriefStatistics SelectedPlayerStats
        {
            get
            {
                return fSelectedPlayerStats;
            }
            set
            {
                fSelectedPlayerStats = value;
                OnPropertyChanged(nameof(SelectedPlayerStats));
                PlayerStatsSet = value == null ? false : true;
            }
        }

        public StatsViewModel(ICricketTeam team, Action<Action<ICricketTeam>> updateTeam, IFileInteractionService fileService, IDialogCreationService dialogService)
            : base("Statistics")
        {
            fFileService = fileService;
            fDialogService = dialogService;
            UpdateTeam = updateTeam;
            Team = team;
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
            if (gotFile.Success != null && (bool)gotFile.Success)
            {
                PlayerBriefStatistics playerStats = new PlayerBriefStatistics(SelectedPlayer, SelectedSeason);
                string extension = Path.GetExtension(gotFile.FilePath).Trim('.');
                ExportType type = extension.ToEnum<ExportType>();
                playerStats.ExportStats(gotFile.FilePath, type);
            }
        }

        public ICommand ExportStatsCommand
        {
            get;
        }

        private void ExecuteExportStatsCommand()
        {
            FileInteractionResult gotFile = fFileService.SaveFile("html", "", filter: "Html Files|*.html|CSV Files|*.csv|All Files|*.*");
            if (gotFile.Success != null && (bool)gotFile.Success)
            {
                TeamBriefStatistics allTimeStats = new TeamBriefStatistics(SelectedSeason);
                string extension = Path.GetExtension(gotFile.FilePath).Trim('.');
                ExportType type = extension.ToEnum<ExportType>();
                allTimeStats.ExportStats(gotFile.FilePath, type);
            }
        }

        public ICommand ExportAllStatsCommand
        {
            get;
        }

        private void ExecuteExportAllStatsCommand()
        {
            FileInteractionResult gotFile = fFileService.SaveFile("html", "", filter: "Html Files|*.html|CSV Files|*.csv|All Files|*.*");
            if (gotFile.Success != null && (bool)gotFile.Success)
            {
                TeamBriefStatistics allTimeStats = new TeamBriefStatistics(Team);
                string extension = Path.GetExtension(gotFile.FilePath).Trim('.');
                ExportType type = extension.ToEnum<ExportType>();
                allTimeStats.ExportStats(gotFile.FilePath, type);
            }
        }

        public ICommand ExportDetailedAllStatsCommand
        {
            get;
        }

        private void ExecuteExportDetailedAllStatsCommand()
        {
            FileInteractionResult gotFile = fFileService.SaveFile("html", "", filter: "Html Files|*.html|CSV Files|*.csv|All Files|*.*");
            if (gotFile.Success != null && (bool)gotFile.Success)
            {
                DetailedAllTimeStatistics allTimeStats = new DetailedAllTimeStatistics(Team);
                string extension = Path.GetExtension(gotFile.FilePath).Trim('.');
                ExportType type = extension.ToEnum<ExportType>();
                allTimeStats.ExportStats(gotFile.FilePath, type);
            }
        }

        public override void UpdateData(ICricketTeam cricketTeam)
        {
            Team = null;
            Team = cricketTeam;
        }
    }
}
