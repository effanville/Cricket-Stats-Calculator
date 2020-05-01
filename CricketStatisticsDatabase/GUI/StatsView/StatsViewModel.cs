using Cricket.Interfaces;
using Cricket.Player;
using Cricket.Statistics;
using CricketStatistics;
using GUISupport;
using GUISupport.Services;
using GUISupport.ViewModels;
using System;
using System.Linq;
using System.Windows.Input;

namespace GUI.ViewModels
{
    public class StatsViewModel : ViewModelBase
    {
        private readonly IFileInteractionService fFileService;
        private readonly IDialogCreationService fDialogService;
        private readonly Action<Action<ICricketTeam>> UpdateTeam;

        public ICricketTeam Team
        {
            get;
            set;
        }

        private bool fSeasonStatsSet;
        public bool SeasonStatsSet
        {
            get { return fSeasonStatsSet; }
            set { fSeasonStatsSet = value; OnPropertyChanged(nameof(SeasonStatsSet)); }
        }

        private TeamSeasonStatistics fSelectedSeasonStats;
        public TeamSeasonStatistics SelectedSeasonStats
        {
            get { return fSelectedSeasonStats; }
            set { fSelectedSeasonStats = value; OnPropertyChanged(nameof(SelectedSeasonStats)); SeasonStatsSet = value == null ? false : true; }
        }

        private ICricketSeason fSelectedSeason;
        public ICricketSeason SelectedSeason
        {
            get { return fSelectedSeason; }
            set { fSelectedSeason = value; OnPropertyChanged(nameof(SelectedSeason)); SelectedSeasonStats = new TeamSeasonStatistics(value); }
        }

        private PlayerName fSelectedPlayer;
        public PlayerName SelectedPlayer
        {
            get { return fSelectedPlayer; }
            set { fSelectedPlayer = value; OnPropertyChanged(nameof(SelectedPlayer)); SelectedPlayerStats = SelectedSeasonStats.SeasonPlayerStats.First(stats => stats.Name.Equals(SelectedPlayer)); }
        }

        private bool fPlayerStatsSet;
        public bool PlayerStatsSet
        {
            get { return fPlayerStatsSet; }
            set { fPlayerStatsSet = value; OnPropertyChanged(nameof(PlayerStatsSet)); }
        }
        private PlayerSeasonStatistics fSelectedPlayerStats;
        public PlayerSeasonStatistics SelectedPlayerStats
        {
            get { return fSelectedPlayerStats; }
            set { fSelectedPlayerStats = value; OnPropertyChanged(nameof(SelectedPlayerStats)); PlayerStatsSet = value == null ? false : true; }
        }

        public StatsViewModel(ICricketTeam team, Action<Action<ICricketTeam>> updateTeam, IFileInteractionService fileService, IDialogCreationService dialogService)
            : base("Statistics")
        {
            fFileService = fileService;
            fDialogService = dialogService;
            UpdateTeam = updateTeam;
            Team = team;
            ExportStatsCommand = new BasicCommand(ExecuteExportStatsCommand);
        }

        public ICommand ExportStatsCommand
        {
            get;
        }

        private void ExecuteExportStatsCommand(object obj)
        {
            var gotFile = fFileService.SaveFile("csv", "", filter: "CSV Files|*.csv|All Files|*.*");
            if (gotFile.Success != null && (bool)gotFile.Success)
            {
                SelectedSeasonStats.ExportStats(gotFile.FilePath);
            }
        }

        public override void UpdateData(ICricketTeam portfolio)
        {
            Team = null;
            Team = portfolio;
        }
    }
}
