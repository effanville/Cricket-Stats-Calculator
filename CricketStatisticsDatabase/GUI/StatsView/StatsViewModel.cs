using Cricket.Interfaces;
using CricketStatistics;
using GUISupport.Services;
using GUISupport.ViewModels;
using System;

namespace GUI.ViewModels
{
    public class StatsViewModel : ViewModelBase
    {
        private readonly IFileInteractionService fFileService;
        private readonly IDialogCreationService fDialogService;
        private readonly Action<Action<ICricketTeam>> UpdateTeam;
        public ICricketTeam Team
        { get; set; }

        private TeamSeasonStatistics fSelectedSeasonStats;
        public TeamSeasonStatistics SelectedSeasonStats
        {
            get { return fSelectedSeasonStats; }
            set { fSelectedSeasonStats = value; OnPropertyChanged(nameof(SelectedSeasonStats)); }
        }

        private ICricketSeason fSelectedSeason;
        public ICricketSeason SelectedSeason
        {
            get { return fSelectedSeason; }
            set { fSelectedSeason = value; OnPropertyChanged(nameof(SelectedSeason)); SelectedSeasonStats = new TeamSeasonStatistics(value); }
        }

        public StatsViewModel(ICricketTeam team, Action<Action<ICricketTeam>> updateTeam, IFileInteractionService fileService, IDialogCreationService dialogService)
            : base ("Statistics")
        {
            fFileService = fileService;
            fDialogService = dialogService;
            UpdateTeam = updateTeam;
            Team = team;
        }

        public override void UpdateData(ICricketTeam portfolio)
        {
            Team = portfolio;
        }
    }
}
